using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.BaseImpl;

namespace LsSecurityModule
{
    [NonPersistent, DefaultProperty("actionId")]
    public class ActionRef : BaseObject
    {
        string _actionId;
        public string actionId
        {
            get
            {
                return _actionId;
            }
            set
            {
                SetPropertyValue("actionId", ref _actionId, value);
            }
        }
        public ActionRef(Session session)
            : base(session)
        {

        }
    }
}
