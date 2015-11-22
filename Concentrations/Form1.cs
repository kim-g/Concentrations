using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Concentrations
{
    public partial class Form1 : Form
    {
        public double Volume;
        public double Mm;
        public double g;
        public double Ca;
        public int Cx;

        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Mass MassForm = new Mass();
            MassForm.Owner = this;
            MassForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cons ConsForm = new Cons();
            ConsForm.Owner = this;
            ConsForm.Vol.Text = Volume.ToString();
            ConsForm.MmEdit.Text = Mm.ToString();
            ConsForm.gEdit.Text = g.ToString();
            ConsForm.ShowModal();
        }
    }
}
