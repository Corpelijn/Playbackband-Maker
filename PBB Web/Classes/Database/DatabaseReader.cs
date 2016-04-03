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
        public static void ReadPlaybackbandFromDatabase(int id)
        {
            if (!DatabaseConnector.IsDatabaseConnectionAvailable()) return;

            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT * FROM PLAYBACKBAND WHERE ID = " + id.ToString());
            new Playbackband((int)(decimal)dt.Rows[0].ItemArray[0], (DateTime)dt.Rows[0].ItemArray[1], (DateTime)dt.Rows[0].ItemArray[2]);

            dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT * FROM BLOK where playbackband_id = " + id.ToString());

            foreach (DataRow row in dt.Rows)
            {
                Playbackband.Instance.AddBlok(new Blok((int)(decimal)row[0], Playbackband.Instance, (string)row[2], (int)(decimal)row[3], (int)(decimal)row[4]));
            }

            foreach (Blok b in Playbackband.Instance.Blokken)
            {
                dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT fragment.id, begintijd, eindtijd, fade_in, fade_out, blok_id, fragment.positie_index, uitvoerwijze, rode_draad " +
                    "FROM fragment join fragment_per_blok on fragment.id = fragment_per_blok.fragment_id where blok_id = " + b.id);
                foreach (DataRow row in dt.Rows)
                {
                    Fragment f = b.AddFragment(new Fragment((int)(decimal)row[0], (int)(decimal)row[6], b, row[7] == DBNull.Value ? "" : (string)row[7], (double)(decimal)row[1], (double)(decimal)row[2], (double)(decimal)row[3], (double)(decimal)row[4], (int)(decimal)row[8]));
                    dt = DatabaseConnector.GetInstance().ExecuteQuery("select liedje.id, data, artiest, titel, taal_id from liedje join fragment on liedje.id = liedje_id where fragment.id =" + f.id);
                    f.liedje = new Liedje((int)(decimal)dt.Rows[0].ItemArray[0], null, (string)dt.Rows[0].ItemArray[2], (string)dt.Rows[0].ItemArray[3], Taal.GetTaalFromID((int)(decimal)dt.Rows[0].ItemArray[4]));
                }
            }

            SessionClass.GetPlaybackband().SetValid();
        }

        //public static bool CheckDatabaseConnection()
        //{
            //if (DatabaseConnector.GetInstance() == null)
            //{
            //    new DatabaseConnector();
            //    DatabaseConnector.GetInstance().OpenConnection("192.168.94.5", 1521, "xe", "PBB", "PBB");
            //}
            //else
            //{
            //    if (!DatabaseConnector.GetInstance().IsConnected())
            //    {
            //        new DatabaseConnector();
            //        DatabaseConnector.GetInstance().OpenConnection("192.168.94.5", 1521, "xe", "PBB", "PBB");
            //    }
            //}
        //}
    }
}