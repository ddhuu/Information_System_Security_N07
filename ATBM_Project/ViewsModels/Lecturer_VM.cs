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

        public List<CourseRegistration> GetRegistrations(bool isHeadDepartment = false)
        {
            string sql = isHeadDepartment ? "SELECT * FROM ADMIN.DANGKY" : "SELECT * FROM ADMIN.UV_CANHAN_DANGKY";
            List<CourseRegistration> courseRegistrations = new List<CourseRegistration>();

            OracleCommand cmd = new OracleCommand(sql, _connection);
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string courseID = reader.GetString(reader.GetOrdinal("MAHP"));
                    string lecturerId = reader.GetString(reader.GetOrdinal("MAGV"));
                    string studentId = reader.GetString(reader.GetOrdinal("MASV"));
                    int semester = reader.GetInt32(reader.GetOrdinal("HK"));
                    int year = reader.GetInt32(reader.GetOrdinal("NAM"));
                    string program = reader.GetString(reader.GetOrdinal("MACT"));
                    double? processGrade;
                    if (reader.IsDBNull(reader.GetOrdinal("DIEMQT")))
                    {
                        processGrade = null;
                    }
                    else
                    {
                        processGrade = reader.GetDouble(reader.GetOrdinal("DIEMQT"));
                    }

                    double? labGrade;
                    if (reader.IsDBNull(reader.GetOrdinal("DIEMTH")))
                    {
                        labGrade = null;
                    }
                    else
                    {
                        labGrade = reader.GetDouble(reader.GetOrdinal("DIEMTH"));
                    }

                    double? finalExamGrade;
                    if (reader.IsDBNull(reader.GetOrdinal("DIEMCK")))
                    {
                        finalExamGrade = null;
                    }
                    else
                    {
                        finalExamGrade = reader.GetDouble(reader.GetOrdinal("DIEMCK"));
                    }

                    double? finalGrade;
                    if (reader.IsDBNull(reader.GetOrdinal("DIEMTK")))
                    {
                        finalGrade = null;
                    }
                    else
                    {
                        finalGrade = reader.GetDouble(reader.GetOrdinal("DIEMTK"));
                    }

                    courseRegistrations.Add(new CourseRegistration
                    {
                        StudentId = studentId,
                        LecturerId = lecturerId,
                        CourseId = courseID,
                        Semester = semester,
                        Year = year,
                        Program = program,
                        LabGrade = labGrade,
                        ProcessGrade = processGrade,
                        FinalExamGrade = finalExamGrade,
                        FinalGrade = finalGrade
                    });
                }
                reader.Close();
            }

            return courseRegistrations;
        }
    }

}