using ATBM_Project.ViewsModels;
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


        public EditUser_View(Admin_VM admin, string userName)
        {
            _admin = admin;
            InitializeComponent();
            _userName = userName;
            Title.Text = userName;
            editPasswordButton_Click(null, null);
        }

        private void editPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            EditUserController.Content = new EditPassword_View(_admin, _userName);
        }
    }

}
