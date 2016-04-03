using PBB_Web.Classes.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Database
{
    public class NamedQueries
    {
        public static List<Recht> GetAlleRechten()
        {
            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("select id, beschrijving, code from recht");
            List<Recht> rechten = new List<Recht>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    rechten.Add(new Recht(Convert.ToInt32(row.ItemArray[0].ToString()), row.ItemArray[1].ToString(), row.ItemArray[2].ToString()));
                }
            }

            return rechten;
        }

        public static List<Taal> GetAlleTalen()
        {
            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("select id, taal, afkorting from taal");
            List<Taal> talen = new List<Taal>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    talen.Add(new Taal(Convert.ToInt32(row.ItemArray[0].ToString()), row.ItemArray[1].ToString(), row.ItemArray[2].ToString()));
                }
            }

            return talen;
        }

        public static List<Playbackband> GetAllePlaybackband()
        {
            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("select id, dag1, dag2 from playbackband");
            List<Playbackband> rechten = new List<Playbackband>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    rechten.Add(new Playbackband(Convert.ToInt32(row.ItemArray[0].ToString()), Convert.ToDateTime(row.ItemArray[1].ToString()), Convert.ToDateTime(row.ItemArray[2].ToString())));
                }
            }

            return rechten;
        }
    }
}