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

namespace ATBM_Project.Views.General
{
    /// <summary>
    /// Interaction logic for Registration_View.xaml
    /// </summary>
    public partial class Registration_View : UserControl
    {
        private OracleConnection _connection;
        private Student_VM student_VM;
        public Registration_View(OracleConnection connection, bool isAffair = false)
        {
            _connection = connection;
            InitializeComponent();
            student_VM = new Student_VM(_connection);
            if (isAffair)
            {
                registrationsDataGrid.ItemsSource = student_VM.GetRegistrations(true);
            }
             registrationsDataGrid.ItemsSource = student_VM.GetRegistrations();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CourseRegistration courseRegistration = ((Button)sender).Tag as CourseRegistration;
                if (courseRegistration != null)
                {
                    (new UpdateGrade_Dialog(_connection, courseRegistration)).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // nothing to do
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            registrationsDataGrid.ItemsSource = student_VM.GetRegistrations();
        }
    }
}
