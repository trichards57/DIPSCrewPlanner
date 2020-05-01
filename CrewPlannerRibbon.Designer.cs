namespace DIPSCrewPlanner
{
    partial class CrewPlannerRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public CrewPlannerRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.SetupSheetButton = this.Factory.CreateRibbonButton();
            this.SetupCredentialsButton = this.Factory.CreateRibbonButton();
            this.UpdateVolunteerListButton = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.GetDipsIDsButton = this.Factory.CreateRibbonButton();
            this.UpdateDIPSButton = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Label = "DIPS Crew Planner";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.SetupSheetButton);
            this.group1.Items.Add(this.SetupCredentialsButton);
            this.group1.Items.Add(this.UpdateVolunteerListButton);
            this.group1.Label = "Setup";
            this.group1.Name = "group1";
            // 
            // SetupSheetButton
            // 
            this.SetupSheetButton.Label = "Setup Sheet";
            this.SetupSheetButton.Name = "SetupSheetButton";
            this.SetupSheetButton.OfficeImageId = "AccessFormWizard";
            this.SetupSheetButton.ShowImage = true;
            this.SetupSheetButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SetupSheetButton_Click);
            // 
            // SetupCredentialsButton
            // 
            this.SetupCredentialsButton.Label = "Setup Credentials";
            this.SetupCredentialsButton.Name = "SetupCredentialsButton";
            this.SetupCredentialsButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SetupCredentialsButton_Click);
            // 
            // UpdateVolunteerListButton
            // 
            this.UpdateVolunteerListButton.Enabled = false;
            this.UpdateVolunteerListButton.Label = "Update Volunteer List";
            this.UpdateVolunteerListButton.Name = "UpdateVolunteerListButton";
            this.UpdateVolunteerListButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.UpdateVolunteerListButton_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.GetDipsIDsButton);
            this.group2.Items.Add(this.UpdateDIPSButton);
            this.group2.Label = "Events";
            this.group2.Name = "group2";
            // 
            // GetDipsIDsButton
            // 
            this.GetDipsIDsButton.Label = "Get DIPS IDs";
            this.GetDipsIDsButton.Name = "GetDipsIDsButton";
            this.GetDipsIDsButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.GetDipsIDsButton_Click);
            // 
            // UpdateDIPSButton
            // 
            this.UpdateDIPSButton.Label = "Update DIPS";
            this.UpdateDIPSButton.Name = "UpdateDIPSButton";
            this.UpdateDIPSButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.UpdateDIPSButton_Click);
            // 
            // CrewPlannerRibbon
            // 
            this.Name = "CrewPlannerRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.CrewPlannerRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton SetupSheetButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton SetupCredentialsButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton UpdateVolunteerListButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton GetDipsIDsButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton UpdateDIPSButton;
    }

    partial class ThisRibbonCollection
    {
        internal CrewPlannerRibbon CrewPlannerRibbon
        {
            get { return this.GetRibbon<CrewPlannerRibbon>(); }
        }
    }
}
