using ATBM_Project.ViewsModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for InsertEmployee_Dialog.xaml
    /// </summary>
    public partial class InsertEmployee_Dialog : Window
    {
        private HeadDepartment_VM _headDepartment;
        public InsertEmployee_Dialog(HeadDepartment_VM headDepartment)
        {
            _headDepartment = headDepartment;
            InitializeComponent();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
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
            string group = inputGroup.Text.Trim();
            
            if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(role) 
                || string.IsNullOrEmpty(unit) || string.IsNullOrEmpty(phoneNumber) || !grantSuccess || string.IsNullOrEmpty(dob))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            if (group != "CS1" && group != "CS2" && group != "CS1,CS2")
            {
                MessageBox.Show("Cơ sở không hợp lệ!");
                return;
            }

            int result = _headDepartment.insertEmployee(new Models.Employee
            {
                ID = id,
                FullName = fullName,
                Gender = gender,
                Role = role,
                PhoneNumber = phoneNumber,
                Unit = unit,
                DOB = dob,
                Grant = grant,
                Group = group,
            });

            if(result > 0)
            {
                this.Close();
            }

        }
    }
}
