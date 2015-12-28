/****************************************************************/
/*                                                              */
/*         Программа для расчёта навесок и концентраций         */
/*                          Версия 1.1                          */
/*                 Окно главного меню программы                 */
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

namespace Concentrations
{
    public partial class Form1 : Form
    {

        /* Константы */
        public static char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        public static string Version = "1.01";
        /*************/

        /* Переменные для отображения начальных значений окон */
        public double Volume;       // Объём колбы
        public double Mm;           // Молярная масса
        public double g;            // Величина навески
        public double Ca;           // Множитель фактической концентрации
        public int Cx;              // Показатель степени 10 исходной концентрации

        private string mM_Folder = "/";     // Переменная, хранящая значение корневой папки в БД

        public string MM_Folder     // Корневая папка в БД
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

        private void button4_Click(object sender, EventArgs e)  // Если пользователь нажал на «Выход»
        {
            Application.Exit();     // Завершить приложение
        }

        private void button1_Click(object sender, EventArgs e)  // Если пользователь нажал на «Навеска»
        {
            Mass MassForm = new Mass();     // Создать новое окно расчёта навески
            MassForm.Owner = this;          // Определить себя его родителем
            MassForm.ShowDialog();          // И показать его модально
        }

        private void button2_Click(object sender, EventArgs e)  // Если пользователь нажал на «Фактическая концентрация»
        {
            Cons ConsForm = new Cons();             // Создать новое окно расчёта факт. концентрации
            ConsForm.Owner = this;                  // Определить себя его родителем
            ConsForm.TextBoxEdit = true;            // Поставить флаг редактирования
            ConsForm.Vol.Text = Volume.ToString();  // Применить начальное значение объёма
            ConsForm.MmEdit.Text = Mm.ToString();   // Применить начальное значение Молярной массы
            ConsForm.TextBoxEdit = false;           // Снять флаг редактирования
            ConsForm.ShowModal();                   // И показать его модально
        }

        private void button3_Click(object sender, EventArgs e)  // Если пользователь нажал на «Разбавление»
        {
            Dilution Dil = new Dilution();          // Создать новое окно расчёта разбавления
            Dil.Owner = this;                       // Определить себя его родителем
            Dil.TextBoxEdit = true;                 // Поставить флаг редактирования
            Dil.Vol.Text = Volume.ToString();       // Применить начальное значение объёма
            Dil.InCaEdit.Text = Ca.ToString();      // Применить начальное значение множетеля жел. концентрации
            Dil.InCxEdit.Text = Cx.ToString();      // Применить начальное значение степени 10 жел. концентрации
            Dil.TextBoxEdit = false;                // Снять флаг редактирования
            Dil.ShowDialog();                       // И показать его модально
        }

        public static void NumbersOnly(object sender)   //Оставляет только цифры и десятичный разделитель
        {
            TextBox textBox = (TextBox)sender;          // Определить TextBox, с которым работаем 
            
            int SelStart = textBox.SelectionStart;      // Запоминаем, где стоял курсор
            int TextLength = textBox.Text.Length;       // ...и длину строки

            textBox.Text = Regex.Replace(textBox.Text, "[^0-9.,]", ""); // Убираем всё, кроме цифр, точки и запятой
            if (TextLength == textBox.Text.Length)      // Если длина не изменилась
            { textBox.SelectionStart = SelStart; }      // Ставим курсор на прежнее место
            else
            {
                textBox.SelectionStart = SelStart - 1;  // В противном случае, сдвигаем на 1 влево
            };                  /* Здесь не учитывается вариант Paste с буквами. В этом случае курсор скакнёт непредсказуемо. Поправить!!! */
        }

        private void button5_Click(object sender, EventArgs e)  // Если пользователь нажал на «О программе...»
        {
            About AboutForm = new About();          // Создать новое окно информации
            AboutForm.ShowDialog();                 // И показать его модально
        }
    }
}
