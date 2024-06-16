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

namespace ATBM_Project.Views.Student
{
    /// <summary>
    /// Interaction logic for UpdateInfoStudent_Dialog.xaml
    /// </summary>
    public partial class UpdateInfoStudent_Dialog : Window
    {
        private OracleConnection _connection;
        private Models.Student _student;
        public UpdateInfoStudent_Dialog(OracleConnection conn, Models.Student student)
        {
            _connection = conn;
            _student = student;
            InitializeComponent();
            inputStudentID.Text = _student.Id;
            inputStudentName.Text = _student.Name;
            inputGender.Text = _student.Gender;
            inputDOB.Text = _student.DOB;
            inputAddress.Text = _student.Address;
            inputPhoneNumber.Text = _student.PhoneNumber;
            inputProgram.Text = _student.Program;
            inputMajor.Text = _student.Major;
            inputCummulativeCredit.Text = _student.CummulativeCredits.ToString();
            inputAvgGrade.Text = _student.AvgGrade.ToString();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {   
            string address = inputAddress.Text.Trim();
            string phoneNumber = inputPhoneNumber.Text.Trim();

            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(phoneNumber))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            string updateQuery = "UPDATE ADMIN.SINHVIEN SET DCHI = :dchi, DT = :dt";

            try
            {
                using (OracleCommand command = new OracleCommand(updateQuery, _connection))
                {
                    command.Parameters.Add(new OracleParameter("dchi", address));
                    command.Parameters.Add(new OracleParameter("dt", phoneNumber));


                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã cập nhật {rowsAffected} học phần");
                    if (rowsAffected > 0)
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật dữ liệu. Vui lòng thử lại sau!\n" + ex.Message);
            }
        }
    }
}
