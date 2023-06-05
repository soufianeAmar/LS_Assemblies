namespace LsNotificationModule
{
    partial class NotificationController
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
            this.aSendEmail = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // aSendEmail
            // 
            this.aSendEmail.Caption = "Send eMail";
            this.aSendEmail.Category = "SendEmail";
            this.aSendEmail.ConfirmationMessage = null;
            this.aSendEmail.Id = "Send eMail";
            this.aSendEmail.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.aSendEmail.TargetObjectType = typeof(eMail);
            this.aSendEmail.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.aSendEmail.ToolTip = null;
            this.aSendEmail.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.aSendEmail.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.aSendEmail_Execute);
            // 
            // NotificationController
            // 
            this.Actions.Add(this.aSendEmail);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction aSendEmail;
    }
}
