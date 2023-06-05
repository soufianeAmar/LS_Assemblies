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
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.SystemModule.Notifications;
using LsSecurityModule;

namespace LsNotificationModule
{
    [DefaultClassOptions, ImageName("notificationItem"), NavigationItem("Settings")]
    [DeferredDeletion(false)]
    public class NotificationItem : BaseObject, ISupportNotifications
    {
        #region BaseObject
        string _id;
        string _subject;
        string _content;
        DateTime _dueDate;
        LsSecuritySystemUser _user;
        private XPCollection<AuditDataItemPersistent> auditTrail;
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
        [XafDisplayName("Subject")]
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
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Content")]
        public string content
        {
            get
            {
                return _content;
            }
            set
            {
                SetPropertyValue("ontent", ref _content, value);
            }
        }
        [XafDisplayName("Due date")]
        public DateTime dueDate
        {
            get
            {
                return _dueDate;
            }
            set
            {
                SetPropertyValue("dueDate", ref _dueDate, value);
            }
        }
        [XafDisplayName("User")]
        public LsSecuritySystemUser user
        {
            get
            {
                return _user;
            }
            set
            {
                SetPropertyValue("user", ref _user, value);
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
        public NotificationItem(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            //if (RemindIn.HasValue)
            //{
            //    AlarmTime = dueDate - RemindIn.Value;
            //}
            //else
            //{
            //    AlarmTime = null;
            //}
            //if (AlarmTime == null)
            //{
            //    RemindIn = null;
            //    IsPostponed = false;
            //}
        }
        #endregion
        #region ISupportNotifications
        DateTime? _alarmTime;
        private TimeSpan? _remindIn;
        private IList<PostponeTime> postponeTimes;

        [Browsable(false)]
        public DateTime? AlarmTime
        {
            get { return _alarmTime; }
            set
            {
                SetPropertyValue("AlarmTime", ref _alarmTime, value);
                if (value == null)
                {
                    RemindIn = null;
                    IsPostponed = false;
                }
            }
        }
        [Browsable(false)]
        public TimeSpan? RemindIn
        {
            get
            {
                return _remindIn;
            }
            set
            {
                SetPropertyValue("RemindIn", ref _remindIn, value);
                if (!IsLoading)
                {
                    if (value != null)
                    {
                        _alarmTime = dueDate - value.Value;
                    }
                    else
                    {
                        _alarmTime = null;
                    }
                }
            }
        }
        [Browsable(false)]
        public bool IsPostponed { get; set; }
        [Browsable(false), NonPersistent]
        public string NotificationMessage
        {
            get { return subject; }
        }
        [Browsable(false), NonPersistent]
        public object UniqueId
        {
            get { return Oid; }
        }
        [ImmediatePostData, NonPersistent, ModelDefault("AllowClear", "False"), DataSourceProperty("PostponeTimeList"), SearchMemberOptions(SearchMemberMode.Exclude)]
        public PostponeTime ReminderTime
        {
            get
            {
                if (RemindIn.HasValue)
                {
                    return PostponeTimeList.Where(x => (x.RemindIn != null && x.RemindIn.Value == RemindIn.Value)).FirstOrDefault();
                }
                else
                {
                    return PostponeTimeList.Where(x => x.RemindIn == null).FirstOrDefault();
                }
            }
            set
            {
                if (!IsLoading)
                {
                    if (value.RemindIn.HasValue)
                    {
                        RemindIn = value.RemindIn.Value;
                    }
                    else
                    {
                        RemindIn = null;
                    }
                }
            }
        }
        [Browsable(false), NonPersistent]
        public IEnumerable<PostponeTime> PostponeTimeList
        {
            get
            {
                if (postponeTimes == null)
                {
                    postponeTimes = CreatePostponeTimes();
                }
                return postponeTimes;
            }
        }
        private IList<PostponeTime> CreatePostponeTimes()
        {
            IList<PostponeTime> result = PostponeTime.CreateDefaultPostponeTimesList();
            result.Add(new PostponeTime("None", null, "None"));
            result.Add(new PostponeTime("AtStartTime", TimeSpan.Zero, "At Start Time"));
            PostponeTime.SortPostponeTimesList(result);
            return result;
        }
        #endregion
    }
}
