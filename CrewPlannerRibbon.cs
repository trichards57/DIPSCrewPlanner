using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;

namespace DIPSCrewPlanner
{
    public partial class CrewPlannerRibbon
    {
        private bool _hasCredentials = false;
        private bool _isOnline = false;

        public bool HasCredentials
        {
            get => _hasCredentials;
            set
            {
                _hasCredentials = value;
                UpdateControls();
            }
        }

        public bool IsOnline
        {
            get => _isOnline;
            set
            {
                _isOnline = value;
                UpdateControls();
            }
        }

        public void UpdateControls()
        {
            GetDipsIDsButton.Enabled = HasCredentials && IsOnline;
            UpdateDIPSButton.Enabled = HasCredentials && IsOnline;
            UpdateVolunteerListButton.Enabled = HasCredentials && IsOnline;
            SetupCredentialsButton.Enabled = IsOnline;

            if (!IsOnline)
            {
                SetupCredentialsButton.SuperTip = "You need to be online.";
                UpdateVolunteerListButton.SuperTip = "You need to be online.";
                GetDipsIDsButton.SuperTip = "You need to be online.";
                UpdateDIPSButton.SuperTip = "You need to be online.";
            }
            else if (!HasCredentials)
            {
                SetupCredentialsButton.SuperTip = string.Empty;
                UpdateVolunteerListButton.SuperTip = "You need to set your Credentials first.";
                GetDipsIDsButton.SuperTip = "You need to set your Credentials first.";
                UpdateDIPSButton.SuperTip = "You need to set your Credentials first.";
            }
            else
            {
                SetupCredentialsButton.SuperTip = string.Empty;
                UpdateVolunteerListButton.SuperTip = string.Empty;
                GetDipsIDsButton.SuperTip = string.Empty;
                UpdateDIPSButton.SuperTip = string.Empty;
            }
        }

        private void AboutButton_Click(object sender, RibbonControlEventArgs e)
        {
            var aboutForm = new AboutBox();
            aboutForm.ShowDialog();
        }

        private void CrewPlannerRibbon_Load(object sender, RibbonUIEventArgs e)
        {
        }

        private void GetDipsIDsButton_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.GetDipsIds();
        }

        private void SetupCredentialsButton_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.SetDipsCredentials();
        }

        private void SetupSheetButton_Click(object sender, RibbonControlEventArgs e)
        {
            var result = MessageBox.Show("This will set up a new crew planner sheet.  It will result in you losing any data currently in this workbook.  It would be wise to close any other workbooks.  Are you sure you want to continue?", "Warning: Data Loss", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
                return;

            Globals.ThisAddIn.SetupBook();
        }

        private void UpdateDIPSButton_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.UploadSheetToDips();
        }

        private void UpdateVolunteerListButton_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.UpdatePeopleList();
        }
    }
}