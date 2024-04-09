using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System.Windows;

namespace ATBM_Project.Views
{
    /// <summary>
    /// Interaction logic for Admin_View.xaml
    /// </summary>
    public partial class Admin_View : Window
    {
        private OracleConnection _connection;
        private string _role;
        private string _user;
        private Admin_VM _admin;
        private ObservableCollection<Users> _listUser { get; set; }

        public Admin_View(OracleConnection conn, string Role, string user)
        {
            _connection = conn;
            _role = Role;
            _user = user;
            _admin = new Admin_VM(_connection, _role, user);
            _listUser = _admin.GetUserData();
            InitializeComponent();
            Title.Text = _user;
            LoadUser();

        }
        private void LoadUser()
        {
            UserController.Content = new User_View(_admin, UserController);
        }
        private void exitClick(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }

        private void userButton_Click(object sender, RoutedEventArgs e)
        {
            UserController.Content = new User_View(_admin, UserController);
        }

        private void roleButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
