using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Businesslayer
{
    [Serializable]
    public class Liedje
    {
        private MemoryStream filecontent;
        private string artiest;
        private string titel;
        private double lengte;

        public Liedje()
        {
            artiest = "";
            titel = "";
            lengte = 0;
        }

        public Liedje(string artiest, string titel, double lengte, string filename)
        {
            this.artiest = artiest;
            this.titel = titel;
            this.lengte = lengte;

            filecontent = new MemoryStream();
            System.IO.FileStream fs = new FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
            fs.CopyTo(filecontent);
            fs.Close();
            fs.Dispose();
        }

        public string Artiest
        {
            get { return artiest; }
            set { artiest = value; }
        }

        public double Lengte
        {
            get { return lengte; }
            set { lengte = value; }
        }

        public string Titel
        {
            get { return titel; }
            set { titel = value; }
        }

        public MemoryStream FileContent
        {
            get { return this.filecontent; }
        }

        public override string ToString()
        {
            if (artiest == "" || titel == "")
                return "";
            else
                return Artiest + " / " + Titel;
        }

        public string WriteToFile(string filename )
        {
            if (filecontent == null)
                return "";

            if (!System.IO.Directory.Exists(File.defaultDir))
            {
                System.IO.Directory.CreateDirectory(File.defaultDir);
            }

            if(filename == "")
                filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\playbackband\\" + this.artiest + this.titel + ".mp3";

            FileStream fs = new FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);

            filecontent.WriteTo(fs);

            fs.Close();
            fs.Dispose();

            return filename;
        }

    }
}
