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

namespace ATBM_Project.Views.Affair
{
    /// <summary>
    /// Interaction logic for UpdateAssignment_Dialog.xaml
    /// </summary>
    public partial class UpdateAssignment_Dialog : Window
    {
        private Affair_VM _affair;
        private Models.Assignment _assignment;
        public UpdateAssignment_Dialog(Affair_VM affair, Models.Assignment assignment)
        {
            _affair = affair;
            _assignment = assignment;
            InitializeComponent();
            inputCourseID.Text = assignment.CourseID;
            inputLecturerID.Text = assignment.LecturerID;
            inputSemester.Text = assignment.Semester.ToString();
            inputYear.Text = assignment.Year.ToString();
            inputProgram.Text = assignment.Program;
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

            if(string.IsNullOrEmpty(lecturerID) || string.IsNullOrEmpty(courseID) || string.IsNullOrEmpty(program) || !semesterSuccess || !yearSuccess)
            {
                MessageBox.Show("Thông tin chưa hợp lệ. Vui lòng nhập lại!");
                return;
            }

            int result = _affair.updateAssigment(_assignment, 
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
