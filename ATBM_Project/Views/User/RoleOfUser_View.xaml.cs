﻿using ATBM_Project.ViewsModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ATBM_Project.Views.User
{
    /// <summary>
    /// Interaction logic for RoleOfUser_View.xaml
    /// </summary>
    public partial class RoleOfUser_View : UserControl
    {
        private Admin_VM _admin;
        private ObservableCollection<Models.Role> _listRole;
        private string _userName;



        public RoleOfUser_View(Admin_VM admin, string userName)
        {
            _admin = admin;
            _userName = userName;

            _listRole = _admin.GetRolesOfUser(_userName);
            InitializeComponent();
            RolesGrid.ItemsSource = _listRole;
        }
    }
}