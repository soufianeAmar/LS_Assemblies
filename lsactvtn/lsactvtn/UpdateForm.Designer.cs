namespace lsactvtn
{
    partial class UpdateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
            this.automaticUpdater1 = new wyDay.Controls.AutomaticUpdater();
            this.lUpdate = new System.Windows.Forms.Label();
            this.bUpdate = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.automaticUpdater1)).BeginInit();
            this.SuspendLayout();
            // 
            // automaticUpdater1
            // 
            this.automaticUpdater1.Animate = false;
            this.automaticUpdater1.Arguments = "PendingUpdates";
            this.automaticUpdater1.ContainerForm = this;
            this.automaticUpdater1.GUID = "f8308cc2-34fd-4bbe-9bb6-af96b4620806";
            this.automaticUpdater1.Location = new System.Drawing.Point(12, 18);
            this.automaticUpdater1.Name = "automaticUpdater1";
            this.automaticUpdater1.Size = new System.Drawing.Size(301, 16);
            this.automaticUpdater1.TabIndex = 0;
            this.automaticUpdater1.UpdateType = wyDay.Controls.UpdateType.OnlyCheck;
            this.automaticUpdater1.wyUpdateCommandline = null;
            this.automaticUpdater1.CheckingFailed += new wyDay.Controls.FailHandler(this.automaticUpdater1_CheckingFailed);
            this.automaticUpdater1.UpdateAvailable += new System.EventHandler(this.automaticUpdater1_UpdateAvailable);
            this.automaticUpdater1.UpdateFailed += new wyDay.Controls.FailHandler(this.automaticUpdater1_UpdateFailed);
            this.automaticUpdater1.UpdateSuccessful += new wyDay.Controls.SuccessHandler(this.automaticUpdater1_UpdateSuccessful);
            this.automaticUpdater1.UpToDate += new wyDay.Controls.SuccessHandler(this.automaticUpdater1_UpToDate);
            // 
            // lUpdate
            // 
            this.lUpdate.Location = new System.Drawing.Point(7, 39);
            this.lUpdate.Name = "lUpdate";
            this.lUpdate.Size = new System.Drawing.Size(306, 64);
            this.lUpdate.TabIndex = 1;
            this.lUpdate.Text = "Vérification des Mises à jour. Veuillez patienter...";
            this.lUpdate.Visible = false;
            // 
            // bUpdate
            // 
            this.bUpdate.Enabled = false;
            this.bUpdate.Image = ((System.Drawing.Image)(resources.GetObject("bUpdate.Image")));
            this.bUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bUpdate.Location = new System.Drawing.Point(319, 12);
            this.bUpdate.Name = "bUpdate";
            this.bUpdate.Size = new System.Drawing.Size(112, 42);
            this.bUpdate.TabIndex = 2;
            this.bUpdate.Text = "Mise à jour";
            this.bUpdate.UseVisualStyleBackColor = true;
            this.bUpdate.Click += new System.EventHandler(this.bUpdate_Click);
            // 
            // bClose
            // 
            this.bClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bClose.Image = ((System.Drawing.Image)(resources.GetObject("bClose.Image")));
            this.bClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bClose.Location = new System.Drawing.Point(319, 60);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(112, 41);
            this.bClose.TabIndex = 3;
            this.bClose.Text = "Fermer";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // UpdateForm
            // 
            this.AcceptButton = this.bUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bClose;
            this.ClientSize = new System.Drawing.Size(448, 112);
            this.ControlBox = false;
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.bUpdate);
            this.Controls.Add(this.lUpdate);
            this.Controls.Add(this.automaticUpdater1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "UpdateForm";
            this.Text = "Mise à jour";
            this.Load += new System.EventHandler(this.UpdateForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.automaticUpdater1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private wyDay.Controls.AutomaticUpdater automaticUpdater1;
        private System.Windows.Forms.Label lUpdate;
        private System.Windows.Forms.Button bUpdate;
        private System.Windows.Forms.Button bClose;
    }
}