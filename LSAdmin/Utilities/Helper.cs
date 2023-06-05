using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Reports;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base.ReportsV2;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.XtraReports.UI;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LSAdmin
{
    public class Helper
    {
        public static string serverName;
        public static string instanceName;
        public static XafApplication currentApp;
        public static string language;
        public static bool checkPlan = true;

        //Test Decryptage
        public static string decryptage(string encrypt, string key)
        {
            using (System.Security.Cryptography.TripleDESCryptoServiceProvider descrypt = new System.Security.Cryptography.TripleDESCryptoServiceProvider())
            {
                using (System.Security.Cryptography.MD5CryptoServiceProvider hashMD5Provider = new System.Security.Cryptography.MD5CryptoServiceProvider())
                {
                    byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                    descrypt.Key = byteHash;
                    descrypt.Mode = System.Security.Cryptography.CipherMode.ECB;
                    byte[] data = Convert.FromBase64String(encrypt);
                    return Encoding.UTF8.GetString(descrypt.CreateDecryptor().TransformFinalBlock(data, 0, data.Length));
                }
            }
        }
        //Test Encrypt
        public static string cryptage(string source, string key)
        {
            using (System.Security.Cryptography.TripleDESCryptoServiceProvider crypt = new System.Security.Cryptography.TripleDESCryptoServiceProvider())
            {
                using (System.Security.Cryptography.MD5CryptoServiceProvider hash = new System.Security.Cryptography.MD5CryptoServiceProvider())
                {
                    byte[] byteHash = hash.ComputeHash(Encoding.UTF8.GetBytes(key));
                    crypt.Key = byteHash;
                    crypt.Mode = System.Security.Cryptography.CipherMode.ECB;
                    byte[] data = Encoding.UTF8.GetBytes(source);
                    return Convert.ToBase64String(crypt.CreateEncryptor().TransformFinalBlock(data, 0, data.Length));
                }
            }
        }
        public static void CreateServerDatabaseFromBackup(string serverName, string database, string bkfileName)
        {
            string password = Core.passwordConfig();
            string id = Core.idConfig();

            Server server = new Server(serverName);
            server.ConnectionContext.LoginSecure = false;
            server.ConnectionContext.Login = id;//"sa";
            server.ConnectionContext.Password = password;//;"58206670";
            //Database db = new Database(server, database);
            if (!server.Databases.Contains(database))
            {
                string defaultDataPath = string.IsNullOrEmpty(server.Settings.DefaultFile) ? server.MasterDBPath : server.Settings.DefaultFile;
                string defaultLogPath = string.IsNullOrEmpty(server.Settings.DefaultLog) ? server.MasterDBLogPath : server.Settings.DefaultLog;
                Restore restore = new Restore()
                {
                    Database = database,
                    Action = RestoreActionType.Database,
                    ReplaceDatabase = true,
                };
                restore.Devices.Add(new BackupDeviceItem(bkfileName, DeviceType.File));
                System.Data.DataTable logicalRestoreFiles = restore.ReadFileList(server); //Column names : {LogicalName}, {PhysicalName}, {Type}, {FileGroupName}, {Size}, {MaxSize}
                foreach (DataRow file in logicalRestoreFiles.Rows)
                {
                    string fileName = System.IO.Path.GetFileName(file["PhysicalName"].ToString());
                    if (file["Type"].ToString() == "D")
                        restore.RelocateFiles.Add(new RelocateFile(database, string.Concat(defaultDataPath, "\\", fileName)));
                    if (file["Type"].ToString() == "L")
                        restore.RelocateFiles.Add(new RelocateFile(database + "_Log", string.Concat(defaultLogPath, "\\", fileName)));
                }
                server.KillAllProcesses(database);
                restore.SqlRestore(server);
            }
        }
        public static void CreateDatabaseFromBackupInPath(string database, string dataPath, string bkfileName)
        {
            Server server = new Server();
            Database db = new Database(server, database);
            if (!server.Databases.Contains(database))
            {
                Restore restore = new Restore()
                {
                    Database = database,
                    Action = RestoreActionType.Database,
                    ReplaceDatabase = true,
                };
                restore.Devices.Add(new BackupDeviceItem(bkfileName, DeviceType.File));
                DataTable dtFileList = restore.ReadFileList(server);
                string dbLogicalName = dtFileList.Rows[0][0].ToString();
                string dbPhysicalName = dtFileList.Rows[0][1].ToString();
                string logLogicalName = dtFileList.Rows[1][0].ToString();
                string logPhysicalName = dtFileList.Rows[1][1].ToString();
                restore.RelocateFiles.Add(new RelocateFile(dbLogicalName, string.Format("{0}{1}.mdf", dataPath, database)));
                restore.RelocateFiles.Add(new RelocateFile(logLogicalName, string.Format("{0}{1}_log.ldf", dataPath, database)));
                server.KillAllProcesses(database);
                restore.SqlRestore(server);
            }
        }
        public static void CreateServerDatabaseInPath(string serverName, string database, string dataPath)
        {
            Server server = new Server(serverName);
            Database db = new Database(server, database);
            if (!server.Databases.Contains(database))
            {
                db.AutoShrink = false;
                db.FileGroups.Add(new FileGroup(db, "PRIMARY"));
                db.FileGroups[0].Files.Add(new DataFile(db.FileGroups[0], database, string.Format("{0}{1}.mdf", dataPath, database)) { Growth = 10, GrowthType = FileGrowthType.Percent });
                db.LogFiles.Add(new LogFile(db, string.Format("{0}_log", database), string.Format("{0}{1}_log.ldf", dataPath, database)) { Growth = 10, GrowthType = FileGrowthType.Percent });
                //db.Collation = "Arabic_CI_AS";
                var script = db.Script();
                db.Create();
            }
            else
                throw new Exception("Base de données existante !");
        }
        public static void CreateServerDatabaseFromBackupInPath(string serverName, string database, string dataPath, string bkfileName)
        {
            Server server = new Server(serverName);
            Database db = new Database(server, database);
            if (!server.Databases.Contains(database))
            {
                Restore restore = new Restore()
                {
                    Database = database,
                    Action = RestoreActionType.Database,
                    ReplaceDatabase = true,
                };
                restore.Devices.Add(new BackupDeviceItem(bkfileName, DeviceType.File));
                DataTable dtFileList = restore.ReadFileList(server);
                string dbLogicalName = dtFileList.Rows[0][0].ToString();
                string dbPhysicalName = dtFileList.Rows[0][1].ToString();
                string logLogicalName = dtFileList.Rows[1][0].ToString();
                string logPhysicalName = dtFileList.Rows[1][1].ToString();
                restore.RelocateFiles.Add(new RelocateFile(dbLogicalName, string.Format("{0}{1}.mdf", dataPath, database)));
                restore.RelocateFiles.Add(new RelocateFile(logLogicalName, string.Format("{0}{1}_log.ldf", dataPath, database)));
                server.KillAllProcesses(database);
                restore.SqlRestore(server);
            }
        }
        public static void CreateReportV2FromResource(string nameSpace, string fileName, bool InPlace, IObjectSpace ObjectSpace)
        {
            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("{0}.{1}", nameSpace, fileName));
            if (s != null)
            {
                string reportName = Path.GetFileNameWithoutExtension(fileName);
                ReportDataV2 reportdata = ObjectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", reportName));
                if (reportdata == null)
                {
                    XafReport xafReport = new XafReport();
                    xafReport.LoadLayout(s);
                    CollectionDataSource cds = new CollectionDataSource()
                    {
                        ObjectTypeName = xafReport.DataType != null ? xafReport.DataType.FullName : string.Empty,
                        CriteriaString = xafReport.Filtering.Filter
                    };
                    XtraReport xtraReport = new XtraReport() { DisplayName = reportName };
                    xtraReport.LoadLayout(s);
                    xtraReport.DataSource = cds;
                    reportdata = ObjectSpace.CreateObject<ReportDataV2>();
                    reportdata.DisplayName = reportName;
                    reportdata.IsInplaceReport = InPlace;
                    ReportsStorage rs = new ReportsStorage();
                    rs.SaveReport(reportdata, xtraReport);
                    reportdata.Save();
                    ((XPObjectSpace)ObjectSpace).Session.CommitTransaction();
                }
            }
        }
        public static void CreateReportV2FromResource(Assembly assembly, string nameSpace, string fileName, bool InPlace, IObjectSpace ObjectSpace)
        {
            Stream s = assembly.GetManifestResourceStream(string.Format("{0}.{1}", nameSpace, fileName));
            if (s != null)
            {
                string reportName = Path.GetFileNameWithoutExtension(fileName);
                ReportDataV2 reportdata = ObjectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", reportName));
                if (reportdata == null)
                {
                    XafReport xafReport = new XafReport();
                    xafReport.LoadLayout(s);
                    CollectionDataSource cds = new CollectionDataSource()
                    {
                        ObjectTypeName = xafReport.DataType != null ? xafReport.DataType.FullName : string.Empty,
                        CriteriaString = xafReport.Filtering.Filter
                    };
                    XtraReport xtraReport = new XtraReport() { DisplayName = reportName };
                    xtraReport.LoadLayout(s);
                    xtraReport.DataSource = cds;
                    reportdata = ObjectSpace.CreateObject<ReportDataV2>();
                    reportdata.DisplayName = reportName;
                    reportdata.IsInplaceReport = InPlace;
                    //ReportsStorage rs = new ReportsStorage();
                    //rs.SaveReport(reportdata, xtraReport);
                    ReportDataProvider.ReportsStorage.SaveReport(reportdata, xtraReport);
                    reportdata.Save();
                    ((XPObjectSpace)ObjectSpace).Session.CommitTransaction();
                }
            }
        }
        public static void ImportReportV2(string fileName, IObjectSpace ObjectSpace)
        {
            string reportDisplayName = Path.GetFileNameWithoutExtension(fileName);
            object result = ObjectSpace.FindObject<ReportDataV2>(CriteriaOperator.Parse("DisplayName = ?", reportDisplayName), true);
            if (result == null)
            {
                XtraReport report = XtraReport.FromFile(fileName, true);
                var reportData = ObjectSpace.CreateObject<ReportDataV2>();
                ReportDataProvider.ReportsStorage.SaveReport(reportData, report);
                reportData.Save();
                ObjectSpace.CommitChanges();
            }
        }
        public static void LoadDefaultReports(string reportFolder, IObjectSpace ObjectSpace)
        {
            if (Directory.Exists(reportFolder))
            {
                string[] reports = Directory.GetFiles(reportFolder);
                for (int i = 0; i < reports.Length; i++)
                {
                    ImportReportV2(reports[i], ObjectSpace);
                }
            }
        }

    }
}
