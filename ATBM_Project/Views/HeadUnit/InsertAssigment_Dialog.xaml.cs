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
    /// Interaction logic for InsertAssigment_Dialog.xaml
    /// </summary>
    public partial class InsertAssigment_Dialog : Window
    {
        private ViewsModels.HeadUnit_VM _headUnit;
        public InsertAssigment_Dialog(ViewsModels.HeadUnit_VM headUnit)
        {
            _headUnit = headUnit;
            InitializeComponent();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            string lecturerID = inputLecturerID.Text.Trim();
            string courseID = inputCourseID.Text.Trim();
            int semester;
            bool semesterSuccess = int.TryParse(inputSemester.Text.Trim(), out semester);
            int year;
            bool yearSuccess = int.TryParse(inputYear.Text.Trim(), out year);
            string program = inputProgram.Text.Trim();

            if(string.IsNullOrEmpty(lecturerID) || string.IsNullOrEmpty(courseID) || string.IsNullOrEmpty(program) 
                || !semesterSuccess || !yearSuccess)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            int result = _headUnit.insertAssigment(new Models.Assignment
            {
                LecturerID = lecturerID,
                CourseID = courseID,
                Program = program,
                Semester = semester,
                Year = year
            });

            if(result > 0)
            {
                this.Close();
            }
        }
    }
}
