using ATBM_Project.ViewsModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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


namespace ATBM_Project.Views.Table
{
    /// <summary>
    /// Interaction logic for Table_View.xaml
    /// </summary>
    public partial class Table_View : UserControl
    {
        private Admin_VM _admin;
        private ObservableCollection<ATBM_Project.Models.Table> _listTable { get; set; }
        private UserControl _userControl;
        public string Type { get; set; }
        public Table_View(Admin_VM admin, UserControl userControl, string type = "Table")
        {
            _admin = admin;
            _userControl = userControl;
            Type = type;
            InitializeComponent();
            _listTable = _admin.GetTablesData(Type.ToLower());
            tablesDataGrid.ItemsSource = _listTable;
            _userControl = userControl;
            txtViewName.Text = Type + "s";
        }

        private void buttonViewTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ATBM_Project.Models.Table table  = (ATBM_Project.Models.Table)tablesDataGrid.SelectedItem;
                if (table != null)
                {
                    Table_Detail_View detailView = new Table_Detail_View(_admin, table.Name, Type);
                    detailView.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No role selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textSearch = searchBox.Text.ToUpper();
            if(textSearch.Length == 0)
            {
                tablesDataGrid.ItemsSource = _listTable;
                return;
            }

            if (textSearch != null)
            {
                var resultSearch = _listTable.Where(t => t.Name.Contains(textSearch));
                tablesDataGrid.ItemsSource = resultSearch;
            }
        }
    }
}
