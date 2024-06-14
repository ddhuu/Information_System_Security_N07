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
    /// Interaction logic for Unit_View.xaml
    /// </summary>
    public partial class Unit_View : UserControl
    {
        private OracleConnection _connection;
        public Unit_View(OracleConnection conn, bool isAffair = false)
        {
            _connection = conn;
            InitializeComponent();
            if(!isAffair)
            {
                btnInsert.Visibility = Visibility.Hidden;
                btnSelect.Visibility = Visibility.Hidden;
                ActionsCol.Width = 0;
                unitNameCol.Width += 190;
            }
            unitsDataGrid.ItemsSource = getUnits();
        }

        public List<Unit> getUnits()
        {
            string SQLcontex = "SELECT * FROM ADMIN.DONVI";
            OracleCommand cmd = new OracleCommand(SQLcontex, _connection);

            List<Unit> units = new List<Unit>();
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string unitID = reader.GetString(reader.GetOrdinal("MADV"));
                    string unitName = reader.GetString(reader.GetOrdinal("TENDV"));
                    string headUnit = reader.GetString(reader.GetOrdinal("TRGDV"));
                    units.Add(new Unit
                    {
                        UnitID = unitID,
                        UnitName = unitName,
                        HeadUnit = headUnit
                    });
                }
            }
            return units;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            unitsDataGrid.ItemsSource = getUnits();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            (new InsertUnit_Dialog(_connection)).ShowDialog();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Unit unit = ((Button)sender).Tag as Unit;
            if(unit != null)
            {
                (new UpdateUnit_Dialog(_connection, unit)).ShowDialog();
            }
        }
    }
}
