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
    /// Interaction logic for InsertUnit_Dialog.xaml
    /// </summary>
    public partial class InsertUnit_Dialog : Window
    {
        private OracleConnection _connection;
        public InsertUnit_Dialog(OracleConnection conn)
        {
            _connection = conn;
            InitializeComponent();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            string unitID = inputUnitID.Text.Trim();
            string unitName = inputUnitName.Text.Trim();
            string headUnit = inputHeadUnit.Text.Trim();

            if (string.IsNullOrEmpty(unitID) || string.IsNullOrEmpty(unitName) || string.IsNullOrEmpty(headUnit))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            string insertQuery = "INSERT INTO ADMIN.DONVI (MADV, TENDV, TRGDV) VALUES (:value1, :value2, :value3)";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(insertQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("value1", unitID));
                    command.Parameters.Add(new OracleParameter("value2", OracleDbType.NVarchar2)).Value = unitName;
                    command.Parameters.Add(new OracleParameter("value3", headUnit));

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã thêm {rowsAffected} đơn vị");
                    if (rowsAffected > 0)
                    {
                        inputUnitID.Text = "";
                        inputUnitName.Text = "";
                        inputHeadUnit.Text = "";
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
