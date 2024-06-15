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

namespace ATBM_Project.Views.HeadUnit
{
    /// <summary>
    /// Interaction logic for Assigment_View.xaml
    /// </summary>
    public partial class Assigment_View : UserControl
    {
        private OracleConnection _connection;
        private HeadUnit_VM _headUnit;
        public Assigment_View(OracleConnection conn)
        {
            _connection = conn;
            _headUnit = new HeadUnit_VM(_connection);
            InitializeComponent();
            assigmentsDataGrid.ItemsSource = _headUnit.GetAssignments();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            (new InsertAssigment_Dialog(_headUnit)).ShowDialog();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Assignment assignment = ((Button)sender).Tag as Assignment;
            if (assignment != null)
            {
                (new UpdateAssigment_Dialog(_headUnit, assignment)).ShowDialog();
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            assigmentsDataGrid.ItemsSource = _headUnit.GetAssignments();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Assignment assignment = ((Button)sender).Tag as Assignment;
            if (assignment == null)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn là muốn xóa dòng này?", "Xác nhận", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _headUnit.deleteAssigment(assignment);
            }
        }
    }
}
