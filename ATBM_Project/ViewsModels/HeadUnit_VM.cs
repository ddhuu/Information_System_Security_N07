using System;
using System.Collections.Generic;
using System.Windows;
using ATBM_Project.Models;
using Oracle.ManagedDataAccess.Client;

namespace ATBM_Project.ViewsModels
{
    public class HeadUnit_VM
    {
        private OracleConnection _connection;
        public HeadUnit_VM(OracleConnection connection)
        {
            _connection = connection;
        }

        public List<Assignment> GetAssignments()
        {
            List<Assignment> assigments = new List<Assignment>();
            string sqlString = "SELECT * FROM ADMIN.PHANCONG";
            OracleCommand cmd = new OracleCommand(sqlString, _connection);
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string lecturerID = reader.GetString(reader.GetOrdinal("MAGV"));
                    string courseID = reader.GetString(reader.GetOrdinal("MAHP"));
                    int semester = reader.GetInt32(reader.GetOrdinal("HK"));
                    int year = reader.GetInt32(reader.GetOrdinal("NAM"));
                    string program = reader.GetString(reader.GetOrdinal("MACT"));
                    assigments.Add(new Models.Assignment
                    {
                        LecturerID = lecturerID,
                        CourseID = courseID,
                        Semester = semester,
                        Year = year,
                        Program = program
                    });
                }
            }
            return assigments;
        }
    }
}
