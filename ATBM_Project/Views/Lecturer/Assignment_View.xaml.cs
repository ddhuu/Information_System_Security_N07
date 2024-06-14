using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Oracle.ManagedDataAccess.Client;
using ATBM_Project.ViewsModels;
using ATBM_Project.Models;

namespace ATBM_Project.Views.Lecturer
{
    /// <summary>
    /// Interaction logic for Assignment_View.xaml
    /// </summary>
    public partial class Assignment_View : UserControl
    {
        private OracleConnection _connection;
        private Lecturer_VM _lecturer;
        public Assignment_View(OracleConnection connection)
        {
            _connection = connection;
            _lecturer = new Lecturer_VM(_connection);
            InitializeComponent();
            assignmentsDataGrid.ItemsSource = _lecturer.getAssignmentList();
        }

/*        private void BtnUpdate_Click(object sender, EventArgs e)
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
        }*/
    }
}
