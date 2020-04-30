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
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
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
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton SetupSheetButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton SetupCredentialsButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton UpdateVolunteerListButton;
    }

    partial class ThisRibbonCollection
    {
        internal CrewPlannerRibbon CrewPlannerRibbon
        {
            get { return this.GetRibbon<CrewPlannerRibbon>(); }
        }
    }
}
