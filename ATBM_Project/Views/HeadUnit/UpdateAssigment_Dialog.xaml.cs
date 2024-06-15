using ATBM_Project.ViewsModels;
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

namespace ATBM_Project.Views.HeadUnit
{
    /// <summary>
    /// Interaction logic for UpdateAssigment_Dialog.xaml
    /// </summary>
    public partial class UpdateAssigment_Dialog : Window
    {
        private HeadUnit_VM _headUnit;
        private Models.Assignment _assigment;
        public UpdateAssigment_Dialog(HeadUnit_VM headUnit, Models.Assignment assignment)
        {
            _headUnit = headUnit;
            _assigment = assignment;
            InitializeComponent();
            inputLecturerID.Text = _assigment.LecturerID;
            inputCourseID.Text = _assigment.CourseID;
            inputSemester.Text = _assigment.Semester.ToString();
            inputYear.Text = _assigment.Year.ToString();
            inputProgram.Text = _assigment.Program;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string lecturerID = inputLecturerID.Text.Trim();
            string courseID = inputCourseID.Text.Trim();
            int semester;
            bool semesterSuccess = int.TryParse(inputSemester.Text.Trim(), out semester);
            int year;
            bool yearSuccess = int.TryParse(inputYear.Text.Trim(), out year);
            string program = inputProgram.Text.Trim();

            if (string.IsNullOrEmpty(lecturerID) || string.IsNullOrEmpty(courseID) || string.IsNullOrEmpty(program) || !semesterSuccess || !yearSuccess)
            {
                MessageBox.Show("Thông tin chưa hợp lệ. Vui lòng nhập lại!");
                return;
            }

            int result = _headUnit.updateAssigment(_assigment,
                new Models.Assignment
                {
                    LecturerID = lecturerID,
                    CourseID = courseID,
                    Semester = semester,
                    Year = year,
                    Program = program
                });

            this.Close();
        }
    }
}
