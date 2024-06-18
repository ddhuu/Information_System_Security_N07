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
            UserController.Content = new Assigment_View(_connection);
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new Lecturer.CourseRegistration_View(_connection);
        }

        private void messageButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new Message.Message_View(_connection);
        }
    }
}
