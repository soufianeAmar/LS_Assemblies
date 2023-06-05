using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSAdmin
{
    public class DBUtils
    {
        public static void CreateDatabase(string database)
        {
            Server server = new Server();
            Database db = new Database(server, database);
            if (!server.Databases.Contains(database))
                db.Create();
            else
                throw new Exception("Base de données existante !");
        }

        public static void CreateServerDatabase(string serverName, string database)
        {
            string password = Core.passwordConfig();
            string id = Core.idConfig();

            //Server server = new Server(serverName);
            string connectionString = string.Format("Integrated Security=false;Pooling=false;Data Source={0};" +
               "Initial Catalog=master;User ID={2};Password={1}", serverName, password,id);
            Server server = new Server(new ServerConnection(new SqlConnection(connectionString)));
            System.Diagnostics.Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tServer created", DateTime.Now));
            Database db = new Database(server, database);
            if (!server.Databases.Contains(database))
                db.Create();
            else
                throw new Exception("Base de données existante !");
        }

        public static void CreateDatabaseFromBackup(string database, string bkfileName)
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
                server.KillAllProcesses(database);
                restore.SqlRestore(server);
            }
        }

        public static void CreateServerDatabaseFromBackup(string serverName, string database, string bkfileName)
        {
            string password = Core.passwordConfig();
            string id = Core.idConfig();

            //Server server = new Server(serverName);
            string connectionString = string.Format("Integrated Security=false;Pooling=false;Data Source={0};" +
               "Initial Catalog=master;User ID={2};Password={1}", serverName, password,id);
            Server server = new Server(new ServerConnection(new SqlConnection(connectionString)));
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
                server.KillAllProcesses(database);
                restore.SqlRestore(server);
            }
        }

        public static void CreateDatabaseInPath(string database, string dataPath)
        {
            Server server = new Server();
            Database db = new Database(server, database);
            if (!server.Databases.Contains(database))
            {
                db.AutoShrink = false;
                db.FileGroups.Add(new FileGroup(db, "PRIMARY"));
                db.FileGroups[0].Files.Add(new DataFile(db.FileGroups[0], database, string.Format("{0}{1}.mdf", dataPath, database)) { Growth = 10, GrowthType = FileGrowthType.Percent });
                db.LogFiles.Add(new LogFile(db, string.Format("{0}_log", database), string.Format("{0}{1}_log.ldf", dataPath, database)) { Growth = 10, GrowthType = FileGrowthType.Percent });
                var script = db.Script();
                db.Create();
            }
            else
                throw new Exception("Base de données existante !");
        }

        public static void CreateServerDatabaseInPath(string serverName, string database, string dataPath)
        {
            string password = Core.passwordConfig();
            string id = Core.idConfig();

            //Server server = new Server(serverName);
            string connectionString = string.Format("Integrated Security=false;Pooling=false;Data Source={0};" +
               "Initial Catalog=master;User ID={2};Password={1}", serverName, password,id);
            Server server = new Server(new ServerConnection(new SqlConnection(connectionString)));
            Database db = new Database(server, database);
            if (!server.Databases.Contains(database))
            {
                db.AutoShrink = false;
                db.FileGroups.Add(new FileGroup(db, "PRIMARY"));
                db.FileGroups[0].Files.Add(new DataFile(db.FileGroups[0], database, string.Format("{0}{1}.mdf", dataPath, database)) { Growth = 10, GrowthType = FileGrowthType.Percent });
                db.LogFiles.Add(new LogFile(db, string.Format("{0}_log", database), string.Format("{0}{1}_log.ldf", dataPath, database)) { Growth = 10, GrowthType = FileGrowthType.Percent });
                var script = db.Script();
                db.Create();
            }
            else
                throw new Exception("Base de données existante !");
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

        public static void CreateServerDatabaseFromBackupInPath(string serverName, string database, string dataPath, string bkfileName)
        {
            string password = Core.passwordConfig();
            string id = Core.idConfig();

            //Server server = new Server(serverName);
            string connectionString = string.Format("Integrated Security=false;Pooling=false;Data Source={0};" +
               "Initial Catalog=master;User ID={2};Password={1}", serverName, password,id);
            Server server = new Server(new ServerConnection(new SqlConnection(connectionString)));
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

        public static void BackupDatabase(string serverName, string database, string fileName)
        {
            string password = Core.passwordConfig();
            string id = Core.idConfig();
            
            Backup backup = new Backup()
            {
                Database = database,
                Action = BackupActionType.Database,
                Initialize = true
            };
            backup.Devices.Add(new BackupDeviceItem(fileName, DeviceType.File));
            //Server server = new Server(serverName);
            Server server = new Server(new ServerConnection(new SqlConnection(string.Format("Integrated Security=false;Pooling=false;Data Source={0};" +
               "Initial Catalog=master;User ID={2};Password={1}", serverName, password,id))));
            backup.SqlBackup(server);
        }

        public static void RestoreDatabase(string serverName, string database, string fileName)
        {
            string password = Core.passwordConfig();
            string id = Core.idConfig();

            Restore restore = new Restore()
            {
                Database = database,
                Action = RestoreActionType.Database,
                ReplaceDatabase = true,
            };
            restore.Devices.Add(new BackupDeviceItem(fileName, DeviceType.File));
            //Server server = new Server(serverName);
            string connectionString = string.Format("Integrated Security=false;Pooling=false;Data Source={0};" +
               "Initial Catalog=master;User ID={2};Password={1}", serverName, password,id);
            Server server = new Server(new ServerConnection(new SqlConnection(connectionString)));
            Database db = server.Databases[database];
            DataTable dtFileList = restore.ReadFileList(server);
            string dbLogicalName = dtFileList.Rows[0][0].ToString();
            string dbPhysicalName = dtFileList.Rows[0][1].ToString();
            string logLogicalName = dtFileList.Rows[1][0].ToString();
            string logPhysicalName = dtFileList.Rows[1][1].ToString();

            restore.RelocateFiles.Add(new RelocateFile(dbLogicalName, db.FileGroups[0].Files[0].FileName));
            restore.RelocateFiles.Add(new RelocateFile(logLogicalName, db.LogFiles[0].FileName));
            server.KillAllProcesses(database);
            restore.SqlRestore(server);
        }

        public static void DropDatabase(string serverName, string database)
        {
            //// Test Amar 30-01-2023
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
            //// Test Amar 30-01-2023
            string password = Core.passwordConfig();
            string id = Core.idConfig();

            string connectionString = string.Format("Integrated Security=false;Pooling=false;Data Source={0};" +
               "Initial Catalog=master;User ID={2};Password={1}", serverName, password,id);
            Server server = new Server(new ServerConnection(new SqlConnection(connectionString)));
            if (server.Databases.Contains(database))
                server.KillDatabase(database);
        }
    }
}
