using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;

namespace ATBM_Project.Views.Role
{
    /// <summary>
    /// Interaction logic for GrantPrivilege_View.xaml
    /// </summary>
    public partial class GrantPrivilege_View : UserControl
    {
        private Admin_VM admin_VM;
        private UserControl userControl;
        private ObservableCollection<ATBM_Project.Models.Table> _tables;
        private ObservableCollection<Models.Role> _roles;

        public GrantPrivilege_View(Admin_VM admin_VM, UserControl userControl)
        {
            InitializeComponent();
            this.admin_VM = admin_VM;
            this.userControl = userControl;
            this._roles = admin_VM.GetRolesData();
            this._tables = admin_VM.GetTablesData("table");
            TableComboBox.ItemsSource = _tables;
            RoleComboBox.ItemsSource = _roles;

        }




        private void ColumnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            List<string> privileges = new List<string> { "SELECT", "DELETE", "UPDATE", "INSERT" };
            PrivilegeComboBox.ItemsSource = privileges;
            string operation = (string)PrivilegeComboBox.SelectedItem;
            if (operation == null)
            {

            }
            else
            {
                if (operation.Equals("SELECT") || operation.Equals("UPDATE"))
                {
                    Models.Table selectedTable = (Models.Table)TableComboBox.SelectedItem;
                    ColumnCheckBox.ItemsSource = admin_VM.GetColumnData(selectedTable.Name);
                }
                else
                {
                    ColumnCheckBox.ItemsSource = null;
                }
            }


        }



        private void Grant_Click(object sender, RoutedEventArgs e)
        {
            Models.Table selectedTable = (Models.Table)TableComboBox.SelectedItem;
            Models.Role selectedRole = (Models.Role)RoleComboBox.SelectedItem;
            string operation = (string)PrivilegeComboBox.SelectedItem;
            List<Models.Column> selectedColumns = new List<Models.Column>();
            FindCheckedCheckBoxes(ColumnCheckBox, selectedColumns);
            string privilege = "";
            if (operation.Equals("SELECT"))
            {
                privilege = "SELECT(";

                for (int i = 0; i < selectedColumns.LongCount(); i++)
                {
                    if (i == selectedColumns.LongCount() - 1)
                    {
                        privilege += selectedColumns[i].Name;
                    }
                    else
                    {
                        privilege += selectedColumns[i].Name + ",";
                    }
                }
                privilege += ")";
            }
            else if (operation.Equals("UPDATE"))
            {
                privilege = "UPDATE(";
                for (int i = 0; i < selectedColumns.LongCount(); i++)
                {
                    if (i == selectedColumns.LongCount() - 1)
                    {
                        privilege += selectedColumns[i].Name;
                    }
                    else
                    {
                        privilege += selectedColumns[i].Name + ",";
                    }
                }
                privilege += ")";

            }
            else
            {
                privilege = operation;
            }
            try
            {
                admin_VM.GrantPrivilegeToRole(operation, selectedTable.Name, selectedRole.Name);
                MessageBox.Show("Grant privilege to role successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }






        }
        private void FindCheckedCheckBoxes(DependencyObject parent, List<Column> selectedColumns)
        {
            // Check if the parent is null
            if (parent == null)
                return;

            // Iterate through the children of the parent
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                // Get the child at index i
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                // If the child is a CheckBox
                if (child is CheckBox checkBox)
                {
                    // If the CheckBox is checked
                    if (checkBox.IsChecked == true)
                    {
                        // Get the corresponding Column object from the CheckBox's DataContext
                        Column column = checkBox.DataContext as Column;

                        // Add the selected column to the list
                        selectedColumns.Add(column);
                    }
                }
                else
                {
                    // If the child is not a CheckBox, recursively search its children
                    FindCheckedCheckBoxes(child, selectedColumns);
                }
            }
        }

        private void PrivilegeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string operation = (string)PrivilegeComboBox.SelectedItem;
            if (operation.Equals("SELECT") || operation.Equals("UPDATE"))
            {
                Models.Table selectedTable = (Models.Table)TableComboBox.SelectedItem;
                ColumnCheckBox.ItemsSource = admin_VM.GetColumnData(selectedTable.Name);
            }
            else
            {
                ColumnCheckBox.ItemsSource = null;
            }

        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            userControl.Content = new Role_View(admin_VM, userControl);

        }
    }
}
