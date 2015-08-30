using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PBB
{
    public partial class newPBB : Form
    {
        private Dictionary<int, string> Roman;

        private List<BlokInfo> bloks;

        public newPBB()
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
            bloks.Clear();
            panel2.Controls.Clear();

            CreateBlock("Intro", 1);

            for (int i = 1; i < numericUpDown1.Value - 1; i++)
            {
                string roman;
                Roman.TryGetValue(i, out roman);
                CreateBlock("Blok " + roman);
            }

            CreateBlock("Finale", 1);

            bloks.Sort();
            bi_PositionUpdated(null);
        }

        private void CreateBlock(string title, int aantal = 10)
        {
            BlokInfo bi = new BlokInfo();
            bi.SetInitValues(title.ToUpper(), aantal);
            //bi.Naam = title.ToUpper();
            //bi.AantalNummers = aantal;
            //bi.Parent = panel2;
            bi.PositionID = bloks.Count * 10;
            bi.Dock = DockStyle.Top;
            bi.PositionUpdated += bi_PositionUpdated;
            //bi.BringToFront();
            bloks.Add(bi);
        }

        void bi_PositionUpdated(BlokInfo sender)
        {
            panel2.Controls.Clear();
            bloks.Sort();

            foreach (BlokInfo bi in bloks)
            {
                bi.PositionUpdated -= bi_PositionUpdated;
                panel2.Controls.Add(bi);
                bi.PositionID = panel2.Controls.Count * 10;
                bi.BringToFront();
                bi.PositionUpdated += bi_PositionUpdated;
            }
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
