using DIPSCrewPlanner.DIPS;
using DIPSCrewPlanner.Model;
using Microsoft.Office.Interop.Excel;
using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace DIPSCrewPlanner
{
    public partial class ThisAddIn
    {
        private const string EventCacheSheetName = "EventCache";
        private const string PeopleCacheSheetName = "PeopleCache";
        private const string SheetDateFormat = "dddd ddMMyy";
        private string _settingsFolder;
        private string _settingsPath;
        public Credentials Credentials { get; private set; }

        public async void SetDipsCredentials()
        {
            try
            {
                Application.Cursor = XlMousePointer.xlWait;

                var form = new DipsCredentialsForm();
                form.Credentials = Credentials;

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
                            tryAgain = true;
                        }
                        else if (!wmrSuccess)
                        {
                            MessageBox.Show("Your WMR password did not work.  Please try again.  If this keeps failing, try logging into DIPS and check that your password hasn't expired.", "Log In Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            tryAgain = true;
                        }
                        else if (!swrSuccess)
                        {
                            MessageBox.Show("Your SWR password did not work.  Please try again.  If this keeps failing, try logging into DIPS and check that your password hasn't expired.", "Log In Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                                    if (!Directory.Exists(_settingsFolder))
                                        Directory.CreateDirectory(_settingsFolder);

                                    var credentialsFile = JsonSerializer.Serialize(Credentials);
                                    File.WriteAllText(_settingsPath, credentialsFile);
                                }
                                catch (SystemException)
                                {
                                    MessageBox.Show("There was a problem saving your credentials.  You will have to try again next time.");
                                }
                            }
                        }
                    }
                } while (tryAgain);
            }
            finally
            {
                Application.Cursor = XlMousePointer.xlDefault;
            }
        }

        public void SetupBook()
        {
            var lastSheetToRemove = "";

            foreach (Worksheet sheet in Application.ActiveWorkbook.Worksheets)
            {
                if (Application.ActiveWorkbook.Worksheets.Count == 1)
                    lastSheetToRemove = sheet.Name;
                else
                    sheet.Delete();
            }

            Worksheet previousSheet = null;

            for (var i = DateTime.Today; i < DateTime.Today.AddMonths(1); i = i.AddDays(1))
            {
                Worksheet newSheet;

                if (previousSheet == null)
                    newSheet = Application.ActiveWorkbook.Worksheets.Add();
                else
                    newSheet = Application.ActiveWorkbook.Worksheets.Add(After: previousSheet);

                SetupDaySheet(i, newSheet);

                previousSheet = newSheet;
            }

            Worksheet eventsCache = Application.ActiveWorkbook.Worksheets.Add(After: previousSheet);
            eventsCache.Name = EventCacheSheetName;
            previousSheet = eventsCache;

            Worksheet peopleCache = Application.ActiveWorkbook.Worksheets.Add(After: previousSheet);
            peopleCache.Name = PeopleCacheSheetName;

            Application.ActiveWorkbook.Worksheets[lastSheetToRemove].Delete();
        }

        private void SetupDaySheet(DateTime date, Worksheet worksheet)
        {
            worksheet.Name = date.ToString(SheetDateFormat);

            var titleRange = worksheet.Range["A1:G1"];
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
            worksheet.Columns[6].ColumnWidth = 9.22;
            worksheet.Columns[7].ColumnWidth = 9.22;

            worksheet.Range["A3"].Value = "Hub";
            worksheet.Range["B3"].Value = "DIPS Reference";
            worksheet.Range["C3"].Value = "Vehicle";
            worksheet.Range["D3"].Value = "Driver";
            worksheet.Range["E3"].Value = "Attendant";
            worksheet.Range["F3"].Value = "Start Time";
            worksheet.Range["G3"].Value = "End Time";
            worksheet.Range["A3:G3"].Font.Bold = true;
            worksheet.Range["A3:G3"].Borders.LineStyle = XlLineStyle.xlContinuous;
            worksheet.Range["A3:G3"].Borders.Weight = 2;
            worksheet.Range["A3:G3"].Interior.Color = ColorTranslator.ToOle(Color.FromArgb(198, 224, 180));

            var startRow = 4;

            foreach (var details in HubDetails.GetDefaultSettings())
            {
                for (var i = 0; i < details.VehicleCount; i++)
                {
                    worksheet.Range[$"A{startRow}"].Value = details.DisplayName;
                    worksheet.Range[$"A{startRow}"].Font.Bold = true;
                    worksheet.Range[$"A{startRow}:G{startRow}"].Borders.LineStyle = XlLineStyle.xlContinuous;
                    worksheet.Range[$"A{startRow}:G{startRow}"].Borders.Weight = 2;
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
            _settingsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DIPSCrewPlanner");
            _settingsPath = Path.Combine(_settingsFolder, "credentials.json");

            if (File.Exists(_settingsPath))
            {
                try
                {
                    var creds = File.ReadAllText(_settingsPath);
                    Credentials = JsonSerializer.Deserialize<Credentials>(creds);
                }
                catch (SystemException)
                {
                    MessageBox.Show("There was a problem loading your DIPS credentials.  You will need to re-enter them.", "Error Loading Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += new System.EventHandler(ThisAddIn_Startup);
            Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion VSTO generated code
    }
}