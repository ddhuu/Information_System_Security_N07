using ATBM_Project.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace ATBM_Project.ViewsModels
{
    public class Student_VM
    {
        private OracleConnection connection;
        public Student_VM(OracleConnection _conn) { 
            connection = _conn;
        }
<<<<<<< HEAD
=======
        public Student getInfor()
        {
            string SQLcontex = $"SELECT * FROM ADMIN.SINHVIEN";
            OracleCommand cmd = new OracleCommand(SQLcontex, connection);

            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string studentID = reader.GetString(reader.GetOrdinal("MASV"));
                    string fullName = reader.GetString(reader.GetOrdinal("HOTEN"));
                    string gender = reader.GetString(reader.GetOrdinal("PHAI"));
                    string phoneNumber = reader.GetString(reader.GetOrdinal("DT"));
                    string address = reader.GetString(reader.GetOrdinal("DCHI"));
                    int cummulativeCredits = reader.GetInt32(reader.GetOrdinal("SOTCTL"));
                    double avgGrade = reader.GetDouble(reader.GetOrdinal("DTBTL"));
                    string major = reader.GetString(reader.GetOrdinal("MANGANH"));
                    DateTime dob = reader.GetDateTime(reader.GetOrdinal("NGSINH"));
                    return new Student
                    {
                        Id = studentID,
                        Name = fullName,
                        Gender = gender,
                        PhoneNumber = phoneNumber,
                        Address = address,
                        Major = major,
                        AvgGrade = avgGrade,
                        CummulativeCredits = cummulativeCredits,
                        DOB = dob.ToString("dd/MM/yyyy")
                    };
                }
                else
                {
                    return null;
                }
            }
        }
>>>>>>> 6cefbae6efdbfe50c05a701b934dd18709767ec4

        public List<Models.Student> getStudentList()
        {
            List<Models.Student> students = new List<Models.Student>();
            string SQLcontext = $"SELECT * FROM ADMIN.SINHVIEN";
            OracleCommand cmd = new OracleCommand(SQLcontext, connection);
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
                        string program = reader.GetString(reader.GetOrdinal("MACT"));
                        DateTime dob = reader.GetDateTime(reader.GetOrdinal("NGSINH"));
                        students.Add (new Models.Student{
                            Id = studentID, 
                            Name = fullName, 
                            Gender = gender,
                            DOB = dob.ToString("dd/MM/yyyy"),
                            Address = address,
                            PhoneNumber = phoneNumber,
                            Program = program,
                            Major = major,
                            CummulativeCredits = cummulativeCredits, 
                            AvgGrade = avgGrade});
                    }
                    reader.Close();
                }
            return students;
        }

        public int updateInfor(Student student)
        {
            string sql = $"UPDATE ADMIN.SINHIEN SET DT = {student.PhoneNumber}, DCHI = {student.Address}";
            OracleCommand cmd = new OracleCommand(sql, connection);
            return cmd.ExecuteNonQuery();
        }
        public List<CourseOpenSchedule> getOpenSchedule()
        {
            List<CourseOpenSchedule> openSchedules = new List<CourseOpenSchedule>();
            string sql = $"SELECT * FROM ADMIN.HOCPHAN";
            OracleCommand cmd = new OracleCommand(sql, connection);
            CourseOpenSchedule schedule = null;
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string courseID = reader.GetString(reader.GetOrdinal("MAHP"));
                    int semester = reader.GetInt32(reader.GetOrdinal("HK"));
                    string program = reader.GetString(reader.GetOrdinal("MACT"));
                    int year = reader.GetInt32(reader.GetOrdinal("NAM"));
                     schedule = new CourseOpenSchedule
                    {
                        courseId = courseID,
                        Semester = semester,
                        Year = year,
                        Program = program
                    };
                    openSchedules.Add(schedule);
                }
                reader.Close(); 
            }
            return openSchedules;
        }
        public List<Course> FindCourses()
        {
            string sql = $"SELECT * FROM ADMIN.HOCPHAN";
            OracleCommand cmd = new OracleCommand(sql, connection);
            List<Course> courses = new List<Course>();
            using(OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string courseID = reader.GetString(reader.GetOrdinal("MAHP"));
                    string courseName = reader.GetString(reader.GetOrdinal("TENHP"));
                    int totalCredits = reader.GetInt32(reader.GetOrdinal("SOTC"));
                    int theoryCredits = reader.GetInt32(reader.GetOrdinal("STLT"));
                    int labCredits = reader.GetInt32(reader.GetOrdinal("STTH"));
                    string unit = reader.GetString(reader.GetOrdinal("MADV"));
                    int maxNumParticipient = reader.GetInt32(reader.GetOrdinal("SOSVTD"));
                    courses.Add(new Course
                    {
                        CourseID = courseID,
                        CourseName = courseName,
                        TotalCredit = totalCredits,
                        TheoryCredit = theoryCredits,
                        LabCredit = labCredits,
                        Unit = unit,
                        MaxNumParticipatient = maxNumParticipient
                    });
                }
                reader.Close();
            }
            return courses;
        }

        public List<CourseRegistration> GetRegistrations()
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

        public int deleteRegistration(CourseRegistration registration)
        {
            string updateQuery = "DELETE FROM ADMIN.DANGKY WHERE MASV = :masv AND MAGV = :magv AND MAHP = :mahp and HK = :hk and NAM = :nam and MACT = :mact";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(updateQuery, connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("masv", registration.StudentId));
                    command.Parameters.Add(new OracleParameter("magv", registration.LecturerId));
                    command.Parameters.Add(new OracleParameter("mahp", registration.CourseId));
                    command.Parameters.Add(new OracleParameter("hk", registration.Semester));
                    command.Parameters.Add(new OracleParameter("nam", registration.Year));
                    command.Parameters.Add(new OracleParameter("mact", registration.Program));

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã xóa {rowsAffected} đăng ký");
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
