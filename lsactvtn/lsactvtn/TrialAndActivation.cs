using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lsactvtn
{
    public partial class TrialAndActivation : Form
    {
        public int TrialDaysRemaining = 0;
        public bool trial = true;
        public bool freePlan = false;
        public bool subscription = false;

        const string Trial = "Vous utilisez la version Démo du logiciel.\nIl vous reste {0} jours.";
        const string TrialExpired = "La version Démo a expiré.";
        const string LicenceWillExpire = "Votre licence expire dans {0} jour(s). Veuillez la renouveller !";
        const string LicenceExpired = "Votre licence a expiré. Veuillez la renouveller !";
        const string ContractWillExpire = "Votre contrat de support technique expire dans {0} jour(s). Veuillez le renouveller !";
        const string ContractExpired = "Votre contrat de support technique a expiré. Veuillez le renouveller !";

        public TrialAndActivation()
        {
            InitializeComponent();
        }

        public void SetTrialText()//int TrialDaysRemaining)
        {
            bActiver.Visible = trial || freePlan;
            if (trial)
            {
                lAcheter.Text = "Acheter";
                if (TrialDaysRemaining > 0)
                {
                    lDemo.Text = string.Format(Trial, TrialDaysRemaining);
                    bDemo.Text = "Continuer à utiliser la version Démo";
                    //bDemo.Enabled = true;
                }
                else
                {
                    lDemo.Text = TrialExpired;
                    bDemo.Text = "Extension de la version Démo";
                    //bDemo.Enabled = false;
                }
            }
            else
            {
                if (freePlan)
                {
                    lDemo.Text = "Vous utilisez la version gratuite du logiciel.";
                    bDemo.Text = "Continuer à utiliser la version gratuite";
                }
                else if (subscription)
                {
                    lAcheter.Text = "Renouveller la licence";
                    bDemo.Text = "Continuer à utiliser le logiciel";
                    if (TrialDaysRemaining > 0)
                    {
                        lDemo.Text = string.Format(LicenceWillExpire, TrialDaysRemaining - 1);
                    }
                    else
                    {
                        lDemo.Text = LicenceExpired;
                        bDemo.Enabled = false;
                    }
                }
                else
                {
                    lAcheter.Text = "Renouveller le contrat de support technique";
                    bDemo.Text = "Continuer à utiliser le logiciel";
                    if (TrialDaysRemaining > 0)
                    {
                        lDemo.Text = string.Format(ContractWillExpire, TrialDaysRemaining - 1);
                    }
                    else
                    {
                        lDemo.Text = ContractExpired;
                        //bDemo.Enabled = false;
                    }
                }
            }
        }

        //public void SetTrialExpiredText()
        //{
        //    lDemo.Text = TrialExpired;
        //    bDemo.Enabled = false;
        //}

        private void bActiver_Click(object sender, EventArgs e)
        {
            //ActivationClass.Demo = false;
            //ActivationClass.Activate(TurboActivate.VersionGUID);
            DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void bDemo_Click(object sender, EventArgs e)
        {
            if (trial)
            {
                if (TrialDaysRemaining > 0)
                    ActivationClass.Demo = true;
                DialogResult = System.Windows.Forms.DialogResult.No;
            }
            else if (freePlan)
            {
                ActivationClass.freePlan = true;
                DialogResult = System.Windows.Forms.DialogResult.No;
            }
            else
                DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void lAcheter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            switch (ActivationClass.ta.VersionGUID)
            {
                case "64f3c5e53a0c13ab92dc1.88990089":
                case "42bc7373574bf68b7e68b2.98642651":
                    if (trial || freePlan)
                    {
                        System.Diagnostics.Process.Start("http://www.imprimecheque.com/COUNTRY.html");//("http://imprimecheque.com/acheter.html");
                        break;
                    }
                    else
                    {
                        System.Diagnostics.Process.Start("http://renouvellement.imprimecheque.com");
                        break;
                    }
                case "2faf2b2a546b6603993273.11498445":
                    {
                        System.Diagnostics.Process.Start("http://www.logiciels-algerie.com/index.php/eureka");
                        break;
                    }
                case "32f81344556ae60acd3aa5.55005360":
                    {
                        System.Diagnostics.Process.Start("http://www.logiciels-algerie.com/index.php/Avocat");
                        break;
                    }
                case "43eea6c555892996b3393.64388222":
                    {
                        System.Diagnostics.Process.Start("http://www.logiciels-algerie.com/index.php/Baridi");
                        break;
                    }
                case "790e4df3551d10834f17b8.72601338":
                    {
                        System.Diagnostics.Process.Start("http://www.logiciels-algerie.com/index.php/ikama");
                        break;
                    }
                case "384a0dfb547d8f3b48e440.44894861":
                    {
                        System.Diagnostics.Process.Start("http://www.logiciels-algerie.com/index.php/lscompta");
                        break;
                    }
                case "5ce82a42547d901a6164c4.10622957":
                    {
                        System.Diagnostics.Process.Start("http://www.logiciels-algerie.com/index.php/mapaye");
                        break;
                    }
                case "74a19157558bbc8b9f47c5.76216877":
                    {
                        System.Diagnostics.Process.Start("http://www.logiciels-algerie.com/index.php/Mawared");
                        break;
                    }
                case "43f418525562dd6a79b815.08178107":
                    {
                        System.Diagnostics.Process.Start("http://www.logiciels-algerie.com/index.php/Mizania");
                        break;
                    }
                case "4beafd24555891f7247469.61408950":
                    {
                        System.Diagnostics.Process.Start("http://www.hr-master.com");
                        break;
                    }
                case "4b8ffaaf5577f79e29f079.14599598":
                    {
                        System.Diagnostics.Process.Start("http://www.logiciels-algerie.com/index.php/SMSSender");
                        break;
                    }
                case "4041b852555890be2f68b9.65261702":
                    {
                        System.Diagnostics.Process.Start("http://www.logiciels-algerie.com/index.php/WinParc");
                        break;
                    }
                default:
                    {
                        System.Diagnostics.Process.Start("http://www.logiciels-algerie.com");
                        break;
                    }
            }
        }
    }
}
