using ATBM_Project.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ATBM_Project.Views.General
{
    /// <summary>
    /// Interaction logic for UpdateGrade_Dialog.xaml
    /// </summary>
    public partial class UpdateGrade_Dialog : Window
    {
        private OracleConnection _connection;
        private CourseRegistration _courseRegistration;
        public UpdateGrade_Dialog(OracleConnection connection, CourseRegistration courseRegistration)
        {
            _connection = connection;
            _courseRegistration = courseRegistration;
            InitializeComponent();
            inputStudentID.Text = _courseRegistration.StudentId;
            inputLecturerID.Text = _courseRegistration.LecturerId;
            inputCourseID.Text = _courseRegistration.CourseId;
            inputSemester.Text = _courseRegistration.Semester.ToString();
            inputYear.Text = _courseRegistration.Year.ToString();
            inputProgram.Text = _courseRegistration.Program;
            inputLabGrade.Text = _courseRegistration.LabGrade.ToString();
            inputProcessGrade.Text = _courseRegistration.ProcessGrade.ToString();
            inputFinalExamGrade.Text = _courseRegistration.FinalExamGrade.ToString();
            inputFinalGrade.Text = _courseRegistration.FinalGrade.ToString();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string courseID = inputCourseID.Text.Trim();
            string studentID = inputStudentID.Text.Trim();
            double labGrade;
            bool labGradeSuccess = double.TryParse(inputLabGrade.Text, out labGrade);
            double processGrade;
            bool processGradeSuccess = double.TryParse(inputProcessGrade.Text, out processGrade);
            double finalExamGrade;
            bool finalExamGradeSuccess = double.TryParse(inputFinalExamGrade.Text, out finalExamGrade);
            double finalGrade;
            bool finalGradeSuccess = double.TryParse(inputFinalGrade.Text, out finalGrade);
            string lecturerID = inputLecturerID.Text.Trim();
            int year;
            bool yearSuccess = int.TryParse(inputYear.Text, out year);
            int semester;
            bool semesterSuccess = int.TryParse(inputSemester.Text, out semester);
            string program = inputProgram.Text.Trim();

            if (string.IsNullOrEmpty(courseID) || string.IsNullOrEmpty(studentID) || string.IsNullOrEmpty(lecturerID) || string.IsNullOrEmpty(program)
                || !labGradeSuccess || !processGradeSuccess || !finalExamGradeSuccess || !finalGradeSuccess || !yearSuccess || !semesterSuccess)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            string updateQuery = "UPDATE ADMIN.UV_CANHAN_DANGKY SET DIEMTH = :diemth, DIEMQT = :diemqt, DIEMCK = :diemck, DIEMTK = :diemtk " +
                "WHERE MASV = :masv AND MAGV = :magv AND MAHP = :mahp AND HK = :hk AND NAM = :nam AND MACT = :mact";

            try
            {
                using (OracleCommand command = new OracleCommand(updateQuery, _connection))
                {
                    command.Parameters.Add(new OracleParameter("diemth", labGrade));
                    command.Parameters.Add(new OracleParameter("diemqt", processGrade));
                    command.Parameters.Add(new OracleParameter("diemck", finalExamGrade));
                    command.Parameters.Add(new OracleParameter("diemtk", finalGrade));
                    command.Parameters.Add(new OracleParameter("masv", studentID));
                    command.Parameters.Add(new OracleParameter("magv", lecturerID));
                    command.Parameters.Add(new OracleParameter("mahp", courseID));
                    command.Parameters.Add(new OracleParameter("hk", semester));
                    command.Parameters.Add(new OracleParameter("nam", year));
                    command.Parameters.Add(new OracleParameter("mact", program));

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
