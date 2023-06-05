using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace LsSecurityModule
{
    public class Helper
    {
        public static LsSecuritySystemRole CreateRole(IObjectSpace ObjectSpace, string RoleName, bool IsAdmin, bool Update)
        {
            Session session = ((XPObjectSpace)ObjectSpace).Session;
            try
            {
                SelectedData sd = session.ExecuteQuery(string.Format("select Oid from SecuritySystemRole where Name = '{0}' and GCRecord is null", RoleName));
                foreach (SelectStatementResultRow ssrr in sd.ResultSet[0].Rows)
                {
                    session.ExecuteQuery(string.Format("insert into LsSecuritySystemRole(Oid) select'{0}' " +
                        "where not exists (select Oid from LsSecuritySystemRole where Oid = '{0}')", ssrr.Values[0]));
                }
                int lsRoleType = session.GetObjectType(session.GetClassInfo<LsSecuritySystemRole>()).Oid;
                session.ExecuteQuery(string.Format("update SecuritySystemRole set ObjectType = {0} " +
                    "where Name = '{1}' and GCRecord is null", lsRoleType, RoleName));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError creating Role : {1}", DateTime.Now, ex.Message));
            }
            ObjectSpace.Refresh();
            LsSecuritySystemRole role = ObjectSpace.FindObject<LsSecuritySystemRole>(new BinaryOperator("Name", RoleName));
            if (role == null)
            {
                role = ObjectSpace.CreateObject<LsSecuritySystemRole>();
                role.Name = RoleName;
                role.IsAdministrative = IsAdmin;
                role.Save();
                ObjectSpace.CommitChanges();
            }
            else
                if (Update)
                {
                    role.IsAdministrative = IsAdmin;
                    role.Save();
                    ObjectSpace.CommitChanges();
                }
            return role;
        }
        public static LsSecuritySystemUser CreateUser(IObjectSpace ObjectSpace, string UserName, string Password, string RoleName, bool IsAdministrator)
        {
            Session session = ((XPObjectSpace)ObjectSpace).Session;
            try
            {
            SelectedData sd = session.ExecuteQuery(string.Format("select Oid from SecuritySystemUser where UserName = '{0}' and GCRecord is null", UserName));
            foreach (SelectStatementResultRow ssrr in sd.ResultSet[0].Rows)
                session.ExecuteQuery(string.Format("insert into LsSecuritySystemUser(Oid) select'{0}' " +
                    "where not exists (select Oid from LsSecuritySystemUser where Oid = '{0}')", ssrr.Values[0]));
            int lsUserType = session.GetObjectType(session.GetClassInfo<LsSecuritySystemUser>()).Oid;
            session.ExecuteQuery(string.Format("update SecuritySystemUser set ObjectType = {0} " +
                "where UserName = '{1}' and GCRecord is null", lsUserType, UserName));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError creating User : {1}", DateTime.Now, ex.Message));
            }
            ObjectSpace.Refresh();
            session = ((XPObjectSpace)ObjectSpace).Session;
            LsSecuritySystemRole Role = Helper.CreateRole(ObjectSpace, RoleName, IsAdministrator, true);
            LsSecuritySystemUser user = ObjectSpace.FindObject<LsSecuritySystemUser>(new BinaryOperator("UserName", UserName));
            if (user == null)
            {
                user = ObjectSpace.CreateObject<LsSecuritySystemUser>();
                user.UserName = UserName;
                user.Roles.Add(Role);
                user.SetPassword(Password);
                user.Save();
                ObjectSpace.CommitChanges();
            }
            return user;
        }

        public static LsSecuritySystemRole CreateDefaultRole(IObjectSpace ObjectSpace)
        {
            LsSecuritySystemRole defaultRole = ObjectSpace.FindObject<LsSecuritySystemRole>(new BinaryOperator("Name", "Default"));
            if (defaultRole == null)
            {
                defaultRole = ObjectSpace.CreateObject<LsSecuritySystemRole>();
                defaultRole.Name = "Default";

                defaultRole.AddObjectAccessPermission<LsSecuritySystemUser>("[Oid] = CurrentUserId()", SecurityOperations.ReadOnlyAccess);
                defaultRole.AddMemberAccessPermission<LsSecuritySystemUser>("ChangePasswordOnFirstLogon", SecurityOperations.Write);
                defaultRole.AddMemberAccessPermission<LsSecuritySystemUser>("StoredPassword", SecurityOperations.Write);
                defaultRole.SetTypePermissionsRecursively<LsSecuritySystemRole>(SecurityOperations.Read, SecuritySystemModifier.Allow);
                defaultRole.SetTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecuritySystemModifier.Allow);
                defaultRole.SetTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecuritySystemModifier.Allow);
            }
            return defaultRole;
        }
        public static void CreateUserAdmin(IObjectSpace ObjectSpace, string userName, string password)
        {
            Session session = ((XPObjectSpace)ObjectSpace).Session;
            try
            {
                SelectedData sd = session.ExecuteQuery("select Oid from SecuritySystemRole where Name = 'Administrateur' and GCRecord is null");
                foreach (SelectStatementResultRow ssrr in sd.ResultSet[0].Rows)
                {
                    session.ExecuteQuery(string.Format("insert into LsSecuritySystemRole(Oid) select'{0}' " +
                        "where not exists (select Oid from LsSecuritySystemRole where Oid = '{0}')", ssrr.Values[0]));
                }
                int lsRoleType = session.GetObjectType(session.GetClassInfo<LsSecuritySystemRole>()).Oid;
                session.ExecuteQuery(string.Format("update SecuritySystemRole set ObjectType = {0} " +
                    "where Name = 'Administrateur' and GCRecord is null", lsRoleType));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError creating Admin Role : {1}", DateTime.Now, ex.Message));
            }
            try
            {
                SelectedData sd = session.ExecuteQuery(string.Format("select Oid from SecuritySystemUser where UserName = '{0}' and GCRecord is null", userName));
                foreach (SelectStatementResultRow ssrr in sd.ResultSet[0].Rows)
                    session.ExecuteQuery(string.Format("insert into LsSecuritySystemUser(Oid) select'{0}' " +
                        "where not exists (select Oid from LsSecuritySystemUser where Oid = '{0}')", ssrr.Values[0]));
                int lsUserType = session.GetObjectType(session.GetClassInfo<LsSecuritySystemUser>()).Oid;
                session.ExecuteQuery(string.Format("update SecuritySystemUser set ObjectType = {0} " +
                    "where UserName = '{1}' and GCRecord is null", lsUserType, userName));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError creating Admin User : {1}", DateTime.Now, ex.Message));
            }
            ObjectSpace.Refresh();
            //session is disposed on ObjectSpace.Refresh. It must be re-assigned
            session = ((XPObjectSpace)ObjectSpace).Session;
            LsSecuritySystemRole role = ObjectSpace.FindObject<LsSecuritySystemRole>(new BinaryOperator("Name", "Administrateur"));
            if (role == null)
            {
                role = ObjectSpace.CreateObject<LsSecuritySystemRole>();
                role.Name = "Administrateur";
                //Create permissions and assign them to the role
                role.IsAdministrative = true;
                role.Save();
            }
            LsSecuritySystemUser user = ObjectSpace.FindObject<LsSecuritySystemUser>(new BinaryOperator("UserName", userName));
            if (user == null)
            {
                user = ObjectSpace.CreateObject<LsSecuritySystemUser>();
                user.UserName = userName;
                // Make the user an administrator
                user.Roles.Add(role);
                // Set a password if the standard authentication type is used
                user.SetPassword(password);
                // Save the user to the database
                user.Save();
            }
            ObjectSpace.CommitChanges();
        }
        public static void CreateUserAdmin(IObjectSpace ObjectSpace, string firstName, string surName, string email, string userName, string password)
        {
            Session session = ((XPObjectSpace)ObjectSpace).Session;
            try
            {
                SelectedData sd = session.ExecuteQuery("select Oid from SecuritySystemRole where Name = 'Administrateur' and GCRecord is null");
                foreach (SelectStatementResultRow ssrr in sd.ResultSet[0].Rows)
                {
                    session.ExecuteQuery(string.Format("insert into LsSecuritySystemRole(Oid) select'{0}' " +
                        "where not exists (select Oid from LsSecuritySystemRole where Oid = '{0}')", ssrr.Values[0]));
                }
                int lsRoleType = session.GetObjectType(session.GetClassInfo<LsSecuritySystemRole>()).Oid;
                session.ExecuteQuery(string.Format("update SecuritySystemRole set ObjectType = {0} " +
                    "where Name = 'Administrateur' and GCRecord is null", lsRoleType));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError creating Admin Role : {1}", DateTime.Now, ex.Message));
            }
            try
            {
                SelectedData sd = session.ExecuteQuery(string.Format("select Oid from SecuritySystemUser where UserName = '{0}' and GCRecord is null", userName));
                foreach (SelectStatementResultRow ssrr in sd.ResultSet[0].Rows)
                    session.ExecuteQuery(string.Format("insert into LsSecuritySystemUser(Oid) select'{0}' " +
                        "where not exists (select Oid from LsSecuritySystemUser where Oid = '{0}')", ssrr.Values[0]));
                int lsUserType = session.GetObjectType(session.GetClassInfo<LsSecuritySystemUser>()).Oid;
                session.ExecuteQuery(string.Format("update SecuritySystemUser set ObjectType = {0} " +
                    "where UserName = '{1}' and GCRecord is null", lsUserType, userName));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError creating Admin User : {1}", DateTime.Now, ex.Message));
            }
            ObjectSpace.Refresh();
            session = ((XPObjectSpace)ObjectSpace).Session;
            LsSecuritySystemRole role = ObjectSpace.FindObject<LsSecuritySystemRole>(new BinaryOperator("Name", "Administrateur"));
            if (role == null)
            {
                role = ObjectSpace.CreateObject<LsSecuritySystemRole>();
                role.Name = "Administrateur";
                role.IsAdministrative = true;
                role.Save();
            }
            LsSecuritySystemUser user = ObjectSpace.FindObject<LsSecuritySystemUser>(new BinaryOperator("UserName", userName));
            if (user == null)
            {
                user = ObjectSpace.CreateObject<LsSecuritySystemUser>();
                user.UserName = userName;
                user.eMail = email;
                user.firstName = firstName;
                user.lastName = surName;
                user.Roles.Add(role);
                user.SetPassword(password);
                user.Save();
            }
            ObjectSpace.CommitChanges();
        }
        public static void CreateUser(IObjectSpace ObjectSpace, string firstName, string surName, string email, string userName, string password)
        {
            Session session = ((XPObjectSpace)ObjectSpace).Session;
            try
            {
                SelectedData sd = session.ExecuteQuery("select Oid from SecuritySystemRole where Name = 'Default' and GCRecord is null");
                foreach (SelectStatementResultRow ssrr in sd.ResultSet[0].Rows)
                {
                    session.ExecuteQuery(string.Format("insert into LsSecuritySystemRole(Oid) select'{0}' " +
                        "where not exists (select Oid from LsSecuritySystemRole where Oid = '{0}')", ssrr.Values[0]));
                }
                int lsRoleType = session.GetObjectType(session.GetClassInfo<LsSecuritySystemRole>()).Oid;
                session.ExecuteQuery(string.Format("update SecuritySystemRole set ObjectType = {0} " +
                    "where Name = 'Default' and GCRecord is null", lsRoleType));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError creating Role : {1}", DateTime.Now, ex.Message));
            }
            try
            {
                SelectedData sd = session.ExecuteQuery(string.Format("select Oid from SecuritySystemUser where UserName = '{0}' and GCRecord is null", userName));
                foreach (SelectStatementResultRow ssrr in sd.ResultSet[0].Rows)
                    session.ExecuteQuery(string.Format("insert into LsSecuritySystemUser(Oid) select'{0}' " +
                        "where not exists (select Oid from LsSecuritySystemUser where Oid = '{0}')", ssrr.Values[0]));
                int lsUserType = session.GetObjectType(session.GetClassInfo<LsSecuritySystemUser>()).Oid;
                session.ExecuteQuery(string.Format("update SecuritySystemUser set ObjectType = {0} " +
                    "where UserName = '{1}' and GCRecord is null", lsUserType, userName));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError creating User : {1}", DateTime.Now, ex.Message));
            }
            ObjectSpace.Refresh();
            session = ((XPObjectSpace)ObjectSpace).Session;
            LsSecuritySystemRole role = CreateDefaultRole(ObjectSpace);
            LsSecuritySystemUser user = ObjectSpace.FindObject<LsSecuritySystemUser>(new BinaryOperator("UserName", userName));
            if (user == null)
            {
                user = ObjectSpace.CreateObject<LsSecuritySystemUser>();
                user.UserName = userName;
                user.eMail = email;
                user.firstName = firstName;
                user.lastName = surName;
                user.Roles.Add(role);
                user.SetPassword(password);
                user.Save();
            }
            ObjectSpace.CommitChanges();
        }

        public static bool IsLSAdmin(IObjectSpace ObjectSpace)
        {
            bool result = false;
            DevExpress.Xpo.Helpers.BaseDataLayer baseDataLayer = ((XPObjectSpace)ObjectSpace).Session.DataLayer as DevExpress.Xpo.Helpers.BaseDataLayer;
            if (!(ObjectSpace is NonPersistentObjectSpace) && baseDataLayer != null)
            {
                DevExpress.Xpo.DB.ConnectionProviderSql connectionProvider = baseDataLayer.ConnectionProvider as DevExpress.Xpo.DB.ConnectionProviderSql;
                if (connectionProvider != null)
                {
                    IDbConnection connection = connectionProvider.Connection;
                    if (connection is SqlConnection)
                        result = connection.Database == "LSAdmin";
                    else if (connection is SQLiteConnection)
                        result = ((SQLiteConnection)connection).DataSource == "LSAdmin";
                }
            }
            return result;
        }
    }
}
