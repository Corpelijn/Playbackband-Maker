using PBB_Web.Classes.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Domain
{
    public class GroepRecht
    {
        public int id;
        public string naam;
        public string code;
        public List<Recht> rechten;

        public GroepRecht(int id, string naam, string code)
        {
            rechten = new List<Recht>();
            this.code = code;
            this.naam = naam;
        }

        public static GroepRecht GetGroepFromId(int id)
        {
            if (!DatabaseConnector.IsDatabaseConnectionAvailable()) return null;

            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT NAAM, CODE FROM GROEP WHERE ID=" + id);
            if (dt.Rows.Count > 0)
            {
                GroepRecht gr = new GroepRecht(id, (string)dt.Rows[0].ItemArray[0], dt.Rows[0].ItemArray[1].ToString());
                dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT RECHT_ID FROM RECHT_PER_GROEP WHERE GROEP_ID=" + id);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        gr.rechten.Add(Recht.GetRechtFromId(Convert.ToInt32(row.ItemArray[0].ToString())));
                    }
                }
                return gr;
            }
            return null;
        }

        public bool Contains(string permission)
        {
            for (int i = 0; i < rechten.Count; i++)
            {
                if (rechten[i].ToString() == permission)
                {
                    return true;
                }
            }

            return false;
        }
    }
}