using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Businesslayer;

namespace PBB
{
    public partial class MediaPlayer : UserControl//, IDisposable
    {
        private AxWMPLib.AxWindowsMediaPlayer wmp;
        private string filename;
        private decimal stopTime = -1;
        private double lengte;

        private bool displaymode;
        private double totalLength = 0;
        private double beginTime = 0;

        public bool DoNotTriggerUpdateEvent;

        public MediaPlayer(Liedje liedje, string filename)
        {
            InitializeComponent();

            DoNotTriggerUpdateEvent = true;
            selectionPanel.Size = new Size(this.Size.Width, this.Size.Height);
            fadeinPanel.Size = new Size(0, this.Size.Height);
            fadeoutPanel.Size = new Size(0, this.Size.Height);
            currentPanel.Size = new Size(3, this.Size.Height);

            this.filename = filename;

            if (filename != "")
            {
                lengte = liedje.Lengte;

                wmp = new AxWMPLib.AxWindowsMediaPlayer();
                wmp.CreateControl();
                wmp.URL = filename;
                wmp.PlayStateChange += wmp_PlayStateChange;
                wmp.Ctlcontrols.stop();
                ChangeButtonText("Play");

                label3.Text = liedje.Artiest + " / " + liedje.Titel;
                label4.Text = "0.00 / 0.00";

                numericUpDown1.Maximum = (decimal)lengte;
                numericUpDown2.Maximum = (decimal)lengte;
                numericUpDown3.Maximum = (decimal)lengte;
                numericUpDown3.Value = (decimal)lengte;

                UpdateSelection();
            }
        }

        public MediaPlayer()
        {
            InitializeComponent();

            DoNotTriggerUpdateEvent = true; 
            selectionPanel.Size = new Size(this.Size.Width, this.Size.Height);
            fadeinPanel.Size = new Size(0, this.Size.Height);
            fadeoutPanel.Size = new Size(0, this.Size.Height);
            currentPanel.Size = new Size(3, this.Size.Height);

            wmp = new AxWMPLib.AxWindowsMediaPlayer();
            wmp.CreateControl();
            wmp.Ctlcontrols.stop();
            ChangeButtonText("Play");

            numericUpDown1.Maximum = (decimal)lengte;
            numericUpDown2.Maximum = (decimal)lengte;
            numericUpDown3.Maximum = (decimal)lengte;
            numericUpDown3.Value = (decimal)lengte;

            UpdateSelection();

        }


        #region "Properties"

        public double FadeInLength
        {
            get { return (double)numericUpDown4.Value; }
            //set { numericUpDown4.Value = (decimal)value; }
        }

        public double FadeOutLength
        {
            get { return (double)numericUpDown5.Value; }
            //set { numericUpDown5.Value = (decimal)value; }
        }

        public double BegintijdFragment
        {
            get
            {
                return (double)(numericUpDown2.Value);// - (checkBox1.Checked ? 0 : numericUpDown4.Value));
            }
        }

        public double EindtijdFragment
        {
            get
            {
                return (double)(numericUpDown3.Value);// + (checkBox2.Checked ? 0 : numericUpDown5.Value));
            }
        }

        public bool FIBinnen
        {
            get { return checkBox1.Checked; }
        }

        public bool FOBinnen
        {
            get { return checkBox2.Checked; }
        }

        public bool DisplayMode
        {
            get { return this.displaymode; }
            set { this.displaymode = value; SetDisplayMode(); }
        }

        #endregion

        public bool IsPlaying()
        {
            if(wmp != null)
            if (wmp.playState == WMPLib.WMPPlayState.wmppsPlaying || wmp.playState == WMPLib.WMPPlayState.wmppsBuffering)
                return true;

            return false;
        }

        private void SetDisplayMode()
        {
            panel2.Enabled = !displaymode;
            if (displaymode)
            {
                panel3.Size = new Size(0, 50);
                this.Size = new Size(this.Size.Width, panel1.Size.Height + panel3.Size.Height + 10);
                panel2.Size = new Size(0, 0);
                this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            }
            else
            {
                this.Size = new Size(683, 81);
                panel2.Size = new Size(683, 67);
                panel3.Size = new Size(0, 0);
                this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            }
        }

        void wmp_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 3)
            {
                ChangeButtonText("Pauze");
            }
            else if (e.newState == 1 || e.newState == 2)
            {
                ChangeButtonText("Play");
            }
        }

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Play")
            {
                wmp.URL = filename;
                stopTime = -1;
                wmp.Ctlcontrols.play();
                timer1.Enabled = true;
            }
            else
            {
                wmp.Ctlcontrols.pause();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            wmp.Ctlcontrols.stop();
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (stopTime != -1)
                if (numericUpDown1.Value >= stopTime)
                    wmp.Ctlcontrols.stop();

            if (!timer2.Enabled)
                timer2.Enabled = true;

            numericUpDown1.Value = (decimal)wmp.Ctlcontrols.currentPosition;
            label4.Text = (wmp.Ctlcontrols.currentPosition - this.beginTime).ToString("0.00") + " / " + totalLength.ToString("0.00");
               
            currentPanel.Location = new Point((int)(this.Size.Width / lengte * wmp.Ctlcontrols.currentPosition) - 1, 0);

            //bereken het volume
            if (stopTime != -1) // Alleen als het play selectie is
            {
                //Controleer of de huidige cursor positie in de fadein tijd is
                if ((decimal)wmp.Ctlcontrols.currentPosition < numericUpDown2.Value + numericUpDown4.Value && checkBox1.Checked)
                {
                    // binnen de fade in tijd
                    wmp.settings.volume = (int)(100 / ((double)numericUpDown4.Value * 1000) * ((wmp.Ctlcontrols.currentPosition - (double)numericUpDown2.Value) * 1000));
                }
                else if ((decimal)wmp.Ctlcontrols.currentPosition < numericUpDown2.Value && !checkBox1.Checked)
                {
                    // binnen de fade in tijd
                    wmp.settings.volume = 100 - (int)(100 / ((double)numericUpDown4.Value * 1000) * (((double)numericUpDown2.Value - wmp.Ctlcontrols.currentPosition) * 1000));
                }
                //Controleer of de huidige cursor positie in de fadeout tijd is
                else if ((decimal)wmp.Ctlcontrols.currentPosition > numericUpDown3.Value - numericUpDown5.Value && checkBox2.Checked)
                {
                    // binnen de fade out tijd
                    wmp.settings.volume = (int)(100 / ((double)numericUpDown5.Value * 1000) * (((double)numericUpDown3.Value - wmp.Ctlcontrols.currentPosition) * 1000));
                }
                //Controleer of de huidige cursor positie in de fadeout tijd is
                else if ((decimal)wmp.Ctlcontrols.currentPosition > numericUpDown3.Value && !checkBox2.Checked)
                {
                    // binnen de fade out tijd
                    wmp.settings.volume = 100 - (int)(100 / ((double)numericUpDown5.Value * 1000) * ((wmp.Ctlcontrols.currentPosition - (double)numericUpDown3.Value) * 1000));
                }
                else
                    wmp.settings.volume = 100;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            numericUpDown2.Value = numericUpDown1.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            numericUpDown3.Value = numericUpDown1.Value;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            stopTime = (numericUpDown3.Value + (checkBox2.Checked ? 0 : numericUpDown5.Value));
            wmp.Ctlcontrols.play();
            double wantedCusorPosition = (double)(numericUpDown2.Value - (checkBox1.Checked ? 0 : numericUpDown4.Value));

        set_again:
            wmp.Ctlcontrols.currentPosition = wantedCusorPosition;
            if (wmp.Ctlcontrols.currentPosition != wantedCusorPosition)
                goto set_again;

            timer1.Enabled = true;
        }

        private void selectionPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (displaymode)
                return;

            int toAdd = 0;
            if (((Panel)sender).Name == "selectionPanel")
            {
                toAdd += selectionPanel.Location.X;
            }
            else if (((Panel)sender).Name == "fadeinPanel")
            {
                toAdd += selectionPanel.Location.X;
                toAdd += fadeinPanel.Location.X;
            }
            else if (((Panel)sender).Name == "fadeoutPanel")
            {
                toAdd += selectionPanel.Location.X;
                toAdd += fadeoutPanel.Location.X;
            }
            currentPanel.Location = new Point(e.Location.X + toAdd, 0);
            wmp.Ctlcontrols.currentPosition = lengte / this.Size.Width * currentPanel.Location.X;
            numericUpDown1.Value = (decimal)lengte / this.Size.Width * currentPanel.Location.X;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value > numericUpDown3.Value)
                return;

            SetFadeMax();

            UpdateSelection();
            if (DoNotTriggerUpdateEvent)
                OnWaardesAangepast();
        }

        private void SetFadeMax()
        {
            //stel het maximum in voor de fade in
            if (checkBox1.Checked)
            {
                numericUpDown4.Maximum = numericUpDown3.Value - numericUpDown2.Value - (checkBox2.Checked ? numericUpDown5.Value : 0);
                if (numericUpDown4.Maximum == 0)
                {
                    numericUpDown4.Value = 0;
                    numericUpDown5.Value = 0;
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                }
            }
            else
            {
                numericUpDown4.Maximum = numericUpDown2.Value;
            }

            //stel het maximum in voor de fade out
            if (checkBox2.Checked)
            {
                numericUpDown5.Maximum = numericUpDown3.Value - numericUpDown2.Value - (checkBox1.Checked ? numericUpDown4.Value : 0);
                if (numericUpDown5.Maximum == 0)
                {
                    numericUpDown4.Value = 0;
                    numericUpDown5.Value = 0;
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                }
            }
            else
            {
                numericUpDown5.Maximum = (decimal)lengte - numericUpDown3.Value;
            }
        }

        public void UpdateSelection()
        {
            //Het start en eindpunt van de selectie
            double startPoint = (double)numericUpDown2.Value;
            double endPoint = (double)numericUpDown3.Value;

            //Bereken of er een extra lengte moet worden toegevoegd aan de selectie voor de fadein en fadeout
            double fadeinLength_toadd = (double)(checkBox1.Checked ? 0 : numericUpDown4.Value);
            double fadeoutLength_toadd = (double)(checkBox2.Checked ? 0 : numericUpDown5.Value);

            //De lengte van het fadein en fadeout fragment
            double fadeinLength = (double)numericUpDown4.Value;
            double fadeoutLength = (double)numericUpDown5.Value;

            // Bereken de lengte van het fragment dat de selectie vormt
            double length = endPoint + fadeoutLength_toadd + fadeinLength_toadd - startPoint;

            selectionPanel.Location = new Point((int)(this.Size.Width / lengte * (startPoint - fadeinLength_toadd)), 0);
            selectionPanel.Size = new Size((int)(this.Size.Width / lengte * length), panel1.Size.Height);
            fadeinPanel.Size = new Size((int)(this.Size.Width / lengte * fadeinLength), panel1.Size.Height);
            fadeoutPanel.Size = new Size((int)(this.Size.Width / lengte * fadeoutLength), panel1.Size.Height);

            totalLength = length;
            this.beginTime = startPoint - fadeinLength_toadd;

            //Zet de cursor op de juiste plek
            currentPanel.Location = new Point((int)(this.Size.Width / lengte * (double)numericUpDown1.Value) - 1, 0);
        }

        private void playSelectieVanafHuidigeLocatieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopTime = (numericUpDown3.Value + (checkBox2.Checked ? 0 : numericUpDown5.Value));
            wmp.Ctlcontrols.play();
            wmp.Ctlcontrols.currentPosition = (double)(numericUpDown1.Value);
            timer1.Enabled = true;
        }

        private void ChangeButtonText(string newText)
        {
            if (newText == "Pauze")
            {
                button1.Text = "Pauze";
            }
            else
            {
                button1.Text = "Play";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetFadeMax();

            UpdateSelection();

            if (DoNotTriggerUpdateEvent)
                OnWaardesAangepast();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UpdateSelection();
        }

        public event WaardesAangepastEventHandler WaardesAangepast;
        public delegate void WaardesAangepastEventHandler(MediaPlayer sender);
        public void OnWaardesAangepast()
        {
            if (WaardesAangepast != null)
                WaardesAangepast(this);
        }

        public void SetFragmentProperties(DateTime BegintijdFragment, DateTime EindtijdFragment, DateTime FadeIn, DateTime FadeOut, bool FIBinnen, bool FOBinnen)
        {
            if (BegintijdFragment == new DateTime(1990, 1, 1))
                return;

            numericUpDown2.Value = (decimal)(BegintijdFragment - new DateTime(2000, 1, 1)).TotalSeconds;
            numericUpDown3.Value = (decimal)(EindtijdFragment - new DateTime(2000, 1, 1)).TotalSeconds;
            checkBox1.Checked = FIBinnen;
            checkBox2.Checked = FOBinnen;

            SetFadeMax();

            numericUpDown4.Value = (decimal)(FadeIn - new DateTime(2000, 1, 1)).TotalSeconds;
            numericUpDown5.Value = (decimal)(FadeOut - new DateTime(2000, 1, 1)).TotalSeconds;
            numericUpDown2.Value -= (FIBinnen ? 0 : numericUpDown4.Value);
            numericUpDown3.Value -= (FOBinnen ? 0 : numericUpDown5.Value);

            label4.Text = "0.00 / " + (EindtijdFragment - BegintijdFragment).TotalSeconds.ToString("0.00");

            UpdateSelection();

            if (DoNotTriggerUpdateEvent)
            {
                OnWaardesAangepast();
            }
            else
            {
                OnRefeshData();
            }
        }

        private bool FadeOutFound = false;
        public event FadeOutStartedEventHandler FadeOutStarted;
        public delegate void FadeOutStartedEventHandler(MediaPlayer sender);
        public void OnFadeOutStarted()
        {
            FadeOutFound = true;
            if (FadeOutStarted != null)
                FadeOutStarted(this);
        }

        private bool FadeInFound = false;
        public event FadeInEndedEventHandler FadeInEnded;
        public delegate void FadeInEndedEventHandler(MediaPlayer sender);
        public void OnFadeInEnded()
        {
            FadeInFound = true;
            if (FadeInEnded != null)
                FadeInEnded(this);
        }

        public event PlaybackStoppedEventHandler PlaybackStopped;
        public delegate void PlaybackStoppedEventHandler(MediaPlayer sender);
        public void OnPlaybackStopped()
        {
            if (PlaybackStopped != null)
                PlaybackStopped(this);
        }

        public event RefeshDataEventHandler RefeshData;
        public delegate void RefeshDataEventHandler(MediaPlayer sender);
        public void OnRefeshData()
        {
            if (RefeshData != null)
                RefeshData(this);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
                timer2.Enabled = false;

            if (wmp.playState != WMPLib.WMPPlayState.wmppsPaused)
            {
                if ((decimal)wmp.Ctlcontrols.currentPosition > numericUpDown3.Value - (checkBox2.Checked ? numericUpDown5.Value : 0) && !FadeOutFound)
                {
                    OnFadeOutStarted();
                }

                if ((decimal)wmp.Ctlcontrols.currentPosition > numericUpDown2.Value + (checkBox1.Checked ? numericUpDown4.Value : 0) && !FadeInFound)
                {
                    OnFadeInEnded();
                }
            }

            if (wmp.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                OnPlaybackStopped();
                timer2.Enabled = false;
            }
        }

        public void Play()
        {
            wmp.Ctlcontrols.play();
            timer1.Enabled = true;
        }

        public void PlaySelectie(double cursorStartPosition = -1)
        {
            button5_Click(null, null);
            if (cursorStartPosition != -1)
                wmp.Ctlcontrols.currentPosition = cursorStartPosition;
        }

        public void Pauze()
        {
            wmp.Ctlcontrols.pause();
        }

        public void Stop()
        {
            button2_Click(null, null);
        }

        public PlayState GetPlayState()
        {
            if (wmp.playState == WMPLib.WMPPlayState.wmppsPlaying)
                return PlayState.Playing;
            else if (wmp.playState == WMPLib.WMPPlayState.wmppsReady)
                return PlayState.Playing;
            else if (wmp.playState == WMPLib.WMPPlayState.wmppsPaused)
                return PlayState.Paused;
            else if (wmp.URL == "")
                return PlayState.Undefined;
            else
                return PlayState.Stopped;
        }

        //public void Dispose()
        //{
        //    //wmp.URL = null;
        //    wmp.close();
        //    //wmp.Dispose();
        //    //System.IO.File.Delete(this.filename);
        //}
    }

    public enum PlayState
    {
        Playing,
        Paused,
        Stopped,
        Undefined
    }
}
