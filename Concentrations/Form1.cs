using System;
using System.Text.RegularExpressions;
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
        private string mM_Folder = "/";

        public string MM_Folder
        {
            get
            {
                return mM_Folder;
            }

            set
            {
                mM_Folder = value;
            }
        }

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
            ConsForm.TextBoxEdit = true;
            ConsForm.Vol.Text = Volume.ToString();
            ConsForm.MmEdit.Text = Mm.ToString();
            //ConsForm.gEdit.Text = g.ToString();
            ConsForm.TextBoxEdit = false;
            ConsForm.ShowModal();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Dilution Dil = new Dilution();
            Dil.Owner = this;
            Dil.TextBoxEdit = true;
            Dil.Vol.Text = Volume.ToString();
            Dil.InCaEdit.Text = Ca.ToString();
            Dil.InCxEdit.Text = Cx.ToString();
            Dil.TextBoxEdit = false;

            Dil.ShowDialog();
        }

        public static void NumbersOnly(object sender)
        {
            TextBox textBox = (TextBox)sender;
            
            int SelStart = textBox.SelectionStart;
            int TextLength = textBox.Text.Length;

            textBox.Text = Regex.Replace(textBox.Text, "[^0-9.,]", "");
            //textBox1.Text = Regex.Replace(textBox1.Text, "[.,]", separator.ToString());
            if (TextLength == textBox.Text.Length)
            { textBox.SelectionStart = SelStart; }
            else
            {
                textBox.SelectionStart = SelStart - 1;
            };
        }

        private void button5_Click(object sender, EventArgs e)
        {
            About AboutForm = new About();
            AboutForm.ShowDialog();
        }
    }
}
