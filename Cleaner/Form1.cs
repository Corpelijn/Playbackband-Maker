using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cleaner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string pid = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] argv = Environment.GetCommandLineArgs();
            if (argv.Length > 1)
            {
                for (int i = 1; i < argv.Length; i++)
                {
                    if (argv[i].StartsWith("pid="))
                    {
                        pid = argv[i].Split(new char[] {'='})[1];
                    }
                }
            }

            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (true)
                {
                    if (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\playbackband\\playbackband" + pid))
                        System.IO.Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\playbackband\\playbackband" + pid, true);

                    if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\playbackband\\playbackband" + pid))
                        break;
                }
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }
    }
}
