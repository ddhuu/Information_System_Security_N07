using System;
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

    }
}
