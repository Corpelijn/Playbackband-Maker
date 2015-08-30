using System;
using System.Collections.Generic;
using System.Text;

namespace Businesslayer
{
    [Serializable]
    public class Blok
    {
        private List<Fragment> fragmenten;
        private string beschrijving;
        private int aantalInBlok;
        private int nummer;

        public Blok(string beschrijving, int aantalInBlok, int nummer)
        {
            this.fragmenten = new List<Fragment>();
            this.beschrijving = beschrijving;
            this.aantalInBlok = aantalInBlok;
            this.nummer = nummer;

            for (int i = 0; i < aantalInBlok; i++)
            {
                fragmenten.Add(new Fragment((nummer * 1000) + 50 * i));
            }
        }

        public List<Fragment> Fragmenten
        {
            get { return fragmenten; }
            set { fragmenten = value; }
        }

        public string Beschrijving
        {
            get { return beschrijving; }
            set { beschrijving = value; }
        }

        public int AantalInBlok
        {
            get { return aantalInBlok; }
            set { aantalInBlok = value; }
        }

        public int Nummer
        {
            get { return nummer; }
            set { nummer = value; }
        }
    }
}
