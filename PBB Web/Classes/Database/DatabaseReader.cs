using PBB_Web.Classes.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Database
{
    public class DatabaseReader
    {
        public static void ReadDatabase()
        {
            CheckDatabaseConnection();

            DataTable dt = DatabaseConnector.Instance.ExecuteQuery("SELECT * FROM PLAYBACKBAND");
            new Playbackband((int)(decimal)dt.Rows[0].ItemArray[0], (DateTime)dt.Rows[0].ItemArray[1], (DateTime)dt.Rows[0].ItemArray[2]);

            dt = DatabaseConnector.Instance.ExecuteQuery("SELECT * FROM BLOK where playbackband_id = " + Playbackband.Instance.id);

            foreach (DataRow row in dt.Rows)
            {
                Playbackband.Instance.AddBlok(new Blok((int)(decimal)row[0], Playbackband.Instance, (string)row[2], (int)(decimal)row[3], (int)(decimal)row[4]));
            }

            foreach (Blok b in Playbackband.Instance.blokken)
            {
                dt = DatabaseConnector.Instance.ExecuteQuery("SELECT fragment.id, begintijd, eindtijd, fade_in, fade_out, blok_id, fragment.\"INDEX\", uitvoerwijze, rode_draad " + 
                    "FROM fragment join fragment_per_blok on fragment.id = fragment_per_blok.fragment_id where blok_id = " + b.id);
                foreach (DataRow row in dt.Rows)
                {
                    Fragment f = b.AddFragment(new Fragment((int)(decimal)row[0], (int)(decimal)row[6], b, row[7] == DBNull.Value ? "" : (string)row[7], (double)(decimal)row[1], (double)(decimal)row[2], (double)(decimal)row[3], (double)(decimal)row[4], (int)(decimal)row[8]));
                    dt = DatabaseConnector.Instance.ExecuteQuery("select liedje.id, data, artiest, titel, taal_id from liedje join fragment on liedje.id = liedje_id where fragment.id =" + f.id);
                    f.liedje = new Liedje((int)(decimal)dt.Rows[0].ItemArray[0], null, (string)dt.Rows[0].ItemArray[2], (string)dt.Rows[0].ItemArray[3], null);
                }
                

            }
        }

        public static bool ValidateCredentials(string username, string password)
        {
            CheckDatabaseConnection();

            DataTable dt = DatabaseConnector.Instance.ExecuteQuery("SELECT * FROM ACCOUNT WHERE USERNAME=\'" + username + "\' AND PASSWORD=\'" + password + "\'");
            return dt.Rows.Count == 0 ? false : true;
        }

        private static void CheckDatabaseConnection()
        {
            if (DatabaseConnector.Instance == null)
            {
                new DatabaseConnector();
                DatabaseConnector.Instance.OpenConnection("192.168.94.5", 1521, "xe", "PBB", "PBB");
            }
            else
            {
                if (!DatabaseConnector.Instance.IsConnected())
                {
                    new DatabaseConnector();
                    DatabaseConnector.Instance.OpenConnection("192.168.94.5", 1521, "xe", "PBB", "PBB");
                }
            }
        }
    }
}