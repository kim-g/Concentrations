﻿using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Concentrations
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("mailto:kim-g@ios.uran.ru");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}