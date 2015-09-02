using System;
using System.Collections.Generic;
using System.Text;

namespace Businesslayer
{
    [Serializable]
    public class Fragment : IComparable
    {
        private Liedje liedje;
        private DateTime begintijd;
        private DateTime eindtijd;
        private DateTime fadein;
        private DateTime fadeout;
        private bool FIBinnen;
        private bool FOBinnen;
        private int nummer;
        private int rodeDraad;

        public Fragment(int dummynr)
        {
            liedje = new Liedje();
            begintijd = new DateTime(1990, 1, 1, 0, 0, 0);
            eindtijd = new DateTime(1990, 1, 1, 0, 1, 0);
            fadein = new DateTime(1990, 1, 1, 0, 0, 0);
            fadeout = new DateTime(1990, 1, 1, 0, 0, 0);
            FIBinnen = false;
            FOBinnen = false;
            nummer = dummynr;
        }

        public Liedje Liedje
        {
            get { return liedje; }
            set { liedje = value; }
        }

        public DateTime BeginTijd
        {
            get { return begintijd; }
            set { begintijd = value; }
        }

        public DateTime EindTijd
        {
            get { return eindtijd; }
            set { eindtijd = value; }
        }

        public DateTime FadeIn
        {
            get { return fadein; }
            set { fadein = value; }
        }

        public DateTime FadeOut
        {
            get { return fadeout; }
            set { fadeout = value; }
        }

        public bool FadeInBinnen
        {
            get { return FIBinnen; }
            set { FIBinnen = value; }
        }

        public bool FadeOutBinnen
        {
            get { return FOBinnen; }
            set { FOBinnen = value; }
        }

        public int Nummer
        {
            get { return nummer; }
            set { nummer = value; }
        }

        public int RodeDraad
        {
            get { return rodeDraad; }
            set { rodeDraad = value; }
        }

        public double StartTijd
        {
            get
            {
                return (this.begintijd - new DateTime(2000, 1, 1)).TotalSeconds - (FIBinnen ? 0 : (this.fadein - new DateTime(2000, 1, 1)).TotalSeconds);// * 2);
            }
        }

        public double StopTijd
        {
            get
            {
                double ret_val = this.liedje.Lengte - (this.eindtijd - new DateTime(2000, 1, 1)).TotalSeconds - (!FOBinnen ? (this.fadeout - new DateTime(2000, 1, 1)).TotalSeconds : 0);// -0.5;
                return (ret_val < 0 ? 0 : ret_val);
            }
        }

        public TimeSpan Duration
        {
            get
            {
                // duration of the song without fades
                TimeSpan d = this.eindtijd - this.begintijd;
                if (!this.FIBinnen)
                    d = d.Add(TimeSpan.FromSeconds((this.fadein - new DateTime(2000, 1, 1)).TotalSeconds));
                if (!this.FOBinnen)
                    d = d.Add(TimeSpan.FromSeconds((this.fadeout - new DateTime(2000, 1, 1)).TotalSeconds));
                return d;
                //return TimeSpan.FromSeconds((this.FOBinnen ? (this.eindtijd - new DateTime(2000, 1, 1)).TotalSeconds : (this.eindtijd - new DateTime(2000, 1, 1)).TotalSeconds + (this.fadeout - new DateTime(2000, 1, 1)).TotalSeconds) - (this.FIBinnen ? (this.begintijd - new DateTime(2000, 1, 1)).TotalSeconds : (this.begintijd - new DateTime(2000, 1, 1)).TotalSeconds + (this.fadein - new DateTime(2000, 1, 1)).TotalSeconds));
            }
        }

        public bool IsDummy()
        {
            if (liedje.Artiest == "" || liedje.Titel == "")
            {
                return true;
            }

            return false;
        }

        public int CompareTo(object obj)
        {
            Fragment f = (Fragment)obj;

            if (f.nummer == this.nummer)
                return 0;
            else if (f.nummer < this.nummer)
                return 1;
            else
                return -1;
        }
    }
}
