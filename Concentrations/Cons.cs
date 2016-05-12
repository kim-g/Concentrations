/****************************************************************/
/*                                                              */
/*         Программа для расчёта навесок и концентраций         */
/*                          Версия 1.1                          */
/*      Модуль расчёта фактической концентрации по навеске      */
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
    public partial class Cons : Form  //Окно расчёта фактической концентрации
    {
        MolInfo GenInfo;

        public Cons()
        {
            InitializeComponent();
        }

        bool TextBoxEdit = false;    // Флаг. Внешнее окно производит редактирование полей ввода. 

        public MolInfo ShowDialog(MolInfo Info)             // Инициализация окна и вывод в модальном режиме.
        {
            GenInfo = Info;
            TextBoxEdit = true;
            Vol.Text = Info.Volume.ToString();
            MmEdit.Text = Info.Mm.ToString();
            gEdit.Text = Info.g.ToString();
            TextBoxEdit = false;
            base.ShowDialog();                          // Выведем окно
            return GenInfo;
        }

        private void button1_Click(object sender, EventArgs e)  // Пользователь нажал на кнопку «Расчитать»
        {
            double volume;      // Объём колбы
            double Mm;          // Молярная масса молекулы
            double g;           // Навеска

            try                 // Считываем объём колбы, убирая не цифры и подменяя точку/запятую на системный десятичный разделитель
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
            if (volume <= 0)        // Если пользователь ввёл отрицательный объём, попросим его быть повнимательнее
            {
                MessageBox.Show("Неправильно введён объём колбы.\nОбъём должен быть больше 0!");
                return;
            };

            try                 // Считываем молярную массу, убирая не цифры и подменяя точку/запятую на системный десятичный разделитель
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
            if (Mm <= 0)            // Если пользователь ввёл отрицательную молярную массу, попросим его быть повнимательнее
            {
                MessageBox.Show("Неправильно введена молярная масса.\nМолярная масса должна быть больше 0!");
                return;
            };

            try                 // Считываем навеску, убирая не цифры и подменяя точку/запятую на системный десятичный разделитель
            {
                g = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(gEdit.Text, "[^0-9.,]", ""),
                        "[.,]", Form1.separator.ToString()));
            }
            catch (FormatException) // Если всё-равно получилось не double число, выводим ошибку
            {
                MessageBox.Show("Неправильно введена навеска.");
                return;
            };
            if (g <= 0)             // Если пользователь ввёл отрицательную навеску, попросим его быть повнимательнее
            {
                MessageBox.Show("Неправильно введена навеска.\nНавеска должна быть больше 0!");
                return;
            };

            double Res;         // Определяем переменныю для формирования результата
                                //                                                        g
            try                 // Пытаемся всё посчитать по формуле:  С(факт.) = 1000 --------
            {                   //                                                      V * Mm
                Res = 1000 * g / volume / Mm;
                ResultLabel.Text = "С = " + Res.ToString() + "моль/л."; // ...и выводим результат
            }
            catch               // В случае какой-либо ошибки
            {
                MessageBox.Show("Ошибка вычислений!");  // Сообщаем об этом
                return;                                 // ...и прекращаем процедуру
            }
                                                        // Посылаем обратно итоговые данные об:
            GenInfo.Volume = volume;                    // - объёме колбы
            GenInfo.Mm = Mm;                            // - молярной массе
            int Cx = (int)Math.Ceiling(Math.Log10(Res)) - 1;    // Вычисляем множитель
            double Ca = Res / Math.Pow(10, Cx);         // и степень 10 для концентрации
            GenInfo.Cx = Cx;                            // Посылаем это родительской форме
            GenInfo.Ca = Ca;
            Clipboard.SetText(Res.ToString());          // и в буфер обмена
        }

        private void button2_Click(object sender, EventArgs e)      // Если пользователь запросил Mm из базы
        {
            Mol_Base Mol_Base_Form = new Mol_Base();                // Создаём новое окно базы
            string NewMm = Mol_Base_Form.GetMm(GenInfo);  // Запрашиваем значение из базы
            if (NewMm != "@Close@") { MmEdit.Text = NewMm; };       // Если пользователь не отменил, то выводим результат
        }

        private void Vol_TextChanged(object sender, EventArgs e)    // Если пользователь изменил текст
        {
            if (!TextBoxEdit) { Form1.NumbersOnly(sender); };       //Оставляем только числа и знак десятичного разделителя.
        }
    }
}
