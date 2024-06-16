using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oracle.ManagedDataAccess.Client;
using ATBM_Project.Models;

namespace ATBM_Project.Views.General
{
    /// <summary>
    /// Interaction logic for Employee_View.xaml
    /// </summary>
    public partial class Employee_View : UserControl
    {
        private OracleConnection _connection;
        public Employee_View(OracleConnection conn)
        {
            _connection = conn;
            InitializeComponent();
            empDataGrid.ItemsSource = GetEmployees();
        }

        public List<Models.Employee> GetEmployees()
        {
            List<Models.Employee> emps = new List<Models.Employee>();
            string SQLcontex = $"SELECT * FROM ADMIN.UV_CANHAN_NHANSU";
            OracleCommand cmd = new OracleCommand(SQLcontex, _connection);

            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string empID = reader.GetString(reader.GetOrdinal("MANV"));
                    string fullName = reader.GetString(reader.GetOrdinal("HOTEN"));
                    string gender = reader.GetString(reader.GetOrdinal("PHAI"));
                    string phoneNumber = reader.GetString(reader.GetOrdinal("DT"));
                    string role = reader.GetString(reader.GetOrdinal("VAITRO"));
                    string unit = reader.GetString(reader.GetOrdinal("MADV"));
                    double grant = reader.GetDouble(reader.GetOrdinal("PHUCAP"));
                    DateTime dob = reader.GetDateTime(reader.GetOrdinal("NGSINH"));
                    emps.Add(new Models.Employee
                    {
                        ID = empID,
                        FullName = fullName,
                        Gender = gender,
                        PhoneNumber = phoneNumber,
                        Role = role,
                        Unit = unit,
                        Grant = grant,
                        DOB = dob.ToString("dd/MM/yyyy")
                    });
                }
            }
            return emps;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            empDataGrid.ItemsSource = GetEmployees();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.Employee emp = ((Button)sender).Tag as Models.Employee;
                if(emp != null)
                {
                    (new UpdateEmployee_Dialog(_connection, emp)).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // nothing to do
            }
        }
    }
}
