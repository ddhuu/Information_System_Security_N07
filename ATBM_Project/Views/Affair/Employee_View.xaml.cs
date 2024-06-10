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

namespace ATBM_Project.Views.Affair
{
    /// <summary>
    /// Interaction logic for Employee_View.xaml
    /// </summary>
    public partial class Employee_View : UserControl
    {
        private Affair_VM _affair;
        public Employee_View(Affair_VM affairvm)
        {
            _affair = affairvm;
            InitializeComponent();
            ATBM_Project.Models.Employee emp = _affair.getInfor();
            IDTextBlock.Text = emp.ID;
            NameTextBlock.Text = emp.FullName;
            GenderTextBlock.Text = emp.Gender;
            DOBTextBlock.Text = emp.DOB.ToString();
            AdditionalFeeTextBlock.Text = emp.Grant.ToString();
            PhoneNumTextBlock.Text = emp.PhoneNumber;
            RoleTextBlock.Text = emp.Role;
            UnitTextBlock.Text = emp.Unit;
        }
    }
}
