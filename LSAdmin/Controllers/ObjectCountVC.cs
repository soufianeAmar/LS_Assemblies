using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.Windows.Forms;
using DevExpress.ExpressApp.FileAttachments.Win;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Utils.Base;

namespace LSAdmin
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ObjectCountVC : ViewController
    {
        public ObjectCountVC()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            NewObjectViewController novc = Frame.GetController<NewObjectViewController>();
            if (novc != null)
                novc.ObjectCreating += novc_ObjectCreating;
            FileAttachmentListViewController falvc = Frame.GetController<FileAttachmentListViewController>();
            if (falvc != null)
                falvc.AddFromFileAction.Executing += AddFromFileAction_Executing;
            ObjectSpace.ObjectSaving += ObjectSpace_ObjectSaving;
        }

        void ObjectSpace_ObjectSaving(object sender, ObjectManipulatingEventArgs e)
        {
            XPObjectSpace objectSpace = (XPObjectSpace)XPObjectSpace.FindObjectSpaceByObject(e.Object); //(XPObjectSpace)sender;
            if (objectSpace != null)
            {
                if (objectSpace.IsNewObject(e.Object) && !objectSpace.IsDeletedObject(e.Object))
                {
                    int maxObjectCount = lsactvtn.ActivationClass.Demo ? Core.DemoRecordCount : lsactvtn.ActivationClass.nombreEnregistrements;
                    if (maxObjectCount > 0)
                    {
                        if (Core.IsLimited(e.Object.GetType()))
                        {
                            System.Diagnostics.Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tObject count limited. Verifying...", DateTime.Now));
                            System.Diagnostics.Trace.WriteLine(e.Object.ToString());
                            int objectCount = objectSpace.GetObjectsCount(e.Object.GetType(), null);
                            System.Diagnostics.Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tobject count = {1}, max object count = {2}",
                                DateTime.Now, objectCount, maxObjectCount));
                            if (objectCount >= maxObjectCount)
                            {
                                MessageBox.Show("Vous avez atteint le nombre d'enregistrements maximum autrorisé. Veuillez contacter l'éditeur du logiciel pour plus d'information",
                                    "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                throw new CancelSavingException();
                            }
                        }
                    }
                    else
                        System.Diagnostics.Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tMaximum object number is null. no limit definied.", DateTime.Now));

                    if (Helper.checkPlan)
                        if (IsPlanLimitReached(View.ObjectTypeInfo.Type))
                        {
                            MessageBox.Show("Vous avez atteint le nombre d'enregistrements maximum autrorisé. Veuillez contacter l'éditeur du logiciel pour plus d'information",
                                "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            throw new CancelSavingException();
                        }
                }
            }
        }

        void AddFromFileAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int maxObjectCount = lsactvtn.ActivationClass.Demo ? Core.DemoRecordCount : lsactvtn.ActivationClass.nombreEnregistrements;
            if (maxObjectCount > 0)
            {
                Type objectType = View.ObjectTypeInfo.Type;
                if (Core.IsLimited(objectType))
                {
                    Session session = ((XPObjectSpace)ObjectSpace).Session;
                    int objectCount = Convert.ToInt32(session.Evaluate(objectType, CriteriaOperator.Parse("Count()"), null));
                    e.Cancel = objectCount >= maxObjectCount;
                    if (e.Cancel)
                        MessageBox.Show("Vous avez atteint le nombre d'enregistrements maximum autrorisé. Veuillez contacter l'éditeur du logiciel pour plus d'information",
                            "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            if (!e.Cancel && Helper.checkPlan)
            {
                e.Cancel = IsPlanLimitReached(View.ObjectTypeInfo.Type);
                if (e.Cancel)
                    MessageBox.Show("Vous avez atteint le nombre d'enregistrements maximum autrorisé. Veuillez contacter l'éditeur du logiciel pour plus d'information",
                        "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void CheckMaximumFolder(ObjectCreatingEventArgs e)
        {
            int folderCount = e.ObjectSpace.GetObjectsCount(e.ObjectType, null);
            if (lsactvtn.ActivationClass.nombreDossiers > 0)
            {
                e.Cancel = folderCount >= lsactvtn.ActivationClass.nombreDossiers;
                if (e.Cancel)
                    MessageBox.Show("Vous avez atteint le nombre de dossiers maximum autrorisé. Veuillez contacter l'éditeur du logiciel pour plus d'information",
                        "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void CheckMaximumObject(ObjectCreatingEventArgs e)
        {
            int maxObjectCount = lsactvtn.ActivationClass.Demo ? Core.DemoRecordCount : lsactvtn.ActivationClass.nombreEnregistrements;
            if (maxObjectCount > 0)
            {
                if (Core.IsLimited(e.ObjectType))
                {
                    int objectCount = ObjectSpace.GetObjectsCount(e.ObjectType, null);
                    e.Cancel = objectCount >= maxObjectCount;
                    if (e.Cancel)
                        MessageBox.Show("Vous avez atteint le nombre d'enregistrements maximum autrorisé. Veuillez contacter l'éditeur du logiciel pour plus d'information",
                            "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }
        private bool IsPlanLimitReached(Type ObjectType)
        {
            string currentUserName = SecuritySystem.CurrentUserName;
            IObjectSpace AdminOS = LSAdmin.Core.CreateWebAdminObjectSpace(LSAdmin.Helper.currentApp);
            if (AdminOS != null)
            {
                LsUser currentUser = AdminOS.FindObject<LsUser>(CriteriaOperator.Parse("userName = ?", currentUserName));
                if (currentUser != null)
                {
                    PaymentPlan plan = currentUser.exercices.ElementAt(0).subscriptionOwner.paymentPlan;
                    List<PlanLimitation> limitations = plan.limitations.Where<PlanLimitation>(pl => pl.objectTypeName == ObjectType.FullName).ToList<PlanLimitation>();
                    if (limitations.Count > 0)
                    {
                        int maxObjectCount = limitations.ElementAt(0).objectCount;
                        if (maxObjectCount > 0)
                        {
                            int objectCount = ObjectSpace.GetObjectsCount(ObjectType, null);
                            return objectCount >= maxObjectCount;
                        }
                    }
                }
            }
            return false;
        }
        private void CheckPlan(ObjectCreatingEventArgs e)
        {
            e.Cancel = IsPlanLimitReached(e.ObjectType);
            if (e.Cancel)
                MessageBox.Show("Vous avez atteint le nombre d'enregistrements maximum autrorisé. Veuillez contacter l'éditeur du logiciel pour plus d'information",
                    "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //string currentUserName = SecuritySystem.CurrentUserName;
            //IObjectSpace AdminOS = LSAdmin.Core.CreateWebAdminObjectSpace(LSAdmin.Helper.currentApp);
            //if (AdminOS != null)
            //{
            //    LsUser currentUser = AdminOS.FindObject<LsUser>(CriteriaOperator.Parse("userName = ?", currentUserName));
            //    if (currentUser != null)
            //    {
            //        PaymentPlan plan = currentUser.exercices.ElementAt(0).subscriptionOwner.paymentPlan;
            //        List<PlanLimitation> limitations = plan.limitations.Where<PlanLimitation>(pl => pl.objectTypeName == e.ObjectType.FullName).ToList<PlanLimitation>();
            //        if (limitations.Count > 0)
            //        {
            //            int maxObjectCount = limitations.ElementAt(0).objectCount;
            //            if (maxObjectCount > 0)
            //            {
            //                int objectCount = ObjectSpace.GetObjectsCount(e.ObjectType, null);
            //                e.Cancel = objectCount >= maxObjectCount;
            //                if (e.Cancel)
            //                    MessageBox.Show("Vous avez atteint le nombre d'enregistrements maximum autrorisé. Veuillez contacter l'éditeur du logiciel pour plus d'information",
            //                        "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //            }
            //        }
            //    }
            //}
        }
        void novc_ObjectCreating(object sender, ObjectCreatingEventArgs e)
        {
            if (e.ObjectType == typeof(Dossier))
                CheckMaximumFolder(e);
            if (Core.IsLimited(e.ObjectType))
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tObject count limited. Verifying...", DateTime.Now));
                CheckMaximumObject(e);
            }
            if (Helper.checkPlan)
                CheckPlan(e);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            NewObjectViewController novc = Frame.GetController<NewObjectViewController>();
            if (novc != null)
                novc.ObjectCreating -= novc_ObjectCreating;
            FileAttachmentListViewController falvc = Frame.GetController<FileAttachmentListViewController>();
            if (falvc != null)
                falvc.AddFromFileAction.Executing -= AddFromFileAction_Executing;
            ObjectSpace.ObjectSaving -= ObjectSpace_ObjectSaving;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
