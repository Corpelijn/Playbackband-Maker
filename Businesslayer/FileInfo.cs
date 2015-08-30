using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Businesslayer
{
    public class FileInfo
    {
        public static string[] GetInfo(string filename)
        {
            List<string> fileinfo = new List<string>();

            TagLib.File tagFile = TagLib.File.Create(filename);
            string artiest = tagFile.Tag.FirstPerformer;
            string titel = tagFile.Tag.Title;

            if (artiest == null)
            {
                // Controleer of de artiest staat aangegeven bij de titel
                if (titel != null)
                {
                    // Controleer of de titel gesplits kan worden
                    string[] title = titel.Split(new char[] {'-'});
                    if (title.Length > 1)
                    {
                        artiest = title[0].Trim();
                        titel = title[1].Trim();
                    }
                }
            }

            fileinfo.Add(artiest);
            fileinfo.Add(titel);
            fileinfo.Add(tagFile.Properties.Duration.TotalSeconds.ToString());

            return fileinfo.ToArray();
        }

        public static double GetDuration(string filename)
        {
            TagLib.File tagFile = TagLib.File.Create(filename);
            return tagFile.Properties.Duration.TotalSeconds;
        }
    }
}
