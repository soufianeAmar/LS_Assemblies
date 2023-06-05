namespace LSAdmin
{
    partial class DBPathVC
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.aParcourir = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // aParcourir
            // 
            this.aParcourir.Caption = "Parcourir";
            this.aParcourir.Category = "cParcourir";
            this.aParcourir.ConfirmationMessage = null;
            this.aParcourir.Id = "Parcourir";
            this.aParcourir.ImageName = "Action_Open";
            this.aParcourir.Shortcut = null;
            this.aParcourir.Tag = null;
            this.aParcourir.TargetObjectType = typeof(Dossier);
            this.aParcourir.TargetViewId = null;
            this.aParcourir.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.aParcourir.ToolTip = null;
            this.aParcourir.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.aParcourir.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Parcourir_Execute);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction aParcourir;
    }
}
