using ATBM_Project.ViewsModels;
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

namespace ATBM_Project.Views.Affair
{
    /// <summary>
    /// Interaction logic for Assignment_View.xaml
    /// </summary>
    public partial class Assignment_View : UserControl
    {
        private Affair_VM _affair;
        public Assignment_View(Affair_VM affair)
        {
            _affair = affair;
            InitializeComponent();
            assigmentsDataGrid.ItemsSource = _affair.getAssigments();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Assignment assignment = ((Button)sender).Tag as Assignment;
            if(assignment != null)
            {
                (new UpdateAssignment_Dialog(_affair, assignment)).ShowDialog();
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            assigmentsDataGrid.ItemsSource = _affair.getAssigments();
        }
    }
}
