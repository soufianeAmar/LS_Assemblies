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
using LsSecurityModule;

namespace LsNotificationModule
{
    [DefaultClassOptions, NavigationItem(false)]
    public class NotificationUser : BaseObject
    {
        LsSecuritySystemUser _user;
        bool _sendMail;
        NotificationSetting _notificationSettings;
        [XafDisplayName("User")]
        public LsSecuritySystemUser user
        {
            get
            {
                return _user;
            }
            set
            {
                SetPropertyValue("user", ref _user, value);
            }
        }
        [XafDisplayName("Send an e-Mail")]
        public bool sendMail
        {
            get
            {
                return _sendMail;
            }
            set
            {
                SetPropertyValue("sendMail", ref _sendMail, value);
            }
        }
        [Association("NotificationSetting-Users")]
        public NotificationSetting notificationSettings
        {
            get
            {
                return _notificationSettings;
            }
            set
            {
                SetPropertyValue("notificationSettings", ref _notificationSettings, value);
            }
        }
        public NotificationUser(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }
    }
}
