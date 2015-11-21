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
    public partial class Cons : Form
    {
        public Cons()
        {
            InitializeComponent();
        }

        public void ShowModal()
        {
            try
            {
                Form1 MainForm = (Form1)this.Owner;
                
            }
            catch
            {
                MessageBox.Show("Ошибка чтения информации из окна навески.");
            };

            ShowDialog();

            
        }

    }
}
