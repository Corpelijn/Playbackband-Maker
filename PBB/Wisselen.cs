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
    public partial class Wisselen : Form
    {
        private Playbackband pbb;

        public Wisselen(Playbackband currentPBB)
        {
            InitializeComponent();

            int nummers_in_pbb = 0;
            for (int i = 0; i < currentPBB.Blokken.Count; i++)
            {
                nummers_in_pbb += currentPBB.Blokken[i].AantalInBlok;
            }

            numericUpDown1.Maximum = nummers_in_pbb;
            numericUpDown2.Maximum = nummers_in_pbb - 1;

            pbb = currentPBB;

            numericUpDown1.Value = 1;
            numericUpDown2.Value = 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == numericUpDown2.Value)
                return;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            label2.Text = getTitelFromId((int)numericUpDown1.Value);
        }

        private string getTitelFromId(int id)
        {
            int last_index = 0;
            for (int i = 0; i < pbb.Blokken.Count; i++)
            {
                if (id > pbb.Blokken[i].AantalInBlok)
                {
                    id -= pbb.Blokken[i].AantalInBlok;
                    last_index++;
                }
                else if (id < pbb.Blokken[i].AantalInBlok)
                {
                    id--;
                    break;
                }
            }

            return pbb.Blokken[last_index].Fragmenten[id].Liedje.Artiest + " - " + pbb.Blokken[last_index].Fragmenten[id].Liedje.Titel;
        }

        public int OriginalNumber
        {
            get { return (int)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        public int DestinationNumber
        {
            get { return (int)numericUpDown2.Value; }
            set { numericUpDown2.Value = value; }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            label4.Text = getTitelFromId((int)numericUpDown2.Value);
        }
    }
}
