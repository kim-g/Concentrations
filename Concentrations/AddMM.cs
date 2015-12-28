/****************************************************************/
/*                                                              */
/*         Программа для расчёта навесок и концентраций         */
/*                          Версия 1.1                          */
/*              Модуль добавления молекулы в базу               */
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
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Concentrations
{
    public partial class AddMM : Form  //Окно добавления записи в БД
    {
        public AddMM()
        {
            InitializeComponent();
        }

        private bool Add = false;       // Пользователь нажал на OK (true) или Отмена (false)
        private string ElName;          // Имя новой молекулы
        private string ElMm;            // Молярная масса новой молекулы

        public bool NewElement(string Folder)   // Подготовка формы и добавление элемента
        {
            ShowDialog();               // Вывести форму
            if (Add)                    //Если пользователь нажал на OK
            {
                XDocument Base = XDocument.Load("base.xml");        //Открываем БД

                if (Folder == "/")                                  // Если мы находимся в корневом каталоге
                {
                    Base.Root.Add(new XElement("molecule",          //Добавляем запись
                                new XAttribute("name", ElName),     //...с нужными атрибутами
                                new XAttribute("Mm", ElMm)));       
                }
                else                                                // Если мы находимся в пользовательской папке
                {
                    XElement FoldEl = null;                         // Создаём новый элемент
                    foreach (XElement El in Base.Root.Elements("folder"))   // ...и находим нужную папку
                    {
                        if (El.Attribute("name").Value == Folder)           // ...по атрибуту name
                        {
                            FoldEl = El;                                    // ...и ссылаемся на него
                        }
                    }
                    if (FoldEl == null)                             // если не нашли папку
                    {
                        Base.Root.Add(new XElement("folder",        //Добавляем папку
                                new XAttribute("name", Folder)));
                        foreach (XElement El in Base.Root.Elements("folder"))   // ...и снова её ищем.
                        {
                            if (El.Attribute("name").Value == Folder)
                            {
                                FoldEl = El;
                            }
                        }
                    }

                    FoldEl.Add(new XElement("molecule",             //Добавляем запись в нужную папку
                                        new XAttribute("name", ElName), //.. с нужными атрибутами
                                        new XAttribute("Mm", ElMm)));
                }

                Base.Save("base.xml");                              // Сохраняемся ;-)

            }
            return Add;                                             // В любом случае возвращаем, чего хотел пользователь.
        }

        private void button2_Click(object sender, EventArgs e)      // Если пользователь нажал «Отмена»
        {
            Add = false;                                            // Сообщаем об этом основной функции
            Close();                                                // ...и закрываем окно 
        }

        private void button1_Click(object sender, EventArgs e)      // Если пользователь нажал «ОК»
        {
            if (Mol_Base.CheckIfExists("molecule", textBox1.Text))  // Проверяем, есть ли уже такая запись
            {
                MessageBox.Show("Соединение с таким названием уже имеется.\nПожалуйста, выберите другое название.");

                return;     //Если да, то выводим сообщение и завершаем процедуру
            };            
            Add = true;                 // Сообщаем об решении пользователя основной функции
            ElName = textBox1.Text;     // Копируем в переменную имя молекулы...
            ElMm = Regex.Replace(textBox2.Text, "[.,]", Form1.separator.ToString()); //...и её мол. массу с правильным десятичным разделителем
            Close();                    // ...и закрываем окно
        }

        private void textBox1_TextChanged(object sender, EventArgs e) //Если пользователь ввёл данные в строку молярной массы
        {
            Form1.NumbersOnly(sender);  //Оставляем только числа и знак десятичного разделителя.
        }
    }
}
