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
using DevExpress.XtraLayout;
using System.Windows.Forms;
using DevExpress.ExpressApp.Win.Layout;
using DevExpress.ExpressApp.Xpo;

namespace LSAdmin.Controllers
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class LayoutController : ViewController
    {
        public LayoutController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            SetDatabaseComponentVisibility();
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        private void SetDatabaseComponentVisibility()
        {
            BaseLayoutItem databaseItem = FindItemByPropertyName(((LayoutControl)View.Control).Items, "database");
            if (databaseItem != null)
            {
                Core.UpdateLSAdminDB(Application);
                XPObjectSpace os = (XPObjectSpace)Core.CreateAdminObjectSpace(Application);
                int BDCount = Convert.ToInt32(os.Session.EvaluateInTransaction(os.Session.GetClassInfo(typeof(Exercice)), CriteriaOperator.Parse("count()"), null));
                if (BDCount == 0)
                    ((LayoutControl)View.Control).HideItem(databaseItem);
            }
        }
        private BaseLayoutItem FindItemByPropertyName(DevExpress.XtraLayout.Utils.ReadOnlyItemCollection items, string data)
        {
            int i = 0;
            bool found = false;
            BaseLayoutItem bli = null;
            while ((i < items.Count) && (!found))
            {
                if (items[i] is XafLayoutControlItem)
                {
                    bli = items[i];
                    if (!(bli is EmptySpaceItem))
                        found = ((XafLayoutControlItem)bli).ViewItem.Id == data;
                }
                else
                {
                    if (items[i] is XafLayoutControlGroup)
                    {
                        bli = (XafLayoutControlGroup)items[i];
                        found = ((XafLayoutControlGroup)bli).Model.Id == data;
                    }
                }
                i++;
            }
            if (found)
                return bli;
            else
                return null;
        }
    }
}
