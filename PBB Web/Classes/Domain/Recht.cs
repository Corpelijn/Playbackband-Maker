using PBB_Web.Classes.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Domain
{
    public class Recht
    {
        public int id;
        public string description;

        public Recht(int id, string description)
        {
            this.id = id;
            this.description = description;
        }

        public static Recht GetRechtFromId(int id)
        {
            DatabaseReader.CheckDatabaseConnection();

            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT BESCHRIJVING FROM RECHT WHERE ID=" + id);
            if (dt.Rows.Count > 0)
            {
                return new Recht(id, dt.Rows[0].ItemArray[0].ToString());
            }
            return null;
        }

        public override string ToString()
        {
            return description;
        }
    }
}