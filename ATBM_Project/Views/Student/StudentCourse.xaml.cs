using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ATBM_Project.Views.Student
{
    /// <summary>
    /// Interaction logic for StudentCourse.xaml
    /// </summary>
    public partial class StudentCourse : UserControl
    {
        private Student_VM student_VM;
        public StudentCourse(Student_VM student_VM)
        {
            this.student_VM = student_VM;
            InitializeComponent();
            ObservableCollection<Course> courses = student_VM.FindCourses();
            
        }
    }
}
