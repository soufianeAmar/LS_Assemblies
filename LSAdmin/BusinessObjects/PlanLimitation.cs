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
    [DefaultClassOptions, NavigationItem(false), ImageName("BO_Folder"), DefaultProperty("name"), CreatableItem(false)]
    public class PlanLimitation : BaseObject
    {
        Type _objectType;
        string _objectTypeName;
        int _objectCount;
        PaymentPlan _plan;
        #region Properties
        //[NonPersistent]
        //public IEnumerable<Type> XafTypes
        //{
        //    get
        //    {
        //        IList<Type> types = new List<Type>();
        //        foreach (ITypeInfo typeInfo in XafTypesInfo.Instance.PersistentTypes)
        //        {
        //            VisibleInReportsAttribute vira = typeInfo.FindAttribute<VisibleInReportsAttribute>(true);
        //            if (vira != null && vira.IsVisible)
        //                types.Add(typeInfo.Type);
        //        }
        //        return types;
        //    }
        //}
        [NonPersistent, XafDisplayName("Object type"), ImmediatePostData(true)]//, DataSourceProperty("XafTypes")]
        public Type objectType
        {
            get
            {
                string assemblyName = System.IO.Path.ChangeExtension(objectTypeName, ".dll");
                System.Reflection.Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().ToList().Find(a => a.ManifestModule.Name == assemblyName);
                if (string.IsNullOrEmpty(objectTypeName))
                    return _objectType;
                else
                    return assembly.GetType(objectTypeName);
            }
            set
            {
                SetPropertyValue("objectType", ref _objectType, value);
                if (value != null)
                    objectTypeName = value.FullName;
            }
        }
        [XafDisplayName("Object type name"), ImmediatePostData(true)]
        public string objectTypeName
        {
            get
            {
                return _objectTypeName;
            }
            set
            {
                
                SetPropertyValue("objectTypeName", ref _objectTypeName, value);
                Type _type = Type.GetType(objectTypeName);
                if (_type != null && _type != objectType)
                    objectType = _type;
            }
        }
        [XafDisplayName("Object count")]
        public int objectCount
        {
            get
            {
                return _objectCount;
            }
            set
            {
                SetPropertyValue("objectCount", ref _objectCount, value);
            }
        }
        #endregion

        #region Associations
        [XafDisplayName("Plan")]
        [Association("Plan-Limitations")]
        public PaymentPlan plan
        {
            get
            {
                return _plan;
            }
            set
            {
                SetPropertyValue("plan", ref _plan, value);
            }
        }
        #endregion

        public PlanLimitation(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }
    }
}
