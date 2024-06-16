using ATBM_Project.Models;
using ATBM_Project.Views.Affair;
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

namespace ATBM_Project.Views.Student
{
    /// <summary>
    /// Interaction logic for Student_View.xaml
    /// </summary>
    public partial class Student_View : UserControl
    {
        private OracleConnection _connection;
        private Student_VM _student;
        public Student_View(OracleConnection conn)
        {
            _connection = conn;
            _student = new Student_VM(_connection);
            InitializeComponent();
            studentDataGrid.ItemsSource = _student.getStudentList();
        }

        private void btnSelect_Click (object sender, RoutedEventArgs e)
        {
            studentDataGrid.ItemsSource = _student.getStudentList();
        }

        private void btnUpdate_Click (object sender, RoutedEventArgs e)
        {
            try
            {
                Models.Student student = ((Button)sender).Tag as Models.Student;
                if (student != null)
                {
                    (new UpdateInfoStudent_Dialog(_connection, student)).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // nothing to do
            }
        }
    }
}
