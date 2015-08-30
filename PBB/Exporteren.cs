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
    public partial class Exporteren : Form
    {
        private Businesslayer.Playbackband currentPBB;

        public Exporteren(Businesslayer.Playbackband pbb)
        {
            InitializeComponent();

            currentPBB = pbb;
        }

        public string Filename
        {
            get { return (radioButton1.Checked ? textBox1.Text : textBox2.Text); }
        }

        public bool Multifiles
        {
            get { return radioButton2.Checked; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                panel2.Enabled = false;
                panel1.Enabled = true;
            }
            else
            {
                panel2.Enabled = true;
                panel1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.None;
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "MP3 bestanden|*.mp3";
            save.FileName = System.IO.Path.GetFileNameWithoutExtension(currentPBB.Filename) + ".mp3";
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            textBox1.Text = save.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.None;
            FolderBrowserDialog save = new FolderBrowserDialog();
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            textBox2.Text = save.SelectedPath;
        }
    }
}
