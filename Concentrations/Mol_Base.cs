using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


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
            XDocument xdoc;

            try
            {
                xdoc = XDocument.Load("base.xml");
            }
            catch
            {
                XElement element4 = new XElement("molecules");
                element4.Save(@"base.xml");
                xdoc = XDocument.Load("base.xml");
            }

            if (MassList != null) { MassList.Clear(); };
            listBox1.Items.Clear();

            foreach (XElement Molecule in xdoc.Element("molecules").Elements("folder"))
            {
                XAttribute nameAt = Molecule.Attribute("name");

                MolarMass Element = new MolarMass();
                Element.Name = nameAt.Value;
                Element.Mm = "folder";
                MassList.Add(Element);

                if (nameAt != null)
                    listBox1.Items.Add("[ " + nameAt.Value + " ]");
            }

            foreach (XElement Molecule in xdoc.Element("molecules").Elements("molecule"))
            {
                XAttribute nameAt = Molecule.Attribute("name");
                XAttribute MmAt = Molecule.Attribute("Mm");

                MolarMass Element = new MolarMass();
                Element.Name = nameAt.Value;
                Element.Mm = MmAt.Value;
                MassList.Add(Element);

                if ((nameAt != null) && (MmAt != null))
                    listBox1.Items.Add(nameAt.Value + " (" + MmAt.Value + ")");
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
