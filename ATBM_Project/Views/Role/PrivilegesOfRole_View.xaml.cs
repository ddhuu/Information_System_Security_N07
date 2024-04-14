using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using System.Windows;
using System.Windows.Controls;

namespace ATBM_Project.Views.Role
{
    /// <summary>
    /// Interaction logic for PrivilegesOfRole.xaml
    /// </summary>
    public partial class PrivilegesOfRole_View : UserControl
    {
        private Admin_VM _admin;
        private UserControl _userControl;
        private string _roleName;
        public PrivilegesOfRole_View(Admin_VM admin, string roleName)
        {
            InitializeComponent();
            _admin = admin;
            _roleName = roleName;
            //admin
            PrivOfRoleGrid.ItemsSource = _admin.GetPrivsOfRole(roleName);

        }

        private void RevokePrivFromRole(object sender, RoutedEventArgs e)
        {
            var priv = PrivOfRoleGrid.SelectedItem as PrivilegeOfTable;
            _admin.RevokePrivFromRole(_roleName, priv);
            PrivOfRoleGrid.ItemsSource = _admin.GetPrivsOfRole(_roleName);
        }

    }
}
