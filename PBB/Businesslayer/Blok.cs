using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBB.Businesslayer
{
    [Serializable]
    public class Blok
    {
        #region "Attributes"

        private List<Fragment> fragmenten;
        private string beschrijving;
        private int aantalInBlok;
        private int nummer;

        private Playbackband parent;

        #endregion

        #region "Constructors"

        public Blok(string beschrijving, int aantalInBlok, int nummer, Playbackband parent)
        {
            this.fragmenten = new List<Fragment>();
            this.beschrijving = beschrijving;
            this.aantalInBlok = aantalInBlok;
            this.nummer = nummer;
            this.parent = parent;

            for (int i = 0; i < aantalInBlok; i++)
            {
                fragmenten.Add(new Fragment((nummer * 1000) + 50 * i));
            }
        }

        #endregion

        #region "Properties"

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

        public Playbackband Parent
        {
            get { return this.parent; }
        }

        #endregion

        #region "Methods"

        public void AddFragment()
        {
            
        }

        public Fragment GetFragment(int index)
        {
            return fragmenten[index];
        }

        public Fragment[] GetFragmenten()
        {
            return fragmenten.ToArray();
        }

        public int GetFragmentCount()
        {
            return fragmenten.Count;
        }

        #endregion
    }
}
