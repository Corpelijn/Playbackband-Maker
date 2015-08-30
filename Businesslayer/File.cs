using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Businesslayer
{
    [Serializable]
    public class File
    {
        private string filename;
        public static string defaultDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\playbackband\\playbackband" + Process.GetCurrentProcess().Id.ToString();
        public static string mainDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\playbackband";

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public File(string filename)
        {
            this.filename = filename;
        }

        public void Save(Playbackband playbackband)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(this.filename, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, playbackband);
            stream.Close();   
        }

        public Playbackband Open()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(this.filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            Playbackband obj = (Playbackband)formatter.Deserialize(stream);
            stream.Close();

            return obj;
        }

    }
}
