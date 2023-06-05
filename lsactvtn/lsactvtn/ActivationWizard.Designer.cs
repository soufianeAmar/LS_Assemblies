namespace lsactvtn
{
    partial class ActivationWizard
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
            this.wizardControl1 = new DevExpress.XtraWizard.WizardControl();
            this.welcomeWizardPage1 = new DevExpress.XtraWizard.WelcomeWizardPage();
            this.SNwizardPage1 = new DevExpress.XtraWizard.WizardPage();
            this.SNEdit = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.completionWizardPage1 = new DevExpress.XtraWizard.CompletionWizardPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.wpContact = new DevExpress.XtraWizard.WizardPage();
            this.teNom = new DevExpress.XtraEditors.TextEdit();
            this.tePrénom = new DevExpress.XtraEditors.TextEdit();
            this.teSociété = new DevExpress.XtraEditors.TextEdit();
            this.teTéléphone = new DevExpress.XtraEditors.TextEdit();
            this.teEMail = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).BeginInit();
            this.wizardControl1.SuspendLayout();
            this.SNwizardPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SNEdit.Properties)).BeginInit();
            this.completionWizardPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.wpContact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teNom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePrénom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSociété.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teTéléphone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEMail.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // wizardControl1
            // 
            this.wizardControl1.CancelText = "Annuler";
            this.wizardControl1.Controls.Add(this.welcomeWizardPage1);
            this.wizardControl1.Controls.Add(this.SNwizardPage1);
            this.wizardControl1.Controls.Add(this.completionWizardPage1);
            this.wizardControl1.Controls.Add(this.wpContact);
            this.wizardControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardControl1.FinishText = "&Terminer";
            this.wizardControl1.HelpText = "&Aide";
            this.wizardControl1.Location = new System.Drawing.Point(0, 0);
            this.wizardControl1.Name = "wizardControl1";
            this.wizardControl1.NextText = "&Suivant>";
            this.wizardControl1.Pages.AddRange(new DevExpress.XtraWizard.BaseWizardPage[] {
            this.welcomeWizardPage1,
            this.wpContact,
            this.SNwizardPage1,
            this.completionWizardPage1});
            this.wizardControl1.PreviousText = "< &Précédent";
            this.wizardControl1.Size = new System.Drawing.Size(538, 371);
            this.wizardControl1.SelectedPageChanging += new DevExpress.XtraWizard.WizardPageChangingEventHandler(this.wizardControl1_SelectedPageChanging);
            this.wizardControl1.NextClick += new DevExpress.XtraWizard.WizardCommandButtonClickEventHandler(this.wizardControl1_NextClick);
            // 
            // welcomeWizardPage1
            // 
            this.welcomeWizardPage1.IntroductionText = "Cet assistant vous guidera à travers les différentes étapes nécessaires pour l\'ac" +
    "tivation de votre copie du logiciel Leadersoft™";
            this.welcomeWizardPage1.Name = "welcomeWizardPage1";
            this.welcomeWizardPage1.ProceedText = "Cliquez sur Suivant pour continuer";
            this.welcomeWizardPage1.Size = new System.Drawing.Size(321, 192);
            this.welcomeWizardPage1.Text = "Bienvenue dans l\'assistant d\'activation des logiciels Leadersoft™";
            // 
            // SNwizardPage1
            // 
            this.SNwizardPage1.Controls.Add(this.SNEdit);
            this.SNwizardPage1.Controls.Add(this.label4);
            this.SNwizardPage1.Controls.Add(this.label3);
            this.SNwizardPage1.Controls.Add(this.label2);
            this.SNwizardPage1.Controls.Add(this.label1);
            this.SNwizardPage1.DescriptionText = "Le numéro de série se trouve à l\'intérieur du boitier du CD d\'installation ou dan" +
    "s votre boite e-Mail";
            this.SNwizardPage1.Name = "SNwizardPage1";
            this.SNwizardPage1.Size = new System.Drawing.Size(506, 226);
            this.SNwizardPage1.Text = "Saisissez votre numéro de série";
            // 
            // SNEdit
            // 
            this.SNEdit.Location = new System.Drawing.Point(116, 135);
            this.SNEdit.Name = "SNEdit";
            this.SNEdit.Properties.Mask.EditMask = "([0-9A-Z]{4}-){6}[0-9A-Z]{4}";
            this.SNEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.SNEdit.Size = new System.Drawing.Size(299, 20);
            this.SNEdit.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Numéro de série";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(44, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(371, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "Numéro de série : XXXX-XXXX-XXXX-XXXX-XXXX-XXXX-XXXX";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Le numéro de série se présente sous la forme :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(280, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "L\'activation affectera le numéro de série à cet ordinateur";
            // 
            // completionWizardPage1
            // 
            this.completionWizardPage1.Controls.Add(this.pictureBox1);
            this.completionWizardPage1.FinishText = "Votre copie du logiciel Leadersoft™ a été activée avec succés";
            this.completionWizardPage1.Name = "completionWizardPage1";
            this.completionWizardPage1.ProceedText = "Cliquez sur Terminer pour fermer l\'assistant d\'activation";
            this.completionWizardPage1.Size = new System.Drawing.Size(321, 238);
            this.completionWizardPage1.Text = "Activation términée";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::lsactvtn.Properties.Resources.validate;
            this.pictureBox1.Location = new System.Drawing.Point(63, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(169, 190);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // wpContact
            // 
            this.wpContact.Controls.Add(this.label9);
            this.wpContact.Controls.Add(this.label8);
            this.wpContact.Controls.Add(this.label7);
            this.wpContact.Controls.Add(this.label6);
            this.wpContact.Controls.Add(this.label5);
            this.wpContact.Controls.Add(this.teEMail);
            this.wpContact.Controls.Add(this.teTéléphone);
            this.wpContact.Controls.Add(this.teSociété);
            this.wpContact.Controls.Add(this.tePrénom);
            this.wpContact.Controls.Add(this.teNom);
            this.wpContact.DescriptionText = "Veuillez saisir vos coordonnées de contact";
            this.wpContact.Name = "wpContact";
            this.wpContact.Size = new System.Drawing.Size(506, 226);
            this.wpContact.Text = "Coordonnées de contact";
            // 
            // teNom
            // 
            this.teNom.Location = new System.Drawing.Point(98, 23);
            this.teNom.Name = "teNom";
            this.teNom.Size = new System.Drawing.Size(149, 20);
            this.teNom.TabIndex = 0;
            // 
            // tePrénom
            // 
            this.tePrénom.Location = new System.Drawing.Point(98, 59);
            this.tePrénom.Name = "tePrénom";
            this.tePrénom.Size = new System.Drawing.Size(149, 20);
            this.tePrénom.TabIndex = 1;
            // 
            // teSociété
            // 
            this.teSociété.Location = new System.Drawing.Point(98, 97);
            this.teSociété.Name = "teSociété";
            this.teSociété.Size = new System.Drawing.Size(304, 20);
            this.teSociété.TabIndex = 2;
            // 
            // teTéléphone
            // 
            this.teTéléphone.Location = new System.Drawing.Point(98, 133);
            this.teTéléphone.Name = "teTéléphone";
            this.teTéléphone.Size = new System.Drawing.Size(122, 20);
            this.teTéléphone.TabIndex = 3;
            // 
            // teEMail
            // 
            this.teEMail.Location = new System.Drawing.Point(98, 167);
            this.teEMail.Name = "teEMail";
            this.teEMail.Size = new System.Drawing.Size(149, 20);
            this.teEMail.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Nom";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Prénom";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Société";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 136);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Téléphone";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(30, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "e-Mail";
            // 
            // ActivationWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 371);
            this.Controls.Add(this.wizardControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ActivationWizard";
            this.Text = "Assistant d\'activation Leadersoft™";
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).EndInit();
            this.wizardControl1.ResumeLayout(false);
            this.SNwizardPage1.ResumeLayout(false);
            this.SNwizardPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SNEdit.Properties)).EndInit();
            this.completionWizardPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.wpContact.ResumeLayout(false);
            this.wpContact.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teNom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tePrénom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teSociété.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teTéléphone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEMail.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraWizard.WizardControl wizardControl1;
        private DevExpress.XtraWizard.WelcomeWizardPage welcomeWizardPage1;
        private DevExpress.XtraWizard.WizardPage SNwizardPage1;
        private DevExpress.XtraWizard.CompletionWizardPage completionWizardPage1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit SNEdit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraWizard.WizardPage wpContact;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit teEMail;
        private DevExpress.XtraEditors.TextEdit teTéléphone;
        private DevExpress.XtraEditors.TextEdit teSociété;
        private DevExpress.XtraEditors.TextEdit tePrénom;
        private DevExpress.XtraEditors.TextEdit teNom;
    }
}