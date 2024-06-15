using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using Oracle.ManagedDataAccess.Client;
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

namespace ATBM_Project.Views.Lecturer
{
    /// <summary>
    /// Interaction logic for Registration_View.xaml
    /// </summary>
    public partial class CourseRegistration_View : UserControl
    {
        private OracleConnection _connection;
        private Lecturer_VM lecturer_VM;
        public CourseRegistration_View(OracleConnection connection)
        {
            _connection = connection;
            InitializeComponent();
            lecturer_VM = new Lecturer_VM(_connection);
            registrationsDataGrid.ItemsSource = lecturer_VM.GetRegistrations();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CourseRegistration courseRegistration = ((Button)sender).Tag as CourseRegistration;
                if (courseRegistration != null)
                {
                    (new UpdateCourseRegistration_Dialog(_connection, courseRegistration)).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // nothing to do
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            registrationsDataGrid.ItemsSource = lecturer_VM.GetRegistrations();
        }
    }
}
