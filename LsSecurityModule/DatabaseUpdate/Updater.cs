using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using System.Collections.Generic;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;

namespace LsSecurityModule.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            UpdateSecurityPermission();
        }
        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
        }
        private void UpdateSecurityPermission()
        {
            IList<LsSecuritySystemRole> roles = ObjectSpace.GetObjects<LsSecuritySystemRole>();
            foreach (LsSecuritySystemRole role in roles)
            {
                foreach (SecuritySystemTypePermissionObject sstpo in role.TypePermissions)
                {
                    ITypeInfo typeClassInfo = XafTypesInfo.Instance.FindTypeInfo(sstpo.TargetType);
                    foreach (IMemberInfo memberInfo in typeClassInfo.Members)
                        if (/*memberInfo.IsVisible &&*/ memberInfo.IsProperty && memberInfo.IsPublic)
                        {
                            Session session = ((XPObjectSpace)ObjectSpace).Session;
                            SecuritySystemMemberPermissionsObject ssmpo = session.FindObject<SecuritySystemMemberPermissionsObject>(PersistentCriteriaEvaluationBehavior.InTransaction,
                                CriteriaOperator.Parse("Owner = ? and Members like ?", sstpo, memberInfo.Name));
                            //if (!ObjectSpace.IsNewObject(sstpo))
                            //    ssmpo = ObjectSpace.FindObject<SecuritySystemMemberPermissionsObject>(CriteriaOperator.Parse("Owner = ? and Members like ?",
                            //        sstpo, memberInfo.Name));
                            if (ssmpo == null)
                            {
                                ssmpo = new SecuritySystemMemberPermissionsObject(session)
                                {
                                    AllowRead = true,
                                    AllowWrite = true,
                                    EffectiveRead = true,
                                    EffectiveWrite = true,
                                    Members = memberInfo.Name
                                };
                                sstpo.MemberPermissions.Add(ssmpo);
                            }
                        }
                    sstpo.Save();
                }
                role.Save();
            }
            ObjectSpace.CommitChanges();
        }
    }
}
