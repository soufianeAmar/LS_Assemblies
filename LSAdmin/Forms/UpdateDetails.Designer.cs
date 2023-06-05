namespace LSAdmin
{
    partial class UpdateDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateDetails));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.bFermer = new DevExpress.XtraEditors.SimpleButton();
            this.cbShowUpdateDetails = new DevExpress.XtraEditors.CheckEdit();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbShowUpdateDetails.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            resources.ApplyResources(this.panelControl1, "panelControl1");
            this.panelControl1.Controls.Add(this.bFermer);
            this.panelControl1.Controls.Add(this.cbShowUpdateDetails);
            this.panelControl1.Name = "panelControl1";
            // 
            // bFermer
            // 
            resources.ApplyResources(this.bFermer, "bFermer");
            this.bFermer.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bFermer.Name = "bFermer";
            // 
            // cbShowUpdateDetails
            // 
            resources.ApplyResources(this.cbShowUpdateDetails, "cbShowUpdateDetails");
            this.cbShowUpdateDetails.Name = "cbShowUpdateDetails";
            this.cbShowUpdateDetails.Properties.AccessibleDescription = resources.GetString("cbShowUpdateDetails.Properties.AccessibleDescription");
            this.cbShowUpdateDetails.Properties.AccessibleName = resources.GetString("cbShowUpdateDetails.Properties.AccessibleName");
            this.cbShowUpdateDetails.Properties.AutoHeight = ((bool)(resources.GetObject("cbShowUpdateDetails.Properties.AutoHeight")));
            this.cbShowUpdateDetails.Properties.Caption = resources.GetString("cbShowUpdateDetails.Properties.Caption");
            this.cbShowUpdateDetails.Properties.DisplayValueChecked = resources.GetString("cbShowUpdateDetails.Properties.DisplayValueChecked");
            this.cbShowUpdateDetails.Properties.DisplayValueGrayed = resources.GetString("cbShowUpdateDetails.Properties.DisplayValueGrayed");
            this.cbShowUpdateDetails.Properties.DisplayValueUnchecked = resources.GetString("cbShowUpdateDetails.Properties.DisplayValueUnchecked");
            // 
            // webBrowser1
            // 
            resources.ApplyResources(this.webBrowser1, "webBrowser1");
            this.webBrowser1.Name = "webBrowser1";
            // 
            // UpdateDetails
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "UpdateDetails";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbShowUpdateDetails.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton bFermer;
        private DevExpress.XtraEditors.CheckEdit cbShowUpdateDetails;
        private System.Windows.Forms.WebBrowser webBrowser1;

    }
}