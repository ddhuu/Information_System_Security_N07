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

namespace ATBM_Project.Views.General
{
    /// <summary>
    /// Interaction logic for InsertCourseOpenSchedule_Dialog.xaml
    /// </summary>
    public partial class InsertCourseOpenSchedule_Dialog : Window
    {
        private OracleConnection _connection;
        public InsertCourseOpenSchedule_Dialog(OracleConnection conn)
        {
            _connection = conn;
            InitializeComponent();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
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

            string insertQuery = "INSERT INTO ADMIN.KHMO (MAHP, HK, NAM, MACT) VALUES (:value1, :value2, :value3, :value4)";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(insertQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("value1", courseID));
                    command.Parameters.Add(new OracleParameter("value2", semester));
                    command.Parameters.Add(new OracleParameter("value3", year));
                    command.Parameters.Add(new OracleParameter("value4", program));;

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã thêm {rowsAffected} kế hoạch mở");
                    if (rowsAffected > 0)
                    {
                        inputCourseID.Text = "";
                        inputSemester.Text = "";
                        inputYear.Text = "";
                        inputProgram.Text = "";
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi chèn dữ liệu. Vui lòng thử lại");
            }
        }
    }
}
