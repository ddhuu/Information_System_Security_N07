using ATBM_Project.ViewsModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace ATBM_Project.Views.Employee
{
    /// <summary>
    /// Interaction logic for Student_View.xaml
    /// </summary>
    public partial class Student_View : UserControl
    {
        private ObservableCollection<Models.Student> _listStudent;
        private Student_VM _studentVM;
        private UserControl _userControl;
        public Student_View(Student_VM student_VM)
        {
            _studentVM = student_VM;
            InitializeComponent();

            /*_listStudent = new ObservableCollection<Models.Student> {
                new Models.Student { Id = "1", Name = "John Doe", Gender = "Male", DOB = new DateTime(2000, 1, 1), Address = "123 Main St", PhoneNumber = "123-456-7890", Program = "Computer Science", Major = "CS", CummulativeCredits = 30, AvgGrade = 3.5 }
            };*/
            _listStudent = _studentVM.getStudentList();
            StudentListView.ItemsSource = _listStudent;

        }
    }
}
