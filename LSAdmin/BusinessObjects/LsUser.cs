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
using LSAdmin;

namespace LSAdmin
{
    [DefaultClassOptions, NavigationItem("Administration"), ImageName("BO_User"), DefaultProperty("userName"), CreatableItem(false)]
    public class LsUser : BaseObject
    {
        #region Fields
        string _firstName;
        string _surName;
        string _email;
        bool _emailConfirmed;
        string _userName;
        string _password;
        //bool _isSubscriptionOwner;
        //LsUser _subscriptionOwner;
        bool _active;
        DateTime _createdOn;
        DateTime _subscriptionExpiresOn;
        PaymentPlan _paymentPlan;
        #endregion

        #region Properties
        [XafDisplayName("First Name")]//, ImmediatePostData(true)]
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
        [XafDisplayName("Surname")]//, ImmediatePostData(true)]
        public string surName
        {
            get
            {
                return _surName;
            }
            set
            {
                SetPropertyValue("surName", ref _surName, value);
            }
        }
        [XafDisplayName("Full name"), Persistent("fullName")]
        public string fullName
        {
            get { return string.Format("{0} {1}", firstName, surName); }
        }
        [XafDisplayName("e-Mail address")]
        public string email
        {
            get
            {
                return _email;
            }
            set
            {
                SetPropertyValue("email", ref _email, value);
            }
        }
        [XafDisplayName("e-Mail confirmed")]
        public bool emailConfirmed
        {
            get
            {
                return _emailConfirmed;
            }
            set
            {
                SetPropertyValue("emailConfirmed", ref _emailConfirmed, value);
            }
        }
        [XafDisplayName("Username")]
        public string userName
        {
            get
            {
                return _userName;
            }
            set
            {
                SetPropertyValue("userName", ref _userName, value);
            }
        }
        [XafDisplayName("Password")]
        public string password
        {
            get
            {
                return _password;
            }
            set
            {
                SetPropertyValue("password", ref _password, value);
            }
        }
        //[XafDisplayName("Is subscription owner")]
        //public bool isSubscriptionOwner
        //{
        //    get
        //    {
        //        return _isSubscriptionOwner;
        //    }
        //    set
        //    {
        //        SetPropertyValue("isSubscriptionOwner", ref _isSubscriptionOwner, value);
        //    }
        //}
        //[XafDisplayName("Subscription owner")]
        //public LsUser subscriptionOwner
        //{
        //    get
        //    {
        //        return _subscriptionOwner;
        //    }
        //    set
        //    {
        //        SetPropertyValue("subscriptionOwner", ref _subscriptionOwner, value);
        //    }
        //}
        [XafDisplayName("Active")]
        public bool active
        {
            get
            {
                return _active;
            }
            set
            {
                SetPropertyValue("active", ref _active, value);
            }
        }
        [XafDisplayName("User created on")]
        public DateTime createdOn
        {
            get
            {
                return _createdOn;
            }
            set
            {
                SetPropertyValue("createdOn", ref _createdOn, value);
            }
        }
        [XafDisplayName("Subscription expires on")]
        public DateTime subscriptionExpiresOn
        {
            get
            {
                return _subscriptionExpiresOn;
            }
            set
            {
                SetPropertyValue("subscriptionExpiresOn", ref _subscriptionExpiresOn, value);
            }
        }
        #endregion

        #region Associations
        [Association("Users-Exercices")]
        public XPCollection<Exercice> exercices
        {
            get { return GetCollection<Exercice>("exercices"); }
        }
        [Association("Plan-Users"), XafDisplayName("Payment Plan")]
        public PaymentPlan paymentPlan
        {
            get
            {
                return _paymentPlan;
            }
            set
            {
                SetPropertyValue("paymentPlan", ref _paymentPlan, value);
            }
        }
        #endregion

        #region Controls

        #endregion
        public LsUser(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
            createdOn = DateTime.Now;
        }
    }
}
