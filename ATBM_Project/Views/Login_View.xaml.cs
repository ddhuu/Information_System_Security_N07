using ATBM_Project.Models;
using ATBM_Project.Views.Affair;
using ATBM_Project.Views.Employee;
using ATBM_Project.Views.HeadDepartment;
using ATBM_Project.Views.HeadUnit;
using ATBM_Project.Views.Lecturer;
using ATBM_Project.Views.Student;
using ATBM_Project.ViewsModels;
using Oracle.ManagedDataAccess.Client;
using System;
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
        private void directWindowUser(OracleConnection connection, Session session)
        {


            string role = session.Role;

            if (session.Username.ToUpper() == "NV001")
            {
                HeadDepartmentPage headDepartment = new HeadDepartmentPage(connection, session.Username);
                this.Close();
                headDepartment.Show();
                return;
            }

            switch (role)
            {
                case "RL_NHANVIENCOBAN":
                    EmployeePage employee = new EmployeePage(connection, session.Username);
                    this.Close();
                    employee.Show();
                    break;
                case "RL_GIANGVIEN":
                    LecturerPage lecturer = new LecturerPage(connection, session.Username);
                    this.Close();
                    lecturer.Show();
                    break;
                case "RL_GIAOVU":
                    AffairPage affair = new AffairPage(connection, session.Username);
                    this.Close();
                    affair.Show();
                    break;
                case "RL_TRUONGDONVI":
                    HeadUnitPage headUnit = new HeadUnitPage(connection, session.Username);
                    this.Close();
                    headUnit.Show();
                    break;
                case "RL_SINHVIEN":
                    StudentPage student = new StudentPage(connection, session.Username);
                    this.Close();
                    student.Show();
                    break;
                case "DBA":
                    Admin_View admin_Window = new Admin_View(connection, session.Role, session.Username);
                    this.Close();
                    admin_Window.Show();
                    break;


                default:
                    break;
            }

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
                var role = Lrole.Where(roleName => roleName == "DBA" || roleName.Contains("RL_")).ToList();
                if (countRole < 0)
                {
                    MessageBox.Show("Account does not have privileges in this system");
                    return;
                }
                else if (countRole == 1)
                {
                    session.Username = user;
                    session.Role = role[0];
                    directWindowUser(conn, session);
                }
                else
                {
                    if (role.Where(roleName => roleName == "DBA").Count() == 1)
                    {
                        session.Username = user;
                        session.Role = role[0];
                        directWindowUser(conn, session);
                    }
                    else
                    {
                        string userRole = "";
                        string sqlText = "select sys_context('user_ctx','role') role from dual";
                        try
                        {
                            using (OracleCommand cmd1 = new OracleCommand(sqlText, conn))
                            {
                                using (OracleDataReader reader1 = cmd1.ExecuteReader())
                                {
                                    while (reader1.Read())
                                    {
                                        userRole = reader1.GetString(reader1.GetOrdinal("ROLE"));
                                    }
                                }
                            }
                            session.Username = user;
                            session.Role = userRole;
                            Console.WriteLine("Username: " + session.Username);
                            Console.WriteLine("Role: " + session.Role);
                            directWindowUser(conn, session);
                        }
                        catch
                        {
                            MessageBox.Show("Account does not have privileges in this system");
                        }


                    }
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
                //MessageBox.Show("Login Successfully");
            }
        }


    }
}
