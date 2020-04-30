namespace DIPSCrewPlanner
{
    partial class DipsCredentialsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SwrUserNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SwrPasswordTextBox = new System.Windows.Forms.TextBox();
            this.WmrPasswordTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.WmrUserNameTextBox = new System.Windows.Forms.TextBox();
            this.SWRPasswordValidationLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.OkayButton = new System.Windows.Forms.Button();
            this.CancelFormButton = new System.Windows.Forms.Button();
            this.SWRUserNameValidationLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.WMRPasswordValidationLabel = new System.Windows.Forms.Label();
            this.WMRUserNameValidationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "SWR DIPs User Name";
            // 
            // SwrUserNameTextBox
            // 
            this.SwrUserNameTextBox.Location = new System.Drawing.Point(11, 54);
            this.SwrUserNameTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SwrUserNameTextBox.Name = "SwrUserNameTextBox";
            this.SwrUserNameTextBox.Size = new System.Drawing.Size(294, 20);
            this.SwrUserNameTextBox.TabIndex = 2;
            this.SwrUserNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateSWRUserName);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 88);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SWR DIPs Password";
            // 
            // SwrPasswordTextBox
            // 
            this.SwrPasswordTextBox.Location = new System.Drawing.Point(11, 104);
            this.SwrPasswordTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SwrPasswordTextBox.Name = "SwrPasswordTextBox";
            this.SwrPasswordTextBox.Size = new System.Drawing.Size(294, 20);
            this.SwrPasswordTextBox.TabIndex = 4;
            this.SwrPasswordTextBox.UseSystemPasswordChar = true;
            this.SwrPasswordTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateSWRPassword);
            // 
            // WmrPasswordTextBox
            // 
            this.WmrPasswordTextBox.Location = new System.Drawing.Point(11, 205);
            this.WmrPasswordTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.WmrPasswordTextBox.Name = "WmrPasswordTextBox";
            this.WmrPasswordTextBox.Size = new System.Drawing.Size(294, 20);
            this.WmrPasswordTextBox.TabIndex = 8;
            this.WmrPasswordTextBox.UseSystemPasswordChar = true;
            this.WmrPasswordTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateWMRPassword);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 188);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "WMR DIPs Password";
            // 
            // WmrUserNameTextBox
            // 
            this.WmrUserNameTextBox.Location = new System.Drawing.Point(11, 154);
            this.WmrUserNameTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.WmrUserNameTextBox.Name = "WmrUserNameTextBox";
            this.WmrUserNameTextBox.Size = new System.Drawing.Size(294, 20);
            this.WmrUserNameTextBox.TabIndex = 6;
            this.WmrUserNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ValidateWMRUserName);
            // 
            // SWRPasswordValidationLabel
            // 
            this.SWRPasswordValidationLabel.AutoSize = true;
            this.SWRPasswordValidationLabel.ForeColor = System.Drawing.Color.Red;
            this.SWRPasswordValidationLabel.Location = new System.Drawing.Point(10, 124);
            this.SWRPasswordValidationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SWRPasswordValidationLabel.Name = "SWRPasswordValidationLabel";
            this.SWRPasswordValidationLabel.Size = new System.Drawing.Size(192, 13);
            this.SWRPasswordValidationLabel.TabIndex = 5;
            this.SWRPasswordValidationLabel.Text = "You need to enter your SWR password";
            this.SWRPasswordValidationLabel.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 7);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(211, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "You need to provide your DIPS credentials.";
            // 
            // OkayButton
            // 
            this.OkayButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkayButton.Location = new System.Drawing.Point(188, 239);
            this.OkayButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OkayButton.Name = "OkayButton";
            this.OkayButton.Size = new System.Drawing.Size(56, 21);
            this.OkayButton.TabIndex = 9;
            this.OkayButton.Text = "Okay";
            this.OkayButton.UseVisualStyleBackColor = true;
            this.OkayButton.Click += new System.EventHandler(this.OkayButton_Click);
            // 
            // CancelFormButton
            // 
            this.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelFormButton.Location = new System.Drawing.Point(248, 239);
            this.CancelFormButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CancelFormButton.Name = "CancelFormButton";
            this.CancelFormButton.Size = new System.Drawing.Size(56, 21);
            this.CancelFormButton.TabIndex = 10;
            this.CancelFormButton.Text = "Cancel";
            this.CancelFormButton.UseVisualStyleBackColor = true;
            this.CancelFormButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SWRUserNameValidationLabel
            // 
            this.SWRUserNameValidationLabel.AutoSize = true;
            this.SWRUserNameValidationLabel.ForeColor = System.Drawing.Color.Red;
            this.SWRUserNameValidationLabel.Location = new System.Drawing.Point(10, 74);
            this.SWRUserNameValidationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SWRUserNameValidationLabel.Name = "SWRUserNameValidationLabel";
            this.SWRUserNameValidationLabel.Size = new System.Drawing.Size(196, 13);
            this.SWRUserNameValidationLabel.TabIndex = 11;
            this.SWRUserNameValidationLabel.Text = "You need to enter your SWR user name";
            this.SWRUserNameValidationLabel.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 138);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "WMR DIPs User Name";
            // 
            // WMRPasswordValidationLabel
            // 
            this.WMRPasswordValidationLabel.AutoSize = true;
            this.WMRPasswordValidationLabel.ForeColor = System.Drawing.Color.Red;
            this.WMRPasswordValidationLabel.Location = new System.Drawing.Point(10, 225);
            this.WMRPasswordValidationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.WMRPasswordValidationLabel.Name = "WMRPasswordValidationLabel";
            this.WMRPasswordValidationLabel.Size = new System.Drawing.Size(194, 13);
            this.WMRPasswordValidationLabel.TabIndex = 13;
            this.WMRPasswordValidationLabel.Text = "You need to enter your WMR password";
            this.WMRPasswordValidationLabel.Visible = false;
            // 
            // WMRUserNameValidationLabel
            // 
            this.WMRUserNameValidationLabel.AutoSize = true;
            this.WMRUserNameValidationLabel.ForeColor = System.Drawing.Color.Red;
            this.WMRUserNameValidationLabel.Location = new System.Drawing.Point(10, 175);
            this.WMRUserNameValidationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.WMRUserNameValidationLabel.Name = "WMRUserNameValidationLabel";
            this.WMRUserNameValidationLabel.Size = new System.Drawing.Size(198, 13);
            this.WMRUserNameValidationLabel.TabIndex = 14;
            this.WMRUserNameValidationLabel.Text = "You need to enter your WMR user name";
            this.WMRUserNameValidationLabel.Visible = false;
            // 
            // DipsCredentialsForm
            // 
            this.AcceptButton = this.OkayButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 272);
            this.ControlBox = false;
            this.Controls.Add(this.WMRUserNameValidationLabel);
            this.Controls.Add(this.WMRPasswordValidationLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.SWRUserNameValidationLabel);
            this.Controls.Add(this.CancelFormButton);
            this.Controls.Add(this.OkayButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.WmrPasswordTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.WmrUserNameTextBox);
            this.Controls.Add(this.SWRPasswordValidationLabel);
            this.Controls.Add(this.SwrPasswordTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SwrUserNameTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DipsCredentialsForm";
            this.Text = "DipsCredentialsForm";
            this.Load += new System.EventHandler(this.DipsCredentialsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SwrUserNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SwrPasswordTextBox;
        private System.Windows.Forms.TextBox WmrPasswordTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox WmrUserNameTextBox;
        private System.Windows.Forms.Label SWRPasswordValidationLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button OkayButton;
        private System.Windows.Forms.Button CancelFormButton;
        private System.Windows.Forms.Label SWRUserNameValidationLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label WMRPasswordValidationLabel;
        private System.Windows.Forms.Label WMRUserNameValidationLabel;
    }
}