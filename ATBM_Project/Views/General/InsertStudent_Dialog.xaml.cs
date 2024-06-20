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

namespace ATBM_Project.Views.General
{
    /// <summary>
    /// Interaction logic for InsertStudent_Dialog.xaml
    /// </summary>
    public partial class InsertStudent_Dialog : Window
    {
        private OracleConnection _connection;
        public InsertStudent_Dialog(OracleConnection conn)
        {
            _connection = conn;
            InitializeComponent();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            string id = inputId.Text.Trim();
            string fullName = inputFullName.Text.Trim();
            string gender = inputGender.Text.Trim();
            string address = inputAddress.Text.Trim();
            string dob = inputDOB.Text.Trim();
            string phoneNumber = inputPhoneNumber.Text.Trim();
            string program = inputProgram.Text.Trim();
            string major = inputMajor.Text.Trim();
            string group = inputGroup.Text.Trim();

            if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(dob)
                || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(program) || string.IsNullOrEmpty(major) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (group != "CS1" && group != "CS2")
            {
                MessageBox.Show("Cơ sở không hợp lệ!");
                return;
            }

            string insertQuery = "INSERT INTO ADMIN.SINHVIEN (MASV, HOTEN, PHAI, NGSINH, DCHI, DT, MACT, MANGANH, SOTCTL, DTBTL, COSO) VALUES (:value1, :value2, :value3, TO_DATE(:value4, 'DD/MM/YYYY'), :value5, :value6, :value7, :value8, 0, 0, :value9)";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(insertQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("value1", id));
                    command.Parameters.Add(new OracleParameter("value2", OracleDbType.NVarchar2)).Value = fullName;
                    command.Parameters.Add(new OracleParameter("value3", gender));
                    command.Parameters.Add(new OracleParameter("value4", dob));
                    command.Parameters.Add(new OracleParameter("value5", OracleDbType.NVarchar2)).Value = address;
                    command.Parameters.Add(new OracleParameter("value6", phoneNumber));
                    command.Parameters.Add(new OracleParameter("value7", program));
                    command.Parameters.Add(new OracleParameter("value8", major));
                    command.Parameters.Add(new OracleParameter("value9", group));

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã thêm {rowsAffected} sinh viên");
                    if (rowsAffected > 0)
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi chèn dữ liệu. Vui lòng thử lại");
            }

        }
    }
}
