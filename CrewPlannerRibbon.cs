using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;

namespace DIPSCrewPlanner
{
    public partial class CrewPlannerRibbon
    {
        private void CrewPlannerRibbon_Load(object sender, RibbonUIEventArgs e)
        {
        }

        private void SetupSheetButton_Click(object sender, RibbonControlEventArgs e)
        {
            var result = MessageBox.Show("This will set up a new crew planner sheet.  It will result in you losing any data currently in this workbook.  It would be wise to close any other workbooks.  Are you sure you want to continue?", "Warning: Data Loss", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;

            Globals.ThisAddIn.SetupBook();
        }
    }
}