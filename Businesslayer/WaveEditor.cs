using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Businesslayer
{
    public class WaveFile
    {
        int FileSize; // 1
        string Format; // 2
        int FmtChunkSize; // 3
        int AudioFormat; // 4
        int NumChannels; // 5
        int SampleRate; // 6
        int ByteRate; // 7
        int BlockAlign; // 8
        int BitsPerSample; // 9
        int DataSize; // 10

        int[][] Data; // 11

        #region Einlesen
        public void LoadWave(string path)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(path); // zu lesende Wave Datei öffnen
            LoadChunk(fs); // RIFF Chunk einlesen
            LoadChunk(fs); // fmt Chunk einlesen
            LoadChunk(fs); // data Chunk einlesen
        }

        private void LoadChunk(System.IO.FileStream fs)
        {
            System.Text.ASCIIEncoding Encoder = new ASCIIEncoding();

            byte[] bChunkID = new byte[4];
            /* Die ersten 4 Bytes einlesen.
            Diese ergeben bei jedem Chunk den jeweiligen Namen. */
            fs.Read(bChunkID, 0, 4);
            string sChunkID = Encoder.GetString(bChunkID); // den Namen aus den Bytes dekodieren

            byte[] ChunkSize = new byte[4];
            /* Die nächsten 4 Bytes ergeben bei jedem Chunk die Größenangabe. */
            fs.Read(ChunkSize, 0, 4);

            if (sChunkID.Equals("RIFF"))
            {
                // beim Riff Chunk ...
                // die Größe in FileSize speichern
                FileSize = System.BitConverter.ToInt32(ChunkSize, 0);
                // das Format einlesen
                byte[] Format = new byte[4];
                fs.Read(Format, 0, 4);
                // ergibt "WAVE" als String
                this.Format = Encoder.GetString(Format);
            }

            if (sChunkID.Equals("fmt "))
            {
                // beim fmt Chunk die Größe in FmtChunkSize speichern
                FmtChunkSize = System.BitConverter.ToInt32(ChunkSize, 0);
                // sowie die anderen Informationen auslesen und speichern
                byte[] AudioFormat = new byte[2];
                fs.Read(AudioFormat, 0, 2);
                this.AudioFormat = System.BitConverter.ToInt16(AudioFormat, 0);
                byte[] NumChannels = new byte[2];
                fs.Read(NumChannels, 0, 2);
                this.NumChannels = System.BitConverter.ToInt16(NumChannels, 0);
                byte[] SampleRate = new byte[4];
                fs.Read(SampleRate, 0, 4);
                this.SampleRate = System.BitConverter.ToInt32(SampleRate, 0);
                byte[] ByteRate = new byte[4];
                fs.Read(ByteRate, 0, 4);
                this.ByteRate = System.BitConverter.ToInt32(ByteRate, 0);
                byte[] BlockAlign = new byte[2];
                fs.Read(BlockAlign, 0, 2);
                this.BlockAlign = System.BitConverter.ToInt16(BlockAlign, 0);
                byte[] BitsPerSample = new byte[2];
                fs.Read(BitsPerSample, 0, 2);
                this.BitsPerSample = System.BitConverter.ToInt16(BitsPerSample, 0);
            }

            if (sChunkID == "\0\0da")
            {
                sChunkID = "data";
                fs.ReadByte();
                fs.ReadByte();
            }

            if (sChunkID == "data")
            {
                // beim data Chunk die Größenangabe in DataSize speichern
                DataSize = System.BitConverter.ToInt32(ChunkSize, 0);

                // der 1. Index von Data spezifiziert den Audiokanal, der 2. das Sample
                Data = new int[this.NumChannels][];
                // Temporäres Array zum Einlesen der jeweiligen Bytes eines Kanals pro Sample
                byte[] temp = new byte[BlockAlign / NumChannels];
                // für jeden Kanal das Data Array auf die Anzahl der Samples dimensionalisieren
                for (int i = 0; i < this.NumChannels; i++)
                {
                    Data[i] = new int[this.DataSize / (NumChannels * BitsPerSample / 8)];
                }

                // nacheinander alle Samples durchgehen
                for (int i = 0; i < Data[0].Length; i++)
                {
                    // alle Audiokanäle pro Sample durchgehen
                    for (int j = 0; j < NumChannels; j++)
                    {
                        // die jeweils genutze Anzahl an Bytes pro Sample und Kanal einlesen
                        if (fs.Read(temp, 0, BlockAlign / NumChannels) > 0)
                        {   // je nachdem, wie viele Bytes für einen Wert genutzt werden,
                            // die Amplitude als Int16 oder Int32 interpretieren
                            if (BlockAlign / NumChannels == 2)
                                Data[j][i] = System.BitConverter.ToInt16(temp, 0);
                            else
                                Data[j][i] = System.BitConverter.ToInt32(temp, 0);
                        }
                        /* else
                         * andere Werte als 2 oder 4 werden nicht behandelt, hier bei Bedarf ergänzen!
                        */
                    }
                }
            }
        }
        #endregion

        #region Schreiben
        public void StoreWave(string path)
        {
            System.IO.FileStream fs = System.IO.File.OpenWrite(path); // zu schreiben Wave Datei öffnen / erstellen
            StoreChunk(fs, "RIFF"); // RIFF Chunk schreiben
            StoreChunk(fs, "fmt "); // fmt Chunk schreiben
            StoreChunk(fs, "data"); // data Chunk schreiben

            fs.Dispose();
        }

        private void StoreChunk(System.IO.FileStream fs, string chunkID)
        {
            System.Text.ASCIIEncoding Decoder = new ASCIIEncoding();
            // den Namen in Bytes konvertieren und schreiben
            fs.Write(Decoder.GetBytes(chunkID), 0, 4);

            if (chunkID == "RIFF")
            {
                // im RIFF Chunk, FileSize als Größe und das Audioformat schreiben
                fs.Write(System.BitConverter.GetBytes(FileSize), 0, 4);
                fs.Write(Decoder.GetBytes(Format), 0, 4);
            }
            if (chunkID == "fmt ")
            {
                // beim fmt Chunk die Größe dieses sowie die weiteren kodierten Informationen schreiben
                fs.Write(System.BitConverter.GetBytes(FmtChunkSize), 0, 4);
                fs.Write(System.BitConverter.GetBytes(AudioFormat), 0, 2);
                fs.Write(System.BitConverter.GetBytes(NumChannels), 0, 2);
                fs.Write(System.BitConverter.GetBytes(SampleRate), 0, 4);
                fs.Write(System.BitConverter.GetBytes(ByteRate), 0, 4);
                fs.Write(System.BitConverter.GetBytes(BlockAlign), 0, 2);
                fs.Write(System.BitConverter.GetBytes(BitsPerSample), 0, 2);
            }
            if (chunkID == "data")
            {
                // beim data Chunk die Größe des Datenblocks als Größenangabe schreiben
                fs.Write(System.BitConverter.GetBytes(DataSize), 0, 4);
                // dann die einzelnen Amplituden, wie beschrieben Sample für Sample mit jeweils allen
                // Audiospuren, schreiben
                for (int i = 0; i < Data[0].Length; i++)
                {
                    for (int j = 0; j < NumChannels; j++)
                    {
                        fs.Write(System.BitConverter.GetBytes(Data[j][i]), 0, BlockAlign / NumChannels);
                    }
                }
            }
        }
        #endregion

        #region Mischen
        private static WaveFile MixWave(WaveFile wf1, WaveFile wf2, double fadeTime)
        {
            int FadeSamples = (int)(fadeTime * wf1.ByteRate / wf1.NumChannels); // number of samples affected by the crossfading
            int FadeBytes = (int)(fadeTime * wf1.ByteRate); // number of affected bytes

            WaveFile Result = new WaveFile(); // resulting Wave file
            Result.FileSize = wf1.FileSize + wf2.DataSize - 2 * FadeBytes; // file size
            Result.Format = "WAVE";

            // copy information from the fmt  chunk
            Result.FmtChunkSize = wf1.FmtChunkSize;
            Result.AudioFormat = wf1.AudioFormat;
            Result.NumChannels = wf1.NumChannels;
            Result.SampleRate = wf1.SampleRate;
            Result.ByteRate = wf1.ByteRate;
            Result.BlockAlign = wf1.BlockAlign;
            Result.BitsPerSample = wf1.BitsPerSample;

            Result.DataSize = wf1.DataSize + wf2.DataSize - 2 * FadeBytes; // new size of the data chunk
            Result.Data = new int[wf1.NumChannels][]; // copy number of channels
            int NumSamples = Result.DataSize / (Result.NumChannels * (Result.BitsPerSample / 8)); //  number of samples in resulting file

            // initialize data arrays for all samples and channels
            for (int i = 0; i < Result.Data.Length; i++)
            {
                Result.Data[i] = new int[NumSamples];
            }

            int PosCounter = 0; // position of the current sample in the Wave file

            // copy the samples from the first Wave file into the data field of the result file
            for (int i = 0; i < wf1.Data[0].Length; i++)
            {
                // copy the current sample for all channels
                for (int j = 0; j < wf1.NumChannels; j++)
                {
                    // if the current sample is in the crossfading time, mix the amplitude value of the 1st file with the amplitude value of the 2nd file
                    if (i > wf1.Data[0].Length - FadeSamples)
                    {
                        Result.Data[j][PosCounter] = (int)(wf1.Data[j][i] * Factor(i - (wf1.Data[0].Length - FadeSamples), FadeSamples, 0) + wf2.Data[j][i - (wf1.Data[0].Length - FadeSamples)] * Factor(i - (wf1.Data[0].Length - FadeSamples), FadeSamples, 1));
                        //Result.Data[j][PosCounter] = (int)(wf1.Data[j][i] + wf2.Data[j][i - (wf1.Data[0].Length - FadeSamples)]);
                    }
                    else
                        Result.Data[j][PosCounter] = wf1.Data[j][i];
                }
                PosCounter++;
            }

            // copy the remaining samples
            for (int i = FadeSamples; i < wf2.Data[0].Length; i++)
            {
                for (int j = 0; j < wf1.NumChannels; j++)
                {
                    Result.Data[j][PosCounter] = wf2.Data[j][i];
                }
                PosCounter++;
            }
            return Result;
        }

        /// <summary>
        /// Diese Funktion dient zur Berechnung der Gewichtung der Amplituden bei Übermischung.
        /// </summary>
        /// <param name="pos">Position in Datei relativ zum Anfang der Überblendezeit</param>
        /// <param name="max">Ende der Überblendung, relativ zu pos</param>
        /// <param name="song">Kann die Werte 0 (auszublendender Song) oder 1 (einzublender Song) annehmen</param>
        /// <returns></returns>
        private static double Factor(int pos, int max, int song)
        {
            if (song == 0)
                return 1 - Math.Pow((double)pos / (double)max, 2);
            else
                return Math.Pow((double)pos / (double)max, 2);
        }

        public static void StoreMixWave(string path, WaveFile wf1, WaveFile wf2, double fadeTime)
        {
            WaveFile Mixed = MixWave(wf1, wf2, fadeTime); // Ergebnisdatei mischen
            Mixed.StoreWave(path); // Ergebnisdatei auf Festplatte speichern
        }

        #endregion

    }
}
