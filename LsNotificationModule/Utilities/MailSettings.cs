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
    [DefaultClassOptions, ImageName("emailSettings"), CreatableItem(false), NavigationItem("Settings")]
    public class MailSettings : BaseObject
    {
        #region Properties
        int _port;
        string _host;
        string _username;
        string _password;
        bool _enableSSL;
        private XPCollection<AuditDataItemPersistent> auditTrail;
        [DevExpress.Xpo.DisplayName("Port")]
        public int port
        {
            get
            {
                return _port;
            }
            set
            {
                SetPropertyValue("port", ref _port, value);
            }
        }
        [DevExpress.Xpo.DisplayName("Host")]
        public string host
        {
            get
            {
                return _host;
            }
            set
            {
                SetPropertyValue("host", ref _host, value);
            }
        }
        [DevExpress.Xpo.DisplayName("Username")]
        public string username
        {
            get
            {
                return _username;
            }
            set
            {
                SetPropertyValue("username", ref _username, value);
            }
        }
        [DevExpress.Xpo.DisplayName("Password"), PasswordPropertyText(true)]
        public string password
        {
            get
            {
                return _password;
            }
            set
            {
                SetPropertyValue("password", ref _password, value);
            }
        }
        [DevExpress.Xpo.DisplayName("Enable SSL")]
        public bool enableSSL
        {
            get
            {
                return _enableSSL;
            }
            set
            {
                SetPropertyValue("enableSSL", ref _enableSSL, value);
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
        public MailSettings(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }
        #endregion
        public static string mailSettingsId = string.Empty;

        //public static MailSettings GetInstance(Session session)
        //{
        //    MailSettings _settings = session.FindObject<MailSettings>(PersistentCriteriaEvaluationBehavior.InTransaction, null);
        //    if (_settings == null)
        //        _settings = new MailSettings(session) { port = 587 };
        //    return _settings;
        //}
        public static MailSettings GetInstance(Session session)
        {
            MailSettings _settings = null;
            if (string.IsNullOrEmpty(mailSettingsId))
                _settings = session.FindObject<MailSettings>(PersistentCriteriaEvaluationBehavior.InTransaction, null);
            else
                _settings = session.GetObjectByKey<MailSettings>(Guid.Parse(mailSettingsId));
            if (_settings == null)
            {
                _settings = new MailSettings(session) { port = 587 };
                _settings.Save();
            }
            mailSettingsId = _settings.Oid.ToString();
            return _settings;
        }

        protected override void OnDeleting()
        {
            using (XPCollection<MailSettings> mailSettings = new XPCollection<MailSettings>(Session))
            {
                if (mailSettings.Count == 1)
                    throw new Exception("Ne peut être supprimé !");
            }
        }
    }
}

