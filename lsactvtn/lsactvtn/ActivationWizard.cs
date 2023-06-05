using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;

namespace lsactvtn
{
    public partial class ActivationWizard : Form
    {
        bool AllowNext;
        string contactDetail;
        public ActivationWizard()
        {
            InitializeComponent();
            AllowNext = true;
            contactDetail = string.Empty;
        }

        private void SendEmail(string sender, string recipient, string subject, string messageBody)
        {
            SmtpClient smtpClient = new SmtpClient("mail.leadersoft.dz", 587) { Credentials = new NetworkCredential("no-reply@leadersoft.dz", "HIbMr~05+],z") };
            smtpClient.Send(sender, recipient, subject, messageBody);
        }
        private void wizardControl1_NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            AllowNext = true;
            if (wizardControl1.SelectedPage == SNwizardPage1)
            {
                if (ActivationClass.ta.CheckAndSavePKey(SNEdit.Text))
                {
                    try
                    {
                        ActivationClass.ta.Activate(Dns.GetHostName());
                        ActivationClass.GetVersion();
                        contactDetail += string.Format("Licence = {0}\n", SNEdit.Text);
                        try
                        {
                            SendEmail("contact@leadersoft.dz", "ic.activation@leadersoft.dz", "IC activation", contactDetail);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Erreur d'envoi de Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        AllowNext = false;
                        MessageBox.Show(ex.Message, "Erreur d'activation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez spécifier un numéro de série valide pour ce produit!", "Clé invalide", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AllowNext = false;
                }
            }
            else
                if (wizardControl1.SelectedPage == wpContact)
                {
                    if (!string.IsNullOrEmpty(teNom.Text))
                        contactDetail += string.Format("Nom = {0}\n", teNom.Text);
                    else
                        AllowNext = false;
                    if (!string.IsNullOrEmpty(tePrénom.Text))
                        contactDetail += string.Format("Prénom = {0}\n", tePrénom.Text);
                    else
                        AllowNext = false;
                    if (!string.IsNullOrEmpty(teSociété.Text))
                        contactDetail += string.Format("Société = {0}\n", teSociété.Text);
                    else
                        AllowNext = false;
                    if (!string.IsNullOrEmpty(teTéléphone.Text))
                        contactDetail += string.Format("Téléphone = {0}\n", teTéléphone.Text);
                    else
                        AllowNext = false;
                    if (!string.IsNullOrEmpty(teEMail.Text))
                        contactDetail += string.Format("e-Mail = {0}\n", teEMail.Text);
                    else
                        AllowNext = false;
                }
        }

        private void wizardControl1_SelectedPageChanging(object sender, DevExpress.XtraWizard.WizardPageChangingEventArgs e)
        {
            e.Cancel = !AllowNext;
        }
    }
}
