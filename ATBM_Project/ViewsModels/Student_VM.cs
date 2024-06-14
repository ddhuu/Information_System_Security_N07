using ATBM_Project.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;

namespace ATBM_Project.ViewsModels
{
    public class Student_VM
    {
        private OracleConnection connection;
        public Student_VM(OracleConnection _conn) { 
            connection = _conn;
        }
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
                        DOB = dob.ToShortDateString()
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public ObservableCollection<Models.Student> getStudentList()
        {
            ObservableCollection<Models.Student> students = new ObservableCollection<Models.Student>();
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
                            DOB = dob.ToShortDateString(),
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
        public ObservableCollection<CourseOpenSchedule> getOpenSchedule()
        {
            ObservableCollection<CourseOpenSchedule> openSchedules = new ObservableCollection<CourseOpenSchedule>();
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
        public ObservableCollection<Course> FindCourses()
        {
            string sql = $"SELECT * FROM ADMIN.HOCPHAN";
            OracleCommand cmd = new OracleCommand(sql, connection);
            ObservableCollection<Course> courses = new ObservableCollection<Course>();
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

        public ObservableCollection<CourseRegistration> GetRegistrations(bool isAffair = false)
        {
            string sql = $"SELECT * FROM ADMIN.UV_CANHAN_DANGKY";
            string sql2 = $"SELECT * FROM ADMIN.DANGKY";
            ObservableCollection<CourseRegistration> courseRegistrations = new ObservableCollection<CourseRegistration>();
            if (isAffair)
            {
                OracleCommand cmd = new OracleCommand(sql2, connection);
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
            }
            else
            {
                OracleCommand cmd = new OracleCommand(sql, connection);
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
            }
            return courseRegistrations;
        }

        public int deleteRegistration(string studentID, string lecturerID, string courseId, string program, int semester, int year)
        {
            string sql = $"DELETE FROM ADMIN.DANGKY";
            OracleCommand cmd = new OracleCommand(sql, connection);
            return cmd.ExecuteNonQuery();
        }
        
    }
}
