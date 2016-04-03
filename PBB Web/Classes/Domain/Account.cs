using PBB_Web.Classes.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

            if (!DatabaseConnector.IsDatabaseConnectionAvailable()) return;

            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT ID, VOORNAAM, ACHTERNAAM FROM UITVOERDER WHERE ACCOUNT_ID=\'" + id.ToString() + "\'");
            if (dt.Rows.Count > 0)
            {
                this.uitvoerder = new Uitvoerder(Convert.ToInt32(dt.Rows[0].ItemArray[0]), dt.Rows[0].ItemArray[1].ToString(), dt.Rows[0].ItemArray[2].ToString());
            }
            ReadRechten();
        }

        public static bool ValidateCredentials(string username, string password)
        {
            if (!DatabaseConnector.IsDatabaseConnectionAvailable()) return false;

            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT * FROM ACCOUNT WHERE USERNAME=\'" + username.ToLower() + "\' AND PASSWORD=\'" + password + "\'");
            return dt.Rows.Count == 0 ? false : true;
        }

        public static Account GetAccountFromDatabase(string username)
        {
            if (!DatabaseConnector.IsDatabaseConnectionAvailable()) return null;

            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT ID FROM ACCOUNT WHERE USERNAME=\'" + username.ToLower() + "\'");
            if (dt.Rows.Count > 0)
            {
                return new Account(Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString()), username);
            }
            return null;
        }

        public static List<Account> GetAccounts()
        {
            if (!DatabaseConnector.IsDatabaseConnectionAvailable()) return null;

            List<Account> accounts = new List<Account>();

            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT USERNAME FROM ACCOUNT");
            foreach (DataRow row in dt.Rows)
            {
                accounts.Add(GetAccountFromDatabase(row.ItemArray[0].ToString()));
            }

            return accounts;
        }

        private void ReadRechten()
        {
            if (!DatabaseConnector.IsDatabaseConnectionAvailable()) return;

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

            dt = DatabaseConnector.GetInstance().ExecuteQuery("select recht.id from recht join rechten_per_account on Rechten_Per_Account.recht_id = recht.id where account_id = " + this.id);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    this.rechten.Add(Recht.GetRechtFromId(Convert.ToInt32(row.ItemArray[0])));
                }
            }
        }

        public bool HasPermission(string permission)
        {
            for (int i = 0; i < groepen.Count; i++)
            {
                if (groepen[i].Contains(permission))
                {
                    return true;
                }
            }
            return false;
        }
    }
}