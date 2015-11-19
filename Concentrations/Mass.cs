using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Concentrations
{
    public partial class Mass : Form
    {
        public Mass()
        {
            InitializeComponent();
        }

        public void ShowModal()
        {
            ShowDialog();
        }

        private void Mass_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
