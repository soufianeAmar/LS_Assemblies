#define GENUINE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using wyDay.Controls;

namespace lsactvtn
{
    public class ActivationClass
    {
        public static TurboActivate ta;
        //public static TA_Flags verifiedTrialFlag = TA_Flags.TA_USER | TA_Flags.TA_VERIFIED_TRIAL;
        public static TA_Flags verifiedTrialFlag = TA_Flags.TA_USER | TA_Flags.TA_UNVERIFIED_TRIAL;
        public static VersionImprimeCheque version = VersionImprimeCheque.HOME;
        public static bool Demo = false;
        public static bool freePlan = false;

        public static int trialDaysRemaining = 0;
        public static bool isGeniune = false;
        public static bool InternetError = false;
        public static bool réseau = false;
        public static bool isServer = false;
        public static bool ADAuthentication = false;
        public static int nombreEnregistrements = 5;
        public static int nombreDossiers = 1;
        public static int nombreDBparDossier = 0;
        public static bool importationEureka = false;
        public static bool importationScrabble = false;
        public static bool gestionTraite = false;
        public static bool gestionDesChèquesEmis = false;
        public static bool paiementMesualité = false;

        public static bool demandePaiement = false;
        public static bool chèqueEmis = false;
        public static bool chèqueReçu = false;
        public static bool remiseChèques = false;
        public static bool chèqueCertifié = false;
        public static bool carnetCheque = false;
        public static bool traiteEmise = false;
        public static bool traiteReçue = false;
        public static bool virementEmis = false;
        public static bool virementReçu = false;
        public static bool versementEspèce = false;
        public static bool dossierImportation = false;
        public static bool ordreTransfert = false;
        public static bool notfication = false;
        public static bool rapports = false;
        public static bool caisse = false;
        public static bool cautionEmise = false;
        public static bool cautionRetenue = false;
        public static bool brouillardBanque = false;
        public static bool prélèvement = false;
        public static bool prévision = false;
        public static bool situationFinancière = false;
        public static int nombreComptes = 1;

        public static bool ContratRésilié = false;
        public static string ContratExpireLe = string.Empty;
        public static string ExpireLe = string.Empty;

        public static void Activate(string GUID)
        {
            if (ta == null)
                ta = new TurboActivate(GUID);
            //TurboActivate.VersionGUID = GUID;
            ActivationWizard aw = new ActivationWizard();
            aw.ShowDialog();
        }

        public static void Update()
        {
            UpdateForm uf = new UpdateForm();
            uf.ShowDialog();
        }

        public static string ReadStringFeature(string featureName)
        {
            string result = string.Empty;
            try
            {
                result = ta.GetFeatureValue(featureName);
                //result = TurboActivate.GetFeatureValue(featureName);
            }
            catch (Exception ex)
            {
                result = string.Empty;
            }
            return result;
        }

        public static int ReadIntFeature(string featureName)
        {
            int result = 0;
            try
            {
                string _result = ta.GetFeatureValue(featureName);
                result = Convert.ToInt32(_result);
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        public static bool ReadBoolFeature(string featureName)
        {
            bool result = false;
            try
            {
                string _result = ta.GetFeatureValue(featureName);
                result = _result == "1";
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public static void GetVersion()
        {
            if (Demo)// || freePlan)
            {
                version = VersionImprimeCheque.DEMO;
                //nombreEnregistrements = 5;
            }
            else if (freePlan)
                version = VersionImprimeCheque.FREEPLAN;
            else
            {
                try
                {
                    string versionString = ta.GetFeatureValue("Version");
                    switch (versionString)
                    {
                        case "Home":
                            version = VersionImprimeCheque.HOME;
                            break;
                        case "Pro":
                            version = VersionImprimeCheque.PRO;
                            break;
                        case "Entreprise":
                            version = VersionImprimeCheque.ENTREPRISE;
                            break;
                        case "Ultimate":
                            version = VersionImprimeCheque.ULTIMATE;
                            break;
                        case "LAC":
                            version = VersionImprimeCheque.LAC;
                            break;
                        default:
                            version = VersionImprimeCheque.HOME;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    version = VersionImprimeCheque.HOME;
                }
                réseau = ReadBoolFeature("Réseau");
                isServer = ReadBoolFeature("Serveur");
                ADAuthentication = ReadBoolFeature("Active Directory Authentication");
                nombreEnregistrements = ReadIntFeature("Nombre d'enregistrements");
                nombreDossiers = ReadIntFeature("Nombre de dossiers");
                nombreDBparDossier = ReadIntFeature("Nombre de DB par dossier");
                importationEureka = ReadBoolFeature("Importation Eureka");
                importationScrabble = ReadBoolFeature("Importation Scrabble");
                gestionTraite = ReadBoolFeature("Gestion des traites");
                gestionDesChèquesEmis = ReadBoolFeature("Gestion des chèques émis");
                paiementMesualité = ReadBoolFeature("Paiement par mensualités");

                demandePaiement = ReadBoolFeature("Module - Demande de paiement");
                chèqueEmis = ReadBoolFeature("Module - Chèque émis");
                chèqueReçu = ReadBoolFeature("Module - Chèque reçu");
                remiseChèques = ReadBoolFeature("Module - Remise de chèques");
                chèqueCertifié = ReadBoolFeature("Module - Chèque de banque");
                carnetCheque = ReadBoolFeature("Module - Carnet de chèques");
                traiteEmise = ReadBoolFeature("Module - Traite émise");
                traiteReçue = ReadBoolFeature("Module - Traite reçue");
                virementEmis = ReadBoolFeature("Module - Virement émis");
                virementReçu = ReadBoolFeature("Module - Virement reçu");
                versementEspèce = ReadBoolFeature("Module - Versement espèce");
                dossierImportation = ReadBoolFeature("Module - Dossier Importation");
                ordreTransfert = ReadBoolFeature("Module - Ordre de transfert");
                notfication = ReadBoolFeature("Module - Notification");
                rapports = ReadBoolFeature("Module - Rapports");
                caisse = ReadBoolFeature("Module - Caisse");
                cautionEmise = ReadBoolFeature("Module - Caution émise");
                cautionRetenue = ReadBoolFeature("Module - Caution retenue");
                brouillardBanque = ReadBoolFeature("Module - Brouillard de banque");
                prélèvement = ReadBoolFeature("Module - Prélèvement");
                prévision = ReadBoolFeature("Module - Prévision");
                situationFinancière = ReadBoolFeature("Module - Situation Financière");
                nombreComptes = ReadIntFeature("Nombre de comptes");

                ContratRésilié = ReadBoolFeature("Contrat résilié");
                ContratExpireLe = ReadStringFeature("Contrat expire le");
                ExpireLe = ReadStringFeature("Expire Le");

                //try
                //{
                //    string _réseau = TurboActivate.GetFeatureValue("Réseau");
                //    réseau = _réseau == "1";
                //}
                //catch (Exception ex)
                //{
                //    réseau = false;
                //}
                //try
                //{
                //    string _isServer = TurboActivate.GetFeatureValue("Serveur");
                //    isServer = _isServer == "1";
                //}
                //catch (Exception ex)
                //{
                //    isServer = false;
                //}
                //try
                //{
                //    string _ADAuthentication = TurboActivate.GetFeatureValue("Active Directory Authentication");
                //    ADAuthentication = _ADAuthentication == "1";
                //}
                //catch (Exception ex)
                //{
                //    ADAuthentication = false;
                //}
                //try
                //{
                //    string _recordCount = TurboActivate.GetFeatureValue("Nombre d'enregistrements");
                //    nombreEnregistrements = Convert.ToInt32(_recordCount);
                //}
                //catch (Exception ex)
                //{
                //    nombreEnregistrements = 0;
                //}
                //try
                //{
                //    string _folderCount = TurboActivate.GetFeatureValue("Nombre de dossiers");
                //    nombreDossiers = Convert.ToInt32(_folderCount);
                //}
                //catch (Exception ex)
                //{
                //    nombreDossiers = 0;
                //}
                //try
                //{
                //    string _dbCount = TurboActivate.GetFeatureValue("Nombre de DB par dossier");
                //    nombreDBparDossier = Convert.ToInt32(_dbCount);
                //}
                //catch (Exception ex)
                //{
                //    nombreDBparDossier = 0;
                //}
                //try
                //{
                //    string _importationEureka = TurboActivate.GetFeatureValue("Importation Eureka");
                //    importationEureka = _importationEureka == "1";
                //}
                //catch (Exception ex)
                //{
                //    importationEureka = false;
                //}
                //try
                //{
                //    string _importationScrabble = TurboActivate.GetFeatureValue("Importation Scrabble");
                //    importationScrabble = _importationScrabble == "1";
                //}
                //catch (Exception ex)
                //{
                //    importationScrabble = false;
                //}
            }
        }
        /// <summary>
        /// Checks whether the application is activated or is in trial version
        /// </summary>
        /// <param name="GUID">Unique identifier of the software</param>
        public static void CheckTrialAndActivation(string GUID, bool freePlan = false)
        {
            if (ta == null)
                ta = new TurboActivate(GUID);
            //TurboActivate.VersionGUID = GUID;
#if GENUINE
            IsGenuineResult gr = ta.IsGenuine();                                                                                //
            isGeniune = ((gr == IsGenuineResult.Genuine) || (gr == IsGenuineResult.GenuineFeaturesChanged));                    // if no internet and the app reports as not geniune
            InternetError = (gr == IsGenuineResult.InternetError);                                                              // and not activated
            if (!((isGeniune) || ((ta.IsActivated()) && InternetError)))                                                        //
#else
            if (!TurboActivate.IsActivated())
#endif
            {
                if (freePlan)
                {
                    trialDaysRemaining = 0;
                }
                else
                {
                    try
                    {
                        ta.UseTrial(verifiedTrialFlag);                                                                             //
                        trialDaysRemaining = (int)ta.TrialDaysRemaining(verifiedTrialFlag);                                         // get the number of trial days remaining
                    }
                    catch
                    {

                    }
                }
#if MOBILIS
                if (trialDaysRemaining <= 0)
                {
#endif
                TrialAndActivation trialForm = new TrialAndActivation() { TrialDaysRemaining = trialDaysRemaining, 
                    trial = !freePlan, freePlan = freePlan };                                                                   //
                trialForm.SetTrialText();                                                                                       // display a form asking the user whether to activate the app
                DialogResult dr = trialForm.ShowDialog();                                                                       // or continue with trial
                if (dr == DialogResult.Yes)                                                                                     //
                {                                                                                                               //
                    Activate(ta.VersionGUID);                                                                                   // if the user chooses to activate
                    gr = ta.IsGenuine();                                                                                        // start the activation process
                    isGeniune = ((gr == IsGenuineResult.Genuine) || (gr == IsGenuineResult.GenuineFeaturesChanged));            //
                    InternetError = (gr == IsGenuineResult.InternetError);                                                      //
                }
                else
                {
                    if (dr == DialogResult.No)                                                                                  //
                    {                                                                                                           // if the user chooses to continue with the trial
                        //if (freePlan)
                        //    throw new Exception("");
                        //else 
                        if (trialDaysRemaining == 0 && !freePlan)                                                                            // and if the trial expired
                        {                                                                                                       //
                            TrialExtension te = new TrialExtension();                                                           // display a form for trial extension
                            te.ShowDialog();                                                                                    //
                        }                                                                                                       //
                    }
                }
            }
            else
            {
                //bool licenceExpire = true;
                //try
                //{
                //    ExpireLe = ta.GetFeatureValue("Expire Le");
                //}
                //catch
                //{
                //    licenceExpire = false;
                //}
                //if (licenceExpire && !string.IsNullOrEmpty(ExpireLe))
                GetVersion();
                if (!string.IsNullOrEmpty(ExpireLe))
                {
                    int joursRestants = (int)((DateTime.Parse(lsactvtn.ActivationClass.ExpireLe) - DateTime.Now).TotalDays + 1);
                    if (joursRestants <= 30)
                    {
                        TrialAndActivation trialForm = new TrialAndActivation() { TrialDaysRemaining = joursRestants, trial = false, subscription = true };
                        trialForm.SetTrialText();
                        DialogResult dr = trialForm.ShowDialog();
                        if (dr == DialogResult.Cancel)
                            throw new Exception("");
                    }
                    //if (!ta.IsDateValid(ExpireLe, TA_DateCheckFlags.TA_HAS_NOT_EXPIRED))
                    //{
                    //    MessageBox.Show("Votre licence a expiré. Veuillez la renouveler !", "Licence expirée", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    throw new Exception("");
                    //}
                }
                else if (!ContratRésilié)
                {
                    int joursRestants = string.IsNullOrEmpty(ContratExpireLe) ? 0 : (int)((DateTime.Parse(ContratExpireLe) - DateTime.Now).TotalDays + 1);
                    if (joursRestants <= 30)
                    {
                        TrialAndActivation trialForm = new TrialAndActivation() { TrialDaysRemaining = joursRestants, trial = false };
                        trialForm.SetTrialText();
                        DialogResult dr = trialForm.ShowDialog();
                        if (dr == DialogResult.Cancel)
                            throw new Exception("");
                    }
                    //if (!ta.IsDateValid(ExpireLe, TA_DateCheckFlags.TA_HAS_NOT_EXPIRED))
                    //{
                    //    MessageBox.Show("Votre licence a expiré. Veuillez la renouveler !", "Licence expirée", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    throw new Exception("");
                    //}
                }

            }
#if MOBILIS
                else
                    ActivationClass.Demo = true;
            }
#endif
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="GUID"></param>
        ///// <param name="trialExtensionAsLicence"></param>
        //public static void CheckTrialAndActivation(string GUID, bool trialExtensionAsLicence)
        //{
        //    if (trialExtensionAsLicence)
        //    {
        //        if (ta == null)
        //            ta = new TurboActivate(GUID);
        //        //TurboActivate.VersionGUID = GUID;
        //        ta.UseTrial(verifiedTrialFlag, Dns.GetHostName());
        //        trialDaysRemaining = (int)ta.TrialDaysRemaining(verifiedTrialFlag);
        //        if (trialDaysRemaining <= 1)
        //        {
        //            TrialExtentionAsLicence teal = new TrialExtentionAsLicence();
        //            teal.ShowDialog();
        //        }
        //        else
        //            ActivationClass.Demo = true;
        //    }
        //    else
        //        CheckTrialAndActivation(GUID);
        //}

        public static bool CheckTrial(string GUID)
        {
            if (ta == null)
                ta = new TurboActivate(GUID);
            //TurboActivate.VersionGUID = GUID;
#if GENUINE
            IsGenuineResult gr = ta.IsGenuine();
            isGeniune = ((gr == IsGenuineResult.Genuine) || (gr == IsGenuineResult.GenuineFeaturesChanged));
            InternetError = (gr == IsGenuineResult.InternetError);
            if (!((isGeniune) || ((ta.IsActivated()) && InternetError)))
#else
            if (!TurboActivate.IsActivated())
#endif
            {
                ta.UseTrial(verifiedTrialFlag);
                trialDaysRemaining = (int)ta.TrialDaysRemaining(verifiedTrialFlag);
                return trialDaysRemaining > 0;
            }
            return false;
        }

        public static bool IsActivated(string GUID)
        {
            GetVersion();
            if (ta == null)
                ta = new TurboActivate(GUID);
            //TurboActivate.VersionGUID = GUID;
#if GENUINE
            return (isGeniune || (ta.IsActivated() && InternetError) || Demo || freePlan);
#else
            return (TurboActivate.IsActivated() || Demo);
#endif
        }

        public static void UpdateSilently()
        {
            AutomaticUpdaterBackend aube = new AutomaticUpdaterBackend()
            {
                GUID = "f8308cc2-34fd-4bbe-9bb6-af96b4620800",
                UpdateType = UpdateType.Automatic,
            };
            aube.Initialize();
            aube.AppLoaded();
            aube.ReadyToBeInstalled += aube_ReadyToBeInstalled;
            if (!aube.ClosingForInstall)
                aube.ForceCheckForUpdate();
        }

        public static void CheckForUpdates()
        {
            AutomaticUpdaterBackend aube = new AutomaticUpdaterBackend()
            {
                GUID = "f8308cc2-34fd-4bbe-9bb6-af96b4620800",
                UpdateType = UpdateType.DoNothing,
            };
            aube.Initialize();
            aube.AppLoaded();
            aube.ReadyToBeInstalled += aube_ReadyToBeInstalled;
            aube.UpdateAvailable += aube_UpdateAvailable;
            if (!aube.ClosingForInstall)
                aube.ForceCheckForUpdate(true);
        }

        static void aube_UpdateAvailable(object sender, EventArgs e)
        {
            bool contratExpiré = false;
            AutomaticUpdaterBackend aube = (AutomaticUpdaterBackend)sender;
            try
            {
                contratExpiré = !ta.IsDateValid(ta.GetFeatureValue("Fin contrat"), TA_DateCheckFlags.TA_HAS_NOT_EXPIRED);
            }
            catch (Exception ex)
            {
                contratExpiré = true;
            }
            if (contratExpiré)
            {
                MessageBox.Show("Une mise à jour est disponible pour votre logiciel, mais votre contrat a expiré.\nVeuillez contacter Leadersoft " +
                    "pour renouveler votre contrat et bénéficier des mises à jour gratuites", "Mise à jour", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                aube.InstallNow();
            }
        }

        private static void aube_ReadyToBeInstalled(object sender, EventArgs e)
        {
            MessageBox.Show("Des mises à jour sont en attente d'installation.\nElles seront installées au prochain demarrage de l'application", 
                "Mise à jour", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}