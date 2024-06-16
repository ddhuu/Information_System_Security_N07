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
using ATBM_Project.ViewsModels;
using Oracle.ManagedDataAccess.Client;

namespace ATBM_Project.Views.HeadDepartment
{
    /// <summary>
    /// Interaction logic for Employee_View.xaml
    /// </summary>
    public partial class Employee_View : UserControl
    {
        private OracleConnection _connection;
        private HeadDepartment_VM _headDepartment;
        public Employee_View(OracleConnection conn)
        {
            _connection = conn;
            _headDepartment = new HeadDepartment_VM(_connection);
            InitializeComponent();
            empDataGrid.ItemsSource = _headDepartment.getAllEmps();

        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            empDataGrid.ItemsSource = _headDepartment.getAllEmps();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            (new InsertEmployee_Dialog(_headDepartment)).ShowDialog();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Models.Employee emp = ((Button)sender).Tag as Models.Employee;
            if(emp == null)
            {
                return;
            }
            (new UpdateEmployee_Dialog(_headDepartment, emp)).ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Models.Employee employee = ((Button)sender).Tag as Models.Employee;
            if(employee == null)
            {
                return;
            }
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn là muốn xóa dòng này?", "Xác nhận", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                _headDepartment.deleteEmployee(employee);
            }
        }
    }
}
