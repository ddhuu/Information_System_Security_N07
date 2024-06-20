using ATBM_Project.ViewsModels;
using ATBM_Project.Models;
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
using System.Windows.Shapes;

namespace ATBM_Project.Views.HeadDepartment
{
    /// <summary>
    /// Interaction logic for UpdateEmployee_Dialog.xaml
    /// </summary>
    public partial class UpdateEmployee_Dialog : Window
    {
        private HeadDepartment_VM _headDepartment;
        private Models.Employee _employee;
        public UpdateEmployee_Dialog(HeadDepartment_VM headDepartment, Models.Employee employee)
        {
            _headDepartment = headDepartment;
            _employee = employee;
            InitializeComponent();
            inputID.Text = _employee.ID;
            inputFullName.Text = _employee.FullName;
            inputGender.Text = _employee.Gender;
            inputDOB.Text = _employee.DOB;
            inputGrant.Text = _employee.Grant.ToString();
            inputRole.Text = _employee.Role;
            inputPhoneNumber.Text = _employee.PhoneNumber;
            inputUnit.Text = _employee.Unit;
            inputGroup.Text = _employee.Group;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string id = inputID.Text.Trim();
            string fullName = inputFullName.Text.Trim();
            string gender = inputGender.Text.Trim();
            //DateTime dob;
            //bool dobSuccess = DateTime.TryParseExact(inputDOB.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob);
            string dob = inputDOB.Text.Trim();
            int grant;
            bool grantSuccess = int.TryParse(inputGrant.Text.Trim(), out grant);
            string phoneNumber = inputPhoneNumber.Text.Trim();
            string unit = inputUnit.Text.Trim();
            string role = inputRole.Text.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(role)
                || string.IsNullOrEmpty(unit) || string.IsNullOrEmpty(phoneNumber) || !grantSuccess || string.IsNullOrEmpty(dob))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            int result = _headDepartment.updateEmployee(_employee,new Models.Employee
            {
                ID = id,
                FullName = fullName,
                Gender = gender,
                Role = role,
                PhoneNumber = phoneNumber,
                Unit = unit,
                DOB = dob,
                Grant = grant
            });

            this.Close();
        }
    }
}
