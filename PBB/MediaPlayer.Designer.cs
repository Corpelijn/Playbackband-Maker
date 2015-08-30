namespace PBB
{
    partial class MediaPlayer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.currentPanel = new System.Windows.Forms.Panel();
            this.selectionPanel = new System.Windows.Forms.Panel();
            this.fadeinPanel = new System.Windows.Forms.Panel();
            this.fadeoutPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.playSelectieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playSelectieVanafHuidigeLocatieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.splitButton1 = new wyDay.Controls.SplitButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.selectionPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.currentPanel);
            this.panel1.Controls.Add(this.selectionPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(683, 14);
            this.panel1.TabIndex = 1;
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.selectionPanel_MouseClick);
            // 
            // currentPanel
            // 
            this.currentPanel.BackColor = System.Drawing.Color.Black;
            this.currentPanel.Location = new System.Drawing.Point(564, 0);
            this.currentPanel.Name = "currentPanel";
            this.currentPanel.Size = new System.Drawing.Size(10, 19);
            this.currentPanel.TabIndex = 3;
            // 
            // selectionPanel
            // 
            this.selectionPanel.BackColor = System.Drawing.Color.Green;
            this.selectionPanel.Controls.Add(this.fadeinPanel);
            this.selectionPanel.Controls.Add(this.fadeoutPanel);
            this.selectionPanel.Location = new System.Drawing.Point(0, 0);
            this.selectionPanel.Name = "selectionPanel";
            this.selectionPanel.Size = new System.Drawing.Size(534, 14);
            this.selectionPanel.TabIndex = 0;
            this.selectionPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.selectionPanel_MouseClick);
            // 
            // fadeinPanel
            // 
            this.fadeinPanel.BackColor = System.Drawing.Color.Lime;
            this.fadeinPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.fadeinPanel.Location = new System.Drawing.Point(0, 0);
            this.fadeinPanel.Name = "fadeinPanel";
            this.fadeinPanel.Size = new System.Drawing.Size(24, 14);
            this.fadeinPanel.TabIndex = 1;
            this.fadeinPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.selectionPanel_MouseClick);
            // 
            // fadeoutPanel
            // 
            this.fadeoutPanel.BackColor = System.Drawing.Color.Lime;
            this.fadeoutPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.fadeoutPanel.Location = new System.Drawing.Point(516, 0);
            this.fadeoutPanel.Name = "fadeoutPanel";
            this.fadeoutPanel.Size = new System.Drawing.Size(18, 14);
            this.fadeoutPanel.TabIndex = 2;
            this.fadeoutPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.selectionPanel_MouseClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.splitButton1);
            this.panel2.Controls.Add(this.checkBox2);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.numericUpDown5);
            this.panel2.Controls.Add(this.numericUpDown4);
            this.panel2.Controls.Add(this.numericUpDown3);
            this.panel2.Controls.Add(this.numericUpDown2);
            this.panel2.Controls.Add(this.numericUpDown1);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 14);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(683, 67);
            this.panel2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "Play";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playSelectieToolStripMenuItem,
            this.playSelectieVanafHuidigeLocatieToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(252, 48);
            // 
            // playSelectieToolStripMenuItem
            // 
            this.playSelectieToolStripMenuItem.Name = "playSelectieToolStripMenuItem";
            this.playSelectieToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
            this.playSelectieToolStripMenuItem.Text = "Play selectie";
            this.playSelectieToolStripMenuItem.Click += new System.EventHandler(this.button5_Click);
            // 
            // playSelectieVanafHuidigeLocatieToolStripMenuItem
            // 
            this.playSelectieVanafHuidigeLocatieToolStripMenuItem.Name = "playSelectieVanafHuidigeLocatieToolStripMenuItem";
            this.playSelectieVanafHuidigeLocatieToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
            this.playSelectieVanafHuidigeLocatieToolStripMenuItem.Text = "Play selectie vanaf huidige positie";
            this.playSelectieVanafHuidigeLocatieToolStripMenuItem.Click += new System.EventHandler(this.playSelectieVanafHuidigeLocatieToolStripMenuItem_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(596, 36);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(84, 17);
            this.checkBox2.TabIndex = 14;
            this.checkBox2.Text = "Naar binnen";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(596, 10);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 17);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Naar binnen";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(422, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Fade out lengte:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(429, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Fade in lengte:";
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.DecimalPlaces = 5;
            this.numericUpDown5.Location = new System.Drawing.Point(512, 35);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(83, 20);
            this.numericUpDown5.TabIndex = 10;
            this.numericUpDown5.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.DecimalPlaces = 5;
            this.numericUpDown4.Location = new System.Drawing.Point(512, 9);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(83, 20);
            this.numericUpDown4.TabIndex = 9;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.DecimalPlaces = 5;
            this.numericUpDown3.Location = new System.Drawing.Point(309, 35);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(83, 20);
            this.numericUpDown3.TabIndex = 8;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 5;
            this.numericUpDown2.Location = new System.Drawing.Point(218, 35);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(85, 20);
            this.numericUpDown2.TabIndex = 7;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 5;
            this.numericUpDown1.Location = new System.Drawing.Point(103, 37);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(76, 20);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(218, 6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(85, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Selectie begin";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(309, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(83, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Selectie einde";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(103, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(76, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 1;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // splitButton1
            // 
            this.splitButton1.AutoSize = true;
            this.splitButton1.ContextMenuStrip = this.contextMenuStrip1;
            this.splitButton1.Location = new System.Drawing.Point(3, 35);
            this.splitButton1.Name = "splitButton1";
            this.splitButton1.Size = new System.Drawing.Size(94, 23);
            this.splitButton1.SplitMenuStrip = this.contextMenuStrip1;
            this.splitButton1.TabIndex = 15;
            this.splitButton1.Text = "Play selectie";
            this.splitButton1.UseVisualStyleBackColor = true;
            this.splitButton1.Click += new System.EventHandler(this.button5_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(683, 0);
            this.panel3.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "label4";
            // 
            // MediaPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.DoubleBuffered = true;
            this.Name = "MediaPlayer";
            this.Size = new System.Drawing.Size(683, 81);
            this.panel1.ResumeLayout(false);
            this.selectionPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel currentPanel;
        private System.Windows.Forms.Panel fadeoutPanel;
        private System.Windows.Forms.Panel fadeinPanel;
        private System.Windows.Forms.Panel selectionPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private wyDay.Controls.SplitButton splitButton1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem playSelectieToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playSelectieVanafHuidigeLocatieToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}
