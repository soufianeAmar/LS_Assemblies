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

namespace LSAdmin
{
    [DefaultClassOptions, NavigationItem("Administration"), ImageName("BO_Opportunity"), DefaultProperty("name"), CreatableItem(false)]
    public class PaymentPlan : BaseObject
    {
        string _name;
        bool _free;
        #region Properties
        [XafDisplayName("Name")]
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
        [XafDisplayName("Free")]
        public bool free
        {
            get
            {
                return _free;
            }
            set
            {
                SetPropertyValue("free", ref _free, value);
            }
        }
        #endregion

        #region Associations
        [XafDisplayName("Plan limitations")]
        [Association("Plan-Limitations")]
        public XPCollection<PlanLimitation> limitations
        {
            get { return GetCollection<PlanLimitation>("limitations"); }
        }
        [XafDisplayName("Users")]
        [Association("Plan-Users")]
        public XPCollection<LsUser> users
        {
            get { return GetCollection<LsUser>("users"); }
        }
        #endregion

        public PaymentPlan(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }
        public static PaymentPlan GetFreePlan(Session session)
        {
            //Get the Singleton's instance if it exists 
            PaymentPlan result = session.FindObject<PaymentPlan>(CriteriaOperator.Parse("name = 'FREE'"));
            //Create the Singleton's instance 
            if (result == null)
            {
                result = new PaymentPlan(session)
                {
                    name = "FREE", free = true,
                };
                result.Save();
            }
            return result;
        }
    }
}
