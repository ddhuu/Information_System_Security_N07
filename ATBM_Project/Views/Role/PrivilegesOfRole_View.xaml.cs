using ATBM_Project.Views.Role;
using ATBM_Project.ViewsModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        public PrivilegesOfRole_View(Admin_VM admin, UserControl userControl)
        {
            InitializeComponent();
            _admin = admin;
            _userControl = userControl;
        }
    }
}
