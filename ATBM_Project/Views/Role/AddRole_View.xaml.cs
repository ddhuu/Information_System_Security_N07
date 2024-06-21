using ATBM_Project.ViewsModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ATBM_Project.Views
{
    /// <summary>
    /// Interaction logic for AddRole_View.xaml
    /// </summary>
    public partial class AddRole_View : UserControl
    {
        private UserControl _userControl;
        private Admin_VM _admin;
        public AddRole_View(Admin_VM admin_VM, UserControl userControl)
        {
            _admin = admin_VM;
            _userControl = userControl;
            InitializeComponent();
        }

        public void addNewRole(object sender, RoutedEventArgs e)
        {
            string text = AddRoleBox.Text;
            try
            {
                _admin.CreateRole(text);
                MessageBox.Show("Add new Role Successfully!");
                _userControl.Content = new Role_View(_admin, _userControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void clickCancel(object sender, RoutedEventArgs e)
        {
            _userControl.Content = new Role_View(_admin, _userControl);
        }
    }
}

