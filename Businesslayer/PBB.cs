using System;
using System.Collections.Generic;
using System.Text;

namespace Businesslayer
{
    [Serializable]
    public class Playbackband
    {
        private List<Blok> blokken;
        private File file;

        public Playbackband(string filename)
        {
            blokken = new List<Blok>();
            file = new File(filename);
        }

        public string Filename
        {
            get { return file.Filename; }
            set { file.Filename = value; }
        }

        public List<Blok> Blokken
        {
            get { return blokken; }
            set { blokken = value; }
        }

        public void Save()
        {
            file.Save(this);
        }

        public void AddBlok(string name, int aantalNummers)
        {
            blokken.Add(new Blok(name, aantalNummers, (blokken.Count + 1)));
        }

        public Fragment GetFragmentFromUniversalID(int id)
        {
            int blok_id = 0;
            for (int i = 0; i < Blokken.Count; i++)
            {
                if (Blokken[i].AantalInBlok <= id)
                {
                    blok_id += 1;
                    id -= Blokken[i].AantalInBlok;
                }
            }

            return Blokken[blok_id].Fragmenten[id];
        }

        public void VerplaatsNummer(int oudNr, int newNr1, int newNr2)
        {
            SetNumbers();

            List<Fragment> fragmenten = new List<Fragment>();
            for (int i = 0; i < this.blokken.Count; i++)
            {
                for (int j = 0; j < this.blokken[i].Fragmenten.Count; j++)
                {
                    fragmenten.Add(this.blokken[i].Fragmenten[j]);
                }
            }

            fragmenten[oudNr - 1].Nummer = (int)(fragmenten[newNr2 - 1].Nummer + fragmenten[newNr1 - 1].Nummer) / 2;

            fragmenten.Sort();

            RegeneratePlaybackband(fragmenten);
        }

        public void VerwisselNummer(int oudNr, int newNr)
        {
            SetNumbers();

            List<Fragment> fragmenten = new List<Fragment>();
            for (int i = 0; i < this.blokken.Count; i++)
            {
                for (int j = 0; j < this.blokken[i].Fragmenten.Count; j++)
                {
                    fragmenten.Add(this.blokken[i].Fragmenten[j]);
                }
            }

            int oldID = fragmenten[oudNr - 1].Nummer;
            int newID = fragmenten[newNr - 1].Nummer;

            fragmenten[oudNr - 1].Nummer = newID;
            fragmenten[newNr - 1].Nummer = oldID;

            fragmenten.Sort();

            RegeneratePlaybackband(fragmenten);
        }

        public void SetNumbers()
        {
            for (int i = 0; i < blokken.Count; i++)
            {
                for (int j = 0; j < blokken[i].Fragmenten.Count; j++)
                {
                    // nummer = bloknr * 1000 + fragmentnr * 50
                    blokken[i].Fragmenten[j].Nummer = (i + 1) * 1000 + j * 50;
                }
            }
        }

        public void RegeneratePlaybackband(List<Fragment> input)
        {
            for (int i = 0; i < this.blokken.Count; i++)
            {
                this.blokken[i].Fragmenten = new List<Fragment>();
            }

            int blokID = 0;
            int aantalInCurrentBlok = 0;
            for (int i = 0; i < input.Count; i++)
            {
                this.blokken[blokID].Fragmenten.Add(input[i]);
                aantalInCurrentBlok++;
                if (aantalInCurrentBlok == this.blokken[blokID].AantalInBlok)
                {
                    aantalInCurrentBlok = 0;
                    blokID++;
                }
            }
        }

        public void CreateAutoSave(string filename)
        {
            this.Filename = filename;
            file.Save(this);
        }

        public void VerwijderFragment(Fragment fragment)
        {
            for (int i = 0; i < blokken.Count; i++)
            {
                for (int j = 0; j < blokken[i].Fragmenten.Count; j++)
                {
                    if (blokken[i].Fragmenten[j] == fragment)
                    {
                        blokken[i].Fragmenten[j] = new Fragment(fragment.Nummer);
                    }
                }
            }
        }

    }
}
