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
    /// Interaction logic for UpdateStudent_Dialog.xaml
    /// </summary>
    public partial class UpdateStudent_Dialog : Window
    {
        private OracleConnection _connection;
        private Models.Student _student;
        public UpdateStudent_Dialog(OracleConnection conn, Models.Student student)
        {
            _student = student;
            _connection = conn;
            InitializeComponent();
            inputId.Text = _student.Id;
            inputFullName.Text = _student.Name;
            inputDOB.Text = _student.DOB;
            inputGender.Text = _student.Gender;
            inputAddress.Text = _student.Address;
            inputProgram.Text = _student.Program;
            inputMajor.Text = _student.Major;
            inputPhoneNumber.Text = _student.PhoneNumber;
            inputAvgGrade.Text = _student.AvgGrade.ToString();
            inputCummulativeCredits.Text = _student.CummulativeCredits.ToString();
            inputGroup.Text = _student.Group;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string id = inputId.Text.Trim();
            string fullName = inputFullName.Text.Trim();
            string gender = inputGender.Text.Trim();
            string address = inputAddress.Text.Trim();
            string dob = inputDOB.Text.Trim();
            string phoneNumber = inputPhoneNumber.Text.Trim();
            string program = inputProgram.Text.Trim();
            string major = inputMajor.Text.Trim();
            int cummulativeCredits;
            bool cummulativeCreditsSuccess = int.TryParse(inputCummulativeCredits.Text, out cummulativeCredits);
            double avgGrade;
            bool avgGradeSuccess = double.TryParse(inputAvgGrade.Text, out avgGrade);

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(dob)
                || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(program) || string.IsNullOrEmpty(major) || string.IsNullOrEmpty(address)
                || !avgGradeSuccess || !cummulativeCreditsSuccess)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            string updateQuery = "UPDATE ADMIN.SINHVIEN SET HOTEN = :hoten, PHAI = :phai, NGSINH = TO_DATE(:ngsinh, 'DD/MM/YYYY'), DCHI = :dchi, DT = :dt, MACT = :mact, MANGANH = :manganh, SOTCTL = :sotctl, DTBTL = :dtbtl where MASV = :masv";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(updateQuery, _connection))
                {
                    // Thêm tham số và gán giá trị

                    command.Parameters.Add(new OracleParameter("hoten", OracleDbType.NVarchar2)).Value = fullName;
                    command.Parameters.Add(new OracleParameter("phai", gender));
                    command.Parameters.Add(new OracleParameter("ngsinh", dob));
                    command.Parameters.Add(new OracleParameter("dchi", OracleDbType.NVarchar2)).Value = address;
                    command.Parameters.Add(new OracleParameter("dt", phoneNumber));
                    command.Parameters.Add(new OracleParameter("mact", program));
                    command.Parameters.Add(new OracleParameter("manganh", major));
                    command.Parameters.Add(new OracleParameter("sotctl", cummulativeCredits));
                    command.Parameters.Add(new OracleParameter("dtbtl", avgGrade));
                    command.Parameters.Add(new OracleParameter("masv", id));

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã cập nhật {rowsAffected} sinh viên");
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
