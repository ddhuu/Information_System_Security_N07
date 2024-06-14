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
    /// Interaction logic for UpdateAssignments_Dialog.xaml
    /// </summary>
    public partial class UpdateAssignment_Dialog : Window
    {
        private OracleConnection _connection;
        private Assignment _assignment;
        public UpdateAssignment_Dialog(OracleConnection conn, Assignment assignment)
        {
            _connection = conn;
            _assignment = assignment;
            InitializeComponent();
            inputCourseID.Text = _assignment.CourseID;
            inputLecturerID.Text = _assignment.LecturerID;
            inputProgram.Text = _assignment.Program;
            inputSemester.Text = _assignment.Semester.ToString();
            inputYear.Text = _assignment.Year.ToString();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string courseID = inputCourseID.Text.Trim();
            string lecturerID = inputLecturerID.Text.Trim();
            string program = inputProgram.Text.Trim();
            int semester;
            bool semesterSuccess = int.TryParse(inputSemester.Text, out semester);
            int year;
            bool yearSuccess = int.TryParse(inputYear.Text, out year);

            if (string.IsNullOrEmpty(courseID) || string.IsNullOrEmpty(lecturerID) || string.IsNullOrEmpty(program)
                || !semesterSuccess || !yearSuccess)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            string updateQuery = "UPDATE ADMIN.PHANCONG SET MAHP = :mahp, MAGV = :magv, MACT = :mact, HK = :hk, NAM = :nam " +
                                 "WHERE MAHP = :mahp AND MAGV = :magv AND HK = :hk AND NAM = :nam AND MACT = :mact";

            try
            {
                using (OracleCommand command = new OracleCommand(updateQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("mahp", courseID));
                    command.Parameters.Add(new OracleParameter("magv", lecturerID));
                    command.Parameters.Add(new OracleParameter("mact", program));
                    command.Parameters.Add(new OracleParameter("stth", semester)); // Assuming stth is semester
                    command.Parameters.Add(new OracleParameter("hk", semester));
                    command.Parameters.Add(new OracleParameter("nam", year));

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
                MessageBox.Show($"Đã xảy ra lỗi khi cập nhật dữ liệu. Vui lòng thử lại sau!\n\n{ex.Message}");
            }
        }

    }
}
