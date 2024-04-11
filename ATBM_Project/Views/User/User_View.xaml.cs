using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ATBM_Project.Views
{
    /// <summary>
    /// Interaction logic for User_View.xaml
    /// </summary>
    public partial class User_View : UserControl
    {
        private ObservableCollection<Users> _listUser { get; set; }
        private Admin_VM _admin;
        private UserControl _userControl;
        public User_View(Admin_VM admin, UserControl userControl)
        {
            _admin = admin;
            _userControl = userControl;
            _listUser = _admin.GetUserData();
            InitializeComponent();
            membersDataGrid.ItemsSource = _listUser;
        }
        private void SearchUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textSearch = SearchUser.Text;
            membersDataGrid.ItemsSource = _listUser;
            if (textSearch != null)
            {
                var resultSearch = _listUser.Where(t => t.Name.Contains(textSearch));
                membersDataGrid.ItemsSource = resultSearch;
            }
        }

        private void deleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Users user = membersDataGrid.SelectedItem as Users;
                _admin.DropUser(user);
                MessageBox.Show("Drop User successfully");
                _userControl.Content = new User_View(_admin, _userControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            _userControl.Content = new AddUser_View(_admin, _userControl);
        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
