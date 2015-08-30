using Businesslayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PBB
{
    public partial class PlayAll : Form
    {
        private Playbackband playbackband;
        //private System.Threading.Thread play_thread;
        //private bool play_thread_abort = false;
        //private int play_thread_pause = 0;
        private string directoryToUse = "";

        private MediaPlayer player1;
        private MediaPlayer player2;
        private int currentIndex;

        public PlayAll(Playbackband pbb, string directoryToUse)
        {
            InitializeComponent();

            this.directoryToUse = directoryToUse;
            playbackband = pbb;
            //play_thread = new System.Threading.Thread(backgroundWorker1_DoWork);
            //play_thread.SetApartmentState(System.Threading.ApartmentState.STA);

            player1 = new MediaPlayer();
            player2 = new MediaPlayer();
        }

        #region "Old"

        //private bool mp1_fadeout = false;
        //private bool mp2_fadeout = false;

        //private void backgroundWorker1_DoWork()
        //{
        //    MediaPlayer mp1 = null;
        //    MediaPlayer mp2 = null;

        //    if (numericUpDown1.Value > 1)
        //    {
        //        //Zet in mp2 het vorige liedje klaar ivm de overgang naar het volgende nummer
        //        for (int i = (int)numericUpDown1.Value - 2; i > -1; i--)
        //        {
        //            mp2 = CreateMPFromNextFragment(fragmenten[i]);
        //            if (mp2 == null)
        //                continue;
        //            else
        //            {
        //                mp2.Name = "mp2";
        //                mp2.Tag = i + 1;
        //                break;
        //            }
        //        }

        //        //stel het liedje in dat er wordt afgespeelt op het moment dat de fadeout begint
        //        OnStartedPlaying((int)mp2.Tag);
        //        mp2.PlaySelectie((fragmenten[(int)mp2.Tag - 1].Eindtijd - new DateTime(2000, 1, 1)).TotalSeconds - (fragmenten[(int)mp2.Tag - 1].FadeOut - new DateTime(2000, 1, 1)).TotalSeconds);
        //    }
        //    else
        //        mp2_fadeout = true; // player doesn't start otherwise



        //    for (int i = (int)numericUpDown1.Value - 1; i < fragmenten.Count; i++)
        //    {
        //        //mp1 (opnieuw) klaarzetten
        //        while (i < fragmenten.Count)
        //        {
        //            mp1 = CreateMPFromNextFragment(fragmenten[i]);
        //            if (mp1 == null)
        //                i++;
        //            else
        //            {
        //                mp1.Name = "mp1";
        //                mp1.Tag = i + 1;
        //                break;
        //            }
        //        }
        //        i++;

        //        //wacht tot mp2 fadeout bereikt heeft
        //        while (!mp2_fadeout)
        //        {
        //            ToDo(mp1, mp2);
        //        }
        //        mp2_fadeout = false;

        //        //mp1 fadein starten (en de rest van het fragment)
        //        if (mp1 != null)
        //        {
        //            OnStartedPlaying((int)mp1.Tag);
        //            mp1.PlaySelectie();
        //        }

        //        //wacht to mp2 klaar is met afspelen
        //        if (mp2 != null)
        //        {
        //            while (mp2.GetPlayState() != PlayState.Stopped)
        //            {
        //                ToDo(mp1, mp2);
        //            }
        //            OnStoppedPlaying((int)mp2.Tag);
        //        }


        //        //mp2 (opnieuw) klaarzetten
        //        while (i < fragmenten.Count)
        //        {
        //            mp2 = CreateMPFromNextFragment(fragmenten[i]);
        //            if (mp2 == null)
        //                i++;
        //            else
        //            {
        //                mp2.Tag = i + 1;
        //                mp2.Name = "mp2";
        //                break;
        //            }
        //        }

        //        //wacht tot mp1 fadeout bereikt heeft
        //        if (mp1 != null)
        //        {
        //            while (!mp1_fadeout)
        //            {
        //                ToDo(mp1, mp2);
        //            }
        //        }
        //        mp1_fadeout = false;

        //        //mp2 fadein afspelen starten (en de rest van het fragment)
        //        if (mp2 != null)
        //        {
        //            OnStartedPlaying((int)mp2.Tag);
        //            mp2.PlaySelectie();
        //        }

        //        //wacht tot mp1 klaar is met afspelen
        //        if (mp1 != null)
        //        {
        //            while (mp1.GetPlayState() != PlayState.Stopped)
        //            {
        //                ToDo(mp1, mp2);
        //            }
        //            OnStoppedPlaying((int)mp1.Tag);
        //        }
        //    }

        //}

        //private void ToDo(MediaPlayer mp1, MediaPlayer mp2)
        //{
        //    Application.DoEvents();
        //    System.Threading.Thread.Sleep(10);

        //    if (play_thread_abort)
        //    {
        //        if (mp1 != null)
        //        {
        //            mp1.Stop();
        //            OnStoppedPlaying((int)mp1.Tag);
        //        }
        //        if (mp2 != null)
        //        {
        //            mp2.Stop();
        //            OnStoppedPlaying((int)mp2.Tag);
        //        }


        //        play_thread.Abort();
        //        return;
        //    }
        //    if (play_thread_pause == 1)
        //    {
        //        if (mp1.GetPlayState() == PlayState.Playing)
        //            mp1.Pauze();
        //        if (mp2.GetPlayState() == PlayState.Playing)
        //            mp2.Pauze();
        //        play_thread_pause = 2;
        //    }
        //    else if (play_thread_pause == 3)
        //    {
        //        if (mp1.GetPlayState() == PlayState.Paused)
        //            mp1.Play();
        //        if (mp2.GetPlayState() == PlayState.Paused)
        //            mp2.Play();
        //        play_thread_pause = 0;
        //    }
        //}

        //private MediaPlayer CreateMPFromNextFragment(Fragment f)
        //{
        //    if (f.IsDummy())
        //        return null;

        //    MediaPlayer mp = new MediaPlayer(f.Liedje, f.Liedje.WriteToFile(this.directoryToUse + "\\" + DateTime.Now.Ticks.ToString() + ".mp3"));
        //    mp.SetFragmentProperties(f.BeginTijd, f.Eindtijd, f.FadeIn, f.FadeOut, f.FadeInBinnen, f.FadeOutBinnen);
        //    mp.FadeOutStarted += mp1_FadeOutStarted;
        //    return mp;
        //}

        //void mp1_FadeOutStarted(MediaPlayer sender)
        //{
        //    if (sender.Name == "mp1")
        //        mp1_fadeout = true;
        //    else
        //        mp2_fadeout = true;
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            //if (play_thread_pause == 2)
            //{
            //    play_thread_pause = 3;
            //    button2.Enabled = false;
            //    button3.Enabled = true;
            //    button4.Enabled = true;
            //    return;
            //}
            //play_thread_abort = false;
            //play_thread = new System.Threading.Thread(backgroundWorker1_DoWork);
            //play_thread.SetApartmentState(System.Threading.ApartmentState.STA);
            //play_thread.Start();
            ////backgroundWorker1_DoWork();

            if (button4.Enabled)
            {
                // op pauze
                if (player1.GetPlayState() == PlayState.Paused)
                {
                    player1.Play();
                }
                if (player2.GetPlayState() == PlayState.Paused)
                {
                    player2.Play();
                }
            }
            else
            {
                // stopped
                panel2.Controls.Clear();

                currentIndex = (int)numericUpDown1.Value - 1;
                CreateNextMediaPlayer(currentIndex, true);
            }

            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //public event StartedOrStoppedPlayingEventHandler StartedPlaying;
        //public event StartedOrStoppedPlayingEventHandler StoppedPlaying;
        //public delegate void StartedOrStoppedPlayingEventHandler(int newNumber);
        //public void OnStartedPlaying(int newNumber)
        //{
        //    if (StartedPlaying != null)
        //        StartedPlaying(newNumber);
        //}

        //public void OnStoppedPlaying(int newNumber)
        //{
        //    if (StoppedPlaying != null)
        //        StoppedPlaying(newNumber);
        //}

        private void button4_Click(object sender, EventArgs e)
        {
            //play_thread_abort = true;

            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;

            if (player1.IsPlaying())
            {
                player1.Stop();
            }
            if (player2.IsPlaying())
            {
                player2.Stop();
            }
        }

        private void PlayAll_FormClosing(object sender, FormClosingEventArgs e)
        {
            //play_thread_abort = true;
            player1.Stop();
            player2.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = true;

            if (player1.IsPlaying())
            {
                player1.Pauze();
            }
            if (player2.IsPlaying())
            {
                player2.Pauze();
            }
        }

        #endregion

        #region "New"

        private void CreateNextMediaPlayer(int index, bool autoPlay = false)
        {
            if (index > fragmenten.Count - 1)
            {
                return;
            }

            Fragment f = fragmenten[index];
            while (f.Liedje.FileContent == null)
            {
                index++;
                if (index == fragmenten.Count)
                {
                    if (!player1.IsPlaying()) panel2.Controls.Remove(player1);
                    if (!player2.IsPlaying()) panel2.Controls.Remove(player2);

                    return;
                }
                f = fragmenten[index];
            }


            if (!player1.IsPlaying())
            {
                panel2.Controls.Remove(player1);
                player1 = new MediaPlayer(f.Liedje, f.Liedje.WriteToFile(this.directoryToUse + "\\" + DateTime.Now.Ticks.ToString() + ".mp3"));
                player1.SetFragmentProperties(f.BeginTijd, f.EindTijd, f.FadeIn, f.FadeOut, f.FadeInBinnen, f.FadeOutBinnen);

                player1.FadeInEnded += player_FadeInEnded;
                player1.FadeOutStarted += player1_FadeOutStarted;

                player1.DisplayMode = true;
                //player1.Location = new Point(12, 0);
                player1.Dock = DockStyle.Top;
                player1.Parent = panel2;
                player1.BringToFront();
                
                if (autoPlay)
                {
                    player1.PlaySelectie();
                }
            }
            else if (!player2.IsPlaying())
            {
                panel2.Controls.Remove(player2);
                player2 = new MediaPlayer(f.Liedje, f.Liedje.WriteToFile(this.directoryToUse + "\\" + DateTime.Now.Ticks.ToString() + ".mp3"));
                player2.SetFragmentProperties(f.BeginTijd, f.EindTijd, f.FadeIn, f.FadeOut, f.FadeInBinnen, f.FadeOutBinnen);

                player2.FadeInEnded += player_FadeInEnded;
                player2.FadeOutStarted +=player2_FadeOutStarted;

                //player2.Location = new Point(12, 81);
                player2.DisplayMode = true;
                player2.Dock = DockStyle.Top;
                player2.Parent = panel2;
                player2.BringToFront();
                
                if (autoPlay)
                {
                    player2.PlaySelectie();
                }
            }
            else
            {
                throw new Exception("ERROR!");
            }

            currentIndex++;
        }

        void player1_FadeOutStarted(MediaPlayer sender)
        {
            player2.PlaySelectie();
        }

        void player2_FadeOutStarted(MediaPlayer sender)
        {
            player1.PlaySelectie();
        }

        void player_FadeInEnded(MediaPlayer sender)
        {
            while (player1.IsPlaying() && player2.IsPlaying())
            {
                Application.DoEvents();
            }
            CreateNextMediaPlayer(currentIndex);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

        #endregion

        private List<Fragment> fragmenten;

        private void PlayAll_Load(object sender, EventArgs e)
        {
            //this.Location = new Point(this.Parent.Location.X + this.Parent.Size.Width / 2 - this.Size.Width / 2, this.Parent.Location.Y + this.Parent.Size.Height / 2 - this.Size.Height / 2);

            fragmenten = new List<Fragment>();
            for (int blok = 0; blok < playbackband.Blokken.Count; blok++)
            {
                for (int fragment = 0; fragment < playbackband.Blokken[blok].Fragmenten.Count; fragment++)
                {
                    fragmenten.Add(playbackband.Blokken[blok].Fragmenten[fragment]);
                }
            }

            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = fragmenten.Count;
        }
    }
}
