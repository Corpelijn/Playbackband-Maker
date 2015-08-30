using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PBB
{
    public partial class BlokInfo : UserControl, IComparable
    {
        private decimal lastValue;
        private int p_id;
        private string placed_name;
        private decimal placed_amount;

        public BlokInfo()
        {
            InitializeComponent();
            this.p_id = 0;
            lastValue = 0;
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

        public int PositionID
        {
            get { return (int)p_id; }
            set { p_id = value; label2.Text = p_id.ToString(); }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (lastValue < numericUpDown2.Value)
            {
                p_id -= 15;
            }
            else
            {
                p_id += 15;
            }

            OnPositionUpdated();

            label2.Text = p_id.ToString();
            lastValue = numericUpDown2.Value;
        }

        public event PositionUpdatedEventHandler PositionUpdated;
        public delegate void PositionUpdatedEventHandler(BlokInfo sender);
        public void OnPositionUpdated()
        {
            if (PositionUpdated != null)
                PositionUpdated(this);
        }

        public int CompareTo(object obj)
        {
            if(obj.GetType() != typeof(BlokInfo))
            {
                return 0;
            }

            BlokInfo bi = (BlokInfo)obj;
            return this.PositionID.CompareTo(bi.PositionID);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.placed_name = textBox1.Text;
        }

        public void SetInitValues(string name, int amount)
        {
            if (placed_name == "")
            {
                this.placed_name = name;
            }
            if (placed_amount == -1)
            {
                this.placed_amount = amount;
            }
        }
    }
}
