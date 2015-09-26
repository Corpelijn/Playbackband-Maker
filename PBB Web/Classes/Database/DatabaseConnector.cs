using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Database
{
    public class DatabaseConnector
    {
        #region "Fields"

        private Oracle.DataAccess.Client.OracleConnection connection;

        #endregion

        #region "Constructors"

        public DatabaseConnector()
        {
            DatabaseConnector.Instance = this;
        }

        #endregion

        #region "Properties"

        public static DatabaseConnector Instance { get; private set; }

        #endregion

        #region "Methods"

        public void OpenConnection(string server, int port, string database, string username, string password)
        {
            string connectionString = "";
            if (username != "")
            {
                connectionString += "user id=" + username + ";";
            }
            if (password != "")
            {
                connectionString += "password=" + password + ";";
            }
            connectionString += "data source=//" + server;
            if (port > 0)
            {
                connectionString += ":" + port.ToString();
            }
            connectionString += "/" + database;

            this.connection = new Oracle.DataAccess.Client.OracleConnection(connectionString);
            this.connection.Open();
        }

        public bool IsConnected()
        {
            if (this.connection == null)
            {
                return false;
            }

            if (this.connection.State == System.Data.ConnectionState.Open)
            {
                return true;
            }

            return false;
        }

        public bool CloseConnection()
        {
            return false;
        }

        public DataTable ExecuteQuery(string command)
        {
            OracleCommand oracleCommand = new OracleCommand(command, this.connection);
            OracleDataReader reader = null;
            try
            {
                reader = oracleCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            DataTable table = new DataTable();
            if(reader != null)
                table.Load(reader);

            return table;
        }

        public bool ExecuteNonQuery(string command)
        {
            OracleCommand oracleCommand = new OracleCommand(command, this.connection);
            try
            {
                oracleCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        #endregion
    }
}