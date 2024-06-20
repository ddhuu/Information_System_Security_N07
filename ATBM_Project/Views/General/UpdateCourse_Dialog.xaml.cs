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
    /// Interaction logic for UpdateCourse_Dialog.xaml
    /// </summary>
    public partial class UpdateCourse_Dialog : Window
    {
        private OracleConnection _connection;
        private Course _course;
        public UpdateCourse_Dialog(OracleConnection con, Course course)
        {
            _connection = con;
            _course = course;
            InitializeComponent();
            inputCourseID.Text = _course.CourseID;
            inputCourseName.Text = _course.CourseName;
            inputTotalCredit.Text = _course.TotalCredit.ToString();
            inputTheoryCredit.Text = _course.TheoryCredit.ToString();
            inputLabCredit.Text = _course.LabCredit.ToString();
            inputMaxNumStudent.Text = _course.MaxNumParticipatient.ToString();
            inputUnit.Text = _course.Unit;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
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

            string updateQuery = "UPDATE ADMIN.HOCPHAN SET TENHP = :tenhp, SOTC = :sotc, STLT = :stlt, STTH = :stth, SOSVTD = :sosvtd, MADV = :madv WHERE MAHP = :mahp";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(updateQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    
                    command.Parameters.Add(new OracleParameter("tenhp", OracleDbType.NVarchar2)).Value = courseName;
                    command.Parameters.Add(new OracleParameter("sotc", totalCredit));
                    command.Parameters.Add(new OracleParameter("stlt", theoryCredit));
                    command.Parameters.Add(new OracleParameter("stth", labCredit));
                    command.Parameters.Add(new OracleParameter("sosvtd", maxNumStudent));
                    command.Parameters.Add(new OracleParameter("madv", unit));
                    command.Parameters.Add(new OracleParameter("mahp", courseID));

                    // Thực thi câu lệnh
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
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật dữ liệu. Vui lòng thử lại sau!");
            }

        
        }
    }
}
