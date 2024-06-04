using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace ATBM_Project.Views.Employee
{
    /// <summary>
    /// Interaction logic for EmployeePage.xaml
    /// </summary>

    public partial class EmployeePage : Window
    {
        private string _userName { get; set; }
        private OracleConnection _connection;

        public EmployeePage(OracleConnection connection, string userName)
        {
            _userName = userName;
            _connection = connection;
            InitializeComponent();
            Title.Text = _userName;
        }

        public void clickBack(object sender, RoutedEventArgs e)
        {
            _connection.Close();
            Login_View login = new Login_View();
            this.Close();
            login.Show();
        }
    }
}
