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

        private bool _usingSwr = false;

        private HttpClient client;
        private IBrowsingContext context;
        private HttpClientHandler handler;

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

            var result = await client.PostAsync(uri, queryData);

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
            var result = await client.GetAsync(uri);

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
            context = BrowsingContext.New(config);
            var loginDoc = await context.OpenAsync("https://dips.sja.org.uk/default.aspx?ReturnUrl=%2f");

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

            handler = new HttpClientHandler
            {
                CookieContainer = new System.Net.CookieContainer()
            };
            client = new HttpClient(handler);
            using (var content = new FormUrlEncodedContent(requestString))
            {
                var res = await client.PostAsync(new Uri("https://dips.sja.org.uk/default.aspx?ReturnUrl=%2f"), content);
            }

            return handler.CookieContainer.Count > 0;
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
                    if (client != null)
                        client.Dispose();
                    if (handler != null)
                        handler.Dispose();
                }

                disposedValue = true;
            }
        }

        #endregion IDisposable Support
    }
}
