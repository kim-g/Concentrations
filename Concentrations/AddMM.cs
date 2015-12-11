using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Concentrations
{
    public partial class AddMM : Form
    {
        public AddMM()
        {
            InitializeComponent();
        }

        private bool Add = false;
        private string ElName;
        private string ElMm;

        public bool NewElement(string Folder)
        {
            ShowDialog();
            if (Add)
            {
                XDocument Base = XDocument.Load("base.xml");

                if (Folder == "/")
                {
                    Base.Root.Add(new XElement("molecule",
                                new XAttribute("name", ElName),
                                new XAttribute("Mm", ElMm)));
                }
                else
                {
                    XElement FoldEl = null;
                    foreach (XElement El in Base.Root.Elements("folder"))
                    {
                        if (El.Attribute("name").Value == Folder)
                        {
                            FoldEl = El;
                        }
                    }
                    if (FoldEl == null)
                    {
                        Base.Root.Add(new XElement("folder",
                                new XAttribute("name", Folder)));
                        foreach (XElement El in Base.Root.Elements("folder"))
                        {
                            if (El.Attribute("name").Value == Folder)
                            {
                                FoldEl = El;
                            }
                        }
                    }

                    FoldEl.Add(new XElement("molecule",
                                        new XAttribute("name", ElName),
                                        new XAttribute("Mm", ElMm)));
                }

                Base.Save("base.xml");

            }
            return Add;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Add = false;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Mol_Base.CheckIfExists("molecule", textBox1.Text))
            {
                MessageBox.Show("Соединение с таким названием уже имеется.\nПожалуйста, выберите другое название.");
                return;
            };
            

            char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];

            Add = true;
            ElName = textBox1.Text;
            ElMm = Regex.Replace(textBox2.Text, "[.,]", separator.ToString());
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Form1.NumbersOnly(sender);
        }
    }
}
