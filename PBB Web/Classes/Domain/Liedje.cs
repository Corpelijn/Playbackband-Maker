using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Domain
{
    public class Liedje
    {
        public int id;
        public byte[] data;
        public string artiest;
        public string titel;
        public Taal taal;

        public Liedje(int id, byte[] data, string artiest, string titel, Taal taal)
        {
            this.id = id;
            this.data = data;
            this.artiest = artiest;
            this.titel = titel;
            this.taal = taal;
        }
    }
}