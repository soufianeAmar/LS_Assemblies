using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace LsNotificationModule
{
    [DefaultClassOptions, DefaultProperty("name"), ImageName("signature"), NavigationItem("Settings")]
    public class Signature : BaseObject
    {
        #region properties
        string _name;
        string _body;
        private XPCollection<AuditDataItemPersistent> auditTrail;
        [XafDisplayName("Name"), ToolTip("Name of the signature")]
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                SetPropertyValue("name", ref _name, value);
            }
        }
        [XafDisplayName("Body"), ToolTip("Signature body"), Size(SizeAttribute.Unlimited)]
        public string body
        {
            get
            {
                return _body;
            }
            set
            {
                SetPropertyValue("body", ref _body, value);
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
        #region Initialization
        public Signature(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }
        #endregion
    }
}
