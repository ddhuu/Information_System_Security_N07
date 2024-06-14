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
using Oracle.ManagedDataAccess.Client;
using ATBM_Project.Models;

namespace ATBM_Project.Views.General
{
    /// <summary>
    /// Interaction logic for UpdateEmployee_Dialog.xaml
    /// </summary>
    public partial class UpdateEmployee_Dialog : Window
    {
        private OracleConnection _connection;
        private Models.Employee _employee;
        public UpdateEmployee_Dialog(OracleConnection conn, Models.Employee emp)
        {
            _connection = conn;
            _employee = emp;
            InitializeComponent();
            inputID.Text = _employee.ID;
            inputFullName.Text = _employee.FullName;
            inputGender.Text = _employee.Gender;
            inputDOB.Text = _employee.DOB;
            inputGrant.Text = _employee.Grant.ToString();
            inputRole.Text = _employee.Role; 
            inputPhoneNumber.Text = _employee.PhoneNumber;
            inputUnit.Text = _employee.Unit;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string ID = _employee.ID;
            string phoneNumber = inputPhoneNumber.Text.Trim();

            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length > 10)
            {
                MessageBox.Show("Vui lòng nhập số điện thoại hợp lệ!");
                return;
            }

            string updateQuery = "UPDATE ADMIN.UV_CANHAN_NHANSU SET DT = :dt WHERE MANV = :manv";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(updateQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("dt", phoneNumber));
                    command.Parameters.Add(new OracleParameter("manv", ID));

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã cập nhật {rowsAffected} nhân viên");
                    if (rowsAffected > 0)
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật dữ liệu. Vui lòng thử lại sau!");
            }
        }
    }
}
