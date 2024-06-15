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

        public int insertAssigment(Assignment newAssignment)
        {
            string insertQuery = "INSERT INTO ADMIN.PHANCONG (MAGV, MAHP, HK, NAM, MACT) VALUES (:value1, :value2, :value3, :value4, :value5)";
            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(insertQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("value1", newAssignment.LecturerID));
                    command.Parameters.Add(new OracleParameter("value2", newAssignment.CourseID));
                    command.Parameters.Add(new OracleParameter("value3", newAssignment.Semester));
                    command.Parameters.Add(new OracleParameter("value4", newAssignment.Year));
                    command.Parameters.Add(new OracleParameter("value5", newAssignment.Program));

                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã thêm {rowsAffected} phân công");

                    // Thực thi câu lệnh
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }


        public int updateAssigment(Assignment oldAssigment, Assignment newAssignment)
        {
            string updateQuery = "UPDATE ADMIN.PHANCONG SET MAGV = :magv, MAHP = :mahp, HK = :hk, NAM = :nam, MACT = :mact WHERE MAGV = :oldmagv AND MAHP = :oldmahp and HK = :oldhk and NAM = :oldnam and MACT = :oldmact";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(updateQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("magv", newAssignment.LecturerID));
                    command.Parameters.Add(new OracleParameter("mahp", newAssignment.CourseID));
                    command.Parameters.Add(new OracleParameter("hk", newAssignment.Semester));
                    command.Parameters.Add(new OracleParameter("nam", newAssignment.Year));
                    command.Parameters.Add(new OracleParameter("mact", newAssignment.Program));

                    command.Parameters.Add(new OracleParameter("oldmagv", oldAssigment.LecturerID));
                    command.Parameters.Add(new OracleParameter("oldmahp", oldAssigment.CourseID));
                    command.Parameters.Add(new OracleParameter("oldhk", oldAssigment.Semester));
                    command.Parameters.Add(new OracleParameter("oldnam", oldAssigment.Year));
                    command.Parameters.Add(new OracleParameter("oldmact", oldAssigment.Program));


                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã cập nhật {rowsAffected} phân công");
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }


        public int deleteAssigment(Assignment assignment)
        {
            string deleteQuery = "DELETE FROM ADMIN.PHANCONG WHERE MAGV = :magv AND MAHP = :mahp and HK = :hk and NAM = :nam and MACT = :mact";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(deleteQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("magv", assignment.LecturerID));
                    command.Parameters.Add(new OracleParameter("mahp", assignment.CourseID));
                    command.Parameters.Add(new OracleParameter("hk", assignment.Semester));
                    command.Parameters.Add(new OracleParameter("nam", assignment.Year));
                    command.Parameters.Add(new OracleParameter("mact", assignment.Program));

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã xóa {rowsAffected} phân công");
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }

        public List<CourseRegistration> getRegistrations()
        {
            List<CourseRegistration> registrations = new List<CourseRegistration>();
            string sqlString = "SELECT * FROM ADMIN.UV_CANHAN_DANGKY";
            OracleCommand cmd = new OracleCommand(sqlString, _connection);
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string studentID = reader.GetString(reader.GetOrdinal("MASV"));
                    string lecturerID = reader.GetString(reader.GetOrdinal("MAGV"));
                    string courseID = reader.GetString(reader.GetOrdinal("MAHP"));
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

                    double? finalExameGrade;
                    if (reader.IsDBNull(reader.GetOrdinal("DIEMCK")))
                    {
                        finalExameGrade = null;
                    }
                    else
                    {
                        finalExameGrade = reader.GetDouble(reader.GetOrdinal("DIEMCK"));
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

                    registrations.Add(new CourseRegistration
                    {
                        StudentId = studentID,
                        LecturerId = lecturerID,
                        CourseId = courseID,
                        Semester = semester,
                        Year = year,
                        Program = program,
                        LabGrade = labGrade,
                        ProcessGrade = processGrade,
                        FinalExamGrade = finalExameGrade,
                        FinalGrade = finalGrade,
                    });
                }
            }
            return registrations;
        }


    }   
}
