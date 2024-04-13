using ATBM_Project.ViewsModels;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ATBM_Project.Views.User
{
    /// <summary>
    /// Interaction logic for GrantPrivilege_View.xaml
    /// </summary>
    public partial class GrantPrivilege_View : UserControl
    {
        private Admin_VM _admin;
        private string _userName;
        private string priv;
        private string table;

        private UserControl _userControl;

        public GrantPrivilege_View(Admin_VM admin, string userName, UserControl userControl)
        {
            _admin = admin;
            _userName = userName;
            _userControl = userControl;
            InitializeComponent();
            loadTableData();
        }

        private void comboBoxTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Option_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void loadTableData()
        {
            try
            {

                ObservableCollection<Models.Table> listTable = _admin.GetTablesData("table");
                foreach (Models.Table table in listTable)
                {
                    comboBoxTable.Items.Add(table.Name);
                }

            }

            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void GrantPriv_Click(object sender, RoutedEventArgs e)
        {
            string isChecked_Sel = (Select.IsChecked.HasValue && Select.IsChecked.Value).ToString();
            string isChecked_Ins = (Insert.IsChecked.HasValue && Insert.IsChecked.Value).ToString();
            string isChecked_Upd = (Update.IsChecked.HasValue && Update.IsChecked.Value).ToString();
            string isChecked_Del = (Delete.IsChecked.HasValue && Delete.IsChecked.Value).ToString();
            string isChecked_Opt = (Option.IsChecked.HasValue && Option.IsChecked.Value).ToString();

            Stack<string> privsgrant = new Stack<string>();
            string query_grant = "GRANT ";
            table = comboBoxTable.SelectedItem.ToString();
            Debug.WriteLine(table);

            if (isChecked_Sel == "True")
            {
                privsgrant.Push("SELECT");
            }
            if (isChecked_Ins == "True")
            {
                privsgrant.Push("INSERT");
            }
            if (isChecked_Upd == "True")
            {
                privsgrant.Push("UPDATE");
            }
            if (isChecked_Del == "True")
            {
                privsgrant.Push("DELETE");
            }

            //GRANT
            if (privsgrant.Count == 0)
            {
                query_grant = "";
            }
            else
            {
                for (int i = privsgrant.Count - 1; i >= 0; i--)
                {
                    if (privsgrant.ElementAt(i) != "")
                    {
                        query_grant += privsgrant.ElementAt(i);
                        if (i != 0)
                        {
                            query_grant += ",";
                        }
                    }
                }
                query_grant += $" ON {table} TO {_userName} ";
                if (isChecked_Opt == "True")
                {
                    query_grant += " WITH GRANT OPTION";
                }
            }

            //execute
            try
            {
                if (query_grant != "")
                {
                    string sql = $"{query_grant}";
                    OracleCommand command = new OracleCommand(sql, _admin.connection);
                    OracleDataReader reader = command.ExecuteReader();

                    reader.Close();
                }
                MessageBox.Show("Add privilege is success!");
            }
            catch (OracleException exp)
            {
                MessageBox.Show(exp.Message);
            }
        }



    }
}
