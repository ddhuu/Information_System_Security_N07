using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ATBM_Project.ViewsModels;
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
        public UsersOfRole_View()
        {
            InitializeComponent();
        }

        private void GrantRoleButton_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
