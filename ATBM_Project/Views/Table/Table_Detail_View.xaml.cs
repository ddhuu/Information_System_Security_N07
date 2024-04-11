using ATBM_Project.ViewsModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace ATBM_Project.Views.Table
{
    /// <summary>
    /// Interaction logic for Table_Detail_View.xaml
    /// </summary>
    public partial class Table_Detail_View : Window
    {
        private ObservableCollection<Models.Column> _listColumns { get; set; }
        private ObservableCollection<Models.PrivilegeOfTable> _listPrivileges { get; set; }
        private string tableName { get; set; }
        public Table_Detail_View(Admin_VM admin, string table, string type = "Table")
        {
            tableName = table;
            _listColumns = admin.GetColumnData(tableName);
            _listPrivileges = admin.getPrivilegesOfTable(tableName);

            InitializeComponent();

            tableDetailGrid.ItemsSource = _listColumns;
            privilegesGrid.ItemsSource = _listPrivileges;
            txtTableName.Text = tableName;
            txtLabel.Text = type + ":";
        }
    }
}
