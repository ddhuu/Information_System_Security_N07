using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace ATBM_Project.Views.Lecturer
{
    /// <summary>
    /// Interaction logic for LecturerPage.xaml
    /// </summary>
    public partial class LecturerPage : Window
    {
        private string _userName;
        private OracleConnection _connection;
        public LecturerPage(OracleConnection connection, string userName)
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
