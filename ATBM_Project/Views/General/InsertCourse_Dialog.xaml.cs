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
    /// Interaction logic for InsertCourse_Dialog.xaml
    /// </summary>
    public partial class InsertCourse_Dialog : Window
    {
        private OracleConnection _connection;
        public InsertCourse_Dialog(OracleConnection conn)
        {
            InitializeComponent();
            _connection = conn;
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            string courseID = inputCourseID.Text.Trim();
            string courseName = inputCourseName.Text.Trim();
            int totalCredit;
            bool totalCreditSuccess = int.TryParse(inputTotalCredit.Text, out totalCredit);
            int theoryCredit;
            bool theoryCreditSuccess = int.TryParse(inputTheoryCredit.Text, out theoryCredit);
            int labCredit;
            bool labCreditSuccess = int.TryParse(inputLabCredit.Text, out labCredit);
            int maxNumStudent;
            bool maxNumStudentSuccess = int.TryParse(inputMaxNumStudent.Text, out maxNumStudent);
            string unit = inputUnit.Text.Trim();

            if (string.IsNullOrEmpty(courseID) || string.IsNullOrEmpty(courseName) || string.IsNullOrEmpty(unit)
                || !totalCreditSuccess || !theoryCreditSuccess || !labCreditSuccess || !maxNumStudentSuccess)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            string insertQuery = "INSERT INTO ADMIN.HOCPHAN (MAHP, TENHP, SOTC, STLT, STTH, SOSVTD, MADV) VALUES (:value1, :value2, :value3, :value4, :value5, :value6, :value7)";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(insertQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("value1", courseID));
                    command.Parameters.Add(new OracleParameter("value2", OracleDbType.NVarchar2)).Value = courseName;
                    command.Parameters.Add(new OracleParameter("value3", totalCredit));
                    command.Parameters.Add(new OracleParameter("value4", theoryCredit));
                    command.Parameters.Add(new OracleParameter("value5", labCredit));
                    command.Parameters.Add(new OracleParameter("value6", maxNumStudent));
                    command.Parameters.Add(new OracleParameter("value7", unit));

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã thêm {rowsAffected} học phần");
                    if (rowsAffected > 0)
                    {
                        inputCourseID.Text = "";
                        inputCourseName.Text = "";
                        inputTotalCredit.Text = "";
                        inputTheoryCredit.Text = "";
                        inputLabCredit.Text = "";
                        inputMaxNumStudent.Text = "";
                        inputUnit.Text = "";
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
