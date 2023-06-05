using System;
using System.Security.Principal;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace LSAdmin.DatabaseUpdate
{
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            //PaymentPlan FreePlan = ObjectSpace.FindObject<PaymentPlan>("name = 'FREE'");
            //if (FreePlan == null)
            //{
            //    FreePlan = ObjectSpace.CreateObject<PaymentPlan>();
            //    FreePlan.name = "FREE";
            //    ObjectSpace.CommitChanges();
            //}
        }
    }
}
