using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace ATBM_Project.Views.Student
{
    /// <summary>
    /// Interaction logic for StudentPage.xaml
    /// </summary>
    /// 
    public partial class StudentPage : Window
    {
        private string _userName;
        private OracleConnection _connection;
        public StudentPage(OracleConnection connection, string userName)
        {
            _userName = userName;
            _connection = connection;
            InitializeComponent();
            Title.Text = userName;
        }

        private void clickBack(object sender, RoutedEventArgs e)
        {
            _connection.Close();
            Login_View login = new Login_View();
            this.Close();
            login.Show();
        }
    }
}
