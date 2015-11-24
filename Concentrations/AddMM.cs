using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;

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

        public bool NewElement()
        {
            ShowDialog();
            if (Add)
            {
                XmlDocument Base = new XmlDocument();
                Base.Load("base.xml");

                XmlElement xRoot = Base.DocumentElement;

                XmlElement MmElem = Base.CreateElement("molecule");
                XmlAttribute nameAttr = Base.CreateAttribute("name");
                XmlAttribute MmAttr = Base.CreateAttribute("Mm");

                XmlText nameText = Base.CreateTextNode(ElName);
                XmlText MmText = Base.CreateTextNode(ElMm);

                nameAttr.AppendChild(nameText);
                MmAttr.AppendChild(MmText);

                MmElem.Attributes.Append(nameAttr);
                MmElem.Attributes.Append(MmAttr);
                xRoot.AppendChild(MmElem);
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
            XmlDocument Base = new XmlDocument();
            Base.Load("base.xml");
            XmlElement xRoot = Base.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Attributes.Count > 0)
                {
                    if (xnode.Attributes.GetNamedItem("name").Value == textBox1.Text)
                    {
                        MessageBox.Show("Соединение с таким названием уже имеется.\nПожалуйста, выберите другое название.");
                        return;
                    };
                }
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
