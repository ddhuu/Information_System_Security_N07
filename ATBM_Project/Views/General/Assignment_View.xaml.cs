using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Oracle.ManagedDataAccess.Client;
using ATBM_Project.ViewsModels;
using ATBM_Project.Models;

namespace ATBM_Project.Views.General
{
    /// <summary>
    /// Interaction logic for Assignment_View.xaml
    /// </summary>
    public partial class Assignment_View : UserControl
    {
        private OracleConnection _connection;
        private Lecturer_VM _lecturer;
        public Assignment_View(OracleConnection connection, bool isHeadUnit = false, bool isAffair = false)
        {
            _connection = connection;
            _lecturer = new Lecturer_VM(_connection);
            InitializeComponent();
            if (!isHeadUnit)
            {
                btnInsert.Visibility = Visibility.Hidden;
                btnSelect.Visibility = Visibility.Hidden;
                ActionsCol.Width = 0;
                lecturerIdCol.Width += 40;
                courseIdCol.Width += 40;
                programCol.Width += 80;
                if (isAffair)
                {

                    assignmentsDataGrid.ItemsSource = _lecturer.getAssignmentList(true);
                    btnSelect.Visibility = Visibility.Visible;
                    ActionsCol.Width += 140;

                }
                else
                {
                    assignmentsDataGrid.ItemsSource = _lecturer.getAssignmentList();
                }
            }
            else
            {
                assignmentsDataGrid.ItemsSource = _lecturer.getAssignmentList();
            }
        }

        private void BtnUpdate_Click (object sender, EventArgs e)
        {

            try
            {
                Assignment assignment = ((Button)sender).Tag as Assignment;
                if (assignment != null)
                {
                    (new UpdateAssignment_Dialog(_connection, assignment)).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // nothing to do
            }
        }
    }
}
