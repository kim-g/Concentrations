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
    public partial class Mol_Base : Form
    {
        private List<MolarMass> MassList;
        private string Res = "";

        public Mol_Base()
        {
            InitializeComponent();
            MassList = new List<MolarMass>();
        }

        private void LoadBase()
        {
            XmlDocument DB = new XmlDocument();
            DB.Load("base.xml");

            XmlElement xRoot = DB.DocumentElement;

            if (MassList != null) { MassList.Clear(); };
            listBox1.Items.Clear();

            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode MName = xnode.Attributes.GetNamedItem("name");
                    XmlNode MMm = xnode.Attributes.GetNamedItem("Mm");
                    if ((MName != null) && (MMm != null))
                        listBox1.Items.Add(MName.Value + " (" + MMm.Value + ")");
                    MolarMass Element = new MolarMass();
                    Element.Name = MName.Value;
                    Element.Mm = MMm.Value;

                    MassList.Add(Element);
                }
            }

            listBox1.Sorted = true;
        }

        public string GetMm()
        {
            LoadBase();
            this.ShowDialog();

            if(Res == "@No@Value@")
            {
                MessageBox.Show("Не найден выбранный элемент. Возможно, ошибка базы.");
                return "";
            }

            return Res;
        }

        private void Mol_Base_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) { return; }

            foreach (MolarMass Element in MassList)
            {
                string ElName = Element.Name + " (" + Element.Mm + ")";
                char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
                string SelElName = Regex.Replace(listBox1.SelectedItem.ToString(), "[,.]", separator.ToString());

                if (SelElName == ElName)
                {
                    Res = Element.Mm;
                    Close();
                    return;
                }
            }
            Res = "@No@Value@";
            Close();
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Res = "@Close@";
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddMM AddMMForm = new AddMM();
            if (AddMMForm.NewElement())
            {
                LoadBase();
            };
                
        }
    }
}
