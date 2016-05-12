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
        MolInfo GenInfo;
        bool TextBoxEdit = false;    // Флаг. Внешнее окно производит редактирование полей ввода. 

        public Mass(MolInfo Info)
        {
            InitializeComponent();
            TextBoxEdit = true;
            Vol.Text = Info.Volume.ToString();
            MmEdit.Text = Info.Mm.ToString();
            CaEdit.Text = Info.Ca.ToString();
            CxEdit.Text = Info.Cx.ToString();
            TextBoxEdit = false;
        }

        public MolInfo ShowDialog(MolInfo Info) //Подготовить и показать окно модально
        {
            GenInfo = Info;
            base.ShowDialog();   // Готовить не надо, только покажем
            return GenInfo;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)   // При изменении текста
        {
            if (!TextBoxEdit) { Form1.NumbersOnly(sender); };       //Оставляем только числа и знак десятичного разделителя.
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
                                                        // ...и посылаем обратно итоговые данные об:
                GenInfo.Volume = volume;                // - объёме колбы
                GenInfo.Mm = Mm;                       // - молярной массе
                GenInfo.g = Res;                       // - навеске
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

            string NewMm = Mol_Base_Form.GetMm(GenInfo); // Запрашиваем значение из базы
            if (NewMm != "@Close@")                             // Если пользователь не отменил
            {
                MmEdit.Text = NewMm;                            //... то выводим результат
            };
        }
    }
}
