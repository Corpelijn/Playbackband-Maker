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
        public string code;

        public Recht(int id, string description, string code)
        {
            this.id = id;
            this.description = description;
            this.code = code;
        }

        public static Recht GetRechtFromId(int id)
        {
            if (!DatabaseConnector.IsDatabaseConnectionAvailable()) return null;

            DataTable dt = DatabaseConnector.GetInstance().ExecuteQuery("SELECT BESCHRIJVING, CODE FROM RECHT WHERE ID=" + id);
            if (dt.Rows.Count > 0)
            {
                return new Recht(id, dt.Rows[0].ItemArray[0].ToString(), dt.Rows[0].ItemArray[1].ToString());
            }
            return null;
        }

        public override string ToString()
        {
            return description;
        }
    }
}