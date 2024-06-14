using ATBM_Project.Models;
using Microsoft.SqlServer.Server;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATBM_Project.ViewsModels
{
    public class HeadDepartment_VM 
    {
        private OracleConnection _connection;
        public HeadDepartment_VM(OracleConnection connection)
        {
            _connection = connection;
        }
        public List<Employee> getAllEmps()
        {
            List<Employee> employees = new List<Employee>();
            string sql = $"SELECT * FROM ADMIN.NHANSU";
            OracleCommand cmd =  new OracleCommand(sql, _connection);
            Employee emp = null;
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string empID = reader.GetString(reader.GetOrdinal("MANV"));
                    string fullName = reader.GetString(reader.GetOrdinal("HOTEN"));
                    string gender = reader.GetString(reader.GetOrdinal("PHAI"));
                    string phoneNumber = reader.GetString(reader.GetOrdinal("DT"));
                    string role = reader.GetString(reader.GetOrdinal("VAITRO"));
                    string unit = reader.GetString(reader.GetOrdinal("MADV"));
                    double grant = reader.GetDouble(reader.GetOrdinal("PHUCAP"));
                    DateTime dob = reader.GetDateTime(reader.GetOrdinal("NGSINH"));
                    emp =  new Employee
                    {
                        ID = empID,
                        FullName = fullName,
                        Gender = gender,
                        PhoneNumber = phoneNumber,
                        Role = role,
                        Unit = unit,
                        Grant = grant,
                        DOB = dob.ToShortDateString()
                    };
                    employees.Add(emp);
                }
                reader.Close();
               
            }
            return employees;

        }
        
    }
}
