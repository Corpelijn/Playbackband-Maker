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
            
        }

        #endregion

        #region "Properties"

        private static DatabaseConnector instance;
        public static DatabaseConnector GetInstance()
        {
            if (instance == null || !instance.IsConnected())
            {
                instance = new DatabaseConnector();
                try
                {
                    instance.OpenConnection("192.168.94.5", 1521, "xe", "PBB", "PBB");
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return instance;
        }

        public static bool IsDatabaseConnectionAvailable()
        {
            try
            {
                if (GetInstance() != null)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

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
            connectionString += "/" + database + ";";

            connectionString += "Connection Timeout=1;";

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
                if (ex.Message == "ORA-03114: not connected to ORACLE")
                {
                    this.connection = null;
                }
                Console.WriteLine(ex.StackTrace);
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