using ATBM_Project.ViewsModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ATBM_Project.Views
{
    /// <summary>
    /// Interaction logic for AddUser_View.xaml
    /// </summary>
    public partial class AddUser_View : UserControl
    {
        private Admin_VM _admin;
        private UserControl _userControl;

        public AddUser_View(Admin_VM admin, UserControl userControl)
        {
            _admin = admin;
            _userControl = userControl;
            InitializeComponent();
        }

        private void addNewUser(object sender, RoutedEventArgs e)
        {
            string username = AddUserBox.Text.ToString();
            string password = PasswordBox.Password.ToString();
            try
            {
                _admin.AddNewUser(username, password);
                MessageBox.Show("Create User Success!");
                _userControl.Content = new User_View(_admin, _userControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clickCancel(object sender, RoutedEventArgs e)
        {
            _userControl.Content = new User_View(_admin, _userControl);
        }

    }
}
