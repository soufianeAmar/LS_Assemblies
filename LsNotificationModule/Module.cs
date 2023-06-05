using System;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Notifications;
using DevExpress.Data.Filtering;
using System.Threading;

namespace LsNotificationModule
{
    public sealed partial class LsNotificationModule : ModuleBase
    {
        public LsNotificationModule()
        {
            InitializeComponent();
        }

        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            application.LoggedOn += application_LoggedOn;
        }

        void application_LoggedOn(object sender, LogonEventArgs e)
        {
            NotificationsModule notificationsModule = Application.Modules.FindModule<NotificationsModule>();
            DefaultNotificationsProvider notificationsProvider = notificationsModule.DefaultNotificationsProvider;
            notificationsProvider.CustomizeNotificationCollectionCriteria += notificationsProvider_CustomizeNotificationCollectionCriteria;
            Thread pmThread = new Thread(() => MailUtils.ProcessEMails(Application.CreateObjectSpace()));
            pmThread.Start();
        }

        void notificationsProvider_CustomizeNotificationCollectionCriteria(object sender, DevExpress.Persistent.Base.General.CustomizeCollectionCriteriaEventArgs e)
        {
            if (e.Type == typeof(NotificationItem))
            {
                e.Criteria = CriteriaOperator.Parse("user is null || user.Oid == CurrentUserId()");//, SecuritySystem.CurrentUser);
            }
        }
    }
}
