using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace ATBM_Project.Views.Affair
{
    /// <summary>
    /// Interaction logic for AffairPage.xaml
    /// </summary>
    public partial class AffairPage : Window
    {
        private string _userName { get; set; }
        private OracleConnection _connection;
        public AffairPage(OracleConnection connection, string userName)
        {
            _connection = connection;
            _userName = userName;
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
