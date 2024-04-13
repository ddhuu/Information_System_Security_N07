using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;


namespace ATBM_Project.Views.Role
{
    /// <summary>
    /// Interaction logic for GrantRole_View.xaml
    /// </summary>
    public partial class GrantRole_View : UserControl
    {
        private OracleConnection _connection;
        private bool isGrantOption;
        private Admin_VM _admin;
        private UserControl _userControl;
        private ObservableCollection<Users> _listUser { get; set; }
        private ObservableCollection<Models.Role> _listRole { get; set; }

        public GrantRole_View(Admin_VM admin, UserControl userControl)
        {
            InitializeComponent();
            _admin = admin;
            _userControl = userControl;
            _listUser = _admin.GetUserData();
            _listRole = admin.GetRolesData();
            UserComboBox.ItemsSource = _listUser;
            RoleComboBox.ItemsSource = _listRole;
        }

        private void UserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            _userControl.Content = new User_View(_admin, _userControl);
        }



        private void GrantButton_Click(object sender, RoutedEventArgs e)
        {
            ATBM_Project.Models.Role role = (ATBM_Project.Models.Role)RoleComboBox.SelectedValue;

            ATBM_Project.Models.Users user = (ATBM_Project.Models.Users)UserComboBox.SelectedValue;
            isGrantOption = grantOptionCheck.IsChecked.Value;
            try
            {
                if (isGrantOption)
                { 
                    _admin.grantRoleToUserWithGrantOption(role.Name, user.Name);
                    MessageBox.Show($"Trao vai trò {role.Name} và grant option cho người dùng {user.Name} thành công");
                }
                else
                {
                    _admin.grantRoleToUser(role.Name, user.Name);
                    MessageBox.Show($"Trao vai trò {role.Name} cho người dùng {user.Name} thành công");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
