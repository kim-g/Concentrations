/****************************************************************/
/*                                                              */
/*         Программа для расчёта навесок и концентраций         */
/*                          Версия 1.1                          */
/*                Модуль управления базой молекул               */
/*                                                              */
/*                     Автор – Григорий Ким                     */
/*                      kim-g@ios.uran.ru                       */
/*    Федеральное государственное бюджетное учреждение науки    */
/*     Институт органического синтеза им. И.Я. Постовского      */
/* Уральского отделения Российской академии наук (ИОС УрО РАН)  */
/*                                                              */
/*                 Распространяется на условиях                 */
/*            Berkeley Software Distribution license            */
/*                                                              */
/****************************************************************/

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
        private string Folder = "/";

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
                try
                {
                    element4.Save(@"base.xml");
                    xdoc = XDocument.Load("base.xml");
                }
                catch
                {
                    MessageBox.Show("Ошибка создания файла с базой соединений.\n" +
                        "Возможно, каталог, в котором находится программа защищён от записи. \n\n" +
                        "Скопируйте программу в обычный каталог для работы с базой соединений",
                        "Ошибка создания базы соединений");
                    return;
                }
            }

            if (MassList != null) { MassList.Clear(); };
            listBox1.Items.Clear();

            button4.Visible = true;

            XElement CurList = xdoc.Root;

            if (Folder != "/")
            {
                foreach (XElement TempElement in xdoc.Root.Elements("folder"))
                {
                    if (TempElement.Attribute("name").Value == Folder)
                    {
                        CurList = TempElement;
                    }
                }

                listBox1.Items.Add("..");

                MolarMass Element = new MolarMass();
                Element.Name = "..";
                Element.Mm = "up_folder";
                MassList.Add(Element);

                button4.Visible = false;
            }

            if (CurList.Element("folder") != null)
            {
                foreach (XElement Molecule in CurList.Elements("folder"))
                {
                    XAttribute nameAt = Molecule.Attribute("name");

                    MolarMass Element = new MolarMass();
                    Element.Name = nameAt.Value;
                    Element.Mm = "folder";
                    MassList.Add(Element);

                    if (nameAt != null)
                        listBox1.Items.Add("[ " + nameAt.Value + " ]");
                };
            }

            if (CurList.Element("molecule") != null)
            {
                foreach (XElement Molecule in CurList.Elements("molecule"))
                {
                    XAttribute nameAt = Molecule.Attribute("name");
                    XAttribute MmAt = Molecule.Attribute("Mm");

                    MolarMass Element = new MolarMass();
                    Element.Name = nameAt.Value;
                    Element.Mm = MmAt.Value;
                    MassList.Add(Element);

                    if ((nameAt != null) && (MmAt != null))
                        listBox1.Items.Add(nameAt.Value + " (" + MmAt.Value + ")");
                };
            }

                listBox1.Sorted = true;
        }

        public string GetMm(string CurFolder)
        {
            Folder = CurFolder;
            LoadBase();
            ShowDialog();

            if(Res == "@No@Value@")
            {
                MessageBox.Show("Не найден выбранный элемент. Возможно, ошибка базы.");
                return "";
            }

            Form1 MainForm = (Form1)Owner.Owner;
            MainForm.MM_Folder = Folder;
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
                string ElName = "[ " + Element.Name + " ]";

                if (listBox1.SelectedItem.ToString() == ElName)
                {
                    Folder = Element.Name;
                    LoadBase();

                    return;
                }
            };

            if (listBox1.SelectedItem.ToString() == "..")
            {
                Folder = "/";
                LoadBase();

                return;
            }

            foreach (MolarMass Element in MassList)
            {
                string ElName = Element.Name + " (" + Element.Mm + ")";
                string SelElName = Regex.Replace(listBox1.SelectedItem.ToString(), "[,.]", 
                    Form1.separator.ToString());

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
            if (AddMMForm.NewElement(Folder))
            {
                LoadBase();
            };
                
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string NewFolder;

            do
            {
                Input_String IS_Form = new Input_String();
                IS_Form.Owner = this;

                NewFolder = IS_Form.GetString("Создание папки", "Имя:");

                if (NewFolder == "@Cancel@") { return; };

                if (NewFolder == "")
                {
                    MessageBox.Show("Введите название папки");
                    NewFolder = "@ERROR@";
                };

                if (Mol_Base.CheckIfExists("folder", NewFolder))
                {
                    MessageBox.Show("Папка с таким названием уже имеется.\nПожалуйста, выберите другое название.");
                    NewFolder = "@ERROR@";
                }
                
            }

            while (NewFolder == "@ERROR@");

            XDocument Base = XDocument.Load("base.xml");

            Base.Root.Add( new XElement("folder",
                            new XAttribute("name", NewFolder)));
            Base.Save("base.xml");
            LoadBase();
        }

        public static Boolean CheckIfExists(string Element, string Name)
        {
            XmlDocument Base = new XmlDocument();
            Base.Load("base.xml");
            XmlElement xRoot = Base.DocumentElement;
            XmlNode childnode = xRoot.SelectSingleNode(Element + "[@name='" + Name + "']");
            return childnode != null;
        }
    }
}
