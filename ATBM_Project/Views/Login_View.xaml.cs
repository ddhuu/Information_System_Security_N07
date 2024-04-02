using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ATBM_Project.Views
{
    /// <summary>
    /// Interaction logic for Login_View.xaml
    /// </summary>
    public partial class Login_View : Window
    {
        public Login_View()
        {
            InitializeComponent();
        }
        private void directWindowUser(OracleConnection conn, Session session)
        {
            if (session.Role != "DBA")
            {
                MessageBox.Show($"Only DBA role is allowed to use this system. Your role: {session.Role} is not permitted.");
                return;
            }

            Admin_View admin_Window = new Admin_View(conn, session.Role, session.Username);
            this.Close();
            admin_Window.Show();
        }
        private void HandleSession(OracleConnection conn, string user)
        {
            Session session = new Session();
            List<string> Lrole = new List<string>();
            int countRole = 0;
            string sqlRoleOfUser = $"select ROLE from SESSION_ROLES";
            using (OracleCommand cmd = new OracleCommand(sqlRoleOfUser, conn))
            {
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Lrole.Add(reader.GetString(reader.GetOrdinal("ROLE")));
                        countRole++;
                    }
                }
                var role = Lrole.Where(hh => hh == "DBA").ToList();
                if (role.Count == 0)
                {
                    MessageBox.Show("Only DBA role is allowed to use this system!");
                    return;
                }
                else
                {
                    session.Username = user;
                    session.Role = role[0];
                    directWindowUser(conn, session);
                }
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Replace(" ", "");
            string password = txtPassword.Password;


            ConnectionDB_VM connectionDB = new ConnectionDB_VM();
            OracleConnection oracleConnection = connectionDB.OracleConnection(username, password);
            if (oracleConnection != null)
            {

                HandleSession(oracleConnection, username);
                MessageBox.Show("Login Successfully");
            }
        }


    }
}
