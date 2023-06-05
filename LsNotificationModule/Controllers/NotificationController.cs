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
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.Metadata;
using System.Threading;
using DevExpress.Persistent.BaseImpl;
using System.Diagnostics;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace LsNotificationModule
{
    public partial class NotificationController : ViewController
    {
        public NotificationController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            if (MailUtils.enableNotifications)
                ObjectSpace.ObjectSaving += ObjectSpace_ObjectSaving;
        }

        void ObjectSpace_ObjectSaving(object sender, ObjectManipulatingEventArgs e)
        {
            IObjectSpace objectSpace = (IObjectSpace)sender;
            if (objectSpace != null)
            {
                string dbName = ((XPObjectSpace)sender).Database;
                if (dbName.Contains('.'))
                {
                    string[] dbComponents = dbName.Split('.');
                    dbName = dbComponents[1];
                }
                //bool IsPFAdmin = dbName == "PFAdmin";
                //bool isLSAdmin = false;
                XPBaseObject currentObject = (XPBaseObject)e.Object;
                Session session = currentObject.Session;
                //System.Data.IDbConnection connection = ((DevExpress.Xpo.DB.ConnectionProviderSql)(((DevExpress.Xpo.Helpers.BaseDataLayer)session.DataLayer).ConnectionProvider)).Connection;
                //System.Data.IDbConnection userConnection = ((DevExpress.Xpo.DB.ConnectionProviderSql)(((DevExpress.Xpo.Helpers.BaseDataLayer)((XPObjectSpace)objectSpace).Session.DataLayer).ConnectionProvider)).Connection;
                //if (connection is SQLiteConnection)
                //    isLSAdmin = ((SQLiteConnection)userConnection).DataSource == "LSAdmin";
                //else if (connection is SqlConnection)
                //    isLSAdmin = userConnection.Database == "LSAdmin";// && lsactvtn.ActivationClass.réseau;
                //if (!isLSAdmin)
                if (!LsSecurityModule.Helper.IsLSAdmin(objectSpace))
                {
                    Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tProcessing notifications...", DateTime.Now));
                    Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tCurrent object is : {1}", DateTime.Now, currentObject));
                    XPCollection<NotificationSetting> notifications = new XPCollection<NotificationSetting>(session,
                        CriteriaOperator.Parse("objectType = ?", currentObject.ClassInfo.ClassType));
                    foreach (NotificationSetting ns in notifications)
                    {
                        string id = string.Format("{0} : {1}", ns.id, ((BaseObject)currentObject).Oid);
                        Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tNotification ID = {1}", DateTime.Now, id));
                        XPCollection<NotificationItem> notificationItems = new XPCollection<NotificationItem>(PersistentCriteriaEvaluationBehavior.InTransaction,
                            session, CriteriaOperator.Parse("id = ?", id));
                        if (currentObject.IsDeleted)
                        {
                            session.Delete(notificationItems);
                        }
                        else
                        {
                            //object[] parameters = null;
                            //if (!string.IsNullOrEmpty(ns.mailTemplate.bodyParameters))
                            //{
                            //    List<object> parameterValues = new List<object>();
                            //    string[] parameterNames = ns.mailTemplate.bodyParameters.Split(';');
                            //    foreach (string parameter in parameterNames)
                            //    {
                            //        if (parameter.StartsWith("$"))
                            //        {
                            //            if (parameter.Contains('.'))
                            //            {
                            //                string[] memberNames = parameter.Substring(1).Split('.');
                            //                XPBaseObject obj = currentObject;
                            //                for (int i = 0; i < memberNames.Length-1; i++)
                            //                {
                            //                    obj = (XPBaseObject)obj.GetMemberValue(memberNames[i]);
                            //                }
                            //                parameterValues.Add(obj.GetMemberValue(memberNames[memberNames.Length - 1]));
                            //            }
                            //            else
                            //                parameterValues.Add(currentObject.GetMemberValue(parameter.Substring(1)));
                            //        }
                            //        else
                            //            parameterValues.Add(parameter);
                            //    }
                            //    parameters = parameterValues.ToArray();
                            //}
                            Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tObtaining message parameters...", DateTime.Now, id));
                            object[] parameters = MailUtils.GetParameters(ns.mailTemplate, (BaseObject)currentObject);
                            string notificationBody = parameters != null ? string.Format(ns.mailTemplate.body, parameters) : ns.mailTemplate.body;
                            IObjectSpace mailOS = LSAdmin.Helper.currentApp.CreateObjectSpace();
                            switch (ns.notificationType)
                            {
                                #region OnCreatingObject
                                case NotificationType.OnCreatingObject:
                                    {
                                        Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tNotification type = OnCreatingObject", DateTime.Now));
                                        if (session.IsNewObject(currentObject) && notificationItems.Count == 0)
                                        {
                                            Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tCreating notifications...", DateTime.Now));
                                            foreach (NotificationUser nu in ns.users)
                                            {
                                                MailUtils.CreateNotification(session, id, nu.user, ns.mailTemplate.subject, notificationBody, DateTime.Now, new TimeSpan(0));
                                                if (nu.sendMail)
                                                {
                                                    eMail mail = MailUtils.CreateEMail(mailOS, ns.mailTemplate, nu.user, parameters);
                                                    Thread smThread = new Thread(() => MailUtils.SendMail(mail));
                                                    smThread.Start();
                                                    //MailUtils.SendMail(mail);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                #endregion
                                #region OnCondition
                                case NotificationType.OnCondition:
                                    {
                                        Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tNotification type = OnCondition", DateTime.Now));
                                        MailUtils.ProcessNotification(mailOS, ns, false);
                                        break;
                                    }
                                #endregion
                                #region OnDateExpression
                                case NotificationType.OnDateExpression:
                                    {
                                        Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tNotification type = OnDateExpression", DateTime.Now));
                                        if (notificationItems.Count == 0)
                                        {
                                            DateTime dueDate = (DateTime)currentObject.Evaluate(ns.dateExpression);
                                            Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tCreating notifications...", DateTime.Now));
                                            foreach (NotificationUser nu in ns.users)
                                            {
                                                MailUtils.CreateNotification(session, id, nu.user, ns.mailTemplate.subject, notificationBody, dueDate, new TimeSpan(0));
                                                if (nu.sendMail)
                                                {
                                                    eMail mail = MailUtils.CreateEMail(mailOS, ns.mailTemplate, nu.user, parameters);
                                                    mail.scheduledDate = dueDate;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                #endregion
                                #region OnDateValue
                                case NotificationType.OnDateValue:
                                    {
                                        Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tNotification type = OnDateValue", DateTime.Now));
                                        if (notificationItems.Count == 0)
                                        {
                                            Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tCreating notifications...", DateTime.Now));
                                            foreach (NotificationUser nu in ns.users)
                                            {
                                                MailUtils.CreateNotification(session, id, nu.user, ns.mailTemplate.subject, notificationBody, ns.dateValue, new TimeSpan(0));
                                                if (nu.sendMail)
                                                {
                                                    eMail mail = MailUtils.CreateEMail(mailOS, ns.mailTemplate, nu.user, parameters);
                                                    mail.scheduledDate = ns.dateValue;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                #endregion
                                #region default
                                default:
                                    {
                                        break;
                                    }
                                #endregion
                            }
                        }
                    }
                }
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            ObjectSpace.ObjectSaving -= ObjectSpace_ObjectSaving;
            base.OnDeactivated();
        }

        private void aSendEmail_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            eMail mail = (eMail)e.CurrentObject;
            if (mail != null)
                MailUtils.SendMail(mail);
        }
    }
}
