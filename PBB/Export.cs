using Businesslayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PBB
{
    public partial class Export : Form
    {
        public Export(Playbackband pbb)
        {
            InitializeComponent();

            liedjes = new List<Fragment>();
            errors = new List<string>();
            for (int i = 0; i < pbb.Blokken.Count; i++)
            {
                for (int j = 0; j < pbb.Blokken[i].Fragmenten.Count; j++)
                {
                    liedjes.Add(pbb.Blokken[i].Fragmenten[j]);
                }
            }

            aantal_liedjes = liedjes.Count;
        }

        private int aantal_liedjes = 50;
        private int bgw1_done = 0;
        private int bgw2_done = 0;
        private int bgw3_done = 0;
        private int bgw4_done = 0;
        private int bgw5_done = 0;
        private int bgw6_done = 0;
        private bool bgw1_haserrors = false;
        private bool bgw2_haserrors = false;
        private bool bgw3_haserrors = false;
        private bool bgw4_haserrors = false;
        private bool bgw5_haserrors = false;
        private bool bgw6_haserrors = false;

        private string outputDir = "";
        private List<Fragment> liedjes;

        private List<string> errors;

        private void UpdateView()
        {
            label2.Text = bgw1_done.ToString() + "/" + aantal_liedjes.ToString() + "  Fragmenten maken van de liedjes";
            label3.Text = bgw2_done.ToString() + "/" + aantal_liedjes.ToString() + "  Fragmenten normalizeren";
            label4.Text = bgw3_done.ToString() + "/" + aantal_liedjes.ToString() + "  Fade-in en fade-out toevoegen aan de fragmenten";
            label5.Text = bgw4_done.ToString() + "/" + aantal_liedjes.ToString() + "  Fragmenten samenvoegen tot één playbackband";
            label6.Text = bgw5_done.ToString() + "/02" + "  Playbackband afvlakken";
            label7.Text = bgw6_done.ToString() + "/" + aantal_liedjes.ToString() + "  Playbackband opsplitsen in losse nummers";

            if (bgw1_done == aantal_liedjes && !bgw1_haserrors)
                pictureBox1.Image = PBB.Properties.Resources.done;
            if (bgw2_done == aantal_liedjes && !bgw2_haserrors)
                pictureBox2.Image = PBB.Properties.Resources.done;
            if (bgw3_done == aantal_liedjes && !bgw3_haserrors)
                pictureBox3.Image = PBB.Properties.Resources.done;
            if (bgw4_done == aantal_liedjes && !bgw4_haserrors)
                pictureBox4.Image = PBB.Properties.Resources.done;
            if (bgw5_done == 2 && !bgw5_haserrors)
                pictureBox5.Image = PBB.Properties.Resources.done;
            if (bgw6_done == aantal_liedjes && !bgw6_haserrors)
                pictureBox6.Image = PBB.Properties.Resources.done;

            if(bgw1_haserrors)
                pictureBox1.Image = PBB.Properties.Resources.error;
            if (bgw2_haserrors)
                pictureBox2.Image = PBB.Properties.Resources.error;
            if (bgw3_haserrors)
                pictureBox3.Image = PBB.Properties.Resources.error;
            if (bgw4_haserrors)
                pictureBox4.Image = PBB.Properties.Resources.error;
            if (bgw5_haserrors)
                pictureBox5.Image = PBB.Properties.Resources.error;
            if (bgw6_haserrors)
                pictureBox6.Image = PBB.Properties.Resources.error;

            progressBar1.Maximum = aantal_liedjes * 5 + 2;
            progressBar1.Value = bgw1_done + bgw2_done + bgw3_done + bgw4_done + bgw5_done + bgw6_done;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Details >>")
            {
                this.Size = new Size(476, 354);
                button1.Text = "<< Minder";
            }
            else
            {
                this.Size = new Size(476, 140);
                button1.Text = "Details >>";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button4.Enabled = false;
            button2.Enabled = false;
            textBox1.Enabled = false;

            outputDir = textBox1.Text;

            button1_Click(null, null);

            backgroundWorker1.RunWorkerAsync();
            backgroundWorker2.RunWorkerAsync();
            backgroundWorker3.RunWorkerAsync();
            backgroundWorker4.RunWorkerAsync();
            backgroundWorker5.RunWorkerAsync();
            backgroundWorker6.RunWorkerAsync();
        }

        private void Export_Load(object sender, EventArgs e)
        {
            button1_Click(null, null);
            UpdateView();
        }

        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //Fragmenten maken uit de liedjes

            //Map aanmaken
            string dir = File.defaultDir + "\\ready_for_export";
            if (System.IO.Directory.Exists(dir))
                System.IO.Directory.Delete(dir, true);
            System.IO.Directory.CreateDirectory(dir);

            dir = File.defaultDir + "\\trimmed";
            if (System.IO.Directory.Exists(dir))
                System.IO.Directory.Delete(dir, true);
            System.IO.Directory.CreateDirectory(dir);

            dir = File.defaultDir + "\\normalized";
            if (System.IO.Directory.Exists(dir))
                System.IO.Directory.Delete(dir, true);
            System.IO.Directory.CreateDirectory(dir);

            dir = File.defaultDir + "\\faded";
            if (System.IO.Directory.Exists(dir))
                System.IO.Directory.Delete(dir, true);
            System.IO.Directory.CreateDirectory(dir);

            dir = File.defaultDir + "\\total";
            if (System.IO.Directory.Exists(dir))
                System.IO.Directory.Delete(dir, true);
            System.IO.Directory.CreateDirectory(dir);

            for (int i = 0; i < liedjes.Count; i++)
            {
                if (backgroundWorker1.CancellationPending)
                    return;

                if (!liedjes[i].IsDummy())
                {
                    MusicEditor.TrimMp3(
                        liedjes[i].Liedje.WriteToFile(File.defaultDir + "\\ready_for_export\\" + (i + 1).ToString() + ".mp3"),
                        File.defaultDir + "\\trimmed\\" + (i + 1).ToString() + ".mp3",
                        liedjes[i].BeginTijd - new DateTime(2000, 1, 1),
                        liedjes[i].EindTijd - new DateTime(2000, 1, 1)
                        );
                }
                else
                {
                    errors.Add("Leeg fragment gevonden (geen mp3 bestand gekoppeld)");
                    backgroundWorker1.ReportProgress(2);
                }

                backgroundWorker1.ReportProgress(1);

                //System.Threading.Thread.Sleep(500);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                bgw1_done += 1;
            }
            if (e.ProgressPercentage == 2)
                bgw1_haserrors = true;

            UpdateView();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            while (bgw2_done < aantal_liedjes)
            {
                if (bgw1_done > 0)
                {
                    //for (int i = bgw2_done; i < bgw1_done; i++)
                    //{
                    if (System.IO.File.Exists(File.defaultDir + "\\trimmed\\" + (bgw2_done + 1).ToString() + ".mp3"))
                    {
                        Process p = new Process();
                        //p.StartInfo.FileName = "mp3gain.exe";
                        //p.StartInfo.Arguments = "/r \"" + File.defaultDir + "\\trimmed\\" + (bgw2_done + 1).ToString() + ".mp3\"";
                        p.StartInfo.FileName = "sox.exe";
                        p.StartInfo.Arguments = "--norm \"" + File.defaultDir + "\\trimmed\\" + (bgw2_done + 1).ToString() + ".mp3\" \"" + File.defaultDir + "\\normalized\\" + (bgw2_done + 1).ToString() + ".mp3\"";
                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        p.Start();
                        p.WaitForExit();

                        bgw2_done += 1;
                    }
                    else
                    {
                        errors.Add("Geen fragment gevonden voor " + (bgw2_done + 1).ToString());
                        bgw2_haserrors = true;
                        bgw2_done += 1;
                    }

                    backgroundWorker2.ReportProgress(1);
                    //}
                }
            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UpdateView();
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bgw2_done = aantal_liedjes;
            UpdateView();
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            while (bgw3_done < aantal_liedjes)
            {
                if (bgw2_done > 0)
                {
                    if (bgw2_done > bgw3_done || bgw2_done == aantal_liedjes)
                    {
                        //for (int i = bgw3_done; i < bgw2_done; i++)
                        //{
                        if (System.IO.File.Exists(File.defaultDir + "\\normalized\\" + (bgw3_done + 1).ToString() + ".mp3"))
                        {
                            Process p = new Process();
                            p.StartInfo.FileName = "sox.exe";
                            p.StartInfo.Arguments = "\"" + File.defaultDir + "\\normalized\\" + (bgw3_done + 1).ToString() + ".mp3\" -t wav \"" + File.defaultDir + "\\faded\\" + (bgw3_done + 1).ToString() + ".wav\" fade h " + (liedjes[bgw3_done].FadeIn - new DateTime(2000, 1, 1)).ToString("G").Replace(",", ".").Remove(0, 2) + " 0 " + (liedjes[bgw3_done].FadeOut - new DateTime(2000, 1, 1)).ToString("G").Replace(",", ".").Remove(0, 2);
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            p.Start();
                            p.WaitForExit();

                            bgw3_done += 1;
                        }
                        else
                        {
                            errors.Add("Geen fragment gevonden voor " + (bgw3_done + 1).ToString());
                            bgw3_haserrors = true;
                            bgw3_done += 1;
                        }

                        backgroundWorker3.ReportProgress(1);
                    }
                }
                //}
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            textBox1.Text = fbd.SelectedPath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy || backgroundWorker2.IsBusy || backgroundWorker3.IsBusy || backgroundWorker4.IsBusy || backgroundWorker5.IsBusy || backgroundWorker6.IsBusy)
            {
                if (MessageBox.Show("Weet je zeker dat je het exporteren van de playbackband wilt annuleren?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    return;
                else
                {
                    backgroundWorker1.CancelAsync();
                    backgroundWorker2.CancelAsync();
                    backgroundWorker3.CancelAsync();
                    backgroundWorker4.CancelAsync();
                    backgroundWorker5.CancelAsync();
                    backgroundWorker6.CancelAsync();
                }
            }

            this.Close();
        }

        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UpdateView();
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            while (bgw4_done < aantal_liedjes)
            {
                if (bgw3_done > 0)
                {
                    if (bgw3_done > bgw4_done || bgw3_done == aantal_liedjes)
                    {
                        if (bgw4_done == 0)
                        {
                            System.IO.File.Copy(File.defaultDir + "\\faded\\" + (bgw4_done + 1) + ".wav", File.defaultDir + "\\total\\total.wav", true);
                            bgw4_done += 1;
                        }
                        else
                        {
                            if (System.IO.File.Exists(File.defaultDir + "\\faded\\" + (bgw4_done + 1).ToString() + ".wav"))
                            {
                                //MusicEditor.MixMp3(File.defaultDir + "\\total\\total.mp3", File.defaultDir + "\\faded\\" + (bgw4_done + 1) + ".mp3", (liedjes[bgw4_done - 1].FadeOut - new DateTime(2000, 1, 1)).TotalSeconds, File.defaultDir + "\\total\\total2.mp3");

                                WaveFile wf1 = new WaveFile();
                                wf1.LoadWave(File.defaultDir + "\\total\\total.wav");

                                WaveFile wf2 = new WaveFile();
                                wf2.LoadWave(File.defaultDir + "\\faded\\" + (bgw4_done + 1) + ".wav");

                                WaveFile.StoreMixWave(File.defaultDir + "\\total\\total2.wav", wf1, wf2, (liedjes[bgw4_done - 1].FadeOut - new DateTime(2000, 1, 1)).TotalSeconds);

                                System.IO.File.Delete(File.defaultDir + "\\total\\total.wav");
                                System.IO.File.Copy(File.defaultDir + "\\total\\total2.wav", File.defaultDir + "\\total\\total.wav");
                                System.IO.File.Delete(File.defaultDir + "\\total\\total2.wav");
                                bgw4_done += 1;
                            }
                            else
                                bgw4_done += 1;
                        }

                        backgroundWorker4.ReportProgress(1);
                    }
                }
            }
            
        }

        private void backgroundWorker4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UpdateView();
        }

        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            while (bgw4_done != aantal_liedjes)
            {
                System.Threading.Thread.Sleep(10);
            }

            #region "OldCode"

            //List<string> filenames = new List<string>();
            //for (int i = 0; i < aantal_liedjes; i++)
            //{
            //    filenames.Add(File.defaultDir + "\\faded\\" + (i).ToString() + ".mp3");
            //}
            //MusicEditor.MixMp3_2(filenames.ToArray(), File.defaultDir + "\\total\\testing.mp3", liedjes);

            //if (bgw4_done == aantal_liedjes)
            //{
            //    if (System.IO.File.Exists(File.defaultDir + "\\total\\total.mp3"))
            //    {
            //        Process p = new Process();
            //        //p.StartInfo.FileName = "mp3gain.exe";
            //        //p.StartInfo.Arguments = "/r \"" + File.defaultDir + "\\trimmed\\" + (bgw2_done + 1).ToString() + ".mp3\"";
            //        p.StartInfo.FileName = "sox.exe";
            //        p.StartInfo.Arguments = "--norm \"" + File.defaultDir + "\\total\\total.mp3\" \"" + File.defaultDir + "\\total\\playbackband.mp3\"";
            //        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //        p.Start();
            //        p.WaitForExit();

            //        bgw5_done += 1;

            //        backgroundWorker5.ReportProgress(1);

            //        p = new Process();
            //        p.StartInfo.FileName = "mp3gain.exe";
            //        p.StartInfo.Arguments = "/r \"" + File.defaultDir + "\\total\\playbackband.mp3\"";
            //        //p.StartInfo.FileName = "sox.exe";
            //        //p.StartInfo.Arguments = "--norm \"" + File.defaultDir + "\\total\\total.mp3\" \"" + File.defaultDir + "\\total\\playbackband.mp3\"";
            //        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //        p.Start();
            //        p.WaitForExit();

            //        bgw5_done += 1;

            //        backgroundWorker5.ReportProgress(1);
            //    }
            //    else
            //    {
            //        errors.Add("Er bestaat geen totaalfragment");
            //        bgw5_haserrors = true;
            //        bgw5_done += 2;

            //        backgroundWorker5.ReportProgress(1);
            //    }
            //}

            #endregion

            Process p = new Process();
            p.StartInfo.FileName = "sox.exe";
            p.StartInfo.Arguments = "--norm \"" + File.defaultDir + "\\total\\total.wav\" -t mp3 \"" + File.defaultDir + "\\total\\total.mp3\"";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();

            bgw5_done += 1;
            backgroundWorker5.ReportProgress(1);

            p = new Process();
            p.StartInfo.FileName = "mp3gain.exe";
            p.StartInfo.Arguments = "/r \"" + File.defaultDir + "\\total\\total.mp3\"";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();

            bgw5_done += 1;
            backgroundWorker5.ReportProgress(1);
        }

        private void backgroundWorker5_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UpdateView();
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void backgroundWorker6_DoWork(object sender, DoWorkEventArgs e)
        {
            while (bgw5_done != 2)
            {
                System.Threading.Thread.Sleep(10);
            }

            TimeSpan lastEndTime = new TimeSpan();
            for (int i = 0; i < liedjes.Count; i++)
            {
                TimeSpan newEndTime = lastEndTime.Add(liedjes[i].EindTijd - liedjes[i].BeginTijd);
                MusicEditor.TrimMp3(File.defaultDir + "\\total\\total.mp3", outputDir + "\\" + liedjes[i].Liedje.Titel + ".mp3", lastEndTime, newEndTime);
                lastEndTime = newEndTime;
            }
        }
    }
}
