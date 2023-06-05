using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.Security.Strategy;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using System.Data.Sql;
using System.Data;
using Microsoft.SqlServer.Management.Smo;
using System.IO;
using wyUpdate.Common;
using Ionic.Zip;
using wyUpdate;
using System.Drawing;
using System.Globalization;
using System.Collections.ObjectModel;
using DevExpress.ExpressApp.Xpo;
using System.Data.SqlClient;
using DevExpress.Xpo.DB.Exceptions;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Updating;
using System.Diagnostics;
using DevExpress.Xpo.Exceptions;
using DevExpress.Persistent.BaseImpl;
using LsSecurityModule;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo.Metadata;
using System.Data.Common;
using System.Configuration;

namespace LSAdmin
{
    public class Core
    {
        //public static string serverName = string.Empty;
        //public static string instanceName = string.Empty;
        //public static Country country = null;
        public static int DemoRecordCount = 5;

        public static IDbConnection GetConnection(XafApplication application)
        {
            if (application.Connection != null)
                return application.Connection;
            else
                return (((XPObjectSpaceProvider)(application.ObjectSpaceProvider)).DataLayer).Connection;
        }

        public static string AdminConnectionString(XafApplication application)
        {
            // Test Amar 30-01-2023
            //string password = (ConfigurationManager.AppSettings["Password"]);
            //string id = (ConfigurationManager.AppSettings["Id"]);
            //if (!string.IsNullOrEmpty(password))
            //    password = Helper.decryptage(password, "EurekaNovoreka");
            //else
            //    password = "58206670";
            //if (!string.IsNullOrEmpty(id))
            //    id = Helper.decryptage(id, "EurekaNovoreka");
            //else
            //    id = "sa";
            string password = passwordConfig();
            string id = idConfig();
            // Test Amar 30-01-2023

            string connectionString = string.Empty;
            IDbConnection connection = GetConnection(application);//(DbConnection)(((XPObjectSpaceProvider)(application.ObjectSpaceProvider)).DataLayer).Connection;
            if (connection is SqlConnection)
                connectionString = string.Format("Integrated Security=false;Pooling=false;Data Source=" +
                    "{0}{1};Initial Catalog=LSAdmin;User ID={3};Password={2}", Helper.serverName, Helper.instanceName, password,id);
            else
                connectionString = string.Format(@"XpoProvider=SQLite;Data Source={0}\Data\LSAdmin", GetApplicationPath());
            return connectionString;
        }
        public static string WebAdminConnectionString(XafApplication application)
        {
            string connectionString = string.Empty;
            IDbConnection connection = GetConnection(application);//(DbConnection)(((XPObjectSpaceProvider)(application.ObjectSpaceProvider)).DataLayer).Connection;
            if (connection is SqlConnection)
                connectionString = string.Format("Integrated Security=false;Pooling=false;Data Source=" +
                    "{0}{1};Initial Catalog=LSAdmin;User ID=novoreka;Password=58206670", Helper.serverName, Helper.instanceName);
            else
                connectionString = string.Format(@"XpoProvider=SQLite;Data Source={0}Data\LSAdmin", GetWebApplicationPath());
            return connectionString;
        }
        public static IObjectSpace CreateAdminObjectSpace(XafApplication application)
        {
            IObjectSpace os = null;
            XPObjectSpaceProvider osp = new XPObjectSpaceProvider(AdminConnectionString(application));
            try
            {
                os = osp.CreateObjectSpace();
            }
            catch (UnableToOpenDatabaseException ex)
            {
                UpdateLSAdminDB(application);
                os = osp.CreateObjectSpace();
            }
            return os;
        }
        public static IObjectSpace CreateWebAdminObjectSpace(XafApplication application)
        {
            IObjectSpace os = null;
            XPObjectSpaceProvider osp = new XPObjectSpaceProvider(WebAdminConnectionString(application));
            try
            {
                os = osp.CreateObjectSpace();
            }
            catch (UnableToOpenDatabaseException ex)
            {
                UpdateLSAdminWebDB(application);
                os = osp.CreateObjectSpace();
            }
            return os;
        }

        public static void UpdateLSAdminDB(XafApplication application)
        {
            string connectionString = AdminConnectionString(application);
            IDataLayer dl = null;
            try
            {
                dl = XpoDefault.GetDataLayer(connectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
            }
            catch (UnableToOpenDatabaseException ex)
            {
                //if ((lsactvtn.ActivationClass.réseau) && (lsactvtn.ActivationClass.version == lsactvtn.VersionImprimeCheque.ENTREPRISE))
                IDbConnection connection = GetConnection(application);//(DbConnection)(((XPObjectSpaceProvider)(application.ObjectSpaceProvider)).DataLayer).Connection;
                if (connection is SqlConnection)
                {
                    Server server = new Server(string.Format("{0}{1}", Helper.serverName, Helper.instanceName));
                    Database lsAdmin = new Database(server, "LSAdmin");
                    lsAdmin.Create();
                    dl = XpoDefault.GetDataLayer(connectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
                }
                else
                {
                    CreateDataFolder();
                    if (!File.Exists(string.Format(@"{0}\Data\LSAdmin", GetApplicationPath())))
                        File.Create(string.Format(@"{0}\Data\LSAdmin", GetApplicationPath()));
                    dl = XpoDefault.GetDataLayer(connectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
                }
            }
            catch (CannotFindAppropriateConnectionProviderException)
            {
                CreateDataFolder();
                if (!File.Exists(string.Format(@"{0}\Data\LSAdmin", GetApplicationPath())))
                    File.Create(string.Format(@"{0}\Data\LSAdmin", GetApplicationPath()));
                dl = XpoDefault.GetDataLayer(connectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
            }
            using (Session session = new Session(dl))
            {
                dl.UpdateSchema(false, session.GetClassInfo(typeof(Dossier)), session.GetClassInfo(typeof(Exercice)), session.GetClassInfo(typeof(LsUser)));
                session.CreateObjectTypeRecords(typeof(Dossier).Assembly);
            }
        }
        public static void UpdateLSAdminWebDB(XafApplication application)
        {
            string connectionString = WebAdminConnectionString(application);
            IDataLayer dl = null;
            try
            {
                dl = XpoDefault.GetDataLayer(connectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
            }
            catch (UnableToOpenDatabaseException ex)
            {
                //if ((lsactvtn.ActivationClass.réseau) && (lsactvtn.ActivationClass.version == lsactvtn.VersionImprimeCheque.ENTREPRISE))
                IDbConnection connection = GetConnection(application);//(DbConnection)(((XPObjectSpaceProvider)(application.ObjectSpaceProvider)).DataLayer).Connection;
                if (connection is SqlConnection)
                {
                    Server server = new Server(string.Format("{0}{1}", Helper.serverName, Helper.instanceName));
                    Database lsAdmin = new Database(server, "LSAdmin");
                    lsAdmin.Create();
                    dl = XpoDefault.GetDataLayer(connectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
                }
                else
                {
                    CreateWebDataFolder();
                    //if (!File.Exists(string.Format(@"{0}Data\LSAdmin", GetWebApplicationPath())))
                    //    File.Create(string.Format(@"{0}Data\LSAdmin", GetWebApplicationPath()));
                    dl = XpoDefault.GetDataLayer(connectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
                }
            }
            catch (CannotFindAppropriateConnectionProviderException)
            {
                CreateWebDataFolder();
                //if (!File.Exists(string.Format(@"{0}Data\LSAdmin", GetWebApplicationPath())))
                //    File.Create(string.Format(@"{0}Data\LSAdmin", GetWebApplicationPath()));
                dl = XpoDefault.GetDataLayer(connectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
            }
            using (Session session = new Session(dl))
            {
                dl.UpdateSchema(false, session.GetClassInfo(typeof(Dossier)), session.GetClassInfo(typeof(Exercice)), session.GetClassInfo(typeof(LsUser)));
                session.CreateObjectTypeRecords(typeof(Dossier).Assembly);
            }
        }
        public static void UpdateApplicationConnectionString(XafApplication application, string folder, string dbName)
        {
            string password = passwordConfig();
            string id = idConfig();

            IDbConnection connection = GetConnection(application);//(DbConnection)(((XPObjectSpaceProvider)(application.ObjectSpaceProvider)).DataLayer).Connection;
            if (connection is SqlConnection)
            {
                if (application.Connection != null)
                {
                    application.Connection.Close();
                    application.Connection.ConnectionString = string.Format("Integrated Security=false;Pooling=false;Data Source=(local);" +
                        "Initial Catalog={0};User ID={2};Password={1}", dbName, password,id);
                }
                else
                    application.ConnectionString = string.Format("Integrated Security=false;Pooling=false;Data Source=(local);" +
                        "Initial Catalog={0};User ID={2};Password={1}", dbName, password,id);
            }
            else
            {
                if (application.Connection != null)
                {
                    application.Connection.Close();
                    application.Connection.ConnectionString = string.Format(@"XpoProvider=SQLite;Data Source={0}\Data\{1}\{2}", Core.GetApplicationPath(), folder, dbName);
                }
                else
                    application.ConnectionString = string.Format(@"XpoProvider=SQLite;Data Source={0}\Data\{1}\{2}", Core.GetApplicationPath(), folder, dbName);
            }
        }
        public static void UpdateWebApplicationConnectionString(XafApplication application, string folder, string dbName)
        {
            string password = passwordConfig();
            string id = idConfig();

            IDbConnection connection = GetConnection(application);//(DbConnection)(((XPObjectSpaceProvider)(application.ObjectSpaceProvider)).DataLayer).Connection;
            if (connection is SqlConnection)
            {
                if (application.Connection != null)
                {
                    application.Connection.Close();
                    application.Connection.ConnectionString = string.Format("Integrated Security=false;Pooling=false;Data Source=(local);" +
                        "Initial Catalog={0};User ID={2};Password={1}", dbName, password,id);
                }
                else
                    application.ConnectionString = string.Format("Integrated Security=false;Pooling=false;Data Source=(local);" +
                        "Initial Catalog={0};User ID={2};Password={1}", dbName, password,id);
            }
            else
            {
                if (application.Connection != null)
                {
                    application.Connection.Close();
                    application.Connection.ConnectionString = string.Format(@"XpoProvider=SQLite;Data Source={0}\Data\{1}\{2}", Core.GetWebApplicationPath(), folder, dbName);
                }
                else
                    application.ConnectionString = string.Format(@"XpoProvider=SQLite;Data Source={0}\Data\{1}\{2}", Core.GetWebApplicationPath(), folder, dbName);
            }
        }
        public static void UpdateDB(XafApplication application)
        {
            try
            {
                DatabaseUpdater dbUpdater = (DatabaseUpdater)application.CreateDatabaseUpdater(application.ObjectSpaceProvider);
                dbUpdater.ForceUpdateDatabase = true;
                dbUpdater.Update();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError updating database : {1}", DateTime.Now, ex.Message));
            }
        }
        /// <summary>
        /// Creates a data folder in the application directory
        /// </summary>
        public static void CreateDataFolder()
        {
            if (!Directory.Exists(string.Format(@"{0}\Data", GetApplicationPath())))
            {
                Directory.CreateDirectory(string.Format(@"{0}\Data", GetApplicationPath()));
            }
        }
        public static void CreateWebDataFolder()
        {
            if (!Directory.Exists(string.Format(@"{0}\Data", GetWebApplicationPath())))
            {
                Directory.CreateDirectory(string.Format(@"{0}\Data", GetWebApplicationPath()));
            }
        }
        public static ReadOnlyCollection<T> GetReadOnlyCollection<T>(DevExpress.Xpo.XPCollection<T> xpcollection)
        {
            List<T> list = new List<T>();
            foreach (T obj in xpcollection)
                list.Add(obj);
            return list.AsReadOnly();
        }
        public static void CreateLsUserAdmin(IObjectSpace ObjectSpace)
        {
            LsSecuritySystemRole role = ObjectSpace.FindObject<LsSecuritySystemRole>(new BinaryOperator("Name", "Administrateur"));
            if (role == null)
            {
                role = ObjectSpace.CreateObject<LsSecuritySystemRole>();
                role.Name = "Administrateur";
                //Create permissions and assign them to the role
                role.IsAdministrative = true;
                role.Save();
            }

            LsSecuritySystemUser user = ObjectSpace.FindObject<LsSecuritySystemUser>(new BinaryOperator("UserName", "Admin"));
            if (user == null)
            {
                user = ObjectSpace.CreateObject<LsSecuritySystemUser>();
                user.UserName = "Admin";
                // Make the user an administrator
                user.Roles.Add(role);
                // Set a password if the standard authentication type is used
                user.SetPassword("123");
                // Save the user to the database
                user.Save();
            }
            ObjectSpace.CommitChanges();
        }
        public static void CreateUserAdmin(IObjectSpace ObjectSpace)
        {
            SecuritySystemRole role = ObjectSpace.FindObject<SecuritySystemRole>(new BinaryOperator("Name", "Administrateur"));
            if (role == null)
            {
                role = ObjectSpace.CreateObject<SecuritySystemRole>();
                role.Name = "Administrateur";
                //Create permissions and assign them to the role
                role.IsAdministrative = true;
                role.Save();
            }

            SecuritySystemUser user = ObjectSpace.FindObject<SecuritySystemUser>(new BinaryOperator("UserName", "Admin"));
            if (user == null)
            {
                user = ObjectSpace.CreateObject<SecuritySystemUser>();
                user.UserName = "Admin";
                // Make the user an administrator
                user.Roles.Add(role);
                // Set a password if the standard authentication type is used
                user.SetPassword("123");
                // Save the user to the database
                user.Save();
            }
            ObjectSpace.CommitChanges();
        }

        public static void CreateSimpleUserAdmin(IObjectSpace ObjectSpace)
        {
            SimpleUser user = ObjectSpace.FindObject<SimpleUser>(new BinaryOperator("UserName", "Admin"));
            if (user == null)
            {
                user = ObjectSpace.CreateObject<SimpleUser>();
                user.UserName = "Admin";
                // Make the user an administrator
                user.IsAdministrator = true;
                // Set a password if the standard authentication type is used
                user.SetPassword("123");
                // Save the user to the database
                user.Save();
            }
            ObjectSpace.CommitChanges();
        }

        public static void CreateAdmin(IObjectSpace os)
        {
            if (SecuritySystem.Instance != null)
            {
                try
                {
                    if (SecuritySystem.Instance is SecuritySimple)
                        Core.CreateSimpleUserAdmin(os);
                    else
                        if (SecuritySystem.UserType == typeof(SecuritySystemUser))
                            CreateUserAdmin(os);
                        else
                            LsSecurityModule.Helper.CreateUserAdmin(os, "Admin", "123");
                }
                catch (SchemaCorrectionNeededException ex)
                {
                    Core.UpdateDB(LsSecurityModule.LsSecurityModule.currentApp);
                    if (SecuritySystem.Instance is SecuritySimple)
                        Core.CreateSimpleUserAdmin(os);
                    else
                        if (SecuritySystem.UserType == typeof(SecuritySystemUser))
                            CreateUserAdmin(os);
                        else
                            LsSecurityModule.Helper.CreateUserAdmin(os, "Admin", "123");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError creating Admin : {1}", DateTime.Now, ex.Message));
                }
            }
        }

        //public static void CreateLSAdministrator(IObjectSpace os)
        //{
        //    if (SecuritySystem.Instance != null)
        //    {
        //        if (SecuritySystem.UserType == typeof(LsSecuritySystemUser))
        //            try
        //            {
        //                LsSecuritySystemRole lsAdminRole = LsSecurityModule.Helper.CreateRole(os, "AdministrateurLS", true, false);
        //                LsSecuritySystemUser lsAdminUser = LsSecurityModule.Helper.CreateUser(os, "AdminLS", "Leadersoft58206670", lsAdminRole);
        //            }
        //            catch (SchemaCorrectionNeededException ex)
        //            {
        //                Core.UpdateDB(LsSecurityModule.LsSecurityModule.currentApp);
        //                LsSecuritySystemRole lsAdminRole = LsSecurityModule.Helper.CreateRole(os, "AdministrateurLS", true, false);
        //                LsSecuritySystemUser lsAdminUser = LsSecurityModule.Helper.CreateUser(os, "AdminLS", "Leadersoft58206670", lsAdminRole);
        //            }
        //            catch (Exception ex)
        //            {
        //                Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError creating Admin : {1}", DateTime.Now, ex.Message));
        //            }
        //    }
        //}

        public static DialogResult dialogResult;
        public static string ValidationContext = string.Empty;
        private static void dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            if (ValidationContext != string.Empty)
            {
                IObjectSpace os = XPObjectSpace.FindObjectSpaceByObject(e.AcceptActionArgs.CurrentObject);
                try
                {
                    Validator.RuleSet.Validate(os, e.AcceptActionArgs.CurrentObject, ValidationContext);
                    dialogResult = DialogResult.OK;
                }
                catch
                { 
                    e.Cancel = true;
                }
            }
            else
                dialogResult = DialogResult.OK;
        }
        public static DialogResult ShowDialog(XafApplication application, IObjectSpace os, object obj)
        {
            ShowViewParameters svp = new ShowViewParameters()
            {
                CreatedView = application.CreateDetailView(os, obj, false),
                TargetWindow = TargetWindow.NewModalWindow,
                Context = TemplateContext.PopupWindow,
                CreateAllControllers = true
            };
            DialogController dc = application.CreateController<DialogController>();
            svp.Controllers.Add(dc);
            dc.Accepting += dc_Accepting;
            dialogResult = DialogResult.Cancel;
            ValidationContext = string.Empty;
            application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            return dialogResult;
        }

        public static DialogResult ShowDialog(XafApplication application, IObjectSpace os, object obj, string validationContext)
        {
            ShowViewParameters svp = new ShowViewParameters()
            {
                CreatedView = application.CreateDetailView(os, obj, false),
                TargetWindow = TargetWindow.NewModalWindow,
                Context = TemplateContext.PopupWindow,
                CreateAllControllers = true
            };
            DialogController dc = application.CreateController<DialogController>();
            svp.Controllers.Add(dc);
            dc.Accepting += dc_Accepting;
            dialogResult = DialogResult.Cancel;
            ValidationContext = validationContext;
            application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            return dialogResult;
        }
        public static DialogResult ShowWebDialog(XafApplication application, IObjectSpace os, object obj)
        {
            ShowViewParameters svp = new ShowViewParameters()
            {
                CreatedView = application.CreateDetailView(os, obj, false),
                TargetWindow = TargetWindow.NewModalWindow,
                Context = TemplateContext.PopupWindow,
                CreateAllControllers = true
            };
            ((DetailView)svp.CreatedView).ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            DialogController dc = application.CreateController<DialogController>();
            svp.Controllers.Add(dc);
            dc.Accepting += dc_Accepting;
            dialogResult = DialogResult.Cancel;
            ValidationContext = string.Empty;
            application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            return dialogResult;
        }

        public static DialogResult ShowWebDialog(XafApplication application, IObjectSpace os, object obj, string validationContext)
        {
            ShowViewParameters svp = new ShowViewParameters()
            {
                CreatedView = application.CreateDetailView(os, obj, false),
                TargetWindow = TargetWindow.NewModalWindow,
                Context = TemplateContext.PopupWindow,
                CreateAllControllers = true
            };
            DialogController dc = application.CreateController<DialogController>();
            svp.Controllers.Add(dc);
            dc.Accepting += dc_Accepting;
            dialogResult = DialogResult.Cancel;
            ValidationContext = validationContext;
            application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            return dialogResult;
        }

        public static float power(float number, int pow)
        {
            float result = 1;
            for (int i = 1; i <= pow; i++)
                result *= number;
            return result;
        }

        public static string TroisChiffresEnLettres(int nombre)
        {
            string[] chiffres = { "", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf" };
            string[] chiffres1 = { "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf" };
            string[] chiffres10 = { "", "dix", "vingt", "trente", "quarante", "cinquante", "soixante", "", "quatre-vingt", "" };
            string result = string.Empty;
            int unités = nombre % 10;
            nombre = nombre / 10;
            int dizaines = nombre % 10;
            nombre = nombre / 10;
            int centaines = nombre % 10;
            if (centaines != 0)
            {
                if (centaines == 1)
                    result = "cent";
                else
                    result = chiffres[centaines] + " cent";
            }
            if ((dizaines != 0) | (unités != 0))
                if (result.Length > 0)
                {
                    if (result[result.Length - 1] != ' ')
                        result += " ";
                }
            //else
            //    result = " ";
            if (dizaines == 1)
                result += chiffres1[unités];
            else
                if ((dizaines == 7) | (dizaines == 9))
                    result += string.Format("{0} {1}", chiffres10[dizaines - 1], chiffres1[unités]);
                else
                    if (dizaines > 0)
                        if (unités == 1)
                            result += string.Format("{0} et {1}", chiffres10[dizaines], chiffres[unités]);
                        else
                            result += string.Format("{0} {1}", chiffres10[dizaines], chiffres[unités]);
                    else
                        result += chiffres[unités];
            return result;
        }
        public static string MontantEnLettres(decimal montant)
        {
            string[] chiffres = {"", "mille", "million", "milliard", "billion", "billiard", "trillion", "trilliard", 
                                    "quadrillion", "quadrilliard", "quintillion", "quintillard"};
            string[] chiffresPluriel = {"", "mille", "millions", "milliards", "billions", "billiards", "trillions", "trilliards", 
                                    "quadrillions", "quadrilliards", "quintillions", "quintillards"};
            string result = string.Empty;
            Int64 TroisChiffres = 0;
            Int64 montantDA = (Int64)montant;
            int montantCTS = (int)((montant - montantDA) * 100);
            int i = 0;
            while (montantDA != 0)
            {
                TroisChiffres = (Int64)montantDA % 1000;
                if (TroisChiffres > 0)
                {
                    string _chiffre = chiffres[i];
                    if (TroisChiffres > 1)
                        _chiffre = chiffresPluriel[i];
                    if ((i == 1) && (TroisChiffres == 1))
                    {
                        if (result == string.Empty)
                            result = _chiffre;
                        else
                            result = string.Format("{0} {1}", _chiffre, result);
                    }
                    else
                    {
                        if (result == string.Empty)
                        {
                            if (i == 0)
                                result = TroisChiffresEnLettres((int)TroisChiffres);
                            else
                                result = string.Format("{0} {1}", TroisChiffresEnLettres((int)TroisChiffres), _chiffre);
                        }
                        else
                        {
                            if (i == 0)
                            {
                                //if (result == string.Empty)
                                //    result = TroisChiffresEnLettres((int)TroisChiffres);
                                //else
                                    result = string.Format("{0} {1}", TroisChiffresEnLettres((int)TroisChiffres), result);
                            }
                            else
                                //if (result == string.Empty)
                                //    result = string.Format("{0} {1}", TroisChiffresEnLettres((int)TroisChiffres), chiffres[i]);
                                //else
                                result = string.Format("{0} {1} {2}", TroisChiffresEnLettres((int)TroisChiffres), _chiffre, result);
                        }
                    }
                }
                montantDA = montantDA / 1000;
                i++;
            }
            if (result != string.Empty)
            {
                result += " Dinars Algériens";
                if (montantCTS != 0)
                    result += " et ";
            }
            if (montantCTS != 0)
                result += TroisChiffresEnLettres(montantCTS) + " Centime(s)";
            if (result != string.Empty)
                result = char.ToUpper(result[0]) + result.Substring(1);
            return result;
        }

        public static string GetApplicationPath()
        {
            string applicationPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            return applicationPath;
        }
        public static string GetWebApplicationPath()
        {
            string applicationPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            return applicationPath;
        }
        public static DataTable GetAvailableSQLServers()
        {
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            DataTable dt = instance.GetDataSources();
            return dt;
        }

        public static bool IsLimited(Type type)
        {
            LimitedAttribute IsLimitedAttrib = (LimitedAttribute)Attribute.GetCustomAttribute(type, typeof(LimitedAttribute));
            return (IsLimitedAttrib != null);
        }

        /// <summary>
        /// Creates a SQLite ObjectSpace
        /// </summary>
        /// <param name="filename">The database file</param>
        /// <returns>The newly created IObjectSpace</returns>
        public static IObjectSpace CreateObjectSpace(string filename)
        {
            XPObjectSpaceProvider objectspaceprovider = new XPObjectSpaceProvider(string.Format("XpoProvider=SQLite;Data Source={0}", filename), 
                new System.Data.SQLite.SQLiteConnection());
            return objectspaceprovider.CreateObjectSpace();
        }

        /// <summary>
        /// Creates a SQL Server ObjectSpace
        /// </summary>
        /// <param name="server">SQL Server name including any named instance</param>
        /// <param name="database">Database name</param>
        /// <param name="sapwd">sa password</param>
        /// <returns>The newly created IObjectSpace</returns>
        public static IObjectSpace CreateObjectSpace(string server, string database, string sapwd, string id)
        {
            XPObjectSpaceProvider objectspaceprovider = new XPObjectSpaceProvider(string.Format("Integrated Security=false;Pooling=false;"+
                "Data Source={0};Initial Catalog={1};User ID={3};Password={2}", server, database, sapwd,id), new SqlConnection());
            return objectspaceprovider.CreateObjectSpace();
        }

        /// <summary>
        /// Creates a MS Excel ObjectSpace
        /// </summary>
        /// <param name="filename">MS Excel file name</param>
        /// <returns>The newly created IObjectSpace</returns>
        public static IObjectSpace CreateObjectSpace(string filename, bool Header, bool MixedData)
        {
            string HDR = Header ? "HDR=Yes;" : "HDR=No;";
            string IMEX = MixedData ? "Imex=2;" : "Imex=1;";
            XPObjectSpaceProvider objectspaceprovider = new XPObjectSpaceProvider(string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Excel 8.0;{1}{2}", filename, HDR, IMEX),
                new System.Data.OleDb.OleDbConnection());
            return objectspaceprovider.CreateObjectSpace();
        }

        /// <summary>
        /// Gets the name of the member that has the ID attribute
        /// </summary>
        /// <param name="ObjectType">the object type to get the ID member from</param>
        /// <returns>the ID member name</returns>
        public static string GetObjectIDMember(Type ObjectType)
        {
            var MemberInfoArray = ObjectType.GetMembers();
            int i = 0;
            bool found = false;
            while ((i < MemberInfoArray.Length) && (!found))
            {
                IDAttribute a = (IDAttribute)Attribute.GetCustomAttribute(MemberInfoArray[i], typeof(IDAttribute));
                found = a != null;
                if (!found)
                    i++;
            }
            if (found)
                return MemberInfoArray[i].Name;
            else
                return string.Empty;
        }

        /// <summary>
        /// Gets an object by specifying its IDMember value
        /// </summary>
        /// <param name="session">The object session</param>
        /// <param name="ObjectType">The object type</param>
        /// <param name="value">the ID member value</param>
        /// <returns>the object with the specified ID member value</returns>
        public static object GetObjectByID(Session session, Type ObjectType, object value)
        {
            object obj = null;
            var IDName = GetObjectIDMember(ObjectType);
            if (IDName != string.Empty)
                obj = session.FindObject(ObjectType, CriteriaOperator.Parse(string.Format("{0} = ?", IDName), value));
            return obj;
        }

        public static object CreateObject(XPObjectSpace os_destination, object source, Type objectType, bool updateRecords = false)
        {
            object obj = os_destination.FindObject(objectType, CriteriaOperator.Parse(string.Format("{0} = ?", GetObjectIDMember(objectType)), 
                ((BaseObject)source).GetMemberValue(GetObjectIDMember(objectType))));
            if (obj == null || updateRecords)
            {
                
                if (obj == null)
                    obj = os_destination.CreateObject(objectType);
                foreach (XPMemberInfo mi in ((BaseObject)obj).ClassInfo.PersistentProperties)
                {
                    if (((BaseObject)((object)source)).ClassInfo.GetPersistentMember(mi.Name) != null)
                    {
                        if (mi.Name != "oid" || os_destination.IsNewObject(obj))
                        {
                            if (!mi.MemberType.IsSubclassOf(typeof(BaseObject)))
                                mi.SetValue(obj, mi.GetValue(source));
                            else
                            {
                                string IDName = GetObjectIDMember(mi.MemberType);
                                if ((IDName != string.Empty) && (mi.GetValue(source) != null))
                                {
                                    object IDValue = ((BaseObject)mi.GetValue(source)).GetMemberValue(IDName);
                                    object memberValue = GetObjectByID(os_destination.Session, mi.MemberType, IDValue);
                                    if (memberValue == null)
                                        memberValue = CreateObject(os_destination, mi.GetValue(source), mi.MemberType);
                                    mi.SetValue(obj, memberValue);//GetObjectByID(os_destination.Session, mi.MemberType, memberValue));
                                    //((BaseObject)mi.GetValue(source)).GetMemberValue(IDName)));
                                }
                            }
                        }
                    }
                }
            }
            return obj;
        }

        public static void ImporterObjet<T>(XPObjectSpace os_source, XPObjectSpace os_destination, CriteriaOperator criteria, bool updateRecords = false)
        {
            XPCollection<T> collection = new XPCollection<T>(os_source.Session, criteria);
            foreach (T element in collection)
            {
                var obj = CreateObject(os_destination, element, typeof(T), updateRecords);
                ((BaseObject)obj).Save();
            }
            os_destination.CommitChanges();
        }

        #region wyUpdate
        public static string InstalledVersion;
        public static string CompanyName;
        public static string ProductName;
        static string m_GUID;
        public static string GUID
        {
            get
            {
                if (string.IsNullOrEmpty(m_GUID))
                {
                    // generate a GUID from the product name
                    char[] invalidChars = Path.GetInvalidFileNameChars();

                    if (ProductName != null && ProductName.IndexOfAny(invalidChars) != -1)
                    {
                        List<char> invalidFilenameChars = new List<char>(invalidChars);

                        // there are bad filename characters
                        //make a new string builder (with at least one bad character)
                        StringBuilder newText = new StringBuilder(ProductName.Length - 1);

                        //remove the bad characters
                        for (int i = 0; i < ProductName.Length; i++)
                        {
                            if (invalidFilenameChars.IndexOf(ProductName[i]) == -1)
                                newText.Append(ProductName[i]);
                        }

                        return newText.ToString();
                    }

                    return ProductName;
                }
                return m_GUID;
            }
            set
            {
                m_GUID = value;
            }
        }

        public static Hashtable Languages = new Hashtable();
        public static bool HideHeaderDivider;
        public static bool CloseOnSuccess;
        public static string CustomWyUpdateTitle;
        public static string PublicSignKey;
        public static string UpdatePassword;
        public static Image TopImage;
        public static Image SideImage;
        public static string TopImageFilename;
        public static string SideImageFilename;

        static void LoadClientData(Stream ms, string updatePathVar, string customUrlArgs)
        {
            ms.Position = 0;

            // Read back the file identification data, if any
            if (!ReadFiles.IsHeaderValid(ms, "IUCDFV2"))
            {
                //free up the file so it can be deleted
                ms.Close();

                throw new Exception("The client file does not have the correct identifier - this is usually caused by file corruption.");
            }

            LanguageCulture lastLanguage = null;
            string serverSite;
            byte bType = (byte)ms.ReadByte();
            while (!ReadFiles.ReachedEndByte(ms, bType, 0xFF))
            {
                switch (bType)
                {
                    case 0x01://Read Company Name
                        CompanyName = ReadFiles.ReadDeprecatedString(ms);
                        break;
                    case 0x02://Product Name
                        ProductName = ReadFiles.ReadDeprecatedString(ms);
                        break;
                    case 0x0A: // GUID
                        m_GUID = ReadFiles.ReadString(ms);
                        break;
                    case 0x03://Read Installed Version
                        InstalledVersion = ReadFiles.ReadDeprecatedString(ms);
                        break;
                    case 0x04://Add server file site

                        serverSite = ReadFiles.ReadDeprecatedString(ms);

                        if (updatePathVar != null)
                            serverSite = serverSite.Replace("%updatepath%", updatePathVar);

                        if (customUrlArgs != null)
                            serverSite = serverSite.Replace("%urlargs%", customUrlArgs);

                        //AddUniqueString(serverSite, ServerFileSites);


                        break;
                    case 0x09://Add client server file site

                        serverSite = ReadFiles.ReadDeprecatedString(ms);

                        if (updatePathVar != null)
                            serverSite = serverSite.Replace("%updatepath%", updatePathVar);

                        if (customUrlArgs != null)
                            serverSite = serverSite.Replace("%urlargs%", customUrlArgs);

                        //AddUniqueString(serverSite, ClientServerSites);
                        break;
                    case 0x11://Header image alignment
                        try
                        {
                            //HeaderImageAlign = (ImageAlign)Enum.Parse(typeof(ImageAlign), ReadFiles.ReadDeprecatedString(ms));
                        }
                        catch { }
                        break;
                    case 0x12://Header text indent
                        //HeaderTextIndent = ReadFiles.ReadInt(ms);
                        break;
                    case 0x13://Header text color
                        //HeaderTextColorName = ReadFiles.ReadDeprecatedString(ms);
                        break;
                    case 0x14: //header image filename
                        //TopImageFilename = ReadFiles.ReadDeprecatedString(ms);
                        break;
                    case 0x15: //side image filename
                        //SideImageFilename = ReadFiles.ReadDeprecatedString(ms);
                        break;
#if CLIENT_READER
                    case 0x18: // language culture
                        Languages.Add(ReadFiles.ReadDeprecatedString(ms));
                        break;
#else
                    case 0x18: // language culture

                        lastLanguage = new LanguageCulture(ReadFiles.ReadDeprecatedString(ms));

                        Languages.Add(lastLanguage.Culture, lastLanguage);
                        break;
                    case 0x16: //language filename

                        if (lastLanguage != null)
                            lastLanguage.Filename = ReadFiles.ReadDeprecatedString(ms);
                        else
                            Languages.Add(string.Empty, new LanguageCulture(null) { Filename = ReadFiles.ReadDeprecatedString(ms) });

                        break;
#endif
                    case 0x17: //hide the header divider
                        HideHeaderDivider = ReadFiles.ReadBool(ms);
                        break;
                    case 0x19:
                        CloseOnSuccess = ReadFiles.ReadBool(ms);
                        break;
                    case 0x1A:
                        CustomWyUpdateTitle = ReadFiles.ReadString(ms);
                        break;
                    case 0x1B:
                        PublicSignKey = ReadFiles.ReadString(ms);
                        break;
                    case 0x1C:
                        UpdatePassword = ReadFiles.ReadString(ms);
                        break;
                    default:
                        ReadFiles.SkipField(ms, bType);
                        break;
                }

                bType = (byte)ms.ReadByte();
            }

            ms.Close();
        }

        public static void OpenClientFile(string m_Filename, ClientLanguage lang, string forcedCulture, string updatePathVar, string customUrlArgs)
        {
            using (ZipFile zip = ZipFile.Read(m_Filename))
            {
                // load the client details (image filenames, languages, etc.)
                using (MemoryStream ms = new MemoryStream())
                {
                    zip["iuclient.iuc"].Extract(ms);

                    //read in the client data
                    LoadClientData(ms, updatePathVar, customUrlArgs);
                }

                // load the top image
                if (!string.IsNullOrEmpty(TopImageFilename))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        zip[TopImageFilename].Extract(ms);

                        // convert the bytes to an images
                        TopImage = Image.FromStream(ms, true);
                    }
                }

                // load the side image
                if (!string.IsNullOrEmpty(SideImageFilename))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        zip[SideImageFilename].Extract(ms);

                        // convert the bytes to an images
                        SideImage = Image.FromStream(ms, true);
                    }
                }


                // Backwards compatability with pre-v1.3 of wyUpdate:
                // if the languages has a culture with a null name, load that file
                if (Languages.Count == 1 && Languages.Contains(string.Empty))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        zip[((LanguageCulture)Languages[string.Empty]).Filename].Extract(ms);
                        lang.Open(ms);
                    }
                }
                else if (Languages.Count > 0)
                {
                    LanguageCulture useLang = null;

                    // use a forced culture
                    if (!string.IsNullOrEmpty(forcedCulture))
                        useLang = (LanguageCulture)Languages[forcedCulture];

                    // try to find the current culture
                    if (useLang == null)
                        useLang = (LanguageCulture)Languages[CultureInfo.CurrentUICulture.Name];

                    // if current culture isn't available, use the default culture (english)
                    if (useLang == null)
                        useLang = (LanguageCulture)Languages["en-US"];


                    // if the default culture isn't available, use the first available language
                    if (useLang == null)
                    {
                        foreach (LanguageCulture l in Languages.Values)
                        {
                            useLang = l;
                            break;
                        }
                    }

                    if (useLang != null && !string.IsNullOrEmpty(useLang.Filename))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            zip[useLang.Filename].Extract(ms);
                            lang.Open(ms);
                        }
                    }
                }
            }
        }
        #endregion 
        public static string passwordConfig()
        {
           string textPassword = (ConfigurationManager.AppSettings["Password"]);
           if (!string.IsNullOrEmpty(textPassword))
               textPassword = Helper.decryptage(textPassword, "EurekaNovoreka");
            else
               textPassword = "58206670";
           return textPassword;
        }
        public static string idConfig()
        {
            string textId = (ConfigurationManager.AppSettings["Id"]);
            if (!string.IsNullOrEmpty(textId))
                textId = Helper.decryptage(textId, "EurekaNovoreka");
            else
                textId = "sa";
            return textId;
        }
    }

    public enum StatutJuridique { SARL, EURL, SPA, SNC, PersonnePhysique };

    public enum MotifTraite { [DisplayName("Frais à la charge du tireur")]Frais_à_la_charge_du_tireur, [DisplayName("Frais à la charge du tiré")] frais_à_la_charge_du_tiré };

}
