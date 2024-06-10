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
    /// Interaction logic for UpdateCourseOpenSchedule_Dialog.xaml
    /// </summary>
    public partial class UpdateCourseOpenSchedule_Dialog : Window
    {
        private OracleConnection _connection;
        private CourseOpenSchedule _cos;
        public UpdateCourseOpenSchedule_Dialog(OracleConnection conn, CourseOpenSchedule cos)
        {
            _connection = conn;
            _cos = cos;
            InitializeComponent();
            inputCourseID.Text = cos.courseId;
            inputSemester.Text = cos.Semester.ToString();
            inputYear.Text = cos.Year.ToString();
            inputProgram.Text = cos.Program;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string courseID = inputCourseID.Text.Trim();
            int semester;
            bool semesterSuccess = int.TryParse(inputSemester.Text, out semester);
            int year;
            bool yearSuccess = int.TryParse(inputYear.Text, out year);
            string program = inputProgram.Text.Trim();

            if (string.IsNullOrEmpty(courseID) || string.IsNullOrEmpty(program) || !semesterSuccess || !yearSuccess)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            string updateQuery = "UPDATE ADMIN.KHMO SET MAHP = :mahp, HK = :hk, NAM = :nam, MACT = :mact WHERE MAHP = :oldmahp and HK = :oldhk and NAM = :oldnam and MACT = :oldmact";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(updateQuery, _connection))
                {
                    // Thêm tham số và gán giá trị

                    command.Parameters.Add(new OracleParameter("mahp", courseID));
                    command.Parameters.Add(new OracleParameter("hk", semester));
                    command.Parameters.Add(new OracleParameter("nam", year));
                    command.Parameters.Add(new OracleParameter("mact", program));
                    command.Parameters.Add(new OracleParameter("oldmahp", _cos.courseId));
                    command.Parameters.Add(new OracleParameter("oldhk", _cos.Semester));
                    command.Parameters.Add(new OracleParameter("oldnam", _cos.Year));
                    command.Parameters.Add(new OracleParameter("oldmact", _cos.Program));


                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã cập nhật {rowsAffected} kế hoạch mở");
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
