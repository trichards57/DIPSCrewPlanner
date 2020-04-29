using DIPSCrewPlanner.DIPS;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DIPSCrewPlanner
{
    public partial class DipsCredentialsForm : Form
    {
        private readonly Regex _userNameRegex = new Regex("[A-Za-z0-9.]+@[A-Za-z]{3}", RegexOptions.Compiled);

        public DipsCredentialsForm()
        {
            InitializeComponent();
        }

        public Credentials Credentials { get; set; }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DipsCredentialsForm_Load(object sender, EventArgs e)
        {
            if (Credentials != null)
            {
                SwrPasswordTextBox.Text = Credentials.SwrDipsPassword;
                SwrUserNameTextBox.Text = Credentials.SwrDipsUsername;
                WmrPasswordTextBox.Text = Credentials.WmrDipsPassword;
                WmrUserNameTextBox.Text = Credentials.WmrDipsUsername;
            }
        }

        private void OkayButton_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            Credentials = new Credentials
            {
                SwrDipsPassword = SwrPasswordTextBox.Text,
                SwrDipsUsername = SwrUserNameTextBox.Text,
                WmrDipsPassword = WmrPasswordTextBox.Text,
                WmrDipsUsername = WmrUserNameTextBox.Text
            };

            Close();
        }

        private void ValidateSWRPassword(object sender, CancelEventArgs e)
        {
            var box = sender as TextBox;

            box.Text = box.Text.Trim();

            if (string.IsNullOrWhiteSpace(box.Text))
            {
                SWRPasswordValidationLabel.Visible = true;
            }
            else
            {
                SWRPasswordValidationLabel.Visible = false;
            }

            OkayButton.Enabled = !(SWRPasswordValidationLabel.Visible
                || SWRUserNameValidationLabel.Visible
                || WMRPasswordValidationLabel.Visible
                || WMRUserNameValidationLabel.Visible);
        }

        private void ValidateSWRUserName(object sender, CancelEventArgs e)
        {
            var box = sender as TextBox;
            box.Text = box.Text.Trim().ToLowerInvariant();

            if (!_userNameRegex.IsMatch(box.Text))
            {
                SWRUserNameValidationLabel.Visible = true;
            }
            else
            {
                SWRUserNameValidationLabel.Visible = false;
            }

            OkayButton.Enabled = !(SWRPasswordValidationLabel.Visible
                || SWRUserNameValidationLabel.Visible
                || WMRPasswordValidationLabel.Visible
                || WMRUserNameValidationLabel.Visible);
        }

        private void ValidateWMRPassword(object sender, CancelEventArgs e)
        {
            var box = sender as TextBox;

            box.Text = box.Text.Trim();

            if (string.IsNullOrWhiteSpace(box.Text))
            {
                WMRPasswordValidationLabel.Visible = true;
            }
            else
            {
                WMRPasswordValidationLabel.Visible = false;
            }

            OkayButton.Enabled = !(SWRPasswordValidationLabel.Visible
                || SWRUserNameValidationLabel.Visible
                || WMRPasswordValidationLabel.Visible
                || WMRUserNameValidationLabel.Visible);
        }

        private void ValidateWMRUserName(object sender, CancelEventArgs e)
        {
            var box = sender as TextBox;
            box.Text = box.Text.Trim().ToLowerInvariant();

            if (!_userNameRegex.IsMatch(box.Text))
            {
                WMRUserNameValidationLabel.Visible = true;
            }
            else
            {
                WMRUserNameValidationLabel.Visible = false;
            }

            OkayButton.Enabled = !(SWRPasswordValidationLabel.Visible
                || SWRUserNameValidationLabel.Visible
                || WMRPasswordValidationLabel.Visible
                || WMRUserNameValidationLabel.Visible);
        }
    }
}