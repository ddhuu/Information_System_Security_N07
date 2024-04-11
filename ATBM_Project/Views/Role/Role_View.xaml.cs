using ATBM_Project.ViewsModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ATBM_Project.Views
{
    /// <summary>
    /// Interaction logic for Role_View.xaml
    /// </summary>
    public partial class Role_View : UserControl
    {

        private Admin_VM _admin;
        private ObservableCollection<Models.Role> _listRole { get; set; }
        private UserControl _userControl;


        public Role_View(Admin_VM admin, UserControl userControl)
        {
            _admin = admin;
            _userControl = userControl;
            InitializeComponent();
            _listRole = _admin.GetRolesData();
            rolesDataGrid.ItemsSource = _listRole;
            _userControl = userControl;

        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void SearchRole_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textSearch = SearchRole.Text;
            rolesDataGrid.ItemsSource = _listRole;
            if (textSearch != null)
            {
                var resultSearch = _listRole.Where(t => t.Name.Contains(textSearch));
                rolesDataGrid.ItemsSource = resultSearch;
            }
        }

        private void buttonDeleteRole_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.Role role = rolesDataGrid.SelectedItem as Models.Role;
                if (role != null)
                {
                    _admin.DropRole(role);
                    MessageBox.Show($"Role {role.Name} has been successfully dropped!");
                    _userControl.Content = new Role_View(_admin, _userControl);
                }
                else
                {
                    MessageBox.Show("No role selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddRole_Click(object sender, RoutedEventArgs e)
        {
            _userControl.Content = new AddRole_View(_admin, _userControl);
        }


    }
}
