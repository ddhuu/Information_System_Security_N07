using ATBM_Project.Models;
using Oracle.ManagedDataAccess.Client;
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

namespace ATBM_Project.Views.General
{
    /// <summary>
    /// Interaction logic for UpdatePhoneNum_Dialog.xaml
    /// </summary>
    public partial class UpdatePhoneNum_Dialog : Window
    {
        private OracleConnection _connection;
        private string _phoneNumber;
        public UpdatePhoneNum_Dialog(OracleConnection conn, string phoneNumber)
        {
            _connection = conn;
            _phoneNumber = phoneNumber;
            InitializeComponent();
            inputPhoneNum.Text = _phoneNumber;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string phoneNumber = inputPhoneNum.Text.Trim();

            if (string.IsNullOrEmpty(phoneNumber))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            string updateQuery = "UPDATE ADMIN.NHANSU SET DT = :sodt";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(updateQuery, _connection))
                {
                    // Thêm tham số và gán giá trị

                    command.Parameters.Add(new OracleParameter("sodt", phoneNumber));


                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã cập nhật số điện thoại");
                    MessageBox.Show(rowsAffected.ToString());
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
