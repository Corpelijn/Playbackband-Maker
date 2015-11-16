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
        public int id;
        public string username;
        public Uitvoerder uitvoerder;
        public AccountSettings settings;
        public List<Recht> rechten;
        public List<GroepRecht> groepen;

        public Account(int id, string username)
        {
            this.id = id;
            this.username = username;
            this.rechten = new List<Recht>();
            this.groepen = new List<GroepRecht>();
            this.settings = new AccountSettings();
            ReadRechten();
        }

        public static bool ValidateCredentials(string username, string password)
        {
            DatabaseReader.CheckDatabaseConnection();

            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT * FROM ACCOUNT WHERE USERNAME=\'" + username.ToLower() + "\' AND PASSWORD=\'" + password + "\'");
            return dt.Rows.Count == 0 ? false : true;
        }

        public static Account GetAccountFromDatabase(string username)
        {
            DatabaseReader.CheckDatabaseConnection();

            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT ID FROM ACCOUNT WHERE USERNAME=\'" + username.ToLower() + "\'");
            if (dt.Rows.Count > 0)
            {
                return new Account(Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString()), username);
            }
            return null;
        }

        private void ReadRechten()
        {
            DatabaseReader.CheckDatabaseConnection();

            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT GROEP_ID FROM GROEPEN_PER_ACCOUNT WHERE ACCOUNT_ID=" + this.id);

            HashSet<int> idToCheck = new HashSet<int>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    idToCheck.Add(Convert.ToInt32(row.ItemArray[0].ToString()));
                }
            }


            int index = 0;
            while (index != idToCheck.Count)
            {
                List<int> temp = idToCheck.ToList();
                groepen.Add(GroepRecht.GetGroepFromId(temp[index]));

                dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT ERVEND_GROEP_ID FROM GROEPRECHT WHERE GROEP_ID=" + temp[index]);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        idToCheck.Add(Convert.ToInt32(row.ItemArray[0].ToString()));
                    }
                }

                index++;
            }
        }
    }
}