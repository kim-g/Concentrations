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
        private List<MolarMass> MassList;       // Массив молекул
        private string Res = "";                // Результат
        private string Folder = "/";            // Папка БД

        public Mol_Base()
        {
            InitializeComponent();
            MassList = new List<MolarMass>();   // Создать новый массив
        }

        private void LoadBase()     // Загрузка БД
        {
            XDocument xdoc;         // Создать переменную БД

            try   // Попытка
            {
                xdoc = XDocument.Load("base.xml");  // ... загрузить её из файла base.xml
            }
            catch   // Если не получается, то, наверное, файла нет, создадим новый
            {
                XElement element4 = new XElement("molecules");  // Добавим раздел <molecules />
                try     // Попробуем сохраниться и прочитать заново
                {
                    element4.Save(@"base.xml");
                    xdoc = XDocument.Load("base.xml");
                }
                catch   // Если не получилось, наверное, нет прав на запись
                {
                    MessageBox.Show("Ошибка создания файла с базой соединений.\n" +
                        "Возможно, каталог, в котором находится программа защищён от записи. \n\n" +
                        "Скопируйте программу в обычный каталог для работы с базой соединений",
                        "Ошибка создания базы соединений"); // Просим пользователя дать нам эти права
                    return; // и прекращаем программу
                }
            }

            if (MassList != null) { MassList.Clear(); }; // Если массив не пуст, очищаем его
            listBox1.Items.Clear();                      // ... как и список

            button4.Visible = true;                      // Показываем кнопку создания папки

            XElement CurList = xdoc.Root;                // Делаем текущим элементом <molecules>

            if (Folder != "/")                          // Если мы в папке
            {
                foreach (XElement TempElement in xdoc.Root.Elements("folder"))  //Ищем, в какой
                {
                    if (TempElement.Attribute("name").Value == Folder)
                    {
                        CurList = TempElement;          // ...и делаем её текущим элементом
                    }
                }

                listBox1.Items.Add("..");               // добавляем пункт выхода в родительскую папку

                MolarMass Element = new MolarMass();    // Создаём элемент выхода в родительскую папку
                Element.Name = "..";                    // с именем ".."
                Element.Mm = "up_folder";
                MassList.Add(Element);                  // Добавляем его к массиву

                button4.Visible = false;                // Убираем кнопку создания папки (Я пока не сделал возможность папки в папке. Это - в следующих версиях.)
            }

            if (CurList.Element("folder") != null)      // Если есть папки 
            {
                foreach (XElement Molecule in CurList.Elements("folder"))   //Ищем их
                {
                    XAttribute nameAt = Molecule.Attribute("name");     // Смотрим их названия

                    MolarMass Element = new MolarMass();                // и добавляем к массиву
                    Element.Name = nameAt.Value;
                    Element.Mm = "folder";
                    MassList.Add(Element);

                    if (nameAt != null)                                 // Называя "[Папка]"
                        listBox1.Items.Add("[ " + nameAt.Value + " ]");
                };
            }

            if (CurList.Element("molecule") != null)    // Если есть молекулы 
            {
                foreach (XElement Molecule in CurList.Elements("molecule"))     //Ищем их
                {
                    XAttribute nameAt = Molecule.Attribute("name");     // Смотрим их названия
                    XAttribute MmAt = Molecule.Attribute("Mm");         // и молярные массы

                    MolarMass Element = new MolarMass();                // и добавляем к массиву
                    Element.Name = nameAt.Value;
                    Element.Mm = MmAt.Value;
                    MassList.Add(Element);

                    if ((nameAt != null) && (MmAt != null))             // Называя "Молекула (Mm)"
                        listBox1.Items.Add(nameAt.Value + " (" + MmAt.Value + ")");
                };
            }

                listBox1.Sorted = true;         // В конце концов всё отсортируем. 
                                                // Папки будут в начале, потому что в списке они называются на "["
        }

        public string GetMm(MolInfo Info)   // Получение молярной массы из базы
        {
            Folder = Info.MM_Folder;                 // Устанавливаем текщую папку
            LoadBase();                         // Загружаем базу
            ShowDialog();                       // .. и показываем окно модально

            /* Когда пользователь закроет окно */

            if(Res == "@No@Value@")             // Если элемента нет
            {
                MessageBox.Show("Не найден выбранный элемент. Возможно, ошибка базы."); //Выдать предупреждение
                return "";                      // Передать пустую строку
            }

            Info.MM_Folder = Folder;                 // И назначить текщую папку для главной формы
            return Res;                                 // Вернуть результат
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)   // Двойной щелчёк по полю
        {
            if (listBox1.SelectedItem == null) { return; }              // Если ничего не выбрано, ничего не делать

            foreach (MolarMass Element in MassList)                 // Ищем папку с соответствующим названием
            {
                string ElName = "[ " + Element.Name + " ]";         // ... с прибавлением []

                if (listBox1.SelectedItem.ToString() == ElName)     // Если нашли
                {
                    Folder = Element.Name;                          // Меняем текущую папку
                    LoadBase();                                     // Перезагружаем базу

                    return;                                         // и завершаем всё.
                }
            };

            if (listBox1.SelectedItem.ToString() == "..")           // Если выбран элемент "В род. папку"
            {
                Folder = "/";                        // Устанавливаем корневую /* потом исправить */ папку
                LoadBase();                          // Перезагружаем базу   

                return;                              // и завершаем всё.
            }

            foreach (MolarMass Element in MassList)     // Ищем молекулу с соответствующим названием
            {
                string ElName = Element.Name + " (" + Element.Mm + ")"; // Формируем строку как в списке
                string SelElName = Regex.Replace(listBox1.SelectedItem.ToString(), "[,.]", 
                    Form1.separator.ToString());  // Заменяем точку/запятую на дес. разделитель

                if (SelElName == ElName)            // Если находим
                {
                    Res = Element.Mm;               // Выводим в результирующую переменную Mm
                    Close();                        // Закрываем окно   
                    return;                         // И прекращаем всё
                }
            }
            Res = "@No@Value@";                 // Если ничего не нашли, присваиваем соответств. значение
            Close();                            // Закрываем окно
            return;
        }

        private void button2_Click(object sender, EventArgs e)  // Если пользователь нажал на «Отмена»
        {
            Res = "@Close@";            // Выводим в результирующую переменную "@Close@"
            Close();                    // ... и закрываем окно
        }

        private void button1_Click(object sender, EventArgs e)  // Пользователь нажал на кнопку «Новое»
        {
            AddMM AddMMForm = new AddMM();      // Создаём окно добавления молекулы
            if (AddMMForm.NewElement(Folder))   // Если пользователь не отменил операцию (вернулось true)
            {
                LoadBase();                     // Перезагрузим базу
            };
                
        }

        private void button4_Click(object sender, EventArgs e)  //Пользователь нажал на кнопку «Новая папка»
        {
            string NewFolder;       // Название новой папки

            do              // Будем в цикле пока не придёт сигнал о выходе
            {
                Input_String IS_Form = new Input_String();      // Создаём окно ввода текстовой строки
                IS_Form.Owner = this;                           // ... и становимся его родителем

                NewFolder = IS_Form.GetString("Создание папки", "Имя:");    // Запрашиваем имя папки

                if (NewFolder == "@Cancel@") { return; };       // Если пользователь отменил, прекращаем обработку

                if (NewFolder == "")        // Если пользователь оставил пустое поле
                {
                    MessageBox.Show("Введите название папки");  // Говорим ему «Сам дурак!»
                    NewFolder = "@ERROR@";                      // И присваиваем переменной метку ошибки
                };

                if (Mol_Base.CheckIfExists("folder", NewFolder))    // Если такая папка уже есть
                {
                    // Сообщаем об этом пользователю
                    MessageBox.Show("Папка с таким названием уже имеется.\nПожалуйста, выберите другое название.");
                    NewFolder = "@ERROR@";  // И присваиваем переменной метку ошибки
                }
                
            }

            while (NewFolder == "@ERROR@");     // Если есть метка ошибки, всё начать сначала.

            XDocument Base = XDocument.Load("base.xml");    // Закрузить базу

            Base.Root.Add( new XElement("folder",
                            new XAttribute("name", NewFolder)));    //Добавить папку с нужным именем
            Base.Save("base.xml");                          // Сохранить результат
            LoadBase();                                     // Перезагрузить базу
        }

        public static Boolean CheckIfExists(string Element, string Name)    // Проверить, есть ли элемент в базе
        {
            XmlDocument Base = new XmlDocument();       // Создать базу
            Base.Load("base.xml");                      // Открыть из файла
            XmlElement xRoot = Base.DocumentElement;    // Перейти в корневой каталог
            XmlNode childnode = xRoot.SelectSingleNode(Element + "[@name='" + Name + "']"); // Запросить конкретный элемент
            return childnode != null;                   // Вернуть true, если элемент существует
        }
    }
}
