using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Domain
{
    public class Taal
    {
        public int id;
        public string taal;
        public string afkorting;

        private static Dictionary<int, Taal> beschikbareTalen;

        public Taal(int id, string taal, string afkorting)
        {
            InitList();
            this.id = id;
            this.taal = taal;
            this.afkorting = afkorting;
        }

        private static void InitList()
        {
            if (beschikbareTalen == null)
            {
                beschikbareTalen = new Dictionary<int, Taal>();
            }
        }

        public static Taal GetTaalFromID(int id)
        {
            InitList();
            Taal taal = null;
            if (!beschikbareTalen.TryGetValue(id, out taal))
            {

                DataTable dt = Database.DatabaseConnector.GetInstance().ExecuteQuery("select * from taal where id=" + id);
                if (dt.Rows.Count > 0)
                {
                    taal = new Taal(id, (string)dt.Rows[0].ItemArray[1], (string)dt.Rows[0].ItemArray[2]);
                    beschikbareTalen.Add(id, taal);
                    return taal;
                }
                return new Taal(id, "Error: Id not found", "ERR:ID");
            }
            else
            {
                return taal == null ? new Taal(id, "Error: Null exception", "ERR:NULL") : taal;
            }
        }
    }
}