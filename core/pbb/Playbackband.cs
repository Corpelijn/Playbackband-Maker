using System;
using System.Collections.Generic;
using System.Text;

namespace pbbcore.pbb
{
    class Playbackband
    {
        private List<Blok> blokken;

        public Playbackband()
        {
            this.blokken = new List<Blok>();
        }

        public void AddBlok(string name)
        {
            blokken.Add(new Blok(name));
        }
    }
}
