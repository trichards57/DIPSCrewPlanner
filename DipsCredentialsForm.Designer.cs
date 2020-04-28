﻿namespace DIPSCrewPlanner
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
            this.SwrPasswordNameTextBox = new System.Windows.Forms.TextBox();
            this.WmrPasswordTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.WmrUserNameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.OkayButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "SWR DIPs User Name";
            // 
            // SwrUserNameTextBox
            // 
            this.SwrUserNameTextBox.Location = new System.Drawing.Point(15, 66);
            this.SwrUserNameTextBox.Name = "SwrUserNameTextBox";
            this.SwrUserNameTextBox.Size = new System.Drawing.Size(391, 22);
            this.SwrUserNameTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "SWR DIPs Password";
            // 
            // SwrPasswordNameTextBox
            // 
            this.SwrPasswordNameTextBox.Location = new System.Drawing.Point(15, 111);
            this.SwrPasswordNameTextBox.Name = "SwrPasswordNameTextBox";
            this.SwrPasswordNameTextBox.Size = new System.Drawing.Size(391, 22);
            this.SwrPasswordNameTextBox.TabIndex = 4;
            // 
            // WmrPasswordTextBox
            // 
            this.WmrPasswordTextBox.Location = new System.Drawing.Point(15, 201);
            this.WmrPasswordTextBox.Name = "WmrPasswordTextBox";
            this.WmrPasswordTextBox.Size = new System.Drawing.Size(391, 22);
            this.WmrPasswordTextBox.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "WMR DIPs Password";
            // 
            // WmrUserNameTextBox
            // 
            this.WmrUserNameTextBox.Location = new System.Drawing.Point(15, 156);
            this.WmrUserNameTextBox.Name = "WmrUserNameTextBox";
            this.WmrUserNameTextBox.Size = new System.Drawing.Size(391, 22);
            this.WmrUserNameTextBox.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "WMR DIPs User Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(280, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "You need to provide your DIPS credentials.";
            // 
            // OkayButton
            // 
            this.OkayButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkayButton.Location = new System.Drawing.Point(250, 231);
            this.OkayButton.Name = "OkayButton";
            this.OkayButton.Size = new System.Drawing.Size(75, 26);
            this.OkayButton.TabIndex = 9;
            this.OkayButton.Text = "Okay";
            this.OkayButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(331, 231);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 26);
            this.CancelButton.TabIndex = 10;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // DipsCredentialsForm
            // 
            this.AcceptButton = this.OkayButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton;
            this.ClientSize = new System.Drawing.Size(418, 269);
            this.ControlBox = false;
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkayButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.WmrPasswordTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.WmrUserNameTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SwrPasswordNameTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SwrUserNameTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DipsCredentialsForm";
            this.Text = "DipsCredentialsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SwrUserNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SwrPasswordNameTextBox;
        private System.Windows.Forms.TextBox WmrPasswordTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox WmrUserNameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button OkayButton;
        private System.Windows.Forms.Button CancelButton;
    }
}