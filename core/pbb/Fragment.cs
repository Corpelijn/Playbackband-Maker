using System;
using System.Collections.Generic;
using System.Text;

namespace pbbcore.pbb
{
    class Fragment
    {
        private Liedje liedje;
        private double starttime;
        private double endtime;
        private double fadein;
        private double fadeout;

        public Fragment(string filename)
        {
            this.liedje = new Liedje(filename);
        }

        public double StartTime
        {
            get { return this.starttime; }
            set { this.starttime = value; }
        }

        public double EndTime
        {
            get { return this.endtime; }
            set { this.endtime = value; }
        }

        public double FadeIn
        {
            get { return this.fadein; }
            set { this.fadein = value; }
        }

        public double FadeOut
        {
            get { return this.fadeout; }
            set { this.fadeout = value; }
        }

        public void GenerateAudioFile(string filename)
        {

        }
    }
}
