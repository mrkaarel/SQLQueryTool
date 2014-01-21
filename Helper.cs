using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace SqlQueryTool
{
    public class Helper
    {
        public static SqlConnection OpenConnection(ConnectionData connectionData)
        {
            string connectionString = String.Empty;
            if (connectionData.UseIntegratedSecurity) {
                connectionString = String.Format("Data Source = {0}; Initial Catalog = {1}; Integrated Security = True; Connection Timeout = {2};", connectionData.ServerName, connectionData.DatabaseName, connectionData.Timeout);
            }
            else {
                connectionString = String.Format("Data Source = {0}; Initial Catalog = {1}; User ID = {2}; Password = {3}; Connection Timeout = {4};", connectionData.ServerName, connectionData.DatabaseName, connectionData.UserName, connectionData.Password, connectionData.Timeout);
            }

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            return conn;
        }
    }
}