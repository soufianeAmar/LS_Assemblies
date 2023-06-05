using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace LsNotificationModule
{
    [DefaultClassOptions, NavigationItem(false), CreatableItem(false)]
    public class FileAttachment : FileAttachmentBase
    {
        #region Propriétés
        private eMail _email;
        private XPCollection<AuditDataItemPersistent> auditTrail;
        [Association("EMail-FileAttachments"), DevExpress.Xpo.DisplayName("eMail")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public eMail email
        {
            get
            {
                return _email;
            }
            set
            {
                SetPropertyValue("mail", ref _email, value);
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
        #endregion
        public FileAttachment(Session session)
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
        }
    }

}

