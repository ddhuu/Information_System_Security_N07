using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using Oracle.ManagedDataAccess.Client;

namespace ATBM_Project.Views.Student
{
    /// <summary>
    /// Interaction logic for CourseRegistration_View.xaml
    /// </summary>
    public partial class CourseRegistration_View : UserControl
    {
        private OracleConnection _connection;
        private Student_VM _student;
        public CourseRegistration_View(OracleConnection conn)
        {
            _connection = conn;
            _student = new Student_VM(_connection);
            InitializeComponent();
            registrationsDataGrid.ItemsSource = _student.GetRegistrations();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            registrationsDataGrid.ItemsSource = _student.GetRegistrations();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            (new RegisterCourse_Dialog(_connection, _student)).ShowDialog();

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            CourseRegistration registration = ((Button)sender).Tag as CourseRegistration;
            if (registration == null)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn là muốn xóa dòng này?", "Xác nhận", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _student.deleteRegistration(registration);
            }
        }
    }
}
