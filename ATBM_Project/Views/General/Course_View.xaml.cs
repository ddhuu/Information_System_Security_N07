using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Oracle.ManagedDataAccess.Client;
using ATBM_Project.Models;
using System;

namespace ATBM_Project.Views.General
{
    /// <summary>
    /// Interaction logic for Course_View.xaml
    /// </summary>
    public partial class Course_View : UserControl
    {
        private OracleConnection _connection;
        public Course_View(OracleConnection con, bool isAffair = false)
        {
            _connection = con;
            InitializeComponent();
            if(!isAffair)
            {
                btnInsert.Visibility = Visibility.Hidden;
                btnSelect.Visibility = Visibility.Hidden;
                ActionsCol.Width = 0;
                courseNameCol.Width += 80;
            }
            coursesDataGrid.ItemsSource = getCourses();
        }

        public List<Course> getCourses()
        {
            string SQLcontex = "SELECT * FROM ADMIN.HOCPHAN";
            OracleCommand cmd = new OracleCommand(SQLcontex, _connection);

            List<Course> courses = new List<Course>();
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string courseID = reader.GetString(reader.GetOrdinal("MAHP"));
                    string courseName = reader.GetString(reader.GetOrdinal("TENHP"));
                    int totalCredit = reader.GetInt32(reader.GetOrdinal("SOTC"));
                    int theoryCredit = reader.GetInt32(reader.GetOrdinal("STLT"));
                    int labCredit = reader.GetInt32(reader.GetOrdinal("STTH"));
                    string unit = reader.GetString(reader.GetOrdinal("MADV"));
                    int maxNumParticipatient = reader.GetInt32(reader.GetOrdinal("SOSVTD"));
                    courses.Add(new Course
                    {
                        CourseID = courseID,
                        CourseName = courseName,
                        TotalCredit = totalCredit,
                        TheoryCredit = theoryCredit,
                        LabCredit = labCredit,
                        Unit = unit,
                        MaxNumParticipatient = maxNumParticipatient,
                    });
                }
            }
            return courses;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Course course = ((Button)sender).Tag as Course;
                if(course != null)
                {
                    (new UpdateCourse_Dialog(_connection, course)).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // nothing to do
            }
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            (new InsertCourse_Dialog(_connection)).ShowDialog();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            coursesDataGrid.ItemsSource = getCourses();
        }
    }
}
