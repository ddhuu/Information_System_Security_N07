using ATBM_Project.Models;
using Microsoft.SqlServer.Server;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            try
            {
                string sql = "SELECT * FROM ADMIN.NHANSU";
                OracleCommand cmd =  new OracleCommand(sql, _connection);
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
                        string group = reader.GetString(reader.GetOrdinal("COSO"));
                        
                        employees.Add(new Employee
                        {
                            ID = empID,
                            FullName = fullName,
                            Gender = gender,
                            PhoneNumber = phoneNumber,
                            Role = role,
                            Unit = unit,
                            Grant = grant,
                            DOB = dob.ToString("dd/MM/yyyy"),
                            Group = group
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                // nothing to do
            }
            return employees;

        }


        public int insertEmployee(Employee employee)
        {
            string insertQuery = "INSERT INTO ADMIN.NHANSU (MANV, HOTEN, PHAI, NGSINH, PHUCAP, DT, VAITRO, MADV, COSO) VALUES (:value1, :value2, :value3, TO_DATE(:value4, 'DD/MM/YYYY'), :value5, :value6, :value7, :value8, :value9)";
            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(insertQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("value1", employee.ID));
                    command.Parameters.Add(new OracleParameter("value2", OracleDbType.NVarchar2)).Value = employee.FullName;
                    command.Parameters.Add(new OracleParameter("value3", employee.Gender));
                    command.Parameters.Add(new OracleParameter("value4", employee.DOB));
                    command.Parameters.Add(new OracleParameter("value5", employee.Grant));
                    command.Parameters.Add(new OracleParameter("value6", employee.PhoneNumber));
                    command.Parameters.Add(new OracleParameter("value7", employee.Role));
                    command.Parameters.Add(new OracleParameter("value8", employee.Unit));
                    command.Parameters.Add(new OracleParameter("value9", employee.Group));

                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã thêm {rowsAffected} nhân viên");

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


        public int updateEmployee(Employee oldEmployee, Employee newEmployee)
        {
            string updateQuery = "UPDATE ADMIN.NHANSU SET HOTEN = :hoten, PHAI = :phai, NGSINH = TO_DATE(:ngsinh, 'DD/MM/YYYY'),  DT = :dt, PHUCAP = :phucap, VAITRO = :vaitro, MADV = :madv WHERE MANV = :manv";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(updateQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("hoten", OracleDbType.NVarchar2)).Value = newEmployee.FullName;
                    command.Parameters.Add(new OracleParameter("phai", newEmployee.Gender));
                    command.Parameters.Add(new OracleParameter("ngsinh", newEmployee.DOB));
                    command.Parameters.Add(new OracleParameter("dt", newEmployee.PhoneNumber));
                    command.Parameters.Add(new OracleParameter("phucap", newEmployee.Grant));
                    command.Parameters.Add(new OracleParameter("vaitro", newEmployee.Role));
                    command.Parameters.Add(new OracleParameter("madv", newEmployee.Unit));
                    command.Parameters.Add(new OracleParameter("manv", oldEmployee.ID));

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã cập nhật {rowsAffected} nhân viên");
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật dữ liệu. Vui lòng thử lại sau!");
            }

            return 0;
        }


        public int deleteEmployee(Employee employee)
        {
            string deleteQuery = "DELETE FROM ADMIN.NHANSU WHERE MANV = :manv";

            // Tạo đối tượng OracleCommand
            try
            {
                using (OracleCommand command = new OracleCommand(deleteQuery, _connection))
                {
                    // Thêm tham số và gán giá trị
                    command.Parameters.Add(new OracleParameter("manv", employee.ID));

                    // Thực thi câu lệnh
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Đã xóa {rowsAffected} nhân viên");
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }
        
    }
}
