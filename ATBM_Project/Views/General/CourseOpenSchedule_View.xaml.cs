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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oracle.ManagedDataAccess.Client;
using ATBM_Project.Models;

namespace ATBM_Project.Views.General
{
    /// <summary>
    /// Interaction logic for CourseOpenSchedule_View.xaml
    /// </summary>
    public partial class CourseOpenSchedule_View : UserControl
    {
        private OracleConnection _connection;

        public CourseOpenSchedule_View(OracleConnection conn, bool isAffair = false)
        {
            _connection = conn;
            InitializeComponent();
            if(!isAffair)
            {
                btnInsert.Visibility = Visibility.Hidden;
                btnSelect.Visibility = Visibility.Hidden;
                ActionsCol.Width = 0;
                programCol.Width += 58;
            }
            courseOpenSchedulesDataGrid.ItemsSource = getData();
        }

        public List<CourseOpenSchedule> getData()
        {
            string SQLcontex = "SELECT * FROM ADMIN.KHMO";
            OracleCommand cmd = new OracleCommand(SQLcontex, _connection);
            List<CourseOpenSchedule> data = new List<CourseOpenSchedule>();

            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string courseID = reader.GetString(reader.GetOrdinal("MAHP"));
                    int semester = reader.GetInt32(reader.GetOrdinal("HK"));
                    int year = reader.GetInt32(reader.GetOrdinal("NAM"));
                    string program = reader.GetString(reader.GetOrdinal("MACT"));
                    data.Add(new CourseOpenSchedule { courseId = courseID, Semester = semester, Year = year, Program = program});
                }
            }
            return data;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            courseOpenSchedulesDataGrid.ItemsSource = getData();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            (new InsertCourseOpenSchedule_Dialog(_connection)).ShowDialog();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            CourseOpenSchedule cos = courseOpenSchedulesDataGrid.SelectedItem as CourseOpenSchedule;
            if (cos == null)
            {
                MessageBox.Show("Vui lòng chọn một khóa học để cập nhật");
            }
            else
            {
                (new UpdateCourseOpenSchedule_Dialog(_connection, cos)).ShowDialog();
            }
        }
    }
}
