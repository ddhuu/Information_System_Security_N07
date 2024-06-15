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

        private void infoButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new General.Employee_View(_connection);
        }

        private void studentButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new General.Student_View(_connection);
        }

        private void compartmentButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new General.Unit_View(_connection);
        }

        private void moduleButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new General.Course_View(_connection);
        }

        private void courseButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new General.CourseOpenSchedule_View(_connection);
        }

        private void assignmentButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new Assignment_View(_connection);
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new CourseRegistration_View(_connection);
        }
    }
}
