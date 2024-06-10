using ATBM_Project.ViewsModels;
using System.Windows.Controls;

namespace ATBM_Project.Views.Employee
{
    /// <summary>
    /// Interaction logic for EmployeeInfo.xaml
    /// </summary>
    public partial class Employee_View : UserControl
    {
        private Employee_VM _employee_VM;
        public Employee_View(Employee_VM employee_VM)
        {
            _employee_VM = employee_VM;
            InitializeComponent();
            ATBM_Project.Models.Employee emp = employee_VM.getInfor();
            IDTextBlock.Text = emp.ID;
            NameTextBlock.Text = emp.FullName;
            GenderTextBlock.Text = emp.Gender;
            DOBTextBlock.Text = emp.DOB.Date.ToString("dd/MM/yyyy");
            AdditionalFeeTextBlock.Text = emp.Grant.ToString();
            PhoneNumTextBlock.Text = emp.PhoneNumber;
            RoleTextBlock.Text = emp.Role;
            UnitTextBlock.Text = emp.Unit;
        }
    }
}
