/****************************************************************/
/*                                                              */
/*         Программа для расчёта навесок и концентраций         */
/*                          Версия 1.0                          */
/*                    Модуль расчёта навески                    */
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
    public partial class Mass : Form
    {
        public Mass()
        {
            InitializeComponent();
        }

        public void ShowModal() //Подготовить и показать окно модально
        {
            ShowDialog();   // Готовить не надо, только покажем
        }

        private void textBox1_TextChanged(object sender, EventArgs e)   // При изменении текста
        {
            Form1.NumbersOnly(sender);  // Оставить только цифры и десятичный разделитель
        }

        private void button1_Click(object sender, EventArgs e)  // При нажатии на кнопку «Расчитать»
        {
            double volume;  // Объём колбы
            double Mm;      // Молярная масса
            double Ca;      // Множитель концентрации
            double Cx;      // Показатель десяти концентрации

            try     // Считываем объём колбы, убирая не цифры и подменяя точку/запятую на системный десятичный разделитель
            {
                volume = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(Vol.Text, "[^0-9.,]", ""), 
                        "[.,]", Form1.separator.ToString()));
            }
            catch (FormatException) // Если всё-равно получилось не double число, выводим ошибку
            {
                MessageBox.Show("Неправильно введён объём колбы.");
                return;
            };
            if (volume<=0)          // Если пользователь ввёл отрицательный объём, попросим его быть повнимательнее
            {
                MessageBox.Show("Неправильно введён объём колбы.\nОбъём должен быть больше 0!");
                return;
            };

            try     // Считываем молярную массу, убирая не цифры и подменяя точку/запятую на системный десятичный разделитель
            {
                Mm = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(MmEdit.Text, "[^0-9.,]", ""),
                        "[.,]", Form1.separator.ToString()));
            }
            catch (FormatException) // Если всё-равно получилось не double число, выводим ошибку
            {
                MessageBox.Show("Неправильно введена молярная масса.");
                return;
            };
            if (Mm <= 0)        // Если пользователь ввёл отрицательную молярную массу, попросим его быть повнимательнее
            {
                MessageBox.Show("Неправильно введена молярная масса.\nМолярная масса должна быть больше 0!");
                return;
            };

            try     // Считываем Желаемую концентрацию, убирая не цифры и подменяя точку/запятую на системный десятичный разделитель
            {
                Ca = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(CaEdit.Text, "[^0-9.,]", ""),
                        "[.,]", Form1.separator.ToString()));
                Cx = 0 - Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(CxEdit.Text, "[^0-9.,]", ""),
                        "[.,]", Form1.separator.ToString()));
            }
            catch (FormatException) // Если всё-равно получилось не double число, выводим ошибку
            {
                MessageBox.Show("Неправильно введена желаемая концентрация.");
                return;
            };
            if (Ca <= 0)        // Если пользователь ввёл отрицательный множитель концентрации, попросим его быть повнимательнее
            {
                MessageBox.Show("Неправильно введена желаемая концентрация.\nЖелаемая концентрация должна быть больше 0!");
                return;
            };

            double Res;         // Определяем переменныю для формирования результата
                                //                                         V * Mm * Ca * 10 ^ Cx
            try                 // Пытаемся всё посчитать по формуле: g = -----------------------
            {                   //                                                 1000
                Res = volume * Mm * Ca * Math.Pow(10, Cx) / 1000;
                ResultLabel.Text = "g = " + Res.ToString("N5") + " г."; // ...и выводим результат
            }
            catch   // В случае какой-либо ошибки
            {
                MessageBox.Show("Ошибка вычислений");   // Сообщаем об этом
                return;                                     
            };

            try
            {
                Clipboard.SetText(Res.ToString());      // Выдаём в буфер обмена

                Form1 MainForm = (Form1)Owner;          // Обращаемся к родительской форме
                                                        // ...и посылаем туда итоговые данные об:
                MainForm.Volume = volume;               // - объёме колбы
                MainForm.Mm = Mm;                       // - молярной массе
                MainForm.g = Res;                       // - навеске
            }
            catch       // В случае какой-либо ошибки
            {
                MessageBox.Show("Ошибка взаимодействия форм");  // Сообщаем об этом
                return;                                         // Сообщаем об этом
            }
        }

        private void button2_Click(object sender, EventArgs e)  // Если пользователь запросил Mm из базы
        {
            Mol_Base Mol_Base_Form = new Mol_Base();            // Создаём новое окно базы
            Mol_Base_Form.Owner = this;                         // Становимся его родителем

            Form1 MainForm = (Form1)Owner;                      // Определяем своего родителя
            string NewMm = Mol_Base_Form.GetMm(MainForm.MM_Folder); // Запрашиваем значение из базы
            if (NewMm != "@Close@")                             // Если пользователь не отменил
            {
                MmEdit.Text = NewMm;                            //... то выводим результат
            };
        }
    }
}
