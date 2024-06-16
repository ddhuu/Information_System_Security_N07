using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
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
    /// Interaction logic for InsertCourseRegistration_Dialog.xaml
    /// </summary>
    public partial class RegisterCourse_Dialog : Window
    {
        private OracleConnection _connection;
        private Student_VM _student;
        public RegisterCourse_Dialog(OracleConnection conn, Student_VM student)
        {
            _connection = conn;
            _student = student;
            InitializeComponent();
            inputStudentID.Text = _student.GetRegistrations().First().StudentId;
            inputProgram.Text = _student.GetRegistrations().First().Program;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string studentID = inputStudentID.Text.Trim();
            string lecturerID = inputLecturerID.Text.Trim();
            string courseID = inputCourseID.Text.Trim();
            int semester;
            bool semesterSuccess = int.TryParse(inputSemester.Text.Trim(), out semester);
            int year;
            bool yearSuccess = int.TryParse(inputYear.Text.Trim(), out year);
            string program = inputProgram.Text.Trim();

            if (string.IsNullOrEmpty(program) || string.IsNullOrEmpty(lecturerID) || string.IsNullOrEmpty(courseID)
                || string.IsNullOrEmpty(studentID) || !semesterSuccess || !yearSuccess)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            int result = _student.insertRegistration(new Models.CourseRegistration
            {
                StudentId = studentID,
                LecturerId = lecturerID,
                CourseId = courseID,
                Semester = semester,
                Year = year,
                Program = program
            });

            if (result > 0)
            {
                this.Close();
            }
        }
    }
}
