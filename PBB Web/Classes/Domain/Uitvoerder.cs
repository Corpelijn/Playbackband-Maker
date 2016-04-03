using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Domain
{
    public class Uitvoerder
    {
        private int id;
        private string voornaam;
        private string achternaam;

        public Uitvoerder(int id, string voornaam, string achternaam)
        {
            this.id = id;
            this.voornaam = voornaam;
            this.achternaam = achternaam;
        }

        public override string ToString()
        {
            return this.voornaam;
        }
    }
}