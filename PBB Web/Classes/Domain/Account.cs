using PBB_Web.Classes.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Domain
{
    public class Account
    {
        public string username;
        public Uitvoerder uitvoerder;

        public Account(string username)
        {
            this.username = username;
        }

        public static bool ValidateCredentials(string username, string password)
        {
            DatabaseReader.CheckDatabaseConnection();

            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT * FROM ACCOUNT WHERE USERNAME=\'" + username + "\' AND PASSWORD=\'" + password + "\'");
            return dt.Rows.Count == 0 ? false : true;
        }

        public static Account GetAccountFromDatabase(string username)
        {
            return new Account(username);
        }
    }
}