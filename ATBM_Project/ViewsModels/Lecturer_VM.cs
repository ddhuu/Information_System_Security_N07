using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATBM_Project.Models;

namespace ATBM_Project.ViewsModels
{
    public class Lecturer_VM : Employee_VM
    {
        public Lecturer_VM(OracleConnection _connection) :base(_connection)
        {

        }

        public ObservableCollection<Models.Assignment> getAssignmentList()
        {
            ObservableCollection<Models.Assignment> assignments = new ObservableCollection<Models.Assignment>();
            string SQLcontext = $"SELECT * FROM ADMIN.UV_CANHAN_PHANCONG";
/*            string SQLcontext = $"SELECT * FROM ADMIN.PHANCONG";*/

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

        public ObservableCollection<Models.CourseRegistration> getRegistrations()
        {
            ObservableCollection<Models.CourseRegistration> registrations = new ObservableCollection<Models.CourseRegistration>();
            string SQLcontext = $"SELECT * FROM ADMIN.UV_CANHAN_DANGKY";
            OracleCommand cmd = new OracleCommand(SQLcontext, connection);
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
                    double labGrade = reader.GetDouble(reader.GetOrdinal("DIEMTH"));
                    double processGrade = reader.GetDouble(reader.GetOrdinal("DIEMQT"));
                    double finalExamGrade = reader.GetDouble(reader.GetOrdinal("DIEMCK"));
                    double finalGrade = reader.GetDouble(reader.GetOrdinal("DIEMTK"));


                    registrations.Add(new Models.CourseRegistration
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
            return registrations;
        }
    }
   
}
