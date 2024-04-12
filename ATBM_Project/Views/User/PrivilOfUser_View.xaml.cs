using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ATBM_Project.Views.User
{
    /// <summary>
    /// Interaction logic for PrivilOfUser_View.xaml
    /// </summary>
    public partial class PrivilOfUser_View : UserControl
    {
        private ObservableCollection<PrivilegeOfTable> _listPriv { get; set; }
        private Admin_VM _admin;
        private string _userName;
        private UserControl _userControl;
        public PrivilOfUser_View(Admin_VM admin, string userName, UserControl userControl)
        {
            _admin = admin;
            _userName = userName;
            _userControl = userControl;
            InitializeComponent();
            List<string> privKind = new List<string>();
            privKind.Add("Cấp trực tiếp");
            privKind.Add("Cấp qua Role");
            grantTypeComboBox.ItemsSource = privKind;
            grantTypeComboBox.SelectedItem = "Cấp trực tiếp";
            _listPriv = _admin.GetPrivilegesOfUser(_userName, 0);
            PrivsGrid.ItemsSource = _listPriv;



        }

        private void ChooseGrantType(object sender, RoutedEventArgs e)
        {
            string option = grantTypeComboBox.SelectedValue as string;
            if (option == "Cấp trực tiếp")
            {
                PrivsGrid.ItemsSource = _admin.GetPrivilegesOfUser(_userName, 0);
            }
            else
            {
                PrivsGrid.ItemsSource = _admin.GetPrivilegesOfUser(_userName, 1);
            }


        }

        private void buttonDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            PrivilegeOfTable priv = PrivsGrid.SelectedItem as PrivilegeOfTable;
            //admin
            _admin.RevokePrivs(priv, _userName);
            PrivsGrid.ItemsSource = _admin.GetPrivilegesOfUser(_userName, 0);
        }

        private void addPrivButton_Click(object sender, RoutedEventArgs e)
        {
            _userControl.Content = new GrantPrivilege_View(_admin, _userName, _userControl);
        }

    }
}
