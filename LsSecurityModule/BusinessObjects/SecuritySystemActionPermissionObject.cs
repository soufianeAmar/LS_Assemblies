using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace LsSecurityModule
{
    [DefaultClassOptions, System.ComponentModel.DisplayName("Action Permission")]
    [NavigationItem(false), CreatableItem(false)]
    public class SecuritySystemActionPermissionObject : BaseObject
    {
        ActionRef _actionRef;
        string _actionId;
        LsSecuritySystemRole _role;
        bool _canExecute;
        private IList<ActionRef> _actionIdList;
        private XPCollection<AuditDataItemPersistent> auditTrail;
        [DevExpress.Xpo.DisplayName("Target"), DataSourceProperty("actionIdList"), NonPersistent, ImmediatePostData(true)]
        public ActionRef actionRef
        {
            get
            {
                if (string.IsNullOrEmpty(actionId))
                    return _actionRef;
                else
                    return new ActionRef(Session) { actionId = actionId };
            }
            set
            {
                SetPropertyValue("actionRef", ref _actionRef, value);
                if (value != null)
                    actionId = value.actionId;
            }
        }
        public string actionId
        {
            get
            {
                return _actionId;
            }
            set
            {
                SetPropertyValue("actionId", ref _actionId, value);
            }
        }
        [DevExpress.Xpo.DisplayName("Can execute")]
        public bool canExecute
        {
            get
            {
                return _canExecute;
            }
            set
            {
                SetPropertyValue("canExecute", ref _canExecute, value);
            }
        }
        [DevExpress.Xpo.DisplayName("Role")]
        [Association("Role-ActionPermissions")]
        public LsSecuritySystemRole role
        {
            get
            {
                return _role;
            }
            set
            {
                SetPropertyValue("role", ref _role, value);
            }
        }

        [Browsable(false), NonPersistent]
        public IEnumerable<ActionRef> actionIdList
        {
            get
            {
                if (_actionIdList == null)
                {
                    _actionIdList = CreateActionIdList();
                }
                return _actionIdList;
            }
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
        public SecuritySystemActionPermissionObject(Session session)
            : base(session)
        {

        }

        private IList<ActionRef> CreateActionIdList()
        {
            IList<ActionRef> result = new List<ActionRef>();
            IModelActions actions = LsSecurityModule.currentApp.Model.ActionDesign.Actions;
            foreach (IModelAction action in actions)
            {
                result.Add(new ActionRef(Session) { actionId = action.Id });
            }
            return result;
        }

    }
}
