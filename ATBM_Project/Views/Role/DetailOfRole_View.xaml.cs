using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace ATBM_Project.Views.Role
{
    /// <summary>
    /// Interaction logic for DetailOfRole_View.xaml
    /// </summary>
    /// 

    public partial class DetailOfRole_View : Window
    {
        private Admin_VM _admin;
        private string roleName;
        private ObservableCollection<Users> _listUser { get; set; }
        public DetailOfRole_View(Admin_VM admin, string roleName)
        {
            _admin = admin;
            this.roleName = roleName;
            _listUser = _admin.GetUsersOfRole(roleName);
            InitializeComponent();
            Title.Text = roleName;
            loadUser();
        }

        private void loadUser()
        {
            DetailOfRole.Content = new UsersOfRole_View(_admin, roleName);
        }

        private void privilegeButton_Click(object sender, RoutedEventArgs e)
        {
            DetailOfRole.Content = new PrivilegesOfRole_View(_admin, roleName);
        }

        private void ViewPrivOnColumn(object sender, RoutedEventArgs e)
        {
            DetailOfRole.Content = new PrivOnColumn_View(_admin, roleName);
        }
    }
}
