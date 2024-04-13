using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using System.Windows;
using System.Windows.Controls;

namespace ATBM_Project.Views.Role
{
    /// <summary>
    /// Interaction logic for UsersOfRole_View.xaml
    /// </summary>
    public partial class UsersOfRole_View : UserControl
    {
        private UserControl _userControl;
        private Admin_VM _admin;
        private string _roleName;
        public UsersOfRole_View(Admin_VM admin, string roleName)
        {
            _admin = admin;
            _roleName = roleName;
            InitializeComponent();
            UserOfRoleGrid.ItemsSource = _admin.GetUsersOfRole(roleName);
        }

        private void GrantRoleButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void RevokeUserFromRole(object sender, RoutedEventArgs e)
        {
            var user = UserOfRoleGrid.SelectedItem as Users;
            //admin
            _admin.RevokeRoleFromUser(_roleName, user.Name);

            UserOfRoleGrid.ItemsSource = _admin.GetUsersOfRole(_roleName);
        }


    }
}
