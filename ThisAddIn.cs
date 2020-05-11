using DIPSCrewPlanner.DIPS;
using DIPSCrewPlanner.Model;
using Microsoft.ApplicationInsights;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DIPSCrewPlanner
{
    public partial class ThisAddIn
    {
        private const string PeopleArea = "PeopleArea";
        private const string PeopleCacheSheetName = "PeopleCache";
        private const string PeopleNamesArea = "PeopleNames";
        private const string SheetDateFormat = "dddd ddMMyy";
        private readonly TelemetryClient _telemetryClient = new TelemetryClient();
        private Credentials _credentials;
        private string _settingsFolder;
        private string _settingsPath;

        public Credentials Credentials
        {
            get => _credentials;
            private set
            {
                _credentials = value;
                Globals.Ribbons.CrewPlannerRibbon.HasCredentials = value != null;
            }
        }

        public async void GetDipsIds()
        {
            try
            {
                Application.Cursor = XlMousePointer.xlWait;

                var swrClient = await GetClient(true);
                if (swrClient == null)
                    return;
                var wmrClient = await GetClient(false);
                if (wmrClient == null)
                    return;

                Worksheet sheet = Application.ActiveSheet;

                var date = DateTime.FromOADate((double)sheet.Range["A1"].Value2);

                foreach (var row in sheet.UsedRange.Rows.OfType<Range>().Skip(3))
                {
                    var eventName = row.Cells[1, 1].Value;
                    if (string.IsNullOrWhiteSpace(eventName))
                        continue;

                    var hub = IdentifyHub(eventName);

                    if (hub == null)
                        continue;

                    int dipsId;

                    if (hub.DipsContext == DipsContext.SWR)
                        dipsId = await swrClient.GetDipsId(date, hub.DipsName);
                    else
                        dipsId = await wmrClient.GetDipsId(date, hub.DipsName);

                    if (dipsId != 0)
                        row.Cells[1, 2].Value = dipsId;
                    else
                    {
                        _telemetryClient.TrackEvent("Could not identify event", new Dictionary<string, string> { { "event", hub.DipsName }, { "date", date.ToString("o") } });
                    }
                }
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex, new Dictionary<string, string> { { "activity", "get-dips-id" } });
            }
            finally
            {
                Application.Cursor = XlMousePointer.xlDefault;
            }
        }

        public async void SetDipsCredentials()
        {
            try
            {
                Application.Cursor = XlMousePointer.xlWait;

                var form = new DipsCredentialsForm
                {
                    Credentials = Credentials
                };

                bool tryAgain;

                do
                {
                    tryAgain = false;
                    var res = form.ShowDialog();

                    if (res == DialogResult.OK)
                    {
                        var credentials = form.Credentials;
                        var client = new DipsClient(credentials);

                        var swrSuccess = await client.Login(true);
                        var wmrSuccess = await client.Login(false);

                        if (!swrSuccess && !wmrSuccess)
                        {
                            MessageBox.Show("Neither your WMR nor your SWR password worked.  Please try again.  If this keeps failing, try logging into DIPS and check that your password hasn't expired.", "Log In Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            _telemetryClient.TrackEvent("Bad user credentials provided", new Dictionary<string, string> { { "account", "both" } });
                            tryAgain = true;
                        }
                        else if (!wmrSuccess)
                        {
                            MessageBox.Show("Your WMR password did not work.  Please try again.  If this keeps failing, try logging into DIPS and check that your password hasn't expired.", "Log In Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            _telemetryClient.TrackEvent("Bad user credentials provided", new Dictionary<string, string> { { "account", "WMR" } });
                            tryAgain = true;
                        }
                        else if (!swrSuccess)
                        {
                            MessageBox.Show("Your SWR password did not work.  Please try again.  If this keeps failing, try logging into DIPS and check that your password hasn't expired.", "Log In Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            _telemetryClient.TrackEvent("Bad user credentials provided", new Dictionary<string, string> { { "account", "SWR" } });
                            tryAgain = true;
                        }
                        else
                        {
                            Credentials = form.Credentials;
                            var saveRes = MessageBox.Show("Log in successful.  Do you want to save your credentials for next time?  Do not do this on a shared or unsecure PC.", "Log In Success", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (saveRes == DialogResult.Yes)
                            {
                                try
                                {
                                    _telemetryClient.TrackEvent("Saving credentials");
                                    if (!Directory.Exists(_settingsFolder))
                                        Directory.CreateDirectory(_settingsFolder);

                                    var credentialsFile = JsonSerializer.Serialize(Credentials);
                                    File.WriteAllText(_settingsPath, credentialsFile);
                                }
                                catch (SystemException ex)
                                {
                                    _telemetryClient.TrackException(ex, new Dictionary<string, string> { { "activity", "save-credentials" } });
                                    MessageBox.Show("There was a problem saving your credentials.  You will have to try again next time.");
                                }
                            }
                            else
                            {
                                _telemetryClient.TrackEvent("Not saving credentials");
                            }
                        }
                    }
                } while (tryAgain);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex, new Dictionary<string, string> { { "activity", "set-credentials" } });
            }
            finally
            {
                Application.Cursor = XlMousePointer.xlDefault;
            }
        }

        public void SetupBook()
        {
            _telemetryClient.TrackEvent("Setup book");
            var lastSheetToRemove = "";

            foreach (Worksheet sheet in Application.ActiveWorkbook.Worksheets)
            {
                if (Application.ActiveWorkbook.Worksheets.Count == 1)
                    lastSheetToRemove = sheet.Name;
                else
                    sheet.Delete();
            }

            Worksheet previousSheet = null;

            Worksheet peopleCache = Application.ActiveWorkbook.Worksheets.Add();
            peopleCache.Name = PeopleCacheSheetName;

            Application.Names.Add(PeopleNamesArea, peopleCache.Range["A1"]);
            Application.Names.Add(PeopleArea, peopleCache.Range["A1:F1"]);

            for (var i = DateTime.Today; i < DateTime.Today.AddDays(14); i = i.AddDays(1))
            {
                Worksheet newSheet;

                if (previousSheet == null)
                    newSheet = Application.ActiveWorkbook.Worksheets.Add();
                else
                    newSheet = Application.ActiveWorkbook.Worksheets.Add(Before: peopleCache);

                SetupDaySheet(i, newSheet);

                previousSheet = newSheet;
            }

            Application.ActiveWorkbook.Worksheets[lastSheetToRemove].Delete();
            peopleCache.Visible = XlSheetVisibility.xlSheetHidden;
        }

        public async void UpdatePeopleList()
        {
            try
            {
                _telemetryClient.TrackEvent("Started updating volunteer list");
                Application.Cursor = XlMousePointer.xlWait;

                var swrClient = await GetClient(true);
                if (swrClient == null)
                    return;
                var wmrClient = await GetClient(false);
                if (wmrClient == null)
                    return;

                var swrResult = (await swrClient.GetEMTs()).ToList();
                var swrUnits = (await swrClient.GetUnits()).ToDictionary(u => u.Name, u => u.Id);
                var wmrResult = (await wmrClient.GetEMTs()).ToList();
                var wmrUnits = (await wmrClient.GetUnits()).ToDictionary(u => u.Name, u => u.Id);

                foreach (var person in swrResult)
                    person.UnitId = swrUnits[person.UnitName];
                foreach (var person in wmrResult)
                    person.UnitId = wmrUnits[person.UnitName];

                var people = Enumerable.Concat(swrResult, wmrResult).ToList();

                var volunteerCount = _telemetryClient.GetMetric("VolunteersDownloaded");
                volunteerCount.TrackValue(people.Count());

                var currentRow = 0;
                var cacheSheet = Application.Worksheets.OfType<Worksheet>().FirstOrDefault(s => s.Name == PeopleCacheSheetName);

                if (cacheSheet == null)
                {
                    cacheSheet = Application.Worksheets.Add();
                    cacheSheet.Name = PeopleCacheSheetName;
                }

                var displayNameSet = new HashSet<string>();

                foreach (var person in people.OrderBy(p => p.LastName).ThenBy(p => p.FirstName))
                {
                    currentRow++;

                    var count = 1;
                    var displayName = person.DisplayName;
                    while (displayNameSet.Contains(displayName))
                    {
                        count++;
                        displayName = $"{person.DisplayName} {count}";
                    }
                    displayNameSet.Add(displayName);

                    cacheSheet.Range[$"A{currentRow}"].Value = displayName;
                    cacheSheet.Range[$"B{currentRow}"].Value = person.DipsId;
                    cacheSheet.Range[$"C{currentRow}"].Value = person.DipsContext.ToString();
                    cacheSheet.Range[$"D{currentRow}"].Value = person.UnitName;
                    cacheSheet.Range[$"E{currentRow}"].Value = person.FirstName;
                    cacheSheet.Range[$"F{currentRow}"].Value = person.LastName;
                    cacheSheet.Range[$"G{currentRow}"].Value = person.UnitId;
                }

                Application.Names.Add(PeopleNamesArea, cacheSheet.Range[$"A1:A{currentRow}"]);
                Application.Names.Add(PeopleArea, cacheSheet.Range[$"A1:I{currentRow}"]);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex, new Dictionary<string, string> { { "activity", "update-volunteer-list" } });
            }
            finally
            {
                Application.Cursor = XlMousePointer.xlDefault;
            }
        }

        public async void UploadSheetToDips()
        {
            _telemetryClient.TrackEvent("Started uploading sheet");

            var timer = new Stopwatch();
            timer.Start();
            var volunteerCount = 0;
            var alreadyLoggedCount = 0;

            try
            {
                Application.Cursor = XlMousePointer.xlWait;

                var swrClient = await GetClient(true);
                var wmrClient = await GetClient(false);

                if (swrClient == null || wmrClient == null)
                    return;

                Worksheet sheet = Application.ActiveSheet;

                var date = DateTime.FromOADate((double)sheet.Range["A1"].Value2);

                foreach (var row in sheet.UsedRange.Rows.OfType<Range>().Skip(3))
                {
                    var eventName = row.Cells[1, 1].Value;
                    if (string.IsNullOrWhiteSpace(eventName))
                        continue;

                    HubDetails hub = IdentifyHub(eventName);

                    var dipsIdText = row.Cells[1, 2].Text;
                    var parseResult = int.TryParse(dipsIdText, out int dipsId);

                    if (!parseResult)
                    {
                        if (!string.IsNullOrWhiteSpace(dipsIdText))
                            _telemetryClient.TrackEvent("Unidentifiable DIPS ID", new Dictionary<string, string> { { "dips-id", dipsIdText } });

                        continue;
                    }

                    string driverName = row.Cells[1, 4].Text;
                    string attendantName = row.Cells[1, 6].Text;

                    if (string.IsNullOrWhiteSpace(row.Cells[1, 8].Text) || string.IsNullOrWhiteSpace(row.Cells[1, 9].Text))
                    {
                        if (!string.IsNullOrWhiteSpace(driverName))
                            MessageBox.Show($"Shift times are missing for {driverName}.  They will not be added to DIPS.", "Missing Shift Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (!string.IsNullOrWhiteSpace(attendantName))
                            MessageBox.Show($"Shift times are missing for {attendantName}.  They will not be added to DIPS.", "Missing Shift Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        continue;
                    }

                    var startTime = date.Date + DateTime.FromOADate((double)row.Cells[1, 8].Value2).TimeOfDay;
                    var endTime = date.Date + DateTime.FromOADate((double)row.Cells[1, 9].Value2).TimeOfDay;

                    IEnumerable<Person> currentStaff;

                    if (hub.DipsContext == DipsContext.SWR)
                        currentStaff = await swrClient.GetBookedVolunteers(dipsId);
                    else
                        currentStaff = await wmrClient.GetBookedVolunteers(dipsId);

                    // Get driver details
                    if (!string.IsNullOrWhiteSpace(driverName))
                    {
                        var trimmedName = driverName.Trim(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ' });
                        var unit = row.Cells[1, 5].Text;

                        if (!currentStaff.Any(p => p.DisplayName == trimmedName && p.UnitName == unit))
                        {
                            var result = await AddVolunteer(hub.DipsContext == DipsContext.SWR ? swrClient : wmrClient, dipsId, driverName, startTime, endTime);
                            if (result)
                                volunteerCount++;
                        }
                        else
                            alreadyLoggedCount++;
                    }

                    // Get attendant details
                    if (!string.IsNullOrWhiteSpace(attendantName))
                    {
                        var trimmedName = attendantName.Trim(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ' });
                        var unit = row.Cells[1, 7].Text;

                        if (!currentStaff.Any(p => p.DisplayName == trimmedName && p.UnitName == unit))
                        {
                            var result = await AddVolunteer(hub.DipsContext == DipsContext.SWR ? swrClient : wmrClient, dipsId, attendantName, startTime, endTime);
                            if (result)
                                volunteerCount++;
                        }
                        else
                            alreadyLoggedCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
            }
            finally
            {
                var uploadMetric = _telemetryClient.GetMetric("UploadTimeTaken");
                var volunteersCountMetric = _telemetryClient.GetMetric("VolunteersUploaded");
                var volunteersNotAddedCountMetric = _telemetryClient.GetMetric("VolunteersAlreadyListed");

                Application.Cursor = XlMousePointer.xlDefault;
                timer.Stop();
                uploadMetric.TrackValue(timer.ElapsedMilliseconds);
                volunteersCountMetric.TrackValue(volunteerCount);
                volunteersNotAddedCountMetric.TrackValue(alreadyLoggedCount);

                string addedMessage;
                string alreadyLoggedMessage;

                switch (volunteerCount)
                {
                    case 0:
                        addedMessage = "Nobody added to DIPS.";
                        break;

                    case 1:
                        addedMessage = "1 person added to DIPS.";
                        break;

                    default:
                        addedMessage = $"{volunteerCount} people added to DIPS.";
                        break;
                }

                switch (alreadyLoggedCount)
                {
                    case 0:
                        alreadyLoggedMessage = "Nobody was already signed up.";
                        break;

                    case 1:
                        alreadyLoggedMessage = "1 person was already signed up.";
                        break;

                    default:
                        alreadyLoggedMessage = $"{alreadyLoggedCount} were already signed up.";
                        break;
                }

                MessageBox.Show($"Update complete.  {addedMessage}  {alreadyLoggedMessage}", "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task<bool> AddVolunteer(DipsClient client, int dipsId, string volunteerName, DateTime startTime, DateTime endTime)
        {
            var driverId = FindVolunteerId(volunteerName);
            var unitId = FindVolunteerUnitId(volunteerName);

            Debug.WriteLine($"Adding volunteer : {driverId}");

            if (driverId != 0)
            {
                var res = await client.AddVolunteer(dipsId, driverId, "ETA", unitId, startTime, endTime);

                if (!res)
                {
                    Debug.WriteLine($"Adding volunteer : failed");
                    _telemetryClient.TrackEvent("Adding a volunteer failed");
                    MessageBox.Show($"Could not add volunteer : {volunteerName}");
                }

                return res;
            }
            else
            {
                Debug.WriteLine($"Adding volunteer : volunteer not found");
                _telemetryClient.TrackEvent("Finding a volunteer failed");
                MessageBox.Show($"Could not find volunteer : {volunteerName}");
                return false;
            }
        }

        private int FindVolunteerId(string name)
        {
            Worksheet sheet = Application.Worksheets[PeopleCacheSheetName];

            foreach (var row in sheet.UsedRange.Rows.OfType<Range>())
            {
                var personName = row.Cells[1, 1].Text as string;

                if (name.Equals(personName, StringComparison.InvariantCultureIgnoreCase))
                {
                    var parseSuccess = int.TryParse(row.Cells[1, 2].Text, out int resultId);

                    if (parseSuccess)
                        return resultId;
                    return 0;
                }
            }

            return 0;
        }

        private int FindVolunteerUnitId(string name)
        {
            Worksheet sheet = Application.Worksheets[PeopleCacheSheetName];

            foreach (var row in sheet.UsedRange.Rows.OfType<Range>())
            {
                var personName = row.Cells[1, 1].Text as string;

                if (name.Equals(personName, StringComparison.InvariantCultureIgnoreCase))
                {
                    var parseSuccess = int.TryParse(row.Cells[1, 7].Text, out int resultId);

                    if (parseSuccess)
                        return resultId;
                    return 0;
                }
            }

            return 0;
        }

        private async Task<DipsClient> GetClient(bool useSwr)
        {
            var client = new DipsClient(Credentials);
            var success = await client.Login(useSwr);
            if (!success)
            {
                MessageBox.Show($"Could not log in to {(useSwr ? "SWR" : "WMR")} DIPS.", "Log In Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _telemetryClient.TrackEvent("DIPS login failed");
                return null;
            }

            _telemetryClient.TrackEvent("DIPS login success");
            return client;
        }

        private HubDetails IdentifyHub(string eventName)
        {
            var hub = HubDetails.GetDefaultSettings().FirstOrDefault(h => h.DisplayName == eventName);

            if (hub == null)
            {
                _telemetryClient.TrackEvent("Unidentifiable hub found", new Dictionary<string, string> { { "hub", eventName } });
            }

            return hub;
        }

        private void SetupDaySheet(DateTime date, Worksheet worksheet)
        {
            worksheet.Name = date.ToString(SheetDateFormat);

            var titleRange = worksheet.Range["A1:I1"];
            titleRange.Merge();
            titleRange.Font.Size = 24;
            titleRange.Borders.LineStyle = XlLineStyle.xlContinuous;
            titleRange.Borders.Weight = 4;
            titleRange.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 242, 204));
            titleRange.Value2 = date;
            titleRange.NumberFormat = "dddd dd mmm yyyy";
            titleRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            worksheet.Columns[1].ColumnWidth = 17.89;
            worksheet.Columns[2].ColumnWidth = 13.11;
            worksheet.Columns[3].ColumnWidth = 6.89;
            worksheet.Columns[4].ColumnWidth = 21.22;
            worksheet.Columns[5].ColumnWidth = 21.22;
            worksheet.Columns[6].ColumnWidth = 21.22;
            worksheet.Columns[7].ColumnWidth = 21.22;
            worksheet.Columns[8].ColumnWidth = 9.22;
            worksheet.Columns[9].ColumnWidth = 9.22;

            worksheet.Range["A3"].Value = "Hub";
            worksheet.Range["B3"].Value = "DIPS Reference";
            worksheet.Range["C3"].Value = "Vehicle";
            worksheet.Range["D3"].Value = "Driver";
            worksheet.Range["E3"].Value = "Driver Unit";
            worksheet.Range["F3"].Value = "Attendant";
            worksheet.Range["G3"].Value = "Attendant Unit";
            worksheet.Range["H3"].Value = "Start Time";
            worksheet.Range["I3"].Value = "End Time";
            worksheet.Range["A3:I3"].Font.Bold = true;
            worksheet.Range["A3:I3"].Borders.LineStyle = XlLineStyle.xlContinuous;
            worksheet.Range["A3:I3"].Borders.Weight = 2;
            worksheet.Range["A3:I3"].Interior.Color = ColorTranslator.ToOle(Color.FromArgb(198, 224, 180));

            var startRow = 4;

            foreach (var details in HubDetails.GetDefaultSettings())
            {
                for (var i = 0; i < details.VehicleCount; i++)
                {
                    worksheet.Range[$"A{startRow}"].Value = details.DisplayName;
                    worksheet.Range[$"A{startRow}"].Font.Bold = true;
                    worksheet.Range[$"A{startRow}:I{startRow}"].Borders.LineStyle = XlLineStyle.xlContinuous;
                    worksheet.Range[$"A{startRow}:I{startRow}"].Borders.Weight = 2;
                    worksheet.Range[$"D{startRow}"].Validation.Add(XlDVType.xlValidateList, Formula1: $"={PeopleNamesArea}");
                    worksheet.Range[$"E{startRow}"].FormulaLocal = $"=VLOOKUP(D{startRow},{PeopleArea},4,FALSE)";
                    worksheet.Range[$"F{startRow}"].Validation.Add(XlDVType.xlValidateList, Formula1: $"={PeopleNamesArea}");
                    worksheet.Range[$"G{startRow}"].FormulaLocal = $"=VLOOKUP(F{startRow},{PeopleArea},4,FALSE)";
                    startRow++;
                }
                startRow++;
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            _telemetryClient.Context.Session.Id = Guid.NewGuid().ToString();
            _telemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();

            _telemetryClient.TrackPageView("DipsCrewPlanner");

            _settingsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DIPSCrewPlanner");
            _settingsPath = Path.Combine(_settingsFolder, "credentials.json");

            Credentials = null;

            if (File.Exists(_settingsPath))
            {
                _telemetryClient.TrackEvent("Credential file found");

                try
                {
                    var creds = File.ReadAllText(_settingsPath);
                    Credentials = JsonSerializer.Deserialize<Credentials>(creds);
                    _telemetryClient.TrackEvent("Credential file loaded");
                }
                catch (SystemException ex)
                {
                    _telemetryClient.TrackException(ex, new Dictionary<string, string> { { "activity", "restore-credentials" } });
                    MessageBox.Show("There was a problem loading your DIPS credentials.  You will need to re-enter them.", "Error Loading Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Globals.Ribbons.CrewPlannerRibbon.IsOnline = NetworkInterface.GetIsNetworkAvailable();
            NetworkChange.NetworkAvailabilityChanged += UpdateNetworkAvailability;
        }

        private void UpdateNetworkAvailability(object sender, NetworkAvailabilityEventArgs e)
        {
            Globals.Ribbons.CrewPlannerRibbon.IsOnline = e.IsAvailable;
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += new EventHandler(ThisAddIn_Startup);
            Shutdown += new EventHandler(ThisAddIn_Shutdown);
        }

        #endregion VSTO generated code
    }
}