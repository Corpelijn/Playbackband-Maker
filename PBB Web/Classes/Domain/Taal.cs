using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Domain
{
    public class Taal
    {
        public int id;
        public string taal;
        public string afkorting;

        public Taal(int id, string taal, string afkorting)
        {
            this.id = id;
            this.taal = taal;
            this.afkorting = afkorting;
        }
    }
}