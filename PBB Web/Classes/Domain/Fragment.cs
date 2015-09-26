using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Domain
{
    public class Fragment
    {
        public int id;
        public int index;
        public Blok parent;
        public Liedje liedje;
        public string uitvoerwijze;
        public double begintijd;
        public double eindtijd;
        public double fadein;
        public double fadeout;
        public bool fadeinbinnen;
        public bool fadeoutbinnen;
        public bool rodedraad;

        public string verlichting;
        public string opkomst_afgaan;

        public Fragment(int id, int index, Blok parent, string uitvoerwijze, double begintijd, double eindtijd, double fadein, double fadeout, int rodedraad)
        {
            this.id = id;
            this.index= index;
            this.parent = parent;
            this.uitvoerwijze = uitvoerwijze;
            this.begintijd = begintijd;
            this.eindtijd = eindtijd;
            this.fadein = fadein;
            this.fadeout = fadeout;
            this.rodedraad = rodedraad == 0 ? false : true;
        }
    }
}