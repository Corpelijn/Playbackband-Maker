using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PBB
{
    public partial class AddMuziek : Form
    {
        public AddMuziek()
        {
            InitializeComponent();
        }

        public string Artiest
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }

        public int Nummer
        {
            get { return (int)numericUpDown1.Value; }
            set { numericUpDown1.Value = (decimal)value; }
        }

        public int NummerMax
        {
            get { return (int)numericUpDown1.Maximum; }
            set { numericUpDown1.Maximum = (decimal)value; }
        }

        public string Titel
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public bool EnableNumberSelection
        {
            set { label3.Enabled = value; numericUpDown1.Enabled = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string temp = textBox1.Text;
            textBox1.Text = textBox2.Text;
            textBox2.Text = temp;
        }
    }
}
