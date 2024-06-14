﻿using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATBM_Project.Models;

namespace ATBM_Project.ViewsModels
{
    public class Lecturer_VM 
    {
        private OracleConnection _connection;
        public Lecturer_VM(OracleConnection connection)
        {
            _connection = connection;
        }

        public List<Models.Assignment> getAssignmentList()
        {
            List<Models.Assignment> assignments = new List<Models.Assignment>();
            string SQLcontext = $"SELECT * FROM ADMIN.UV_CANHAN_PHANCONG";

            OracleCommand cmd = new OracleCommand(SQLcontext, _connection);
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string lecturerID = reader.GetString(reader.GetOrdinal("MAGV"));
                    string courseID = reader.GetString(reader.GetOrdinal("MAHP"));
                    int semester = reader.GetInt16(reader.GetOrdinal("HK"));
                    int year = reader.GetInt16(reader.GetOrdinal("NAM"));
                    string program = reader.GetString(reader.GetOrdinal("MACT"));
                    assignments.Add(new Models.Assignment
                    {
                        LecturerID = lecturerID,
                        CourseID = courseID,
                        Semester = semester,
                        Year = year,
                        Program = program
                    });
                }
                reader.Close();
            }

            return assignments;
        }

        /*public ObservableCollection<Models.Registration> getRegistrations()
        {
            ObservableCollection<Models.Registration> registrations = new ObservableCollection<Models.Registration>();
            string SQLcontext = $"SELECT * FROM ADMIN.UV_CANHAN_DANGKY";
            OracleCommand cmd = new OracleCommand(SQLcontext, connection);
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string lecturerID = reader.GetString(reader.GetOrdinal("MAGV"));
                    string courseID = reader.GetString(reader.GetOrdinal("MAHP"));
                    int semester = reader.GetInt16(reader.GetOrdinal("HK"));
                    int year = reader.GetInt16(reader.GetOrdinal("NAM"));
                    string program = reader.GetString(reader.GetOrdinal("MACT"));
                    string studentID = reader.GetString(reader.GetOrdinal("MASV"));
                    double labGrade = reader.GetDouble(reader.GetOrdinal("DIEMTH"));
                    double progressGrade = reader.GetDouble(reader.GetOrdinal("DIEMQT"));
                    double finalGrade = reader.GetDouble(reader.GetOrdinal("DIEMCK"));
                    double totalGrade = reader.GetDouble(reader.GetOrdinal("DIEMTK"));


                    registrations.Add(new Models.Registration
                    {
                        studentID = studentID,
                        lecturerID = lecturerID,
                        courseID = courseID,
                        Semester = semester,
                        Year = year,
                        Program = program,
                        labGrade = labGrade,
                        progressGrade = progressGrade,
                        finalGrade = finalGrade,
                        totalGrade = totalGrade
                    });
                }
                reader.Close();
            }
            return registrations;
        }*/
    }
   
}
