using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace ATBM_Project.Views.HeadDepartment
{
    /// <summary>
    /// Interaction logic for HeadDepartmentPage.xaml
    /// </summary>
    public partial class HeadDepartmentPage : Window
    {
        private string _userName;
        private OracleConnection _connection;
        public HeadDepartmentPage(OracleConnection connection, string userName)
        {
            _connection = connection;
            _userName = userName;
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

        private void infoButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new Employee_View(_connection);
        }

        private void studentButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new General.Student_View(_connection);
        }

        private void compartmentButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new General.Unit_View(_connection);
        }

        private void courseButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new General.Course_View(_connection);
        }

        private void openScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new General.CourseOpenSchedule_View(_connection);
        }

        private void assignmentButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new HeadUnit.Assigment_View(_connection);
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new Lecturer.CourseRegistration_View(_connection, true);
        }
    }
}
