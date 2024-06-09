using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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
        private Student_VM student_VM;
        public StudentPage(OracleConnection connection, string userName)
        {
            _userName = userName;
            _connection = connection;
            student_VM = new Student_VM(connection);
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
        private void inforButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new StudentInfor(student_VM);
            
            MessageBox.Show(student_VM.getInfor().Name);
        }
        private void courseButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new StudentInfor(student_VM);
           ;
        }
        private void moduleButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new StudentInfor(student_VM);
            ObservableCollection<Course> courses = student_VM.FindCourses();
            MessageBox.Show($"Number of courses {courses.Count}");
        }

    }
}
