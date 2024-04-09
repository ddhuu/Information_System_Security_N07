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

    }
}
