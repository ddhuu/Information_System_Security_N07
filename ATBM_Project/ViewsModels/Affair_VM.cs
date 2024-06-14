using System;
using System.Collections.Generic;
using System.Windows;
using ATBM_Project.Models;
using Oracle.ManagedDataAccess.Client;


namespace ATBM_Project.ViewsModels
{
    public class Affair_VM
    {
        public OracleConnection connection { get; set; }

        public Affair_VM(OracleConnection con)
        {
            connection = con;
        }

        public List<Assignment> getAssigments()
        {
            List<Assignment> assigments = new List<Assignment>();
            string sqlString = "SELECT * FROM ADMIN.PHANCONG";
            OracleCommand cmd = new OracleCommand(sqlString, connection);
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

        // return the number of affected rows
        public int updateAssigment(Assignment oldAssigment, Assignment newAssignment)
        {
            string updateQuery = "UPDATE ADMIN.PHANCONG SET MAGV = :magv, MAHP = :mahp, HK = :hk, NAM = :nam, MACT = :mact WHERE MAGV = :oldmagv AND MAHP = :oldmahp and HK = :oldhk and NAM = :oldnam and MACT = :oldmact";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(updateQuery, connection))
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
                    Console.WriteLine(newAssignment.LecturerID);
                    Console.WriteLine(oldAssigment.LecturerID);
                    Console.WriteLine(newAssignment.CourseID);
                    Console.WriteLine(oldAssigment.CourseID);
                    Console.WriteLine(newAssignment.Semester);
                    Console.WriteLine(oldAssigment.Semester);
                    Console.WriteLine(newAssignment.Year);
                    Console.WriteLine(oldAssigment.Year);
                    Console.WriteLine(newAssignment.Program);
                    Console.WriteLine(oldAssigment.Program);


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

        public List<CourseRegistration> getRegistrations()
        {
            List<CourseRegistration> registrations = new List<CourseRegistration>();
            string sqlString = "SELECT * FROM ADMIN.DANGKY";
            OracleCommand cmd = new OracleCommand(sqlString, connection);
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
                    if(reader.IsDBNull(reader.GetOrdinal("DIEMTK")))
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


        public int insertRegistration(CourseRegistration registration)
        {
            string insertQuery = "INSERT INTO ADMIN.DANGKY (MASV, MAGV, MAHP, HK, NAM, MACT) VALUES (:value1, :value2, :value3, :value4, :value5, :value6)";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(insertQuery, connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("value1", registration.StudentId));
                    command.Parameters.Add(new OracleParameter("value2", registration.LecturerId));
                    command.Parameters.Add(new OracleParameter("value3", registration.CourseId));
                    command.Parameters.Add(new OracleParameter("value4", registration.Semester));
                    command.Parameters.Add(new OracleParameter("value5", registration.Year));
                    command.Parameters.Add(new OracleParameter("value6", registration.Program)); ;

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã thêm {rowsAffected} đăng ký");
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã quá thời hạn điều chỉnh đăng ký học phần trong học kỳ {registration.Semester} năm {registration.Year}");
            }
            return 0;
        }

    }
}
