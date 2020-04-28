using DIPSCrewPlanner.Model;
using Microsoft.Office.Interop.Excel;
using System;
using System.Drawing;

namespace DIPSCrewPlanner
{
    public partial class ThisAddIn
    {
        private const string EventCacheSheetName = "EventCache";
        private const string PeopleCacheSheetName = "PeopleCache";
        private const string SheetDateFormat = "dddd ddMMyy";

        public void SetDipsCredentials()
        {
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
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion VSTO generated code
    }
}