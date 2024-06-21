using ATBM_Project.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace ATBM_Project.ViewsModels
{
    public class Admin_VM
    {
        public OracleConnection connection { get; set; }
        public string _role { get; set; }
        public string _user { get; set; }
        private object converter;
        private string[] colors = new string[]
{
    "#F44336", // Red
    "#E91E63", // Pink
    "#9C27B0", // Purple
    "#673AB7", // Deep Purple
    "#3F51B5", // Indigo
    "#2196F3", // Blue
    "#03A9F4", // Light Blue
    "#00BCD4", // Cyan
    "#009688", // Teal
    "#4CAF50", // Green
    "#8BC34A", // Light Green
    "#CDDC39", // Lime
    "#FFEB3B", // Yellow
    "#FFC107", // Amber
    "#FF9800", // Orange
    "#FF5722", // Deep Orange
    "#795548", // Brown
    "#9E9E9E", // Grey
    "#607D8B"  // Blue Grey
};
        public Admin_VM(OracleConnection conn, string Role, string user)
        {
            connection = conn;
            _role = Role;
            _user = user;
        }

        public ObservableCollection<Users> GetUserData()
        {
            ObservableCollection<Users> members = new ObservableCollection<Users>();
            var converter = new BrushConverter();
            //connection.Open();
            string SQLcontext = "SELECT * FROM dba_users";
            using (OracleCommand cmd = new OracleCommand(SQLcontext, connection))
            {
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    int i = 1;
                    while (reader.Read())
                    {
                        string empName = reader.GetString(reader.GetOrdinal("USERNAME"));
                        string dayCreated = reader.GetString(reader.GetOrdinal("CREATED"));
                        char firstCharName = empName[0];
                        members.Add(new Users { Number = i.ToString(), Character = firstCharName.ToString(), BgColor = (Brush)converter.ConvertFromString(colors[(i % 7)]), Name = empName, date_created = dayCreated });
                        i++;
                    }
                    reader.Close();
                }
            }
            return members;
        }
        public bool executeSQL(string sql)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(sql, connection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public void DropUser(Users user)
        {
            if (user == null || user.Name == null || connection == null)
            {
                throw new ArgumentNullException("User, User.Name or connection is null");
            }
            // Thực thi ALTER SESSION


            // Thực thi DROP USER
            string dropUserSQL = $"DROP USER {user.Name} CASCADE";
            OracleCommand dropUserCmd = new OracleCommand(dropUserSQL, connection);
            dropUserCmd.ExecuteNonQuery();

        }
        public void AddNewUser(string userID, string password)
        {


            string SQLCreateUser = $"CREATE USER {userID} IDENTIFIED BY {password}";
            OracleCommand cmdCreateUser = new OracleCommand(SQLCreateUser, connection);
            cmdCreateUser.ExecuteNonQuery();

            string SQLGrantSession = $"GRANT CREATE SESSION TO {userID}";
            OracleCommand cmdGrantSession = new OracleCommand(SQLGrantSession, connection);
            cmdGrantSession.ExecuteNonQuery();
        }


        public ObservableCollection<Role> GetRolesData()
        {
            ObservableCollection<Role> roles = new ObservableCollection<Role>();
            var converter = new BrushConverter();
            string SQLcontext = "select distinct granted_role from dba_role_privs";
            OracleCommand cmd = new OracleCommand(SQLcontext, connection);
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                int i = 1;
                while (reader.Read())
                {
                    string empName = reader.GetString(reader.GetOrdinal("GRANTED_ROLE"));
                    char firstCharName = empName[0];
                    roles.Add(new Role { Number = i.ToString(), Character = firstCharName.ToString(), BgColor = (Brush)converter.ConvertFromString(colors[(i % 7)]), Name = empName });
                    i++;
                }
            }
            return roles;
        }

        public void CreateRole(string roleName)
        {

            string SQLcontext = $"CREATE ROLE {roleName}";
            OracleCommand cmd = new OracleCommand(SQLcontext, connection);
            cmd.ExecuteNonQuery();
        }
        public void DropRole(Role role)
        {
            // Drop the role
            string SQLcontex = $"DROP ROLE {role.Name}";
            OracleCommand cmd = new OracleCommand(SQLcontex, connection);

            cmd.ExecuteNonQuery();
        }

        public ObservableCollection<Models.Table> GetTablesData(string type = "table")
        {
            ObservableCollection<Models.Table> tables = new ObservableCollection<Models.Table>();

            string SQLcontext = "SELECT table_name, count(*) as number_cols \n" +
                                "FROM user_tab_columns \n" +
                                $"where table_name in (select {type}_name from user_{type}s) \n" +
                                "group by table_name";
            OracleCommand cmd = new OracleCommand(SQLcontext, connection);
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                int i = 1;
                while (reader.Read())
                {
                    string tableName = reader.GetString(reader.GetOrdinal("TABLE_NAME"));
                    int numCols = reader.GetInt32(reader.GetOrdinal("NUMBER_COLS"));
                    tables.Add(new Models.Table { Number = i, Name = tableName, NumCols = numCols });
                    i++;
                }
            }
            return tables;
        }

        public ObservableCollection<Column> GetColumnData(string objectName)
        {
            ObservableCollection<Column> columns = new ObservableCollection<Column>();

            string SQLcontext = $"SELECT column_name, data_type, data_length FROM user_tab_columns where table_name = UPPER('{objectName}')";
            OracleCommand cmd = new OracleCommand(SQLcontext, connection);
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                int i = 1;
                while (reader.Read())
                {
                    string colName = reader.GetString(reader.GetOrdinal("COLUMN_NAME"));
                    string dataType = reader.GetString(reader.GetOrdinal("DATA_TYPE"));
                    int dataLength = reader.GetInt32(reader.GetOrdinal("DATA_LENGTH"));
                    columns.Add(new Column { Number = i, Name = colName, DataType = dataType, DataLength = dataLength });
                    i++;
                }
            }
            return columns;
        }

        // this function will be used to get privilleges of role/user in a table/view
        public ObservableCollection<PrivilegeOfTable> getPrivilegesOfTable(string objectName)
        {
            ObservableCollection<PrivilegeOfTable> privileges = new ObservableCollection<PrivilegeOfTable>();

            string SQLcontext = $"select owner, table_name, grantee, privilege, grantable from dba_tab_privs where table_name = upper('{objectName}')";
            OracleCommand cmd = new OracleCommand(SQLcontext, connection);
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                int i = 1;
                while (reader.Read())
                {
                    string owner = reader.GetString(reader.GetOrdinal("OWNER"));
                    string tableName = reader.GetString(reader.GetOrdinal("TABLE_NAME"));
                    string grantee = reader.GetString(reader.GetOrdinal("GRANTEE"));
                    string privilege = reader.GetString(reader.GetOrdinal("PRIVILEGE"));
                    string grantable = reader.GetString(reader.GetOrdinal("GRANTABLE"));
                    privileges.Add(new PrivilegeOfTable
                    {
                        Number = i,
                        Owner = owner,
                        TableName = tableName,
                        Grantee = grantee,
                        Privilege = privilege,
                        Grantable = grantable
                    });
                    i++;
                }
            }
            return privileges;
        }

        public void grantRoleToUser(string role, string user)
        {
            string SQLcontex = $"GRANT {role} TO {user}";
            OracleCommand cmd = new OracleCommand(SQLcontex, connection);
            cmd.ExecuteNonQuery();
        }

        public void grantRoleToUserWithGrantOption(string role, string user)
        {
            string SQLcontex = $"GRANT {role} TO {user} WITH ADMIN OPTION";
            OracleCommand cmd = new OracleCommand(SQLcontex, connection);
            cmd.ExecuteNonQuery();
        }
        public void GrantPrivilegeToUserWithAdminOption(string privilege, string target, string user)
        {
            //string grantPrivilegeSql = "GRANT :privilege ON :target TO :role";
            //OracleCommand cmd = new OracleCommand(grantPrivilegeSql, connection);
            // cmd.Parameters.Add(new OracleParameter("privilege", privilege));
            //cmd.Parameters.Add(new OracleParameter("target", target));
            //cmd.Parameters.Add(new OracleParameter("role", role));
            //cmd.ExecuteNonQuery();
            string grantPrivilegeSql = $"GRANT {privilege} ON {target} TO {user} WITH GRANT OPTION";

            using (OracleCommand cmd = new OracleCommand(grantPrivilegeSql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void GrantPrivilegeToUser(string privilege, string target, string user)
        {
            //string grantPrivilegeSql = "GRANT :privilege ON :target TO :role";
            //OracleCommand cmd = new OracleCommand(grantPrivilegeSql, connection);
            // cmd.Parameters.Add(new OracleParameter("privilege", privilege));
            //cmd.Parameters.Add(new OracleParameter("target", target));
            //cmd.Parameters.Add(new OracleParameter("role", role));
            //cmd.ExecuteNonQuery();
            string grantPrivilegeSql = $"GRANT {privilege} ON {target} TO {user} WITH GRANT OPTION";

            using (OracleCommand cmd = new OracleCommand(grantPrivilegeSql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }


        public void GrantPrivilegeToRole(string privilege, string target, string role)
        {
            //string grantPrivilegeSql = "GRANT :privilege ON :target TO :role";
            //OracleCommand cmd = new OracleCommand(grantPrivilegeSql, connection);
            // cmd.Parameters.Add(new OracleParameter("privilege", privilege));
            //cmd.Parameters.Add(new OracleParameter("target", target));
            //cmd.Parameters.Add(new OracleParameter("role", role));
            //cmd.ExecuteNonQuery();
            string grantPrivilegeSql = $"GRANT {privilege} ON {target} TO {role}";

            using (OracleCommand cmd = new OracleCommand(grantPrivilegeSql, connection))
            {
                cmd.ExecuteNonQuery();
            }


        }


        public void EditUserPassword(string userName, string pwd)
        {
            string SQLContext = "ALTER USER :userName IDENTIFIED BY :pwd";
            using (OracleCommand cmd = new OracleCommand(SQLContext, connection))
            {
                cmd.Parameters.Add(new OracleParameter("userName", userName));
                cmd.Parameters.Add(new OracleParameter("pwd", pwd));

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OracleException ex)
                {
                    // Log error, rethrow, return an error message or handle it appropriately.
                    Console.WriteLine(ex.Message);
                }
            }
        }


        public ObservableCollection<PrivilegeOfTable> GetPrivilegesOfUser(string userName, int type)
        {
            ObservableCollection<PrivilegeOfTable> privs = new ObservableCollection<PrivilegeOfTable>();
            try
            {
                string SQLcontext = "";
                if (type == 1)
                {
                    SQLcontext = $"SELECT grantee, owner, table_name, privilege, grantor FROM dba_tab_privs where grantee in (select granted_role from DBA_role_privs where grantee = '{userName}')";
                }
                else
                {
                    SQLcontext = $"Select grantee, owner, table_name, privilege, grantor from dba_tab_privs where grantee = '{userName}'";
                }

                using (OracleCommand cmd = new OracleCommand(SQLcontext, connection))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        int number = 1;
                        while (reader.Read())
                        {
                            string grantee = reader.GetString(reader.GetOrdinal("GRANTEE"));
                            string owner = reader.GetString(reader.GetOrdinal("OWNER"));
                            string tableName = reader.GetString(reader.GetOrdinal("TABLE_NAME"));
                            string grantor = reader.GetString(reader.GetOrdinal("GRANTOR"));
                            string priv = reader.GetString(reader.GetOrdinal("PRIVILEGE"));

                            privs.Add(new PrivilegeOfTable
                            {
                                Grantee = grantee,
                                Owner = owner,
                                TableName = tableName,
                                Privilege = priv,
                                Grantable = grantor,
                                Number = number++,

                            });

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return privs;
        }


        public void RevokePrivs(PrivilegeOfTable priv, string userName)
        {
            string SQLContext = $"REVOKE {priv.Privilege} ON {priv.TableName} FROM {userName}";
            try
            {
                using (OracleCommand cmd = new OracleCommand(SQLContext, connection))
                {
                    cmd.ExecuteNonQuery();
                    return;
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }



        public ObservableCollection<Role> GetRolesOfUser(string userName)
        {
            ObservableCollection<Role> roles = new ObservableCollection<Role>();
            var converter = new BrushConverter();
            string SQLcontext = $"select * from dba_role_privs where grantee = '{userName}'";
            using (OracleCommand cmd = new OracleCommand(SQLcontext, connection))
            {
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    int i = 1;
                    while (reader.Read())
                    {
                        string roleName = reader.GetString(reader.GetOrdinal("GRANTED_ROLE"));
                        char firstCharName = roleName[0];
                        roles.Add(new Role
                        {
                            Number = i.ToString(),
                            Name = roleName,
                            AdminOption = reader.GetString(reader.GetOrdinal("ADMIN_OPTION"))
                        });
                        i++;
                    }
                }
            }
            return roles;
        }


        public ObservableCollection<Users> GetUsersOfRole(string role)
        {
            var users = new ObservableCollection<Users>();

            try
            {
                string SQLcontext = "select * from dba_role_privs where granted_role = :role";
                using (OracleCommand cmd = new OracleCommand(SQLcontext, connection))
                {
                    cmd.Parameters.Add(new OracleParameter("role", $"{role}"));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        int i = 1;
                        while (reader.Read())
                        {
                            users.Add(new Users
                            {
                                Number = i.ToString(),
                                Name = reader.GetString(reader.GetOrdinal("GRANTEE")),
                                admin_option = reader.GetString(reader.GetOrdinal("ADMIN_OPTION"))
                            });
                            i++;
                        }
                    }
                }
            }
            catch
            {
                return new ObservableCollection<Users>();
            }

            return users;
        }

        public void RevokeRoleFromUser(string role, string username)
        {
            string SQLcontex = $"Revoke {role} from {username}";
            OracleCommand cmd = new OracleCommand(SQLcontex, connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ObservableCollection<PrivilegeOfTable> GetPrivsOfRole(string role)
        {
            var privileges = new ObservableCollection<PrivilegeOfTable>();

            try
            {
                string SQLcontext = "SELECT * FROM DBA_tab_PRIVS where grantee LIKE :role";
                using (OracleCommand cmd = new OracleCommand(SQLcontext, connection))
                {
                    cmd.Parameters.Add(new OracleParameter("role", $"{role}"));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        int i = 1;
                        while (reader.Read())
                        {
                            privileges.Add(new PrivilegeOfTable
                            {
                                Grantee = reader.GetString(reader.GetOrdinal("GRANTEE")),
                                Number = i,
                                TableName = reader.GetString(reader.GetOrdinal("TABLE_NAME")),
                                Privilege = reader.GetString(reader.GetOrdinal("PRIVILEGE")),
                                Grantable = reader.GetString(reader.GetOrdinal("GRANTOR"))
                            });
                            i++;
                        }
                    }
                }
            }
            catch
            {
                return new ObservableCollection<PrivilegeOfTable>();
            }

            return privileges;
        }

        public void RevokePrivFromRole(string role, PrivilegeOfTable privileges)
        {
            string SQLcontext = $"REVOKE {privileges.Privilege} ON {privileges.TableName} FROM {role}";

            using (OracleCommand cmd = new OracleCommand(SQLcontext, connection))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        public ObservableCollection<PrivilegeOfTable> GetPrivsOnColumnOfRole(string role)
        {
            ObservableCollection<PrivilegeOfTable> Privs = new ObservableCollection<PrivilegeOfTable>();
            string SQLcontext = $"select * from DBA_COL_PRIVS where grantee LIKE '{role}'";

            try
            {
                using (OracleCommand cmd = new OracleCommand(SQLcontext, connection))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        int number = 1;
                        while (reader.Read())
                        {
                            string grantee = reader.GetString(reader.GetOrdinal("GRANTEE"));
                            string owner = reader.GetString(reader.GetOrdinal("OWNER"));
                            string tableName = reader.GetString(reader.GetOrdinal("TABLE_NAME"));
                            string grantor = reader.GetString(reader.GetOrdinal("GRANTOR"));
                            string priv = reader.GetString(reader.GetOrdinal("PRIVILEGE"));
                            string col = reader.GetString(reader.GetOrdinal("COLUMN_NAME"));
                            Privs.Add(new PrivilegeOfTable
                            {
                                Grantee = grantee,
                                Owner = owner,
                                TableName = tableName,
                                Privilege = priv,
                                Grantable = grantor,
                                Number = number++,
                                column = col
                            });
                        }
                    }
                }
                return Privs;
            }
            catch (OracleException ex)
            {
                throw new Exception("Oracle error: " + ex.Message);
            }


        }

        public List<Audits> GetStandardAudit()
        {
            try
            {
                var list = new List<Audits>();
                string SQLcontext = $"SELECT SESSIONID, USERNAME, ACTION_NAME, OBJ_NAME,SQL_TEXT, TO_CHAR(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') AS TIMESTAMP, RETURNCODE FROM DBA_AUDIT_TRAIL WHERE OWNER='ADMIN' ORDER BY TIMESTAMP DESC ";

                using (OracleCommand cmd = new OracleCommand(SQLcontext, connection))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Audits
                            {
                                SESSION_ID = reader.IsDBNull(reader.GetOrdinal("SESSIONID")) ? string.Empty : reader.GetString(reader.GetOrdinal("SESSIONID")),
                                USERNAME = reader.IsDBNull(reader.GetOrdinal("USERNAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("USERNAME")),
                                ACTION_NAME = reader.IsDBNull(reader.GetOrdinal("ACTION_NAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("ACTION_NAME")),
                                OBJECT_NAME = reader.IsDBNull(reader.GetOrdinal("OBJ_NAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("OBJ_NAME")),
                                TIMESTAMP = reader.IsDBNull(reader.GetOrdinal("TIMESTAMP")) ? string.Empty : reader.GetString(reader.GetOrdinal("TIMESTAMP")),
                                STATUS = reader.IsDBNull(reader.GetOrdinal("RETURNCODE")) ? "Error" : reader.GetInt32(reader.GetOrdinal("RETURNCODE")) == 0 ? "Success" : "Error",
                                SQL_TEXT = reader.IsDBNull(reader.GetOrdinal("SQL_TEXT")) ? string.Empty : reader.GetString(reader.GetOrdinal("SQL_TEXT")),
                            });
                        }
                    }
                }

                return list;
            }
            catch
            {
                return new List<Audits>();
            }
        }

        public List<Audits> GetFGAudit(string policyName)
        {
            try
            {
                var list = new List<Audits>();
                string SQLcontext = $"SELECT SESSION_ID, DB_USER, STATEMENT_TYPE, OBJECT_NAME, TO_CHAR(TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') AS TIMESTAMP, SQL_TEXT FROM DBA_FGA_AUDIT_TRAIL WHERE OBJECT_SCHEMA='ADMIN' AND POLICY_NAME='{policyName}' ORDER BY TIMESTAMP DESC ";

                using (OracleCommand cmd = new OracleCommand(SQLcontext, connection))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Audits
                            {
                                SESSION_ID = reader.IsDBNull(reader.GetOrdinal("SESSION_ID")) ? string.Empty : reader.GetString(reader.GetOrdinal("SESSION_ID")),
                                USERNAME = reader.IsDBNull(reader.GetOrdinal("DB_USER")) ? string.Empty : reader.GetString(reader.GetOrdinal("DB_USER")),
                                ACTION_NAME = reader.IsDBNull(reader.GetOrdinal("STATEMENT_TYPE")) ? string.Empty : reader.GetString(reader.GetOrdinal("STATEMENT_TYPE")),
                                OBJECT_NAME = reader.IsDBNull(reader.GetOrdinal("OBJECT_NAME")) ? string.Empty : reader.GetString(reader.GetOrdinal("OBJECT_NAME")),
                                TIMESTAMP = reader.IsDBNull(reader.GetOrdinal("TIMESTAMP")) ? string.Empty : reader.GetString(reader.GetOrdinal("TIMESTAMP")),
                                SQL_TEXT = reader.IsDBNull(reader.GetOrdinal("SQL_TEXT")) ? string.Empty : reader.GetString(reader.GetOrdinal("SQL_TEXT")),

                            });
                        }
                    }
                }

                return list;
            }
            catch
            {
                return new List<Audits>();
            }
        }

        public void execProcedure(string procedureName, params object[] parameters)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection;
            cmd.CommandText = procedureName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;


            for (int i = 0; i < parameters.Length; i++)
            {
                cmd.Parameters.Add(new OracleParameter("param" + (i + 1), parameters[i]));
            }

            cmd.ExecuteNonQuery();
        }

        public void execProcedure(string procedureName, out int result)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = connection;
            cmd.CommandText = procedureName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            OracleParameter outputParam = new OracleParameter("result", OracleDbType.Int32, ParameterDirection.Output);
            cmd.Parameters.Add(outputParam);

            cmd.ExecuteNonQuery();
            result = int.Parse(cmd.Parameters["result"].Value.ToString());
        }

        public void EnableAuditPolicy(string schema, string objectName, string policyName, bool enable)
        {
            string plsql = $@"
    BEGIN
        dbms_fga.enable_policy(
            object_schema => '{schema}',
            object_name   => '{objectName}',
            policy_name   => '{policyName}',
            enable        => {enable.ToString().ToLower()}
        );
    END;";

            using (OracleCommand cmd = new OracleCommand(plsql, connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }

        public void DisableAuditPolicy(string schema, string objectName, string policyName)
        {
            string plsql = $@"
    BEGIN
        dbms_fga.disable_policy(
            object_schema => '{schema}',
            object_name   => '{objectName}',
            policy_name   => '{policyName}'
        );
    END;";

            using (OracleCommand cmd = new OracleCommand(plsql, connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }




    }
}
