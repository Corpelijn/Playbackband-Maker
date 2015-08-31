using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PBB.Businesslayer
{
    [Serializable]
    public class File
    {
        #region "Attributes"

        private string filename;

        #endregion

        #region "Static Attributes"

        public static string defaultDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\playbackband\\playbackband" + Process.GetCurrentProcess().Id.ToString();
        public static string mainDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\playbackband";

        #endregion

        #region "Constructors"
        #endregion

        #region "Properties"

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public File(string filename)
        {
            this.filename = filename;
        }

        #endregion

        #region "Methods"

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

        #endregion
    }
}
