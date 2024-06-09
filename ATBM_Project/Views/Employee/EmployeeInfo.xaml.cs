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

namespace ATBM_Project.Views.Employee
{
    /// <summary>
    /// Interaction logic for EmployeeInfo.xaml
    /// </summary>
    public partial class EmployeeInfo : UserControl
    {
        private Employee_VM _employee_VM;
        public EmployeeInfo(Employee_VM employee_VM)
        {
            _employee_VM = employee_VM;
            InitializeComponent();
            ATBM_Project.Models.Employee emp = employee_VM.getInfor();
            NameTextBlock.Text = emp.FullName;
        }
    }
}
