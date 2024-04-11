using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace ATBM_Project.Views.User
{
    /// <summary>
    /// Interaction logic for EditUser_View.xaml
    /// </summary>
    public partial class EditUser_View : Window
    {
        private Admin_VM _admin;
        private string _userName;
        private string _roles;
        private ObservableCollection<PrivilegeOfTable> _listPriv { get; set; }


        public EditUser_View(Admin_VM admin, string userName)
        {
            _admin = admin;
            _userName = userName;
            _listPriv = _admin.GetPrivilegesOfUser(_userName, 0);
            InitializeComponent();
            Title.Text = userName;
            loadPriv();
        }

        private void loadPriv()
        {
            EditUserController.Content = new PrivilOfUser_View(_admin, _userName);
        }

        private void editPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            EditUserController.Content = new EditPassword_View(_admin, _userName);
        }

        private void privButton_Click(object sender, RoutedEventArgs e)
        {
            EditUserController.Content = new PrivilOfUser_View(_admin, _userName);
        }
    }

}
