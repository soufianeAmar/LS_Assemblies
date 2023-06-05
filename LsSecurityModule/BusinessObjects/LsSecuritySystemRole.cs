using DevExpress.ExpressApp.Security.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Security;
using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;

namespace LsSecurityModule
{
    [DefaultClassOptions, System.ComponentModel.DisplayName("Role"), NavigationItem("Security"), CreatableItem(false)]
    public class LsSecuritySystemRole : SecuritySystemRole
    {
        RoleType _roleType;
        [XafDisplayName("Role type")]
        public RoleType roleType
        {
            get
            {
                return _roleType;
            }
            set
            {
                SetPropertyValue("roleType", ref _roleType, value);
            }
        }

        private XPCollection<AuditDataItemPersistent> auditTrail;
        [Association("Role-ActionPermissions")]
        public XPCollection<SecuritySystemActionPermissionObject> ActionPermissions
        {
            get { return GetCollection<SecuritySystemActionPermissionObject>("ActionPermissions"); }
        }
        public XPCollection<AuditDataItemPersistent> AuditTrail
        {
            get
            {
                if (auditTrail == null)
                {
                    auditTrail = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditTrail;
            }
        }
        public LsSecuritySystemRole(Session session)
            : base(session)
        {
            
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (LsSecurityModule.currentApp != null)
            {
                IModelActions actions = LsSecurityModule.currentApp.Model.ActionDesign.Actions;
                foreach (IModelAction action in actions)
                {
                    SecuritySystemActionPermissionObject ssapo = Session.FindObject<SecuritySystemActionPermissionObject>(PersistentCriteriaEvaluationBehavior.InTransaction,
                        CriteriaOperator.Parse("role = ? and actionId = ?", this, action.Id));
                    if (ssapo == null)
                    {
                        ssapo = new SecuritySystemActionPermissionObject(Session)
                        {
                            actionId = action.Id,
                            canExecute = true,
                            role = this,
                        };
                        ssapo.Save();
                    }
                }
            }
        }

    }
}
