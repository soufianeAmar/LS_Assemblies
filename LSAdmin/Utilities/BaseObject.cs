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
using DevExpress.Persistent.Validation;
using DevExpress.Persistent.BaseImpl;

namespace LSAdmin
{
    [DefaultClassOptions]
    public class LSBaseObject : BaseObject
    {
        Guid _lsOid;
        [Browsable(false), LSAdmin.ID]
        public Guid lsOid
        {
            get
            {
                return _lsOid;
            }
            set
            {
                SetPropertyValue("lsOid", ref _lsOid, value);
            }
        }

        public LSBaseObject(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}
