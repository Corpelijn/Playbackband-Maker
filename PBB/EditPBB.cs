using Businesslayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PBB
{
    public partial class EditPBB : Form
    {
        private Dictionary<int, string> Roman;

        private List<BlokInfo> bloks;

        private List<int> legeBlokken;
        private List<int[]> legeFragmenten;

        public EditPBB(Playbackband pbb)
        {
            InitializeComponent();

            Roman = new Dictionary<int, string>();
            Roman.Add(1, "I");
            Roman.Add(2, "II");
            Roman.Add(3, "III");
            Roman.Add(4, "IV");
            Roman.Add(5, "V");
            Roman.Add(6, "VI");
            Roman.Add(7, "VII");
            Roman.Add(8, "VIII");
            bloks = new List<BlokInfo>();

            legeBlokken = new List<int>();
            legeFragmenten = new List<int[]>();
        }

        private void AnalysePBB(Playbackband pbb)
        {
            // Vind lege blokken en fragmenten en markeer deze
            for (int i = 0; i < pbb.Blokken.Count; i++)
            {
                bool emptyBlok = true;
                for (int j = 0; j < pbb.Blokken[i].Fragmenten.Count; j++)
                {
                    if (pbb.Blokken[i].Fragmenten[j].IsDummy())
                    {
                        legeFragmenten.Add(new int[] { i, j });
                    }
                    else emptyBlok = false;
                }

                if (emptyBlok)
                {
                    legeBlokken.Add(i);
                }
            }
        }

        public string Filename
        {
            get { return label3.Text; }
            set { label3.Text = value; }
        }

        public System.Windows.Forms.Control.ControlCollection Blokken
        {
            get { return panel2.Controls; }
        }

        private void newPBB_Load(object sender, EventArgs e)
        {
            numericUpDown1_ValueChanged(null, null);
        }

        void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void CreateBlock(string title, int aantal = 10)
        {
            BlokInfo bi = new BlokInfo();
            bi.SetInitValues(title.ToUpper(), aantal);
            //bi.Naam = title.ToUpper();
            //bi.AantalNummers = aantal;
            bi.Parent = panel2;
            bi.Dock = DockStyle.Top;
            bi.BringToFront();
            bloks.Add(bi);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
