using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NAudio.Wave;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using NAudio.Wave.SampleProviders;
using NAudio.Lame;

namespace Businesslayer
{
    public class PBBRenderer
    {
        public static PBBRenderer Instance { get; set; }
        private string workingDir;
        private bool stopThreads;
        private bool continueTillDone;

        public int ActiveStatus = 0;

        private List<Fragment> fragmenten;
        private List<string> uneditedWAV;
        private List<string> trimmedWAV;
        private List<string> fadeInOutWAV;
        private List<string> normalizedWAV;
        private int cutMP3;
        private int totalFragments;

        private string outputDir;
        private bool multi;

        private int fragmentenDone;
        private List<string> WAVFiles;
        private int WAVFileDone;
        private List<string> trimmedFiles;
        private int trimmedDone;

        // Create a new object to render the output data
        public PBBRenderer(string workingDirectory, string filename, bool multifile)
        {
            this.workingDir = workingDirectory;
            this.fragmenten = new List<Fragment>();
            this.uneditedWAV = new List<string>();
            this.trimmedWAV = new List<string>();
            this.fadeInOutWAV = new List<string>();
            this.normalizedWAV = new List<string>();
            this.cutMP3 = 0;
            this.totalFragments = 0;
            this.outputDir = filename;
            this.multi = multifile;

            this.WAVFiles = new List<string>();
            this.trimmedFiles = new List<string>();
        }

        /// <summary>
        /// Start running the renderer to export the audio
        /// </summary>
        public void Start()
        {
            stopThreads = false;
            continueTillDone = false;
            this.fragmentenDone = 0;
            this.WAVFileDone = 0;
            this.trimmedDone = 0;
            new System.Threading.Thread(ConvertMP3ToWAV).Start();
            new System.Threading.Thread(TrimWAVFiles).Start();
            new System.Threading.Thread(NormalizeWAVFiles).Start();
            new System.Threading.Thread(FadeInOutWAVFiles).Start();
            new System.Threading.Thread(MixWAVFiles).Start();
            new System.Threading.Thread(ConvertWAVToMP3AndNormalize).Start();
            new System.Threading.Thread(CutMP3).Start();
            //new System.Threading.Thread(NormalizeVolume).Start();
            //new System.Threading.Thread(MixFiles).Start();
            OnUpdated();
        }

        /// <summary>
        /// Stops all the export threads immediately
        /// </summary>
        public void Stop()
        {
            stopThreads = true;
            continueTillDone = false;
        }

        /// <summary>
        /// Stops all the export threads after all they are done
        /// </summary>
        public void StopWhenDone()
        {
            stopThreads = true;
            continueTillDone = true;
        }

        /// <summary>
        /// Add a fragment object to export
        /// </summary>
        /// <param name="fragment"></param>
        public void AddFragment(Fragment fragment)
        {
            if (this.fragmenten.Contains(fragment))
                return;

            lock (this.fragmenten)
            {
                this.fragmenten.Add(fragment);
            }

            if(fragment.Liedje.FileContent != null)
                this.totalFragments++;
        }

        private void ConvertMP3ToWAV()
        {
            this.CreateDirectoryIfNotExists(workingDir + "\\export\\mp3");
            this.CreateDirectoryIfNotExists(workingDir + "\\export\\wav");

            int convertedFiles = 0;

            while (true)
            {
                // Check if all threads need to be stopped
                if (StopCurrentThread()) break;

                // Convert the files to MP3
                List<Fragment> fragments = null;
                lock (this.fragmenten)
                {
                    fragments = new List<Fragment>(fragmenten);
                }

                for (int i = convertedFiles; i < fragments.Count; i++)
                {
                    string MP3 = workingDir + "\\export\\mp3\\" + (convertedFiles + 1).ToString() + ".mp3";
                    string WAV = workingDir + "\\export\\wav\\" + (convertedFiles + 1).ToString() + ".wav";

                    fragments[i].Liedje.WriteToFile(MP3);

                    try
                    {
                        ConvertMP3ToWaveFile(MP3, WAV);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("convert: " + ex.Message);
                    }

                    if (fragments[i].IsDummy())
                        continue;

                    lock(uneditedWAV)
                        uneditedWAV.Add(WAV);

                    convertedFiles++;
                    OnUpdated();
                }

                System.Threading.Thread.Sleep(1);
            }
        }

        private void TrimWAVFiles()
        {
            this.CreateDirectoryIfNotExists(workingDir + "\\export\\trim");

            int convertedFiles = 0;

            while (true)
            {
                // Check if all threads need to be stopped
                if (StopCurrentThread()) break;

                // Trim the WAV files
                List<string> unedited = null;
                lock (this.uneditedWAV)
                {
                    unedited = new List<string>(this.uneditedWAV);
                }

                for (int i = convertedFiles; i < unedited.Count; i++)
                {
                    string outputFile = workingDir + "\\export\\trim\\" + Path.GetFileNameWithoutExtension(unedited[i]) +".wav";
                    try
                    {
                        TrimWavFile(
                            unedited[i],
                            outputFile,
                            TimeSpan.FromSeconds(fragmenten[convertedFiles].StartTijd),
                            TimeSpan.FromSeconds(fragmenten[convertedFiles].StopTijd)
                            );
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("trim: " + ex.Message);
                    }

                    lock(trimmedWAV)
                        trimmedWAV.Add(outputFile);

                    convertedFiles++;
                    OnUpdated();
                }

                System.Threading.Thread.Sleep(1);
            }
        }

        private void NormalizeWAVFiles()
        {
            this.CreateDirectoryIfNotExists(workingDir + "\\export\\normal");

            int convertedFiles = 0;

            while (true)
            {
                // Check if all threads need to be stopped
                if (StopCurrentThread()) break;

                // Mix the WAV files together
                List<string> trim = null;
                lock (this.trimmedWAV)
                {
                    trim = new List<string>(trimmedWAV);
                }

                for (int i = convertedFiles; i < trim.Count; i++)
                {
                    string outputFile = workingDir + "\\export\\normal\\" + Path.GetFileNameWithoutExtension(trim[i]) + ".wav";
                    string inputFile = trim[i];

                    try
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = "sox.exe";
                        p.StartInfo.Arguments = "--rate 44100 --norm \"" + inputFile + "\" \"" + outputFile + "\"";
                        //"/r /c \"" + inputFile + "\""));

                        p.StartInfo.RedirectStandardError = true;
                        p.StartInfo.UseShellExecute = false;

                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        p.Start();
                        p.WaitForExit();

                        Console.WriteLine("SOX Output: ");
                        Console.WriteLine(p.StandardError.ReadToEnd());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("normalize: " + ex.Message);
                    }

                    //System.IO.File.Copy(inputFile, outputFile);

                    lock(normalizedWAV)
                        normalizedWAV.Add(outputFile);

                    convertedFiles++;
                    OnUpdated();
                }

                System.Threading.Thread.Sleep(1);
            }
        }

        private void FadeInOutWAVFiles()
        {
            this.CreateDirectoryIfNotExists(workingDir + "\\export\\fade");

            int convertedFiles = 0;

            while (true)
            {
                // Check if all threads need to be stopped
                if (StopCurrentThread()) break;

                // Mix the WAV files together
                List<string> fades = null;
                lock (this.normalizedWAV)
                {
                    fades = new List<string>(normalizedWAV);
                }

                for (int i = convertedFiles; i < fades.Count; i++)
                {
                    string outputFile = workingDir + "\\export\\fade\\" + (convertedFiles + 1).ToString() + ".wav";
                    string inputFile = fades[i];

                    try
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = "sox.exe";
                        p.StartInfo.Arguments = "\"" + inputFile + "\" -t wav \"" + outputFile + "\" fade h " + (fragmenten[i].FadeIn - new DateTime(2000, 1, 1)).ToString("G").Replace(",", ".").Remove(0, 2) + " 0 " + (fragmenten[i].FadeOut - new DateTime(2000, 1, 1)).ToString("G").Replace(",", ".").Remove(0, 2);
                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        p.Start();
                        p.WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("fade: " + ex.Message);
                    }

                    //System.IO.File.Copy(fades[i], outputFile);

                    lock(fadeInOutWAV)
                        fadeInOutWAV.Add(outputFile);

                    convertedFiles++;
                    OnUpdated();
                }

                System.Threading.Thread.Sleep(1);
            }
        }

        private void MixWAVFiles()
        {
            this.CreateDirectoryIfNotExists(workingDir + "\\export\\total");

            while (true)
            {
                // Check if all threads need to be stopped
                if (StopCurrentThread()) return;

                if (fadeInOutWAV.Count == this.totalFragments && totalFragments > 0)
                {
                    break;
                }

                System.Threading.Thread.Sleep(1);
            }

            ActiveStatus = 1;

            List<WAVInputFile> files = new List<WAVInputFile>();
            for (int i = 0; i < fadeInOutWAV.Count; i++)
            {
                files.Add(new WAVInputFile(this.fadeInOutWAV[i], (fragmenten[i].FadeOut - new DateTime(2000, 1, 1)).TotalSeconds));
            }

            OnUpdated();

            try
            {
                WAVFile.MixAudioFiles(files.ToArray(), workingDir + "\\export\\total\\output.wav", workingDir + "\\export\\total\\temp");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            ActiveStatus = 3;
            OnUpdated();
        }

        private void ConvertWAVToMP3AndNormalize()
        {
            OnUpdated();
            this.CreateDirectoryIfNotExists(workingDir + "\\export\\totalmp3");

            while (true)
            {
                // Check if all threads need to be stopped
                if (StopCurrentThread()) return;

                if (ActiveStatus == 3)
                    break;

                System.Threading.Thread.Sleep(1);
            }

            string inputFile = workingDir + "\\export\\total\\output.wav";
            string outputFile = workingDir + "\\export\\totalmp3\\output.mp3";

            this.WaveToMP3(inputFile, outputFile);

            Process p = new Process();
            p.StartInfo.FileName = "mp3gain.exe";
            p.StartInfo.Arguments = "/r /c \"" + outputFile + "\"";
            //"/r /c \"" + inputFile + "\""));
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();

            ActiveStatus = 4;
            OnUpdated();
            //MessageBox.Show("De playbackband is gemixed en klaar", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CutMP3()
        {
            while (true)
            {
                // Check if all threads need to be stopped
                if (StopCurrentThread()) return;

                if (ActiveStatus == 4)
                    break;

                System.Threading.Thread.Sleep(1);
            }

            TimeSpan totalTime = new TimeSpan();
            Mp3Frame frame = null;


            Mp3FileReader reader = new Mp3FileReader(workingDir + "\\export\\totalmp3\\output.mp3");
            if (multi)
            {
                this.CreateDirectoryIfNotExists(outputDir);

                for (int i = 0; i < fragmenten.Count; i++)
                {
                    if (fragmenten[i].Liedje.FileContent == null)
                    {
                        continue;
                    }

                    // Create a frame stream to write to
                    FileStream file = new FileStream(outputDir + "\\" + (i + 1).ToString("00") + " - " + fragmenten[i].Liedje.Artiest + " - " + fragmenten[i].Liedje.Titel + ".mp3", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                    // Write the last frame of the previous song if it is available
                    if (frame != null)
                    {
                        file.Write(frame.RawData, 0, frame.RawData.Length);
                    }

                    // 
                    TimeSpan d = fragmenten[i].EindTijd - fragmenten[i].BeginTijd;
                    if (!fragmenten[i].FadeInBinnen)
                        d = d.Add(TimeSpan.FromSeconds((fragmenten[i].FadeIn - new DateTime(2000, 1, 1)).TotalSeconds));

                    // Continue as long as there are frames available
                    while ((frame = reader.ReadNextFrame()) != null)
                    {
                        if (reader.CurrentTime.TotalSeconds - totalTime.TotalSeconds < d.TotalSeconds)
                        {
                            file.Write(frame.RawData, 0, frame.RawData.Length);
                            frame = null;
                        }
                        else
                            break;
                    }

                    totalTime = totalTime.Add(d);

                    this.cutMP3++;
                }
            }
            else
            {
                cutMP3 = totalFragments;
                System.IO.File.Copy(workingDir + "\\export\\totalmp3\\output.mp3", outputDir, true);
            }

            ActiveStatus = 5;
            OnUpdated();

            
            ActiveStatus = 6;
            OnUpdated();
            MessageBox.Show("De playbackband is klaar!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool StopCurrentThread()
        {
            return (stopThreads && !continueTillDone);
        }

        /// <summary>
        /// Converting the mp3 data to wav data
        /// </summary>
        /// <param name="mp3Data"></param>
        /// <returns></returns>
        private void ConvertMP3ToWaveFile(string inputFile, string outputFile)
        {
            if (!System.IO.File.Exists(inputFile))
            {
                return;
            }

            using (Mp3FileReader reader = new Mp3FileReader(inputFile))
            {
                using (WaveStream convertedStream = WaveFormatConversionStream.CreatePcmStream(reader))
                {
                    WaveFileWriter.CreateWaveFile(outputFile, convertedStream);
                }
            }

            return;
        }

        public void WaveToMP3(string waveFileName, string mp3FileName, int bitRate = 128)
        {
            using (var reader = new WaveFileReader(waveFileName))
            using (var writer = new LameMP3FileWriter(mp3FileName, reader.WaveFormat, bitRate))
                reader.CopyTo(writer);
        }

        /// <summary>
        /// Trim the wav data to the right length
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void TrimWavFile(string input, string output, TimeSpan start, TimeSpan end)
        {
            using (WaveFileReader reader = new WaveFileReader(input))
            {
                using (WaveFileWriter writer = new WaveFileWriter(output, reader.WaveFormat))
                {
                    int segement = reader.WaveFormat.AverageBytesPerSecond / 1000;

                    int startPosition = (int)start.TotalMilliseconds * segement;
                    startPosition = startPosition - startPosition % reader.WaveFormat.BlockAlign;

                    int endBytes = (int)end.TotalMilliseconds * segement;
                    endBytes = endBytes - endBytes % reader.WaveFormat.BlockAlign;
                    int endPosition = (int)reader.Length - endBytes;

                    try
                    {
                        TrimWavFile(reader, writer, startPosition, endPosition);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("trim2: " + ex.Message);
                        MessageBox.Show(start.ToString() + "\n\n" + end.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Actually trimming the wav data
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="writer"></param>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        private void TrimWavFile(WaveFileReader reader, WaveFileWriter writer, int startPosition, int endPosition)
        {
            reader.Position = startPosition;
            byte[] buffer = new byte[1024];
            while (reader.Position < endPosition)
            {
                int segment = (int)(endPosition - reader.Position);
                if (segment > 0)
                {
                    int bytesToRead = Math.Min(segment, buffer.Length);
                    int bytesRead = reader.Read(buffer, 0, bytesToRead);
                    if (bytesRead > 0)
                    {
                        writer.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        /// <summary>
        /// Return the total fragment actions
        /// </summary>
        public int TotalFragmentsActions
        {
            get
            {
                return totalFragments * 5;  // convert, trim, normalize, fade, cut
            }
        }

        /// <summary>
        /// Return the fragments done
        /// </summary>
        public int FragmentActionsDone
        {
            get
            {
                return this.uneditedWAV.Count + this.trimmedWAV.Count + this.fadeInOutWAV.Count + this.normalizedWAV.Count + this.cutMP3;
            }                                   
        }                                       

        public event UpdatedEventHandler Updated;
        public delegate void UpdatedEventHandler();
        private void OnUpdated()
        {
            if (this.fragmentenDone + this.WAVFileDone + this.trimmedDone == this.TotalFragmentsActions)
            {
                this.continueTillDone = false;
            }

            if (Updated != null)
            {
                Updated();
            }
        }

        /// <summary>
        /// Creates a new temoprary directory
        /// </summary>
        /// <param name="directory"></param>
        private void CreateDirectoryIfNotExists(string directory)
        {
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
        }
    }
}
