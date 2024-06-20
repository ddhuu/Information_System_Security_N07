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
    /// Interaction logic for UpdateUnit_Dialog.xaml
    /// </summary>
    public partial class UpdateUnit_Dialog : Window
    {
        private OracleConnection _connection;
        private Unit _unit;
        public UpdateUnit_Dialog(OracleConnection conn, Unit unit)
        {
            _connection = conn;
            _unit = unit;
            InitializeComponent();
            inputUnitID.Text = _unit.UnitID;
            inputUnitName.Text = _unit.UnitName;
            inputHeadUnit.Text = _unit.HeadUnit;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string unitID = inputUnitID.Text.Trim();
            string unitName = inputUnitName.Text.Trim();
            string headUnit = inputHeadUnit.Text.Trim();

            if (string.IsNullOrEmpty(unitID) || string.IsNullOrEmpty(unitName) || string.IsNullOrEmpty(headUnit))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }

            string updateQuery = "UPDATE ADMIN.DONVI SET TENDV = :tendv, TRGDV = :trgdv WHERE MADV = :madv";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(updateQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("tendv", OracleDbType.NVarchar2)).Value = unitName;
                    command.Parameters.Add(new OracleParameter("trgdv", headUnit));
                    command.Parameters.Add(new OracleParameter("madv", unitID));

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã cập nhật {rowsAffected} đơn vị");
                    if (rowsAffected > 0)
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật dữ liệu. Vui lòng thử lại sau!");
            }
        }
    }
}
