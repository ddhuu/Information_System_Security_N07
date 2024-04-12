using ATBM_Project.ViewsModels;
using System.Windows;
using System.Windows.Controls;

namespace ATBM_Project.Views.User
{
    /// <summary>
    /// Interaction logic for EditPassword_View.xaml
    /// </summary>
    public partial class EditPassword_View : UserControl
    {
        private Admin_VM _admin;
        private string _userName;
        public EditPassword_View(Admin_VM admin, string userName)
        {
            _admin = admin;
            _userName = userName;
            InitializeComponent();
        }

        private void EditPassword(object sender, RoutedEventArgs e)
        {
            PasswordBox newPassword = new PasswordBox();
            newPassword = NewPasswordBox;

            string pwd = newPassword.Password;
            _admin.EditUserPassword(_userName, pwd);

            MessageBox.Show("Change Password is success!");


        }

    }
}
