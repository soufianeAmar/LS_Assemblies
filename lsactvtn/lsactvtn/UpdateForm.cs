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
    public partial class UpdateForm : Form
    {
        public UpdateForm()
        {
            InitializeComponent();
            LocalizeUpdater("fr");
        }

        //public void InstallPendingUpdate()
        //{
        //    automaticUpdater1.InstallNow();
        //}

        private void LocalizeUpdater(string language)
        {
            if (language == "fr")
            {
                automaticUpdater1.Translation.PrematureExitTitle = "wyUpdate exited prematurely";
                automaticUpdater1.Translation.PrematureExitMessage = "wyUpdate ended before the current update step could be completed.";
                automaticUpdater1.Translation.CheckForUpdatesMenu = "Vérifier les mises à jour";
                automaticUpdater1.Translation.DownloadUpdateMenu = "Télécharger et mettre à jour maintenant";
                automaticUpdater1.Translation.InstallUpdateMenu = "Installer les mises à jour maintenant";
                automaticUpdater1.Translation.CancelUpdatingMenu = "Annuler la mise à jour";
                automaticUpdater1.Translation.CancelCheckingMenu = "Annuler la vérification des mises à jour";
                automaticUpdater1.Translation.HideMenu = "Cacher";
                automaticUpdater1.Translation.ViewChangesMenu = "Voir les changements dans la version %version%";
                automaticUpdater1.Translation.StopChecking = "Arreter la vérification des mises à jour pour le moment";
                automaticUpdater1.Translation.StopDownloading = "Arreter le téléchargement des mises à jour pour le moment";
                automaticUpdater1.Translation.StopExtracting = "Arreter l'extraction des mises à jour pour le moment";
                automaticUpdater1.Translation.TryAgainLater = "Essayer ultérieurement";
                automaticUpdater1.Translation.TryAgainNow = "Ré-essayer maintenant";
                automaticUpdater1.Translation.ViewError = "Voir les détails des erreurs";
                automaticUpdater1.Translation.CloseButton = "Fermer";
                automaticUpdater1.Translation.ErrorTitle = "Erreur";
                automaticUpdater1.Translation.UpdateNowButton = "Mettre à jour maintenant";
                automaticUpdater1.Translation.ChangesInVersion = "Les changements dans la version %version%";
                automaticUpdater1.Translation.FailedToCheck = "Echec de vérification des mises à jour.";
                automaticUpdater1.Translation.FailedToDownload = "Echec de téléchargement des mises à jour.";
                automaticUpdater1.Translation.FailedToExtract = "Echec d'extraction des mises à jour.";
                automaticUpdater1.Translation.Checking = "Vérification des mises à jour";
                automaticUpdater1.Translation.Downloading = "Téléchargement des mises à jour";
                automaticUpdater1.Translation.Extracting = "Extraction des mises à jour";
                automaticUpdater1.Translation.UpdateAvailable = "Les mises à jour sont prêtes à être installées.";
                automaticUpdater1.Translation.InstallOnNextStart = "Les mises à jour seront installées au prochain lancement de l'application.";
                automaticUpdater1.Translation.AlreadyUpToDate = "Vous possédez déjà la dernière version";
                automaticUpdater1.Translation.SuccessfullyUpdated = "L'application a été mise à jour à la version %version% avec succés";
                automaticUpdater1.Translation.UpdateFailed = "Echec d'installation des mises à jour.";
            }
        }

        private void automaticUpdater1_CheckingFailed(object sender, wyDay.Controls.FailArgs e)
        {
            lUpdate.Text = "La vérification des mises à jour a échoué. vérifiez votre connexion à Internet puis ré-essayez!";
            bUpdate.Enabled = false;
            bClose.Enabled = true;
        }

        private void automaticUpdater1_UpdateAvailable(object sender, EventArgs e)
        {
            lUpdate.Text = "Des mises à jour sont disponibles!";
            bUpdate.Enabled = true;
            bClose.Enabled = true;
        }

        private void automaticUpdater1_UpdateFailed(object sender, wyDay.Controls.FailArgs e)
        {
            lUpdate.Text = "La mise à jour a échoué!";
            bUpdate.Enabled = true;
            bClose.Enabled = true;
        }

        private void automaticUpdater1_UpdateSuccessful(object sender, wyDay.Controls.SuccessArgs e)
        {
            lUpdate.Text = "L'application a été mise à jour!";
            bUpdate.Enabled = false;
            bClose.Enabled = true;
        }

        private void automaticUpdater1_UpToDate(object sender, wyDay.Controls.SuccessArgs e)
        {
            lUpdate.Text = "L'application est à jour!";
            bUpdate.Enabled = false;
            bClose.Enabled = true;
        }

        private void bUpdate_Click(object sender, EventArgs e)
        {
            automaticUpdater1.InstallNow();
            //bClose.Enabled = true;
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            automaticUpdater1.Cancel();
            Close();
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            automaticUpdater1.ForceCheckForUpdate();
        }
    }
}
