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
        
    }
}
