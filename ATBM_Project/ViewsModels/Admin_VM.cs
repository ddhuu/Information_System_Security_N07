using ATBM_Project.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
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
        public void ExecuteAlterSession()
        {


            // Thực thi ALTER SESSION
            string alterSessionSQL = "ALTER SESSION SET \"_oracle_script\"=true";
            OracleCommand alterSessionCmd = new OracleCommand(alterSessionSQL, connection);
            alterSessionCmd.ExecuteNonQuery();

        }

        public void DropUser(Users user)
        {
            if (user == null || user.Name == null || connection == null)
            {
                throw new ArgumentNullException("User, User.Name or connection is null");
            }
            // Thực thi ALTER SESSION
            ExecuteAlterSession();

            // Thực thi DROP USER
            string dropUserSQL = $"DROP USER {user.Name} CASCADE";
            OracleCommand dropUserCmd = new OracleCommand(dropUserSQL, connection);
            dropUserCmd.ExecuteNonQuery();

        }
        public void AddNewUser(string userID, string password)
        {
            ExecuteAlterSession();

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
            ExecuteAlterSession();
            string SQLcontext = $"CREATE ROLE {roleName}";
            OracleCommand cmd = new OracleCommand(SQLcontext, connection);
            cmd.ExecuteNonQuery();
        }
        public void DropRole(Role role)
        {
            // Drop the role
            MessageBox.Show($"{role.Name}");
            string SQLcontex = $"DROP ROLE {role.Name}";
            OracleCommand cmd = new OracleCommand(SQLcontex, connection);
            cmd.ExecuteNonQuery();
        }

        public ObservableCollection<Table> GetTablesData(string type = "table")
        {
            ObservableCollection<Table> tables = new ObservableCollection<Table>();

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
                    tables.Add(new Table { Number = i, Name = tableName, NumCols = numCols });
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
                                Number = number++
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




    }
}
