using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Windows;

namespace ATBM_Project.ViewsModels
{
    internal class ConnectionDB_VM : IDisposable
    {
        private OracleConnection connection;

        public OracleConnection OracleConnection(string username, string password)
        {
            try
            {
                string conString = ConfigurationManager.ConnectionStrings["OracleDB"].ConnectionString;
                conString = conString.Replace("{username}", username).Replace("{password}", password);
                connection = new OracleConnection(conString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public void Dispose()
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}