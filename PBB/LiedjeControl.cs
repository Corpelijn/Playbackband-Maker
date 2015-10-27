using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Businesslayer;
using System.Runtime.InteropServices;

namespace PBB
{
    public partial class LiedjeControl : UserControl
    {
        [DllImport("user32.dll")]
        static extern bool LockWindowUpdate(IntPtr hWndLock);


        public LiedjeControl(Playbackband current)
        {
            InitializeComponent();

            label4.Text = "";
            comboBox1.SelectedIndex = 0;
            this.currentPBB = current;
        }

        private bool setPlayerActive = false;
        private MediaPlayer mp = null;
        private string filename = "";
        private Playbackband currentPBB;

        public LiedjeControl(Fragment song, Playbackband current)
        {
            InitializeComponent();

            label4.Text = "";
            liedje = song;
            this.rodeDraad = song.RodeDraad;
            this.currentPBB = current;

            DateTime t = new DateTime(1990,1,1);
            if (liedje.BeginTijd != t)
                label3.Text = GetTimeFromDouble((liedje.BeginTijd - new DateTime(2000, 1, 1)).TotalSeconds).ToString("mm.ss") + " - " + GetTimeFromDouble((liedje.EindTijd - new DateTime(2000, 1, 1)).TotalSeconds).ToString("mm.ss");
            else
                label3.Text = "00.00 - 00.00";
            
            UpdateView();

            comboBox1.SelectedIndex = this.rodeDraad;
        }

        private int rodeDraad = 0;
        private Fragment liedje;

        public int RodeDraad
        {
            get { return rodeDraad; }
            set { rodeDraad = value; UpdateView(); }
        }

        public string Blok
        {
            get { return label4.Text; }
            set { label4.Text = value; UpdateView(); }
        }

        public int ID
        {
            get { return Convert.ToInt32(label1.Text); }
            set { label1.Text = value.ToString("00"); UpdateView(); }
        }

        public bool SetPlayerActive
        {
            get { return setPlayerActive; }
            set { setPlayerActive = value; UpdatePlayer(); }
        }

        public Fragment Liedje
        {
            get { return this.liedje; }
            set { this.liedje = value; }
        }

        private delegate void DelegateAddMuziek(string filename);
        private DelegateAddMuziek delegateAddMuziek;

        private void AddMuziek(string filename)
        {
            Form1 f = ((Form1)this.Parent.Parent);
            f.AddMuziek(false, filename, Convert.ToInt32(label1.Text));
        }

        private void LiedjeControl_Load(object sender, EventArgs e)
        {
            button1_Click(null, null);

            delegateAddMuziek = new DelegateAddMuziek(AddMuziek);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Uitvouwen")
            {
                button1.Text = "Inklappen";
                if (liedje.IsDummy())
                    this.Size = new Size(this.Size.Width, 30 + 24);
                else
                    this.Size = new Size(this.Size.Width, 142);
                this.SetPlayerActive = true;
                this.UpdatePlayer();
            }
            else
            {
                button1.Text = "Uitvouwen";
                this.Size = new Size(this.Size.Width, 24);
                this.SetPlayerActive = false;
            }
        }

        private void UpdateView()
        {
            if (label4.Text != "")
            {
                label2.Font = new Font(label2.Font, FontStyle.Bold);
                label4.Font = new Font(label4.Font, FontStyle.Bold);
            }

            if (liedje != null)
            {
                label2.Text = liedje.Liedje.ToString();
            }

            if(currentlyPlaying)
                panel2.BackColor = Color.ForestGreen;
            else if(rodeDraad != 0)
                panel2.BackColor = Color.Salmon;
            else
                panel2.BackColor = SystemColors.Control;

            if (rodeDraad != 0)
            {
                OnRequestRodeDraad();
            }
        }

        private void UpdatePlayer()
        {
            if (setPlayerActive)
            {
                if (liedje != null)
                {
                    if (!liedje.IsDummy())
                    {
                        if (this.filename == "")
                        {
                            this.filename = liedje.Liedje.WriteToFile(File.defaultDir + "\\" + DateTime.Now.Ticks.ToString() + ".mp3");

                            mp = new MediaPlayer(liedje.Liedje, this.filename);
                            mp.WaardesAangepast += mp_WaardesAangepast;
                            mp.RefeshData += mp_RefeshData;
                            mp.Parent = panel7;
                            mp.Location = new Point(10, 35);
                            mp.SetFragmentProperties(liedje.BeginTijd, liedje.EindTijd, liedje.FadeIn, liedje.FadeOut, liedje.FadeInBinnen, liedje.FadeOutBinnen);

                            //mp_WaardesAangepast(mp);
                        }
                    }
                }
            }
            else
            {
                if (mp != null)
                {
                    //mp.Dispose();
                    //mp = null;
                    //System.IO.File.Delete(filename);
                }
            }
        }

        void mp_RefeshData(MediaPlayer sender)
        {
            label3.Text = GetTimeFromDouble(sender.BegintijdFragment).ToString("mm.ss") + " - " + GetTimeFromDouble(sender.EindtijdFragment).ToString("mm.ss");
        }

        public void UpdateFragmentProperties()
        {
            mp.DoNotTriggerUpdateEvent = false;
            mp.SetFragmentProperties(liedje.BeginTijd.AddSeconds(liedje.FadeInBinnen ? 0 : (liedje.FadeIn - new DateTime(2000, 1, 1)).TotalSeconds * 2), liedje.EindTijd, liedje.FadeIn, liedje.FadeOut, liedje.FadeInBinnen, liedje.FadeOutBinnen);
            mp.DoNotTriggerUpdateEvent = true;

            UpdatePlayer();
        }

        private void mp_WaardesAangepast(MediaPlayer sender)
        {
            liedje.BeginTijd = GetTimeFromDouble(sender.BegintijdFragment);
            liedje.EindTijd = GetTimeFromDouble(sender.EindtijdFragment);
            liedje.FadeIn = GetTimeFromDouble(sender.FadeInLength);
            liedje.FadeOut = GetTimeFromDouble(sender.FadeOutLength);
            liedje.FadeInBinnen = sender.FIBinnen;
            liedje.FadeOutBinnen = sender.FOBinnen;
            liedje.RodeDraad = this.rodeDraad;

            label3.Text = GetTimeFromDouble(sender.BegintijdFragment).ToString("mm.ss") + " - " + GetTimeFromDouble(sender.EindtijdFragment).ToString("mm.ss");

            if (RodeDraad == 1)
            {
                OnRodeDraadChanged();
            }
        }

        private DateTime GetTimeFromDouble(double seconds)
        {
            int HH = 0;
            int MM = 0;
            int SS = 0;
            int MI = 0;

            double time = seconds;
            HH = (int)Math.Floor(time / 3600); // 3600 is aantal seconden in een uur
            time = time - (HH * 3600);
            MM = (int)Math.Floor(time / 60);
            time = time - (MM * 60);
            SS = (int)Math.Floor(time);
            time = time - SS;
            MI = (int)Math.Floor(time * 1000);
            return new DateTime(2000, 1, 1, HH, MM, SS, MI);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //TOOD: this.RodeDraad = checkBox1.Checked;
        }

        public bool IsDummy()
        {
            return liedje.IsDummy();
        }

        private bool currentlyPlaying = false;
        public bool CurrentlyPlaying
        {
            get { return currentlyPlaying; }
            set { currentlyPlaying = value; UpdateView();  }
        }

        private void LiedjeControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void LiedjeControl_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Array a = (Array)e.Data.GetData(DataFormats.FileDrop);

                if (a != null)
                {
                    // Extract string from first array element
                    // (ignore all files except first if number of files are dropped).
                    string s = a.GetValue(0).ToString();

                    // Call OpenFile asynchronously.
                    // Explorer instance from which file is dropped is not responding
                    // all the time when DragDrop handler is active, so we need to return
                    // immidiately (especially if OpenFile shows MessageBox).                    

                    this.BeginInvoke(delegateAddMuziek, new Object[] { s });

                    ((Form)this.Parent.Parent).Activate();        // in the case Explorer overlaps this form
                }
            }
            catch// (Exception ex)
            {
                //Trace.WriteLine("Error in DragDrop function: " + ex.Message);

                // don't show MessageBox here - Explorer is waiting !
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rodeDraad = comboBox1.SelectedIndex;

            UpdateRodeDraad();

            if(this.mp != null)
                mp_WaardesAangepast(this.mp);

            this.UpdateView();
        }

        private void UpdateRodeDraad()
        {
            // 
            OnRequestRodeDraad();



            this.UpdateView();
        }

        public event RequestRodeDraadEventHandler RequestRodeDraad;
        public delegate void RequestRodeDraadEventHandler(LiedjeControl sender);
        private void OnRequestRodeDraad()
        {
            if (RequestRodeDraad != null)
            {
                RequestRodeDraad(this);
            }

            button1_Click(null, null);
            button1_Click(null, null);
        }

        public event RodeDraadChangedEventHandler RodeDraadChanged;
        public delegate void RodeDraadChangedEventHandler(LiedjeControl sender);
        private void OnRodeDraadChanged()
        {
            if (RodeDraadChanged != null)
            {
                RodeDraadChanged(this);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Weet je zeker dat je het muziek fragment wil verwijderen?", "Playbackband maker", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //OnRenewMe();
                comboBox1.SelectedIndex = 0;

                this.currentPBB.VerwijderFragment(this.liedje);
                this.filename = "";
                this.liedje = new Fragment(this.liedje.Nummer);
                this.mp = null;

                UpdateView();

                button1_Click(null, null);
                button1_Click(null, null);
            }
        }

        public event RenewMeEventHandler RenewMe;
        public delegate void RenewMeEventHandler(LiedjeControl sender);
        private void OnRenewMe()
        {
            if (RenewMe != null)
            {
                RenewMe(this);
            }
        }
    }
}
