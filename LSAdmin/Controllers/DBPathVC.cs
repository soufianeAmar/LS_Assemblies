using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;

namespace LSAdmin
{
    public partial class DBPathVC : ViewController
    {
        public DBPathVC()
        {
            InitializeComponent();
            RegisterActions(components);
        }
        /// <summary>
        /// Utilisée pour parcourir les emplacement sur l'ordinateur
        /// </summary>
        /// <param name="BrowseFolders">Parcourt les dossiers (n'affiche aucun fichier)</param>
        /// <param name="FilterValue">Utilisé si BrowseFolders a la valeur FAUX. Représente le filtre de fichiers à utiliser</param>
        /// <param name="OpenDialog">Utilisé si BrowseFolders a la valeur FAUX. Ouvre une boite de dialogue pour ouvrir un fichier si la valeur VRAI est utilisée. Sinon, affiche une boite de dialogue pour la sauvegarde</param>
        /// <returns></returns>
        private string Parcourir(bool BrowseFolders, string FilterValue, bool OpenDialog)
        {
            if (BrowseFolders)
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        //if (fbd.SelectedPath.EndsWith("\\"))
                            return fbd.SelectedPath;
                        //else
                        //    return string.Format("{0}\\", fbd.SelectedPath);
                    }
                    else
                        return null;
                }
            }
            else
            {
                if (OpenDialog)
                {
                    using (OpenFileDialog ofd = new OpenFileDialog { Filter = FilterValue })
                    {
                        if (ofd.ShowDialog() == DialogResult.OK)
                            return ofd.FileName;
                        else
                            return null;
                    }
                }
                else
                {
                    using (SaveFileDialog sfd = new SaveFileDialog { Filter = FilterValue })
                    {
                        if (sfd.ShowDialog() == DialogResult.OK)
                            return sfd.FileName;
                        else
                            return null;
                    }
                }
            }
        }
        private void Parcourir_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //if (View.ObjectTypeInfo.Name == "Exercice")
            if (View.ObjectTypeInfo.Type == typeof(Dossier))
            {
                string dbPath = Parcourir(true, "", false);
                if (dbPath != null)
                    ((Dossier)View.CurrentObject).chemin = dbPath;
                    //((Exercice)View.CurrentObject).chemin = dbPath;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            aParcourir.Active.SetItemValue("", ObjectSpace.IsNewObject(View.CurrentObject));
        }
    }
}
