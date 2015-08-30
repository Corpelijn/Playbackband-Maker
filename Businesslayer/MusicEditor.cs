using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Businesslayer
{
    public static class MusicEditor
    {
        public static void TrimMp3(string inputPath, string outputPath, TimeSpan? begin, TimeSpan? end)
        {
            if (begin.HasValue && end.HasValue && begin > end)
                throw new ArgumentOutOfRangeException("end", "end should be greater than begin");

            using (var reader = new Mp3FileReader(inputPath))
            using (var writer = System.IO.File.Create(outputPath))
            {
                Mp3Frame frame;
                while ((frame = reader.ReadNextFrame()) != null)
                    if (reader.CurrentTime >= begin || !begin.HasValue)
                    {
                        if (reader.CurrentTime <= end || !end.HasValue)
                            writer.Write(frame.RawData, 0, frame.RawData.Length);
                        else break;
                    }
            }
        }

        public static void MixMp3(string inputPath1, string inputPath2, double LastFadeOutLength, string outputPath)
        {
            if (LastFadeOutLength == 0)
                LastFadeOutLength = 0.01;
            double seconds = FileInfo.GetDuration(inputPath1) - LastFadeOutLength;

            Process p = new Process();
            p.StartInfo.FileName = "cmd";
            //p.StartInfo.Arguments = "\"" + inputPath2 + "\" -p pad " + seconds.ToString().Replace(",",".") + " 0 | sox - -m \"" + inputPath1 + "\" \"" + outputPath + "\"";
            p.StartInfo.Arguments = " /C \"\"" + Environment.CurrentDirectory + "\\sox.exe\" \"" + inputPath2 + "\" -p pad " + seconds.ToString().Replace(",", ".") + " 0 | \"" + Environment.CurrentDirectory + "\\sox.exe\" - -m \"" + inputPath1 + "\" \"" + outputPath + "\"\"";

            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();

            p.WaitForExit();
            //sox "D:\Users\Bas\Desktop\trimmed\2.mp3" -p pad 52 0 | sox - -m "D:\Users\Bas\Desktop\trimmed\1.mp3" "D:\Users\Bas\Desktop\trimmed\output.mp3"
        }

        public static void MixMp3_2(string[] inputPaths, string outputPath, List<Fragment> liedjes)
        {
            //if (LastFadeOutLength == 0)
            //    LastFadeOutLength = 0.01;
            //double seconds = FileInfo.GetDuration(inputPath1) - LastFadeOutLength;
            List<double> startSilence = new List<double>();
            startSilence.Add(0);
            for (int i = 1; i < inputPaths.Length; i++)
            {
                Fragment frag = liedjes[i];
                startSilence.Add(FileInfo.GetDuration(inputPaths[i]) + startSilence[i-1] - (frag.FadeOut - new DateTime(2000,1,1)).TotalSeconds );
            }

            Process p = new Process();
            p.StartInfo.FileName = "cmd";
            
            //p.StartInfo.Arguments = " /C \"\"" + Environment.CurrentDirectory + "\\sox.exe\" \"" + inputPath2 + "\" -p pad " + seconds.ToString().Replace(",", ".") + " 0 | \"" + Environment.CurrentDirectory + "\\sox.exe\" - -m \"" + inputPath1 + "\" \"" + outputPath + "\"\"";

            for (int i = inputPaths.Length - 1; i > 0; i--)
            {
                p.StartInfo.Arguments += "\"" + Environment.CurrentDirectory + "\\sox.exe\" \"" + inputPaths[i] + "\" -p pad " + startSilence[i] + " 0 | ";
            }
            p.StartInfo.Arguments += Environment.CurrentDirectory + "\\sox\" - -m \"" + inputPaths[0] + "\" \"" + outputPath + "\"";

            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();

            p.WaitForExit();
            //sox "D:\Users\Bas\Desktop\trimmed\2.mp3" -p pad 52 0 | sox - -m "D:\Users\Bas\Desktop\trimmed\1.mp3" "D:\Users\Bas\Desktop\trimmed\output.mp3"
        }
    }
}
