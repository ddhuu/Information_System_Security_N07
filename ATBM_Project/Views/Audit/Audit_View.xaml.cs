﻿using ATBM_Project.Models;
using ATBM_Project.ViewsModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ATBM_Project.Views.Audit
{
    /// <summary>
    /// Interaction logic for Audit_View.xaml
    /// </summary>
    public partial class Audit_View : UserControl
    {
        private List<Audits> _listAudit { get; set; }
        private Admin_VM _adminVM;
        private UserControl _userControl;
        private bool _isFirstLoad = true;

        private bool _isInitializing = true;
        public Audit_View(Admin_VM adminVM, UserControl userControl)
        {
            _adminVM = adminVM;
            _userControl = userControl;
            InitializeComponent();
            List<string> auditType = new List<string>();
            auditType.Add("Standard Audit");
            auditType.Add("Fine-Grained Audit For DIEM");
            auditType.Add("Fine-Grained Audit For PHUCAP");
            auditTypeCombobox.ItemsSource = auditType;
            auditTypeCombobox.SelectedItem = "Standard Audit";
            _listAudit = _adminVM.GetStandardAudit();
            AuditGrid.ItemsSource = _listAudit;
            SQL_TEXT.Visibility = Visibility.Collapsed;
            int result;
            _adminVM.execProcedure("sp_get_audit_status", out result);
            auditCheckBox.IsChecked = result == 1 ? true : false;
            _adminVM.execProcedure("get_fg_audit_status", out result);
            fineGrainAuditCheckBox.IsChecked = result == 1 ? true : false;
            _isInitializing = false;
        }
        private void ChangeAuditType(object sender, RoutedEventArgs e)
        {
            string type = auditTypeCombobox.SelectedItem as string;
            if (type == "Standard Audit")
            {
                AuditGrid.ItemsSource = _adminVM.GetStandardAudit();
                SQL_TEXT.Visibility = Visibility.Collapsed;
                fineGrainAuditCheckBox.IsEnabled = false;
                fineGrainAuditCheckBox.Visibility = Visibility.Collapsed;
                auditCheckBox.IsEnabled = true;
                auditCheckBox.Visibility = Visibility.Visible;
            }
            else if (type == "Fine-Grained Audit For PHUCAP")
            {
                AuditGrid.ItemsSource = _adminVM.GetFGAudit("FGA_POLICY_PHUCAP");
                SQL_TEXT.Visibility = Visibility.Visible;
                fineGrainAuditCheckBox.IsEnabled = true;
                fineGrainAuditCheckBox.Visibility = Visibility.Visible;
                auditCheckBox.IsEnabled = false;
                auditCheckBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                AuditGrid.ItemsSource = _adminVM.GetFGAudit("FGA_POLICY_DIEM");
                SQL_TEXT.Visibility = Visibility.Visible;
                fineGrainAuditCheckBox.IsEnabled = true;
                fineGrainAuditCheckBox.Visibility = Visibility.Visible;
                auditCheckBox.IsEnabled = false;
                auditCheckBox.Visibility = Visibility.Collapsed;
            }
        }



        private void SearchUser_TextChange(object sender, TextChangedEventArgs e)
        {
            string textSearch = SearchUser.Text;
            AuditGrid.ItemsSource = _listAudit;
            if (textSearch != null)
            {
                var searchResult = _listAudit.Where(t => t.USERNAME.Contains(textSearch));
                AuditGrid.ItemsSource = searchResult;
            }


        }

        private async void Switch_Checked(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;
            string type = auditTypeCombobox.SelectedItem as string;
            if (type != "Standard Audit") return;

            CheckBox checkBox = sender as CheckBox;
            if (checkBox.IsChecked == true)
            {
                checkBox.IsHitTestVisible = false;
                await Task.Run(() => _adminVM.execProcedure("sp_audit_user", "NHANSU", "enable"));
                await Task.Run(() => _adminVM.execProcedure("sp_audit_user", "SINHVIEN", "enable"));
                checkBox.IsHitTestVisible = true;
                if (!_isFirstLoad)
                {
                    MessageBox.Show("Standard Audit  successfully enabled.");
                }


            }
        }

        private async void Switch_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;
            string type = auditTypeCombobox.SelectedItem as string;
            if (type != "Standard Audit") return;

            CheckBox checkBox = sender as CheckBox;
            if (checkBox.IsChecked == false)
            {
                checkBox.IsHitTestVisible = false;
                await Task.Run(() => _adminVM.execProcedure("sp_audit_user", "NHANSU", "disable"));
                await Task.Run(() => _adminVM.execProcedure("sp_audit_user", "SINHVIEN", "disable"));
                checkBox.IsHitTestVisible = true;
                if (!_isFirstLoad)
                {
                    MessageBox.Show("Standard Audit successfully disabled.");
                }

            }
        }

        private async void Switch_Checked_FG(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.IsChecked == true)
            {
                checkBox.IsHitTestVisible = false;
                await Task.Run(() => _adminVM.EnableAuditPolicy("ADMIN", "DANGKY", "FGA_POLICY_DIEM", true));
                await Task.Run(() => _adminVM.EnableAuditPolicy("ADMIN", "NHANSU", "FGA_POLICY_PHUCAP", true));
                checkBox.IsHitTestVisible = true;
                if (!_isFirstLoad)
                {
                    MessageBox.Show("Fine-Grained Audit  successfully enabled.");
                }

            }
        }

        private async void Switch_Unchecked_FG(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.IsChecked == false)
            {
                checkBox.IsHitTestVisible = false;
                await Task.Run(() => _adminVM.DisableAuditPolicy("ADMIN", "DANGKY", "FGA_POLICY_DIEM"));
                await Task.Run(() => _adminVM.DisableAuditPolicy("ADMIN", "NHANSU", "FGA_POLICY_PHUCAP"));
                checkBox.IsHitTestVisible = true;
                if (!_isFirstLoad)
                {
                    MessageBox.Show("Fine-Grained Audit successfully disabled.");
                }

            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _isFirstLoad = false;
        }




    }
}