using ATBM_Project.ViewsModels;
using Oracle.ManagedDataAccess.Client;
using System.Windows;
using ATBM_Project.Views.General;

namespace ATBM_Project.Views.Affair
{
    /// <summary>
    /// Interaction logic for AffairPage.xaml
    /// </summary>
    public partial class AffairPage : Window
    {
        private string _userName { get; set; }
        private OracleConnection _connection;
        private Affair_VM _affair;
        public AffairPage(OracleConnection connection, string userName)
        {
            _connection = connection;
            _userName = userName;
            InitializeComponent();
            Title.Text = userName;
            _affair = new Affair_VM(_connection);
        }

        public void clickBack(object sender, RoutedEventArgs e)
        {
            _connection.Close();
            Login_View login = new Login_View();
            this.Close();
            login.Show();
        }

        private void infoButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new Employee_View(_affair);
        }

        private void courseButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new Course_View(_connection, true);
        }

        private void courseOpenScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new CourseOpenSchedule_View(_connection, true);
        }

        private void unitButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new Unit_View(_connection, true);
        }
    }
}
