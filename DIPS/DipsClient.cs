using AngleSharp;
using AngleSharp.Html.Dom;
using DIPSCrewPlanner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DIPSCrewPlanner.DIPS
{
    internal class DipsClient : IDisposable
    {
        private readonly Credentials _creds;
        private readonly Regex CyclingHoursRegex = new Regex(@"Cycle responder operational hours<\/td>.+?([\d\.]+) Hrs<\/td>", RegexOptions.Compiled | RegexOptions.Singleline);

        private Dictionary<string, int> _cachedDipsValues = new Dictionary<string, int>();
        private HttpClient _client;
        private IBrowsingContext _context;
        private HttpClientHandler _handler;
        private bool _usingSwr = false;

        public DipsClient(Credentials creds)
        {
            _creds = creds;
        }

        private string Password
        {
            get => _usingSwr ? _creds.SwrDipsPassword : _creds.WmrDipsPassword;
            set
            {
                if (_usingSwr)
                    _creds.SwrDipsPassword = value;
                else
                    _creds.WmrDipsPassword = value;
            }
        }

        private string UserName
        {
            get => _usingSwr ? _creds.SwrDipsUsername : _creds.WmrDipsUsername;
            set
            {
                if (_usingSwr)
                    _creds.SwrDipsUsername = value;
                else
                    _creds.WmrDipsUsername = value;
            }
        }

        public async Task<int> GetDipsId(DateTime date, string name)
        {
            var key = name + date.ToString("o");

            if (_cachedDipsValues.ContainsKey(key))
                return _cachedDipsValues[key];

            var uri = new Uri("https" + $"://dips.sja.org.uk/{(_usingSwr ? "SWR" : "WMR")}/DutySystem-ListDay.asp?newdist=&filter=&UID={date:dd/MM/yyyy}");
            var result = await _client.GetAsync(uri);

            var pageContent = await result.Content.ReadAsStringAsync();

            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(r => r.Content(pageContent));

            var tableRows = document.QuerySelectorAll("tr.normal");

            foreach (IHtmlTableRowElement row in tableRows)
            {
                var eventName = row.Children[1].TextContent;
                if (eventName.StartsWith(name))
                {
                    var dipsId = row.Children[0].TextContent;
                    var idPart = dipsId.Split(new[] { '/' }, 2, StringSplitOptions.RemoveEmptyEntries)[1];

                    if (idPart.EndsWith("-M"))
                        idPart = idPart.Substring(0, 6);

                    var parseSuccess = int.TryParse(idPart, out var id);

                    if (parseSuccess)
                    {
                        _cachedDipsValues[key] = id;
                        return id;
                    }
                }
            }

            return 0;
        }

        public async Task<IEnumerable<Person>> GetEMTs()
        {
            var uri = new Uri("https" + $"://dips.sja.org.uk/{(_usingSwr ? "SWR" : "WMR")}/FindMemberCountyWide.asp?lookuptype=lookup&region=true&MI=");
            var parameters = new Dictionary<string, string>() {
                {"area", "all" },
                {"division", "all" },
                {"badge", "" },
                {"PIN", "" },
                {"firstname", "" },
                {"surname", "" },
                {"rank", "all" },
                {"role", "ETA" },
                {"driving", "all" },
                {"drivingskills", "all" },
            };
            var queryData = new FormUrlEncodedContent(parameters);

            var result = await _client.PostAsync(uri, queryData);

            if (!result.IsSuccessStatusCode)
                return Enumerable.Empty<Person>();

            var pageContent = await result.Content.ReadAsStringAsync();

            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(r => r.Content(pageContent));

            var tableRows = document.QuerySelectorAll("tr.smallnormal:not([bgcolor=\"pink\"])");

            var peopleList = new List<Person>();

            foreach (IHtmlTableRowElement row in tableRows)
            {
                var role = row.Children[2].TextContent;

                if (role == "Advanced First Aider" || role == "First Aider" || role == "Not Operational" || role == "Support Member")
                    continue;

                var name = row.Children[0].Children[0].TextContent;

                if (name.StartsWith("External Ambulance Crew") || name.StartsWith("Ambulance Service"))
                    continue;

                var unitName = row.Children[4].TextContent;

                if (unitName.Contains("DO NOT USE"))
                    continue;

                var clickLink = row.Children[0].Children[0].Attributes["onclick"]?.Value;
                if (clickLink == null)
                {
                    clickLink = row.Children[0].Children[0].Attributes["href"].Value;
                    clickLink = clickLink.Split(new[] { '(', ')', ',' })[1].Trim('\'');
                }

                var dipsIdString = clickLink.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries).Last().Trim('\'', '"', ' ');
                var parseSuccess = int.TryParse(dipsIdString, out var dipsId);
                if (!parseSuccess)
                    continue;

                var nameParts = name.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                var person = new Person
                {
                    FirstName = nameParts[0],
                    LastName = nameParts[1],
                    DipsId = dipsId,
                    UnitName = unitName,
                    DipsContext = _usingSwr ? DipsContext.SWR : DipsContext.WMR
                };

                peopleList.Add(person);
            }

            return peopleList;
        }

        public async Task<decimal> GetHours(int year, string dipsId)
        {
            var uri = new Uri("https" + $"://dips.sja.org.uk/{(_usingSwr ? "SWR" : "WMR")}/YearEndReport-Single.asp?type=getreport&member={dipsId}&listing=single&year={year}");
            var result = await _client.GetAsync(uri);

            var resultString = await result.Content.ReadAsStringAsync();
            var matches = CyclingHoursRegex.Matches(resultString);

            if (matches.Count != 1)
                return 0;

            var hoursString = matches[0].Groups[1].Value;
            var success = decimal.TryParse(hoursString, out var hours);

            return success ? hours : 0;
        }

        public async Task<bool> Login(bool useSwr)
        {
            _usingSwr = useSwr;

            var config = Configuration.Default.WithDefaultLoader();
            _context = BrowsingContext.New(config);
            var loginDoc = await _context.OpenAsync("https://dips.sja.org.uk/default.aspx?ReturnUrl=%2f");

            if (!(loginDoc.QuerySelector("#__VIEWSTATE") is IHtmlInputElement viewState)
                || !(loginDoc.QuerySelector("#__VIEWSTATEGENERATOR") is IHtmlInputElement viewStateGenerator)
                || !(loginDoc.QuerySelector("#__EVENTVALIDATION") is IHtmlInputElement eventValidation))
                return false;

            var requestString = new Dictionary<string, string>()
            {
                { "UserName", UserName },
                { "Password", Password },
                { "__VIEWSTATE", viewState.Value },
                { "__VIEWSTATEGENERATOR", viewStateGenerator.Value },
                { "__EVENTVALIDATION", eventValidation.Value },
                { "drpVersion", "2010" },
                { "btnSubmit", "Login to DIPS" }
            };

            _handler = new HttpClientHandler
            {
                CookieContainer = new System.Net.CookieContainer()
            };
            _client = new HttpClient(_handler);
            using (var content = new FormUrlEncodedContent(requestString))
            {
                var res = await _client.PostAsync(new Uri("https://dips.sja.org.uk/default.aspx?ReturnUrl=%2f"), content);
            }

            return _handler.CookieContainer.Count > 0;
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_client != null)
                        _client.Dispose();
                    if (_handler != null)
                        _handler.Dispose();
                }

                disposedValue = true;
            }
        }

        #endregion IDisposable Support
    }
}
