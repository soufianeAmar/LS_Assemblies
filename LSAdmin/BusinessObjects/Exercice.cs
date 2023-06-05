using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using System.Data.SqlClient;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using Microsoft.SqlServer.Management.Smo;
using System.Windows.Forms;
using System.IO;
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.Utils;

namespace LSAdmin
{
    [DefaultClassOptions, NavigationItem(false), ImageName("exercice"), DefaultProperty("db_name"), CreatableItem(false)]
    public class Exercice : BaseObject
    {
        #region Fields
        Dossier _dossier;
        int _exercice;
        string _designation;
        string _db_name;
        string _chemin;
        bool _accessible;
        bool _importation;
        Exercice _exercice_precedent;
        int _maxUsers;
        LsUser _subscriptionOwner;
        #endregion

        #region Propriété
        //[DevExpress.Xpo.DisplayName("Name")]
        //public string name
        //{
        //    get
        //    {
        //        return designation;
        //    }
        //}
        [DevExpress.Xpo.DisplayName("Dossier"), ImmediatePostData(true), RuleRequiredField()]
        [Association("Dossier-Exercices")]
        public Dossier dossier
        {
            get { return _dossier; }
            set
            {
                Dossier _value = XPObjectSpace.FindObjectSpaceByObject(this).GetObject<Dossier>(value);
                SetPropertyValue("dossier", ref _dossier, _value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    if (_value != null)
                    {
                        if (_value.code_dossier != designation)
                            db_name = string.Format("{0}_{1}", _value, designation);
                        else
                            db_name = designation;
                    }
                }
            }
        }
        [DevExpress.Xpo.DisplayName("Exercice"), ImmediatePostData(true)]
        public int exercice
        {
            get { return _exercice; }
            set { SetPropertyValue("exercice", ref _exercice, value); }
        }
        [DevExpress.Xpo.DisplayName("Designation")]
        public string designation
        {
            get { return _designation; }
            set
            {
                SetPropertyValue("designation", ref _designation, value);
                if (!IsLoading && !IsSaving && !IsDeleted)
                {
                    if (dossier != null)
                    {
                        if (dossier.code_dossier != value)
                            db_name = string.Format("{0}_{1}", dossier, value);
                        else
                            db_name = designation;
                    }
                }
            }
        }
        [DevExpress.Xpo.DisplayName("Base de données")]//, Persistent("db_name")]
        public string db_name
        {
            //get
            //{
            //    //if (!IsLoading && !IsSaving)
            //    if (!IsDeleted)
            //        if (dossier.code_dossier != designation)
            //            return string.Format("{0}_{1}", dossier, designation);
            //        else
            //            return designation;
            //    else
            //        return string.Empty;
            //    //return string.Format("{0}{1}", dossier, exercice);
            //}
            get { return _db_name; }
            set { SetPropertyValue("db_name", ref _db_name, value); }
        }
        [DevExpress.Xpo.DisplayName("Chemin de la base de données"), Browsable(false)]
        public string chemin
        {
            get { return _chemin; }
            set { SetPropertyValue("chemin", ref _chemin, value); }
        }
        [DevExpress.Xpo.DisplayName("Accessible"), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool accessible
        {
            get { return _accessible; }
            set { SetPropertyValue("accessible", ref _accessible, value); }
        }
        [DevExpress.Xpo.DisplayName("Importation"), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool importation
        {
            get { return _importation; }
            set { SetPropertyValue("importation", ref _importation, value); }
        }
        [DevExpress.Xpo.DisplayName("Exercice précédent"), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public Exercice exercice_precedent
        {
            get { return _exercice_precedent; }
            set { SetPropertyValue("exercice_precedent", ref _exercice_precedent, value); }
        }
        [DevExpress.Xpo.DisplayName("Max users")]
        public int maxUsers
        {
            get
            {
                return _maxUsers;
            }
            set
            {
                SetPropertyValue("maxUsers", ref _maxUsers, value);
            }
        }
        [DevExpress.Xpo.DisplayName("Subscription owner"), DataSourceProperty("users")]
        public LsUser subscriptionOwner
        {
            get
            {
                return _subscriptionOwner;
            }
            set
            {
                SetPropertyValue("subscriptionOwner", ref _subscriptionOwner, value);
            }
        }
        #endregion
        
        #region Association
        [Association("Users-Exercices")]
        public XPCollection<LsUser> users
        {
            get { return GetCollection<LsUser>("users"); }
        }
        #endregion

        public Exercice(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false:
            // if (!IsLoading){
            //    It is now OK to place your initialization code here.
            // }
            // or as an alternative, move your initialization code into the AfterConstruction method.
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
            string ApplicationFolder = Path.GetDirectoryName(Application.ExecutablePath);
            //string DataFolder = string.Format("{0}\\Data\\", ApplicationFolder);
            //if (!Directory.Exists(DataFolder))
            //    Directory.CreateDirectory(DataFolder);
            //chemin = DataFolder;
            exercice = DateTime.Today.Year;
            accessible = true;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            System.Data.IDbConnection connection = ((DevExpress.Xpo.DB.ConnectionProviderSql)(((DevExpress.Xpo.Helpers.BaseDataLayer)Session.DataLayer).ConnectionProvider)).Connection;
            //if ((lsactvtn.ActivationClass.réseau) && (lsactvtn.ActivationClass.version == lsactvtn.VersionImprimeCheque.ENTREPRISE))
            if (connection is SqlConnection)
            {
                string SQLServerInstance = string.Format("{0}{1}", Helper.serverName, Helper.instanceName);
                Server server = new Server(SQLServerInstance);
                if (IsDeleted)
                {
                    //Database db = server.Databases[db_name];
                    if (server.Databases.Contains(db_name))
                    {
                        if (MessageBox.Show("La suppression d'une base de données est définitive et irréversible. Continuer?", "Suppression",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            server.KillDatabase(db_name);
                        }
                    }
                }
                else
                {
                    if (CheckMaximumDatabases())
                        throw new Exception("Vous avez atteint le nombre maximum de bases de données pour ce dossier. Veuillez contacter l'éditeur du logiciel pour plus d'information");
                    else
                    {
                        Database db = new Database(server, db_name);
                        if (!server.Databases.Contains(db_name))
                        {
                            //db.AutoShrink = false;
                            //db.FileGroups.Add(new FileGroup(db, "PRIMARY"));
                            //db.FileGroups[0].Files.Add(new DataFile(db.FileGroups[0], db_name, string.Format("{0}{1}.mdf", chemin, db_name)) 
                            //    { Growth = 10, GrowthType = FileGrowthType.Percent});
                            //db.LogFiles.Add(new LogFile(db, string.Format("{0}_log", db_name), string.Format("{0}{1}_log.ldf", chemin, db_name)) 
                            //    { Growth = 10, GrowthType = FileGrowthType.Percent });
                            //var script = db.Script();
                            //db.Create();
                            string dataFolder = dossier.chemin.EndsWith("\\") ? dossier.chemin : dossier.chemin + "\\";
                            string fichier_modele = string.Format(@"{0}\Modele\modele.bak", Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath));
                            if (File.Exists(fichier_modele))
                                Helper.CreateServerDatabaseFromBackupInPath(SQLServerInstance, db_name, dataFolder, fichier_modele);
                            else
                                Helper.CreateServerDatabaseInPath(SQLServerInstance, db_name, dataFolder);
                        }
                        else
                        {
                            db = server.Databases[db_name];
                            chemin = Path.GetDirectoryName(db.FileGroups[0].Files[0].FileName);
                        }
                        //throw new Exception("Base de données existante !");
                        //MessageBox.Show("Base de données existante !", "Erreur de création", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
                if ((!IsDeleted) &&(CheckMaximumDatabases()))
                    throw new Exception("Vous avez atteint le nombre maximum de bases de données pour ce dossier. Veuillez contacter l'éditeur du logiciel pour plus d'information");
        }

        private bool CheckMaximumDatabases()
        {
            if (lsactvtn.ActivationClass.nombreDBparDossier > 0)
            {
                XPObjectSpace os = (XPObjectSpace)XPObjectSpace.FindObjectSpaceByObject(this);
                bool error = os.GetObjectsCount(typeof(Exercice), CriteriaOperator.Parse("dossier = ?", dossier)) >= lsactvtn.ActivationClass.nombreDBparDossier;
                return error;
            }
            else
                return false;
        }



    }

}
