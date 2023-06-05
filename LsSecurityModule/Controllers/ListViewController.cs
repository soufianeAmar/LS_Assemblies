using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Security.Strategy;

namespace LsSecurityModule
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class ListViewController : ViewController
    {

        public ListViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void ListViewController_Activated(object sender, EventArgs e)
        {
            if(lsactvtn.ActivationClass.Demo == false)
            {
                try
                {
                    if (View.ObjectTypeInfo.Type.Name != "LsUser" && View.ObjectTypeInfo.Type.Name != "Dossier"
                        && View.ObjectTypeInfo.Type.Name != "Exercice" && View.ObjectTypeInfo.Type.Name != "PaymentPlan"
                         && View.ObjectTypeInfo.Type.Name != "PlanLimitation")
                    {
                        //Session session = ((XPObjectSpace)ObjectSpace).Session;
                        //if(session != null)
                        //    if ((((System.Data.SQLite.SQLiteConnection)session.Connection).DataSource) != "LSAdmin")
                        //    {

                        //LsSecuritySystemUser currentUser = session.GetObjectByKey<LsSecuritySystemUser>(SecuritySystem.CurrentUserId);
                        LsSecuritySystemUser currentUser = (LsSecuritySystemUser)SecuritySystem.CurrentUser;

                        foreach (SecuritySystemRole role in currentUser.Roles)
                        {
                            List<string> Names = new List<string>();
                            List<string> Criterias = new List<string>();

                            foreach (SecuritySystemTypePermissionObject TPO in role.TypePermissions)
                            {
                                Names.Add(TPO.TargetType.Name);
                                string criteria = "";
                                int j = 0;
                                foreach (SecuritySystemObjectPermissionsObject OPO in TPO.ObjectPermissions)
                                {
                                    criteria += OPO.Criteria + " ";
                                    j += 1;
                                    if (TPO.ObjectPermissions.Count > 1 && j < TPO.ObjectPermissions.Count)
                                        criteria += "And";
                                }
                                Criterias.Add(criteria);
                            }
                            if (View is ListView)
                            {
                                string name = View.ObjectTypeInfo.Name;
                                int n = -1;
                                for (int i = 0; i < Names.Count; i++)
                                {
                                    if (Names[i] == name)
                                        n = i;
                                }
                                if (n >= 0)
                                    ((ListView)View).CollectionSource.Criteria["Filter"] = ObjectSpace.ParseCriteria(Criterias[n]);
                                //((ListView)View).CollectionSource.Criteria["Filter"] = session.ParseCriteria(Criterias[n]);
                            }
                        }
                    }
                }
                catch (Exception er) { }
            }
        }
    }
}
