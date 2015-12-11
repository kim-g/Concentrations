using System;
using System.Windows.Forms;

namespace Concentrations
{
    public partial class Input_String : Form
    {
        private string Res="@Null@";

        public Input_String()
        {
            InitializeComponent();
        }

        public string GetString(string Title, string Label)
        {
            Text = Title;
            label1.Text = Label;

            ShowDialog();

            if (Res == "@Null@")
            { Res = ""; };

            return Res;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Res = "@Cancel@";
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Res = textBox1.Text;
            Close();
        }
    }
}
