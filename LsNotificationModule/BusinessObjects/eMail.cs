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
using LsNotificationModule;
using LsSecurityModule;

namespace LsNotificationModule
{
    [DefaultClassOptions, ImageName("email"), NavigationItem("Settings")]
    public class eMail : BaseObject
    {
        #region properties
        string _subject;
        string _senderEMail;
        string _senderName;
        LsSecuritySystemUser _recipient;
        string _recipientEMail;
        string _recipientName;
        string _replyTo;
        eMailTemplate _eMailTemplate;
        string _body;
        Signature _signature;
        DateTime? _scheduledDate;
        DateTime _date;
        EMailState _sendingState;
        private XPCollection<AuditDataItemPersistent> auditTrail;
        [XafDisplayName("Subject"), ToolTip("Subject of the eMail")]
        public string subject
        {
            get
            {
                return _subject;
            }
            set
            {
                SetPropertyValue("subject", ref _subject, value);
            }
        }
        [XafDisplayName("From (eMail)"), ToolTip("eMail address of the sender")]
        public string senderEmail
        {
            get
            {
                return _senderEMail;
            }
            set
            {
                SetPropertyValue("senderEMail", ref _senderEMail, value);
            }
        }
        [XafDisplayName("From (Name)"), ToolTip("Name of the sender")]
        public string senderName
        {
            get
            {
                return _senderName;
            }
            set
            {
                SetPropertyValue("senderName", ref _senderName, value);
            }
        }
        [XafDisplayName("To user"), ToolTip("The user who should receive the e-mail")]
        public LsSecuritySystemUser recipient
        {
            get
            {
                return _recipient;
            }
            set
            {
                SetPropertyValue("recipient", ref _recipient, value);
                if (value != null)
                {
                    recipientName = value.fullName;
                    recipientEMail = value.eMail;
                }
            }
        }
        [XafDisplayName("To (eMail)"), ToolTip("eMail address of the recipient")]
        public string recipientEMail
        {
            get
            {
                return _recipientEMail;
            }
            set
            {
                SetPropertyValue("recipientEMail", ref _recipientEMail, value);
            }
        }
        [XafDisplayName("To (Name)"), ToolTip("Name of the recipient")]
        public string recipientName
        {
            get
            {
                return _recipientName;
            }
            set
            {
                SetPropertyValue("recipientName", ref _recipientName, value);
            }
        }
        [XafDisplayName("Reply to"), ToolTip("eMail address to reply to")]
        public string replyTo
        {
            get
            {
                return _replyTo;
            }
            set
            {
                SetPropertyValue("replyTo", ref _replyTo, value);
            }
        }
        [DevExpress.Xpo.DisplayName("Use a model")] //DataSourceProperty("eMailTemplateDataSource", DevExpress.Persistent.Base.DataSourcePropertyIsNullMode.SelectNothing)]
        public eMailTemplate eMailTemplate
        {
            get { return _eMailTemplate; }
            set { SetPropertyValue("eMailTemplate", ref _eMailTemplate, value); }
        }
        [XafDisplayName("Body"), ToolTip("eMail body"), Size(SizeAttribute.Unlimited)]
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
        [XafDisplayName("Signature"), ToolTip("Signature")]
        public Signature signature
        {
            get
            {
                return _signature;
            }
            set
            {
                SetPropertyValue("signature", ref _signature, value);
            }
        }
        [XafDisplayName("Scheduled date"), ToolTip("Scheduled date")]
        public DateTime? scheduledDate
        {
            get
            {
                return _scheduledDate;
            }
            set
            {
                SetPropertyValue("scheduledDate", ref _scheduledDate, value);
            }
        }
        [XafDisplayName("Date")]
        public DateTime date
        {
            get { return _date; }
            set { SetPropertyValue("date", ref _date, value); }
        }
        [XafDisplayName("Sending state"), ToolTip("Sending state")]
        public EMailState sendingState
        {
            get
            {
                return _sendingState;
            }
            set
            {
                SetPropertyValue("sendingState", ref _sendingState, value);
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
        [Association("EMail-FileAttachments"), DevExpress.Xpo.DisplayName("File attachments")]
        public XPCollection<FileAttachment> fileAttachments
        {
            get { return GetCollection<FileAttachment>("fileAttachments"); }
        }
        #endregion
        #region Initialization
        public eMail(Session session)
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

    public enum EMailState
    {
        ToSend,
        Sent,
        Failed,
        Cancelled
    }
}
