using ATBM_Project.Models;
<<<<<<< HEAD
using ATBM_Project.Views.Role;
=======
>>>>>>> de88887ca8f9cce2957982f17da56ac12e304965
using ATBM_Project.Views.User;
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

<<<<<<< HEAD
        private void GrantRoleButton_Click(object sender, RoutedEventArgs e)
        {
            _userControl.Content = new GrantRole_View(_admin, _userControl);
            
            
        }

        private void GrantPrivilegeButton_Click(object sender, RoutedEventArgs e)
        {
            _userControl.Content = new GrantPrivilegeToUser_View(_admin, _userControl);
=======
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            Users user = membersDataGrid.SelectedItem as Users;
            EditUser_View editUser = new EditUser_View(_admin, user.Name);

            editUser.Width = this.Width;
            editUser.Height = this.Height;
            editUser.Left = (this.Width - editUser.Width) / 2;
            editUser.Top = (this.Height - editUser.Height) / 2;
            editUser.Show();
>>>>>>> de88887ca8f9cce2957982f17da56ac12e304965
        }
    }
}
