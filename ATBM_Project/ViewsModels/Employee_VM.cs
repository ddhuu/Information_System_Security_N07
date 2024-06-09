

using ATBM_Project.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace ATBM_Project.ViewsModels
{
    public class Employee_VM
    {
        public OracleConnection connection { get; set; }
        public Employee_VM(OracleConnection con) { 
            connection = con;
        }
        public Employee getInfor()
        {
            string SQLcontex = $"SELECT * FROM ADMIN.NHANSU_NV_VIEW";
            OracleCommand cmd = new OracleCommand(SQLcontex, connection);
           
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string empID = reader.GetString(reader.GetOrdinal("MANV"));
                    string fullName = reader.GetString(reader.GetOrdinal("HOTEN"));
                    string gender = reader.GetString(reader.GetOrdinal("PHAI"));
                    string phoneNumber = reader.GetString(reader.GetOrdinal("DT"));
                    string role = reader.GetString(reader.GetOrdinal("VAITRO"));
                    string unit = reader.GetString(reader.GetOrdinal("MADV"));
                    double grant = reader.GetDouble(reader.GetOrdinal("PHUCAP"));
                    DateTime dob = reader.GetDateTime(reader.GetOrdinal("NGSINH"));
                    return new Employee
                    {
                        ID = empID,
                        FullName = fullName,
                        Gender = gender,
                        PhoneNumber = phoneNumber,
                        Role = role,
                        Unit = unit,
                        Grant = grant,
                        DOB = dob
                    };
                }
                else
                {
                    return null;
                }
            }
           
        }
        public void updatePhoneNumber(string phoneNumber)
        {
            string sql = $"UPDATE ADMIN.NHANSU_NV_VIEW SET DT = {phoneNumber}";
            OracleCommand cmd = new OracleCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
