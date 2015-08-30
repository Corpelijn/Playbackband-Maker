using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Businesslayer
{
    public class WAVInputFile
    {
        private string filename;
        private double fadeTime;

        public WAVInputFile(string filename, double fadeTime)
        {
            this.filename = filename;
            this.fadeTime = fadeTime;
        }

        public string Filename
        {
            get { return this.filename; }
            set { this.filename = value; }
        }

        public double FadeTime
        {
            get { return this.fadeTime; }
            set { this.fadeTime = value; }
        }

        public override string ToString()
        {
            return filename;
        }
    }
}
