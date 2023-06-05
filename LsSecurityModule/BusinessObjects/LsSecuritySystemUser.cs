using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.Base;
using System.Drawing;
using DevExpress.Persistent.BaseImpl;
namespace LsSecurityModule
{
    [DefaultClassOptions, System.ComponentModel.DisplayName("User"), NavigationItem("Security"), CreatableItem(false)]
    public class LsSecuritySystemUser : SecuritySystemUser
    {
        #region Properties
        string _firstName;
        [DevExpress.Xpo.DisplayName("First name")]
        public string firstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                SetPropertyValue("firstName", ref _firstName, value);
            }
        }
        string _middleName;
        [DevExpress.Xpo.DisplayName("Middle name")]
        public string middleName
        {
            get
            {
                return _middleName;
            }
            set
            {
                SetPropertyValue("middleName", ref _middleName, value);
            }
        }
        string _lastName;
        [DevExpress.Xpo.DisplayName("Last name")]
        public string lastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                SetPropertyValue("lastName", ref _lastName, value);
            }
        }
        [DevExpress.Xpo.DisplayName("Full name"), Persistent("fullName")]
        public string fullName
        {
            get
            {
                string _fullName = null;
                if (!string.IsNullOrEmpty(firstName)) 
                    _fullName += string.Format("{0} ", firstName);
                if (!string.IsNullOrEmpty(middleName)) 
                    _fullName += string.Format("{0} ", middleName);
                if (!string.IsNullOrEmpty(lastName)) 
                    _fullName += string.Format("{0} ", lastName);
                return _fullName;
            }
        }
        string _eMail;
        [DevExpress.Xpo.DisplayName("e-Mail")]
        public string eMail
        {
            get
            {
                return _eMail;
            }
            set
            {
                SetPropertyValue("eMail", ref _eMail, value);
            }
        }
        string _cellPhone;
        [DevExpress.Xpo.DisplayName("Cell phone")]
        public string cellPhone
        {
            get
            {
                return _cellPhone;
            }
            set
            {
                SetPropertyValue("cellPhone", ref _cellPhone, value);
            }
        }
        string _landLine;
        [DevExpress.Xpo.DisplayName("Land line")]
        public string landLine
        {
            get
            {
                return _landLine;
            }
            set
            {
                SetPropertyValue("landLine", ref _landLine, value);
            }
        }
        string _gmailAccount;
        [DevExpress.Xpo.DisplayName("GMail account")]
        public string gmailAccount
        {
            get
            {
                return _gmailAccount;
            }
            set
            {
                SetPropertyValue("gmailAccount", ref _gmailAccount, value);
            }
        }
        string _facebookAccount;
        [DevExpress.Xpo.DisplayName("Facebook account")]
        public string facebookAccount
        {
            get
            {
                return _facebookAccount;
            }
            set
            {
                SetPropertyValue("facebookAccount", ref _facebookAccount, value);
            }
        }
        string _twitterAccount;
        [DevExpress.Xpo.DisplayName("Twitter account")]
        public string twitterAccount
        {
            get
            {
                return _twitterAccount;
            }
            set
            {
                SetPropertyValue("twitterAccount", ref _twitterAccount, value);
            }
        }
        string _linkedinAccount;
        [DevExpress.Xpo.DisplayName("LinkedIn account")]
        public string linkedinAccount
        {
            get
            {
                return _linkedinAccount;
            }
            set
            {
                SetPropertyValue("linkedinAccount", ref _linkedinAccount, value);
            }
        }
        Image _picture;
        private XPCollection<AuditDataItemPersistent> auditTrail;
        [DevExpress.Xpo.DisplayName("Picture")]
        public Image picture
        {
            get
            {
                return _picture;
            }
            set
            {
                SetPropertyValue("picture", ref _picture, value);
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
        public LsSecuritySystemUser(DevExpress.Xpo.Session session)
            : base(session)
        {
            
        }
    }
}
