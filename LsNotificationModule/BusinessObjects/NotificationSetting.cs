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
using System.Reflection;
using LsSecurityModule;

namespace LsNotificationModule
{
    [DefaultClassOptions, ImageName("notificationSettings"), NavigationItem("Settings")]
    public class NotificationSetting : BaseObject
    {
        #region Fields
        string _id;
        Type _objectType;
        string _objectTypeName;
        NotificationType _notificationType;
        string _dateExpression;
        DateTime _dateValue;
        string _condition;
        eMailTemplate _mailTemplate;
        bool _sendMail;
        string _listePropriétés;
        //LsSecuritySystemUser _user;
        private XPCollection<AuditDataItemPersistent> auditTrail;
        #endregion
        #region Properties
        [XafDisplayName("ID"), RuleUniqueValue]
        public string id
        {
            get
            {
                return _id;
            }
            set
            {
                SetPropertyValue("id", ref _id, value);
            }
        }
        [XafDisplayName("Object type"), ImmediatePostData(true)]
        public Type objectType
        {
            get
            {
                //string assemblyName = System.IO.Path.ChangeExtension(objectTypeName, ".dll");
                //Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().ToList().Find(a => a.ManifestModule.Name == assemblyName);
                if (string.IsNullOrEmpty(objectTypeName))
                    return _objectType;
                else
                    //return assembly.GetType(objectTypeName);
                    //return Type.GetType(objectTypeName);
                    return XafTypesInfo.Instance.FindTypeInfo(objectTypeName).Type;
            }
            set
            {
                SetPropertyValue("objectType", ref _objectType, value);
                objectTypeName = value != null ? value.FullName : null;
                if (!IsLoading)
                {
                    if (objectType != null)
                    {
                        PropertyInfo[] props = objectType.GetProperties();
                        listePropriétés = "";
                        foreach (PropertyInfo prp in props)
                        {
                            listePropriétés += prp.Name + "\r\n";
                        }
                    }
                }
                //AssemblyName an = value != null ? value.Assembly.GetName() : null;
                //objectTypeName = an != null ? string.Format("{0}, {1}, Culture={2}", value.FullName, an.Name, an.CultureInfo.Name) : null;
                ////objectTypeName = value != null ? value.AssemblyQualifiedName : null;
            }
        }
        [XafDisplayName("Object type name"), Persistent, Size(SizeAttribute.Unlimited)]
        public string objectTypeName
        {
            get
            {
                return _objectTypeName;
            }
            set
            {
                SetPropertyValue("objectTypeName", ref _objectTypeName, value);
            }
        }
        [XafDisplayName("Notification type")]
        public NotificationType notificationType
        {
            get
            {
                return _notificationType;
            }
            set
            {
                SetPropertyValue("notificationType", ref _notificationType, value);
            }
        }
        [XafDisplayName("Date expression")]
        public string dateExpression
        {
            get
            {
                return _dateExpression;
            }
            set
            {
                SetPropertyValue("dateExpression", ref _dateExpression, value);
            }
        }
        [XafDisplayName("Date value")]
        public DateTime dateValue
        {
            get
            {
                return _dateValue;
            }
            set
            {
                SetPropertyValue("dateValue", ref _dateValue, value);
            }
        }
        [XafDisplayName("Condition"), Size(500)]
        public string condition
        {
            get
            {
                return _condition;
            }
            set
            {
                SetPropertyValue("condition", ref _condition, value);
            }
        }
        [XafDisplayName("eMail template"), RuleRequiredField("emailTemplateRequired", DefaultContexts.Save)]
        public eMailTemplate mailTemplate
        {
            get
            {
                return _mailTemplate;
            }
            set
            {
                SetPropertyValue("mailTemplate", ref _mailTemplate, value);
            }
        }
        [DevExpress.Xpo.DisplayName("Liste des propriétés"), ModelDefault("AllowEdit", "False")]
        [Size(SizeAttribute.Unlimited)]
        public string listePropriétés
        {
            get { return _listePropriétés; }
            set { SetPropertyValue("listePropriétés", ref _listePropriétés, value); }
        }
        //[XafDisplayName("Send an e-Mail")]
        //public bool sendMail
        //{
        //    get
        //    {
        //        return _sendMail;
        //    }
        //    set
        //    {
        //        SetPropertyValue("sendMail", ref _sendMail, value);
        //    }
        //}
        //[XafDisplayName("User")]
        //public LsSecuritySystemUser user
        //{
        //    get
        //    {
        //        return _user;
        //    }
        //    set
        //    {
        //        SetPropertyValue("user", ref _user, value);
        //    }
        //}
        [XafDisplayName("Users")]
        [Association("NotificationSetting-Users")]
        public XPCollection<NotificationUser> users
        {
            get
            {
                return GetCollection<NotificationUser>("users");
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
        #region Controls
        [RuleFromBoolProperty("isMailTemplateValid", DefaultContexts.Save, CustomMessageTemplate = "E-Mail template is missing some information.\n")]
        public bool isMailTemplateValid
        {
            get
            {
                bool _sendMail = false;
                foreach (NotificationUser nu in users)
                {
                    if (nu.sendMail)
                        _sendMail = true;
                }
                return !_sendMail | (mailTemplate != null && !(string.IsNullOrEmpty(mailTemplate.subject) | string.IsNullOrEmpty(mailTemplate.senderEmail) |
                    string.IsNullOrEmpty(mailTemplate.body)));
            }
        }
        #endregion
        public NotificationSetting(Session session)
            : base(session)
        {

        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }

    public enum NotificationType
    {
        OnCreatingObject,
        OnDateExpression,
        OnDateValue,
        OnCondition
    }
}
