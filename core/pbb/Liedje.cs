using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace pbbcore.pbb
{
    class Liedje
    {
        private string title;
        private string artist;
        private byte[] filecontent;

        public Liedje(string filename)
        {
        }

        public Liedje(string title, string artist, byte[] filecontent)
        {
            this.title = title;
            this.artist = artist;
            this.filecontent = filecontent;
        }

        public Image GenerateImage()
        {
            return new Bitmap(1, 1);
        }
    }
}
