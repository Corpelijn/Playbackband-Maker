using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Businesslayer;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace PBB
{
    public partial class Form1 : Form
    {
        private Playbackband currentPBB;

        [DllImport("user32.dll")]
        static extern bool LockWindowUpdate(IntPtr hWndLock);

        public Form1()
        {
            InitializeComponent();

            WorkingControlsEnabled(false);

            if (System.IO.File.Exists(File.mainDir + "\\AutoSave.pbb"))
            {
                //if (MessageBox.Show("Er is iets mis gegaan tijdens het afsluiten van de applicatie de laatste keer dat de applicatie gestart is. Er is een herstel bestand beschikbaar.\n\nWilt u dit bestand herstellen?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                //{
                //    backgroundWorker1.RunWorkerAsync();

                //    File f = new File(File.mainDir + "\\AutoSave.pbb");
                //    currentPBB = f.Open();
                //    WorkingControlsEnabled(true);

                //    UpdateView();

                //    backgroundWorker1.CancelAsync();
                //}
            }
        }

        private void opslaanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();

            currentPBB.Save();

            backgroundWorker1.CancelAsync();
        }

        private void openenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.FileName = "";
            of.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            of.Filter = "Playbackband bestanden|*.pbb";
            if (of.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            backgroundWorker1.RunWorkerAsync();

            File f = new File(of.FileName);
            currentPBB = f.Open();

            WorkingControlsEnabled(true);

            UpdateView();

            backgroundWorker1.CancelAsync();
        }

        private void afsluitenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            
        }

        private void toevoegenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddMuziek(true);
        }

        public void AddMuziek(bool PickFilename, string filename = "", int prefixNummer = -1)
        {
            if (PickFilename)
            {
                OpenFileDialog of = new OpenFileDialog();
                of.FileName = "";
                of.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                of.Filter = "Muziek bestanden|*.mp3";
                if (of.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;
                filename = of.FileName;
            }

            backgroundWorker1.RunWorkerAsync();

            // vind het eerste nummer dat niet in gebruik is
            int index_not_in_use = 0;
            for (int i = panel1.Controls.Count - 1; i > - 1; i--)
            {
                if (((LiedjeControl)panel1.Controls[i]).IsDummy())
                {
                    index_not_in_use = panel1.Controls.Count - 1 - i;
                    break;
                }
            }
            int nummers_in_pbb = panel1.Controls.Count;
            
            //Controleer of het bestand bestaat
            if (!System.IO.File.Exists(filename))
            {
                throw new System.IO.FileNotFoundException("Het bestand kan niet gevonden worden!");
            }

            string[] fi = FileInfo.GetInfo(filename);

            AddMuziek am = new AddMuziek()
            {
                Artiest = fi[0],
                Titel = fi[1],
                Nummer = (prefixNummer != -1 ? prefixNummer : index_not_in_use + 1),
                NummerMax = nummers_in_pbb,
                EnableNumberSelection = (prefixNummer != -1 ? false : true)
            };

            backgroundWorker1.CancelAsync();

            if (am.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            backgroundWorker1.RunWorkerAsync();

            //Vind het blok waarin het nummer zit
            int index_to_use = am.Nummer - 1;
            int index_blok = 0;
            for (int i = 0; i < currentPBB.Blokken.Count; i++)
            {
                if (currentPBB.Blokken[i].AantalInBlok <= index_to_use)
                {
                    index_to_use -= currentPBB.Blokken[i].AantalInBlok;
                    index_blok++;
                }
                else
                    break;
            }

            currentPBB.Blokken[index_blok].Fragmenten[index_to_use].Liedje = new Liedje(am.Artiest, am.Titel, Convert.ToDouble(fi[2]), filename);

            UpdateView(am.Nummer - 1);

            backgroundWorker1.CancelAsync();
        }

        void WorkingControlsEnabled(bool status)
        {
            opslaanToolStripMenuItem.Enabled = status;
            muziekToolStripMenuItem.Enabled = status;
            toolStripMenuItem1.Enabled = status;
        }

        private void nieuwToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.FileName = "example.pbb";
            sf.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sf.Filter = "Playbackband bestanden|*.pbb";
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            newPBB npbb = new newPBB();
            npbb.Filename = sf.FileName;
            if (npbb.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            backgroundWorker1.RunWorkerAsync();

            currentPBB = new Playbackband(sf.FileName);

            for (int i = npbb.Blokken.Count -1; i > -1 ; i--)
            {
                currentPBB.AddBlok(((BlokInfo)npbb.Blokken[i]).Naam, (int)((BlokInfo)npbb.Blokken[i]).AantalNummers);
            }

            currentPBB.Save();

            WorkingControlsEnabled(true);

            UpdateView();

            backgroundWorker1.CancelAsync();
        }

        private void UpdateView(int indexToUpdate = -1)
        {
            LockWindowUpdate(this.Handle);

            if (indexToUpdate == -1)
            {
                panel1.Controls.Clear();
                int countnum = 1;
                for (int i = 0; i < currentPBB.Blokken.Count; i++)
                {
                    for (int j = 0; j < currentPBB.Blokken[i].AantalInBlok; j++)
                    {
                        LiedjeControl lc1 = new LiedjeControl(currentPBB.Blokken[i].Fragmenten[j]);
                        if (j == 0)
                        {
                            lc1.Blok = currentPBB.Blokken[i].Beschrijving;
                        }
                        lc1.Parent = panel1;
                        lc1.ID = countnum;
                        lc1.Dock = DockStyle.Top;
                        lc1.BringToFront();
                        lc1.RequestRodeDraad += lc1_RequestRodeDraad;
                        lc1.RodeDraadChanged += lc1_RodeDraadChanged;
                        lc1.RenewMe += lc1_RenewMe;
                        countnum++;
                    }

                }
            }
            else
            {
                //Vind het blok waarin het nummer zit
                int index_to_use = indexToUpdate;
                int index_blok = 0;
                for (int i = 0; i < currentPBB.Blokken.Count; i++)
                {
                    if (currentPBB.Blokken[i].AantalInBlok <= index_to_use)
                    {
                        index_to_use -= currentPBB.Blokken[i].AantalInBlok;
                        index_blok++;
                    }
                    else
                        break;
                }

                LiedjeControl lc1 = new LiedjeControl(currentPBB.Blokken[index_blok].Fragmenten[index_to_use]);
                if (index_to_use == 0)
                {
                    lc1.Blok = currentPBB.Blokken[index_blok].Beschrijving;
                }
                lc1.Parent = panel1;
                lc1.ID = indexToUpdate + 1;
                lc1.Dock = DockStyle.Top;
                lc1.BringToFront();

                panel1.Controls.SetChildIndex(lc1, panel1.Controls.Count - 1 - indexToUpdate);
                panel1.Controls.RemoveAt(panel1.Controls.Count - 2 - indexToUpdate);
            }

            LockWindowUpdate(IntPtr.Zero);
        }

        void lc1_RenewMe(LiedjeControl sender)
        {
            for (int i = 0; i < panel1.Controls.Count; i++)
            {
                if (panel1.Controls[i] == sender)
                {
                    //TODO: Replace control 
                }
            }
        }

        void lc1_RodeDraadChanged(LiedjeControl sender)
        {
            for (int i = 0; i < panel1.Controls.Count; i++)
            {
                LiedjeControl lc = (LiedjeControl)panel1.Controls[i];
                if (lc.RodeDraad == 1 && sender != lc)
                {
                    lc.Liedje = sender.Liedje;
                    lc.UpdateFragmentProperties();
                }
            }
        }

        void lc1_RequestRodeDraad(LiedjeControl sender)
        {
            for (int i = 0; i < currentPBB.Blokken.Count; i++)
            {
                for (int j = 0; j < currentPBB.Blokken[i].Fragmenten.Count; j++)
                {
                    if (currentPBB.Blokken[i].Fragmenten[j].RodeDraad == 1)
                    {
                        sender.Liedje = currentPBB.Blokken[i].Fragmenten[j];
                        break;
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //remove all directories that are not running
            if (System.IO.Directory.Exists(File.mainDir))
            {
                Process[] processen = Process.GetProcessesByName("pbb.exe");
                string[] dirs = System.IO.Directory.GetDirectories(File.mainDir);

                for(int i = 0; i<dirs.Length; i++)
                {
                    for (int j = 0; j < processen.Length; j++)
                    {
                        if (dirs[i].EndsWith(processen[j].Id.ToString()))
                        {
                            goto found_dir;
                        }
                    }
                    //not found
                    System.IO.Directory.Delete(dirs[i], true);
                found_dir: ;
                }
            }

            if(System.IO.Directory.Exists(File.defaultDir))
                System.IO.Directory.Delete(File.defaultDir, true);

            System.IO.Directory.CreateDirectory(File.defaultDir);
        }

        private void allesAfspelenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (System.IO.Directory.Exists(File.defaultDir + "\\playall"))
            //    System.IO.Directory.Delete(File.defaultDir + "\\playall", true);

            allesAfspelenToolStripMenuItem.Enabled = false;

            string directory = File.defaultDir + "\\playall" + DateTime.Now.Ticks.ToString();
            System.IO.Directory.CreateDirectory(directory);

            PlayAll pa = new PlayAll(currentPBB, directory);
            pa.FormClosing += pa_FormClosing;

            pa.Show();
        }

        void pa_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (System.IO.Directory.Exists(File.defaultDir + "\\playall"))
            //    System.IO.Directory.Delete(File.defaultDir + "\\playall", true);

            allesAfspelenToolStripMenuItem.Enabled = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Loading l = new Loading();
            l.Location = new Point(this.Location.X + this.Size.Width / 2 - l.Size.Width / 2, this.Location.Y + this.Size.Height / 2 - l.Size.Height / 2);
            l.Show();

            while (!backgroundWorker1.CancellationPending)
            {
                Application.DoEvents();

                System.Threading.Thread.Sleep(10);
            }

            l.Hide();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Export ex = new Export(currentPBB);
            //ex.ShowDialog();
            
            Exporteren ex = new Exporteren(currentPBB);
            if (ex.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            CustomExport ce = new CustomExport(currentPBB, ex.Filename, ex.Multifiles);
            ce.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false; 
            if (currentPBB != null)
            {
                DialogResult reply = MessageBox.Show("Wilt u de wijzigingen opslaan voor het afsluiten?", "Playbackband maker", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (reply == System.Windows.Forms.DialogResult.Yes)
                {
                    currentPBB.Save();
                }
                else if (reply == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            Process p = new Process();
            p.StartInfo.FileName = "cleaner.exe";
            p.StartInfo.Arguments = "pid=" + Process.GetCurrentProcess().Id.ToString();
            p.Start();

            if (System.IO.File.Exists(File.mainDir + "\\AutoSave.pbb"))
            {
                System.IO.File.Delete(File.mainDir + "\\AutoSave.pbb");
            }
        }

        private void verplaatsenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Verplaats vp = new Verplaats(currentPBB);
            if (vp.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            currentPBB.VerplaatsNummer(vp.OriginalNumber, vp.DestinationNumber, vp.DestinationNumber2);

            UpdateView();
        }

        private void wisselenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Wisselen vp = new Wisselen(currentPBB);
            if (vp.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            currentPBB.VerwisselNummer(vp.OriginalNumber, vp.DestinationNumber);

            UpdateView();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (currentPBB != null)
            {
                //currentPBB.CreateAutoSave(File.mainDir + "\\autoSave.pbb");
            }
        }

        private void blokkenWijzigenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
