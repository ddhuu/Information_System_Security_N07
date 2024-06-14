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

namespace ATBM_Project.Views.Affair
{
    /// <summary>
    /// Interaction logic for CourseRegistration_View.xaml
    /// </summary>
    public partial class CourseRegistration_View : UserControl
    {
        private Affair_VM _affair;
        public CourseRegistration_View(Affair_VM affair)
        {
            _affair = affair;
            InitializeComponent();
            registrationsDataGrid.ItemsSource = _affair.getRegistrations();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            registrationsDataGrid.ItemsSource = _affair.getRegistrations();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            (new InsertCourseRegistration_Dialog(_affair)).ShowDialog();
        }
    }
}
