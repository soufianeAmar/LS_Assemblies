using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.IO;
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.Utils;
using System.Data.SqlClient;

namespace LSAdmin
{
    [DefaultClassOptions, NavigationItem("Administration"), ImageName("dossier"), DefaultProperty("code_dossier"), CreatableItem(false)]
    public class Dossier : BaseObject
    {
        #region Propriétés
        //[DevExpress.Xpo.DisplayName("Name")]
        //public string name
        //{
        //    get
        //    {
        //        return description;
        //    }
        //}
        string _code_dossier;
        [DevExpress.Xpo.DisplayName("Code dossier")]
        public string code_dossier
        {
            get { return _code_dossier; }
            set { SetPropertyValue("nom", ref _code_dossier, value); }
        }
        string _description;
        [DevExpress.Xpo.DisplayName("Description")]
        public string description
        {
            get { return _description; }
            set { SetPropertyValue("description", ref _description, value); }
        }
        string _chemin;
        [DevExpress.Xpo.DisplayName("Chemin des bases de données")]//, Browsable(false)]
        public string chemin
        {
            get { return _chemin; }
            set { SetPropertyValue("chemin", ref _chemin, value); }
        }
        [Association("Dossier-Exercices")]
        public XPCollection<Exercice> exercices
        {
            get { return GetCollection<Exercice>("exercices"); }
        }
        #endregion
        public Dossier(Session session)
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
            chemin = string.Format(@"{0}\Data", Core.GetApplicationPath());
        }
        protected override void OnSaving()
        {
            if (!lsactvtn.ActivationClass.Demo)
            {
                base.OnSaving();
                if (!IsDeleted)
                {
                    System.Data.IDbConnection connection = ((DevExpress.Xpo.DB.ConnectionProviderSql)(((DevExpress.Xpo.Helpers.BaseDataLayer)Session.DataLayer).ConnectionProvider)).Connection;
                    //if ((!lsactvtn.ActivationClass.réseau) || (lsactvtn.ActivationClass.version != lsactvtn.VersionImprimeCheque.ENTREPRISE))
                    if (!(connection is SqlConnection))
                        if (!(string.IsNullOrEmpty(code_dossier)))
                        {
                            string dossier = string.Format(@"{0}\{1}", chemin, code_dossier);
                            if (!Directory.Exists(dossier))
                                Directory.CreateDirectory(dossier);
                        }
                }
                else
                {
                    //XPCollection<Exercice> exercices = new XPCollection<Exercice>(Session, CriteriaOperator.Parse("dossier = ?", this));
                    Session.Delete(exercices);
                }
            }
            else
                throw new Exception("Vous ne pouvez pas créer de dossiers dans la version demo!");
        }
    }

}
