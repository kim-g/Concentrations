/****************************************************************/
/*                                                              */
/*         Программа для расчёта навесок и концентраций         */
/*                          Версия 1.1                          */
/*              Модуль расчёта разбавления раствора             */
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
    public partial class Dilution : Form  // Окно расчёта разбавлений
    {
        public Dilution()
        {
            InitializeComponent();
        }

        public bool TextBoxEdit = false;  // Флаг. Внешнее окно производит редактирование полей ввода.

        private void Vol_TextChanged(object sender, EventArgs e)    //Если пользователь изменил значение
        {
            if (!TextBoxEdit) { Form1.NumbersOnly(sender); };       //Оставляем только числа и знак десятичного разделителя.
        }

        private void button1_Click(object sender, EventArgs e)      // Пользователь нажал «Расчитать»
        {
            double volume;      // Объём колбы              
            double InCa, Ca;    // Концентрация  C = Ca * 10 ^ Cx
            int InCx, Cx;       //

            // Объём
            try         // Считываем объём колбы, убирая не цифры и подменяя точку/запятую на системный десятичный разделитель
            {
                volume = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(Vol.Text, "[^0-9.,]", ""),
                        "[.,]", Form1.separator.ToString()));
            }
            catch(FormatException)      // Если всё-равно получилось не double число, выводим ошибку
            {
                MessageBox.Show("Неправильно введён объём колбы.");
                return;
            }
            if (volume <= 0)            // Если пользователь ввёл отрицательный объём, попросим его быть повнимательнее
            {
                MessageBox.Show("Неправильно введён объём колбы.\nОбъём должен быть больше 0!");
                return;
            };

            //InC
            try         // Считываем значения исходной концентрации, убирая не цифры и подменяя точку/запятую на системный десятичный разделитель
            {
                InCa = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(InCaEdit.Text, "[^0-9.,]", ""),
                        "[.,]", Form1.separator.ToString()));
                InCx = 0 - Convert.ToInt32(Regex.Replace(InCxEdit.Text, "[^0-9]", ""));
            }
            catch (FormatException)     // Если всё-равно получилось не double число, выводим ошибку
            {
                MessageBox.Show("Неправильно введена исходная концетрация.");
                return;
            }
            if (InCa <= 0)              // Если пользователь ввёл отрицательную концентрацию, попросим его быть повнимательнее
            {
                MessageBox.Show("Неправильно введена исходная концетрация.\nМножитель исходной концентрации должен быть больше 0!");
                return;
            };

            //C
            try         // Считываем значения желаемой концентрации, убирая не цифры и подменяя точку/запятую на системный десятичный разделитель
            {
                Ca = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(CaEdit.Text, "[^0-9.,]", ""),
                        "[.,]", Form1.separator.ToString()));
                Cx = 0 - Convert.ToInt32(Regex.Replace(CxEdit.Text, "[^0-9]", ""));
            }
            catch (FormatException) // Если всё-равно получилось не double число, выводим ошибку
            {
                MessageBox.Show("Неправильно введена исходная концетрация.");
                return;
            }
            if (InCa <= 0)          // Если пользователь ввёл отрицательную концентрацию, попросим его быть повнимательнее
            {
                MessageBox.Show("Неправильно введена исходная концетрация.\nМножитель исходной концентрации должен быть больше 0!");
                return;
            };

            double Res;     // Определяем переменныю для формирования результата
                            //                                                  Ca * 10 ^ Cx
            try             // Пытаемся всё посчитать по формуле: Vалик. = V ------------------
            {               //                                                InCa * 10 ^ InCx
                Res = volume * Ca * Math.Pow(10, Cx) / InCa / Math.Pow(10, InCx);
            }
            catch           // В случае какой-либо ошибки
            {
                MessageBox.Show("Ошибка вычислений");   // Сообщаем об этом
                return;                                 // ...и прекращаем процедуру
            };

            ResultLabel.Text = "Аликвота " + Res.ToString() + " мл.";   // Выводим результат

            Form1 MainForm = (Form1)Owner;      // Обращаемся к родительской форме и посылаем туда итоговые данные об:
            MainForm.Volume = volume;           // - объёме колбы
            MainForm.Ca = InCa;                 // - Исходной концентрации
            MainForm.Cx = InCx;
        }
    }
}
