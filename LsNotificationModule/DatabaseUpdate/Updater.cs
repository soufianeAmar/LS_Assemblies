using System;
using System.Security.Principal;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;

namespace LsNotificationModule.DatabaseUpdate
{
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            Session session = ((XPObjectSpace)ObjectSpace).Session;
            MailSettings mailSettings = MailSettings.GetInstance(session);
            if (string.IsNullOrEmpty(mailSettings.host))
            {
                mailSettings.host = "Mail.Leadersoft.dz";
                mailSettings.port = 587;
                mailSettings.username = "no-reply@leadersoft.dz";
                mailSettings.password = "HIbMr~05+],z";
                mailSettings.Save();
                ObjectSpace.CommitChanges();
            }
        }
    }
}
