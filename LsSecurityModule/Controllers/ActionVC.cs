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
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace LsSecurityModule
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class ActionVC : ViewController
    {
        public ActionVC()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            ObjectSpace.ObjectSaving += ObjectSpace_ObjectSaving;
            CheckActionPermissions();
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
        private void ObjectSpace_ObjectSaving(object sender, ObjectManipulatingEventArgs e)
        {
            UpdateMemberPermissions(e.Object);
            //UpdateActionPermissions(e.Object);
        }
        private void UpdateMemberPermissions(object objectToSave)
        {
            if (objectToSave is SecuritySystemTypePermissionObject)
            {
                SecuritySystemTypePermissionObject sstpo = objectToSave as SecuritySystemTypePermissionObject;
                ITypeInfo typeClassInfo = XafTypesInfo.Instance.FindTypeInfo(sstpo.TargetType);
                foreach (IMemberInfo memberInfo in typeClassInfo.Members)
                    if (memberInfo.IsProperty && memberInfo.IsPublic)
                    {
                        Session session = ((XPObjectSpace)ObjectSpace).Session;
                        SecuritySystemMemberPermissionsObject ssmpo = session.FindObject<SecuritySystemMemberPermissionsObject>(PersistentCriteriaEvaluationBehavior.InTransaction,
                            CriteriaOperator.Parse("Owner = ? and Members like ?", sstpo, memberInfo.Name));
                        if (ssmpo == null)
                        {
                            ssmpo = new SecuritySystemMemberPermissionsObject(session)
                            {
                                AllowRead = true,
                                AllowWrite = true,
                                EffectiveRead = true,
                                EffectiveWrite = true,
                                Members = memberInfo.Name
                            };
                            sstpo.MemberPermissions.Add(ssmpo);
                        }
                    }
            }
        }
        private void UpdateActionPermissions(object objectToSave)
        {
            if (objectToSave is LsSecuritySystemRole)
            {
                Session session = ((XPObjectSpace)ObjectSpace).Session;
                if (session.IsNewObject(objectToSave))
                {
                    IModelActions actions = LsSecurityModule.currentApp.Model.ActionDesign.Actions;
                    foreach (IModelAction action in actions)
                    {
                        SecuritySystemActionPermissionObject ssapo = session.FindObject<SecuritySystemActionPermissionObject>(PersistentCriteriaEvaluationBehavior.InTransaction,
                            CriteriaOperator.Parse("role = ? and actionId = ?", objectToSave, action.Id));
                        if (ssapo == null)
                        {
                            ssapo = new SecuritySystemActionPermissionObject(session)
                            {
                                actionId = action.Id,
                                canExecute = true,
                                role = (LsSecuritySystemRole)objectToSave,
                            };
                            ssapo.Save();
                        }
                    }
                }
            }
        }
        private void CheckActionPermissions()
        {
            if ((SecuritySystem.CurrentUser != null) && (SecuritySystem.UserType == typeof(LsSecuritySystemUser)))
            {
                Session session = ((XPObjectSpace)ObjectSpace).Session;
                System.Data.IDbConnection connection = null;

                DevExpress.Xpo.Helpers.BaseDataLayer baseDataLayer = session.DataLayer as DevExpress.Xpo.Helpers.BaseDataLayer;
                DevExpress.Xpo.DB.ConnectionProviderSql connectionProvider = baseDataLayer.ConnectionProvider as DevExpress.Xpo.DB.ConnectionProviderSql;

                if (connectionProvider != null)//DevExpress.Xpo.DB.ConnectionProviderSql)
                    connection = ((DevExpress.Xpo.DB.ConnectionProviderSql)(((DevExpress.Xpo.Helpers.BaseDataLayer)session.DataLayer).ConnectionProvider)).Connection;
                if (connection != null)
                {
                    //bool isLSAdmin = false;
                    //if (connection.GetType() == typeof(SQLiteConnection))
                    //    isLSAdmin = ((SQLiteConnection)connection).DataSource == "LSAdmin";
                    //else if (connection.GetType() == typeof(SqlConnection))
                    //    isLSAdmin = connection.Database == "LSAdmin";// && lsactvtn.ActivationClass.réseau;
                    //if (!isLSAdmin)
                    if (!Helper.IsLSAdmin(ObjectSpace))
                    {
                        foreach (Controller controller in Frame.Controllers)
                        {
                            foreach (ActionBase action in controller.Actions)
                            {
                                //foreach (SecuritySystemRole role in ((LsSecuritySystemUser)SecuritySystem.CurrentUser).Roles)
                                foreach (LsSecuritySystemRole role in ((LsSecuritySystemUser)SecuritySystem.CurrentUser).Roles)
                                {
                                    //SecuritySystemRole _role = ObjectSpace.GetObject<SecuritySystemRole>(role);
                                    LsSecuritySystemRole _role = ObjectSpace.GetObject<LsSecuritySystemRole>(role);
                                    if (_role != null)
                                    {
                                        SecuritySystemActionPermissionObject permission = _role.ActionPermissions.FirstOrDefault(actionPermission => 
                                            actionPermission.actionId == action.Id);
                                        //SecuritySystemActionPermissionObject permission = 
                                        //    session.FindObject<SecuritySystemActionPermissionObject>(CriteriaOperator.Parse("actionId = ? and role = ?",
                                        //    action.Id, _role));
                                        if (permission != null)
                                            action.Active.SetItemValue("", permission.canExecute);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
