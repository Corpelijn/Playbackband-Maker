using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Domain
{
    public class Playbackband
    {
        public int id;
        public DateTime dag1;
        public DateTime dag2;
        private List<Blok> blokken;

        private bool valid;

        private static string[] roman = new string[] {"I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI" };

        public Playbackband(DateTime dag1, DateTime dag2, int blokkenAantal, bool intro, bool finale)
        {
            this.blokken = new List<Blok>();
            this.dag1 = dag1;
            this.dag2 = dag2;
            this.id = -1;
            this.valid = false;

            if (intro)
                this.AddBlok(new Blok(this, "INTRO", 1, 0));
            if (finale)
                this.AddBlok(new Blok(this, "FINALE", 1, 200));

            for (int i = 0; i < blokkenAantal; i++)
            {
                this.AddBlok(new Blok(this, "BLOK " + roman[i], 10, (i + 1) * 10));
            }
        }

        public Playbackband(int id, DateTime dag1, DateTime dag2)
        {
            this.blokken = new List<Blok>();
            this.dag1 = dag1;
            this.dag2 = dag2;
            this.id = id;
            this.valid = false;

            SessionClass.SetPlaybackband(this);
        }

        public void AddBlok(Blok blok)
        {
            blokken.Add(blok);
            blokken.Sort();
        }

        public static Playbackband Instance
        {
            get
            {
                return SessionClass.GetPlaybackband();
            }
        }

        public void SetValid()
        {
            valid = true;
        }

        public bool IsValid()
        {
            return valid;
        }

        public List<Blok> Blokken
        {
            get { return blokken; }
        }
    }
}