using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PBB
{
    public partial class BlokInfo : UserControl
    {
        private string placed_name;
        private decimal placed_amount;
        public bool controle;
        public int verwijder;

        public BlokInfo()
        {
            InitializeComponent();
            placed_name = "";
            placed_amount = -1;
        }

        public decimal AantalNummers
        {
            get { return numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        public string Naam
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public bool Controle
        {
            get { return controle; }
            set 
            { 
                controle = value;
                if (!controle)
                {
                    label3.Visible = false;
                    this.MaximumSize = new Size(440, 41);
                }
                else
                {
                    label3.Visible = true;
                    this.MaximumSize = new Size(0, 0);
                }
            }
        }

        public int VerwijderbareFragmenten
        {
            get { return verwijder; }
            set { verwijder = value; label3.Text = "Fragmenten die verwijderd kunnen worden: " + verwijder; }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.placed_name = textBox1.Text;
        }

        public void SetInitValues(string name, int amount)
        {
            textBox1.Text = name;
            numericUpDown1.Value = amount;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.placed_amount = numericUpDown1.Value;
        }
    }
}
