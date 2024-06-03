using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace ATBM_Project.Views.HeadUnit
{
    /// <summary>
    /// Interaction logic for HeadUnitPage.xaml
    /// </summary>
    /// 

    public partial class HeadUnitPage : Window
    {
        private string _userName;
        private OracleConnection _connection;
        public HeadUnitPage(OracleConnection connection, string userName)
        {
            _userName = userName;
            _connection = connection;
            InitializeComponent();
            Title.Text = userName;
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
