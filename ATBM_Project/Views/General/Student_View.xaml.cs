﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oracle.ManagedDataAccess.Client;
using ATBM_Project.Models;

namespace ATBM_Project.Views.General
{
    /// <summary>
    /// Interaction logic for Student_View.xaml
    /// </summary>
    public partial class Student_View : UserControl
    {
        private OracleConnection _connection;
        public Student_View(OracleConnection conn, bool isAffair = false)
        {
            _connection = conn;
            InitializeComponent();
            if (!isAffair)
            {
                btnInsert.Visibility = Visibility.Hidden;
                btnSelect.Visibility = Visibility.Hidden;
                ActionsCol.Width = 0;
                /*addressCol.Width += 0;*/
            }
            studentsDataGrid.ItemsSource = GetStudents();
        }

        public List<Models.Student> GetStudents()
        {
            List<Models.Student> students = new List<Models.Student>();

            string sqlString = "SELECT * FROM ADMIN.SINHVIEN";
            OracleCommand cmd = new OracleCommand(sqlString, _connection);
            try
            {

            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string studentID = reader.GetString(reader.GetOrdinal("MASV"));
                    string fullName = reader.GetString(reader.GetOrdinal("HOTEN"));
                    string gender = reader.GetString(reader.GetOrdinal("PHAI"));
                    string phoneNumber = reader.GetString(reader.GetOrdinal("DT"));
                    string address = reader.GetString(reader.GetOrdinal("DCHI"));
                    int cummulativeCredits = reader.GetInt32(reader.GetOrdinal("SOTCTL"));
                    double avgGrade = reader.GetDouble(reader.GetOrdinal("DTBTL"));
                    string major = reader.GetString(reader.GetOrdinal("MANGANH"));
                    string dob = reader.GetDateTime(reader.GetOrdinal("NGSINH")).ToString("dd/MM/yyyy");
                    string program = reader.GetString(reader.GetOrdinal("MACT"));
                    string group = reader.GetString(reader.GetOrdinal("COSO"));
                    students.Add(new Models.Student
                    {
                        Id = studentID,
                        Name = fullName,
                        Gender = gender,
                        PhoneNumber = phoneNumber,
                        Address = address,
                        Major = major,
                        AvgGrade = avgGrade,
                        CummulativeCredits = cummulativeCredits,
                        DOB = dob,
                        Program = program,
                        Group = group,
                    });
                }
            }
            }
            catch (Exception ex){
                MessageBox.Show(ex.Message);
            }
            return students;
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            (new InsertStudent_Dialog(_connection)).ShowDialog();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            studentsDataGrid.ItemsSource = GetStudents();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Models.Student student = ((Button)sender).Tag as Models.Student;
            if(student == null)
            {
                return;
            }

            (new UpdateStudent_Dialog(_connection, student)).ShowDialog();  
        }
    }
}
