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
        public List<Blok> blokken;

        public Playbackband(int id, DateTime dag1, DateTime dag2)
        {
            this.blokken = new List<Blok>();
            Playbackband.Instance = this;
            this.dag1 = dag1;
            this.dag2 = dag2;
            this.id = id;
        }

        public static Playbackband Instance { get; private set; }

        public void AddBlok(Blok blok)
        {
            blokken.Add(blok);
        }
    }
}