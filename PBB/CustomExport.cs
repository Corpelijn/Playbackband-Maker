using Businesslayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PBB
{
    public partial class CustomExport : Form
    {
        private DateTime startTime;

        public CustomExport(Playbackband pbb, string filename, bool multifiles)
        {
            InitializeComponent();

            PBBRenderer.Instance = new PBBRenderer(File.defaultDir, filename, multifiles);
            PBBRenderer.Instance.Updated += Instance_Updated;
            WAVFile.Updated += Instance_Updated;

            PBBRenderer.Instance.Start();
            startTime = DateTime.Now;
            timer1.Enabled = true;

            for (int i = 0; i < pbb.Blokken.Count; i++)
            {
                for (int j = 0; j < pbb.Blokken[i].Fragmenten.Count; j++)
                {
                    if(!pbb.Blokken[i].Fragmenten[j].IsDummy())
                        PBBRenderer.Instance.AddFragment(pbb.Blokken[i].Fragmenten[j]);
                }
            }

            PBBRenderer.Instance.StopWhenDone();
        }

        void Instance_Updated()
        {
            double a = (PBBRenderer.Instance.TotalFragmentsActions + 100);
            double b = (PBBRenderer.Instance.FragmentActionsDone + WAVFile.GetAmountDone());
            double p = 100 / a * b;
            string status = ((int)p).ToString() + "% - " ;

            string time = (DateTime.Now - startTime).Hours.ToString("00") + ":" + (DateTime.Now - startTime).Minutes.ToString("00") + ":" + (DateTime.Now - startTime).Seconds.ToString("00");

            if (PBBRenderer.Instance.ActiveStatus == 0) status += "MP3 bestanden voorbereiden. . .";
            else if (PBBRenderer.Instance.ActiveStatus == 1) status += "Audio mixen . . .";
            else if (PBBRenderer.Instance.ActiveStatus == 2) status += "Mix afronden . . .";
            else if (PBBRenderer.Instance.ActiveStatus == 3) status += "Converteren naar MP3 . . .";
            else if (PBBRenderer.Instance.ActiveStatus == 4) status += "MP3 knippen . . .";
            else if (PBBRenderer.Instance.ActiveStatus == 5) status += "Opruimen . . .";
            else if (PBBRenderer.Instance.ActiveStatus == 6) status += "Klaar";

            if (label1.InvokeRequired || progressBar1.InvokeRequired)
            {
                this.label3.BeginInvoke((MethodInvoker)delegate() { label3.Text = "Tijd gekost: " + time; });
                this.label2.BeginInvoke((MethodInvoker)delegate() { label2.Text = status; });
                this.label1.BeginInvoke((MethodInvoker)delegate() { label1.Text = (PBBRenderer.Instance.FragmentActionsDone + (int)WAVFile.GetAmountDone()) + "/" + (PBBRenderer.Instance.TotalFragmentsActions + 100); });
                this.progressBar1.BeginInvoke((MethodInvoker)delegate() { progressBar1.Maximum = PBBRenderer.Instance.TotalFragmentsActions + 100; });
                this.progressBar1.BeginInvoke((MethodInvoker)delegate() { progressBar1.Value = PBBRenderer.Instance.FragmentActionsDone + (int)WAVFile.GetAmountDone(); });
            }
            else
            {
                label3.Text = "Tijd gekost: " + time;
                label2.Text = status;
                label1.Text = (PBBRenderer.Instance.FragmentActionsDone + (int)WAVFile.GetAmountDone()) + "/" + (PBBRenderer.Instance.TotalFragmentsActions + 100);
                progressBar1.Maximum = PBBRenderer.Instance.TotalFragmentsActions + 100;
                progressBar1.Value = PBBRenderer.Instance.FragmentActionsDone + (int)WAVFile.GetAmountDone();
            }

            Application.DoEvents();
        }

        private void CustomExport_FormClosing(object sender, FormClosingEventArgs e)
        {
            PBBRenderer.Instance.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Instance_Updated();
            if (PBBRenderer.Instance.ActiveStatus == 6)
            {
                timer1.Enabled = false;
            }
        }
    }
}
