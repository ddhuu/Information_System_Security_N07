using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATBM_Project.ViewsModels
{
    public class Lecturer_VM : Employee_VM
    {
        public Lecturer_VM(OracleConnection _connection) :base(_connection)
        {
            
        }   
    }
   
}
