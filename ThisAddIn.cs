﻿using DIPSCrewPlanner.DIPS;
using DIPSCrewPlanner.Model;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace DIPSCrewPlanner
{
    public partial class ThisAddIn
    {
        private const string EventCacheSheetName = "EventCache";
        private const string PeopleArea = "PeopleArea";
        private const string PeopleCacheSheetName = "PeopleCache";
        private const string PeopleNamesArea = "PeopleNames";
        private const string SheetDateFormat = "dddd ddMMyy";
        private Credentials _credentials;
        private string _settingsFolder;
        private string _settingsPath;

        public Credentials Credentials
        {
            get { return _credentials; }
            private set
            {
                _credentials = value;
                if (value != null)
                    Globals.Ribbons.CrewPlannerRibbon.EnableControls();
                else
                    Globals.Ribbons.CrewPlannerRibbon.DisableControls();
            }
        }

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

            Worksheet eventsCache = Application.ActiveWorkbook.Worksheets.Add();
            eventsCache.Name = EventCacheSheetName;

            Worksheet peopleCache = Application.ActiveWorkbook.Worksheets.Add(After: eventsCache);
            peopleCache.Name = PeopleCacheSheetName;

            Application.Names.Add(PeopleNamesArea, peopleCache.Range["A1"]);
            Application.Names.Add(PeopleArea, peopleCache.Range["A1:F1"]);

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

            Application.ActiveWorkbook.Worksheets[lastSheetToRemove].Delete();
        }

        public async void UpdatePeopleList()
        {
            try
            {
                Application.Cursor = XlMousePointer.xlWait;
                var client = new DipsClient(Credentials);
                await client.Login(true);
                var swrResult = await client.GetEMTs();
                await client.Login(false);
                var wmrResult = await client.GetEMTs();

                var people = Enumerable.Concat(swrResult, wmrResult);

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
                }

                Application.Names.Add(PeopleNamesArea, cacheSheet.Range[$"A1:A{currentRow}"]);
                Application.Names.Add(PeopleArea, cacheSheet.Range[$"A1:I{currentRow}"]);
            }
            finally
            {
                Application.Cursor = XlMousePointer.xlDefault;
            }
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
            _settingsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DIPSCrewPlanner");
            _settingsPath = Path.Combine(_settingsFolder, "credentials.json");

            Credentials = null;

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
