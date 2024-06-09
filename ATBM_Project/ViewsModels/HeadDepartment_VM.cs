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
    public class HeadDepartment_VM : Lecturer_VM
    {
        public HeadDepartment_VM(OracleConnection _connection) : base(_connection)
        {
            
        }
        public ObservableCollection<Employee> getAllEmps()
        {
            ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
            string sql = $"SELECT * FROM ADMIN.NHANSU";
            OracleCommand cmd =  new OracleCommand(sql, connection);
            Employee emp = null;
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                int i = 0;
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
                        DOB = dob
                    };
                    employees.Add(emp);
                    i++;
                }
                reader.Close();
               
            }
            return employees;

        }
        public int addEmployee(Employee emp)
        {
            string sql = $"INSERT INTO ADMIN.NHANVIEN(MANV, HOTEN, NGAYSINH, PHAI, PHUCAP, DT, VAITRO, MADV)" +
                $" values ({emp.ID}, {emp.FullName}, {emp.DOB}, {emp.Gender}, {emp.Grant}, {emp.PhoneNumber}, {emp.Role}, {emp.Unit})";

            OracleCommand cmd = new OracleCommand(sql, connection);
            return cmd.ExecuteNonQuery();
        }
        public int deleteEmployee(Employee emp)
        {
            string sql = $"DELETE FROM ADMIN.NHANVIEN WHERE MANV = {emp.ID}";
            OracleCommand cmd = new OracleCommand(sql, connection);
            return cmd.ExecuteNonQuery();
        }
        public int updateEmp(string empID, Employee emp)
        {
            string sql = $"UPDATE ADMIN.NHANVIEN" +
                $" SET HOTEN = {emp.FullName}, PHAI = {emp.Gender}, NGAYSINH = {emp.DOB}, PHUCAP = {emp.Grant}, DT = {emp.PhoneNumber}, VAITRO = ${emp.Role}, MADV = {emp.Unit}"
                + $"WHERE MASV = {emp.ID}";
            OracleCommand cmd = new OracleCommand(sql, connection);
            return cmd.ExecuteNonQuery();
        }
        
        
        

        
    }
}
