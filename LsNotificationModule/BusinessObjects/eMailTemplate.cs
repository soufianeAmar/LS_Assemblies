using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using DevExpress.Xpo.Metadata;
using System.Collections;
using DevExpress.Xpo.Metadata.Helpers;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;

namespace LsNotificationModule
{
    [DefaultClassOptions, ImageName("emailTemplate"), NavigationItem("Settings"), DefaultProperty("name")]
    public class eMailTemplate : BaseObject, ICheckedListBoxItemsProvider
    {
        #region properties
        string _name;
        string _subject;
        string _senderEMail;
        string _senderName;
        string _replyTo;
        string _body;
        string _bodyParameters;
        Signature _signature;
        Type _objectType;
        string _objectTypeName;
        private XPCollection<AuditDataItemPersistent> auditTrail;
        [XafDisplayName("Name"), ToolTip("Name of the eMail template"), RuleRequiredField]
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                SetPropertyValue("name", ref _name, value);
            }
        }
        //string _appliesTo;
        //[XafDisplayName("Applies to"), ToolTip("Persistent object concerned by the eMail")]
        //public string appliesTo
        //{
        //    get
        //    {
        //        return _appliesTo;
        //    }
        //    set
        //    {
        //        SetPropertyValue("appliesTo", ref _appliesTo, value);
        //    }
        //}
        [XafDisplayName("Subject"), ToolTip("Subject of the eMail"), RuleRequiredField]
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
        [XafDisplayName("From (eMail)"), ToolTip("eMail address of the sender"), RuleRequiredField]
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
        //string _recipientEMail;
        //[XafDisplayName("To (eMail)"), ToolTip("eMail address of the recipient")]
        //public string recipientEMail
        //{
        //    get
        //    {
        //        return _recipientEMail;
        //    }
        //    set
        //    {
        //        SetPropertyValue("recipientEMail", ref _recipientEMail, value);
        //    }
        //}
        //string _recipientName;
        //[XafDisplayName("To (Name)"), ToolTip("Name of the recipient")]
        //public string recipientName
        //{
        //    get
        //    {
        //        return _recipientName;
        //    }
        //    set
        //    {
        //        SetPropertyValue("recipientName", ref _recipientName, value);
        //    }
        //}
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
        [XafDisplayName("Body"), ToolTip("eMail body"), Size(SizeAttribute.Unlimited), RuleRequiredField]
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
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        public string bodyParameters
        {
            get
            {
                return _bodyParameters;
            }
            set
            {
                SetPropertyValue("bodyParameters", ref _bodyParameters, value);
            }
        }
        [ValueConverter(typeof(TypeToStringConverter))]
        [TypeConverter(typeof(LocalizedClassInfoTypeConverter))]
        [XafDisplayName("Object type")]//, ImmediatePostData(true)]
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
                    if (objectType != null && (string.IsNullOrEmpty(bodyParameters)))
                    {
                        OnChanged("bodyParameters");
                        OnItemsChanged();
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
        public eMailTemplate(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }
        #endregion

       #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> properties = new Dictionary<object, string>();
            if (targetMemberName == "bodyParameters" && objectType != null)
            {
                ITypeInfo typeInfo = XafTypesInfo.Instance.FindTypeInfo(objectType);
                foreach (IMemberInfo memberInfo in typeInfo.Members)
                {
                    if (memberInfo.IsVisible)
                    {
                        properties.Add("$" + memberInfo.Name, CaptionHelper.GetMemberCaption(typeInfo, memberInfo.Name));
                    }
                }
            }
            return properties;
        }
        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }
        #endregion
    }
}

