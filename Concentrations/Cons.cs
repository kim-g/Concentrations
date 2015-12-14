/****************************************************************/
/*                                                              */
/*         Программа для расчёта навесок и концентраций         */
/*                          Версия 1.0                          */
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
    public partial class Cons : Form
    {
        public Cons()
        {
            InitializeComponent();
        }

        public bool TextBoxEdit = false;

        public void ShowModal()
        {
            try
            {
                Form1 MainForm = (Form1)Owner;
                
            }
            catch
            {
                MessageBox.Show("Ошибка чтения информации из окна навески.");
            };

            ShowDialog();

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double volume;
            double Mm;
            double g;
            char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];

            try
            {
                volume = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(Vol.Text, "[^0-9.,]", ""),
                        "[.,]", separator.ToString()));
            }
            catch (FormatException)
            {
                MessageBox.Show("Неправильно введён объём колбы.");
                return;
            };
            if (volume <= 0)
            {
                MessageBox.Show("Неправильно введён объём колбы.\nОбъём должен быть больше 0!");
                return;
            };

            try
            {
                Mm = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(MmEdit.Text, "[^0-9.,]", ""),
                        "[.,]", separator.ToString()));
            }
            catch (FormatException)
            {
                MessageBox.Show("Неправильно введена молярная масса.");
                return;
            };
            if (Mm <= 0)
            {
                MessageBox.Show("Неправильно введена молярная масса.\nМолярная масса должна быть больше 0!");
                return;
            };

            try
            {
                g = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(gEdit.Text, "[^0-9.,]", ""),
                        "[.,]", separator.ToString()));
            }
            catch (FormatException)
            {
                MessageBox.Show("Неправильно введена навеска.");
                return;
            };
            if (g <= 0)
            {
                MessageBox.Show("Неправильно введена навеска.\nНавеска должна быть больше 0!");
                return;
            };

            double Res;

            try
            {
                Res = 1000 * g / volume / Mm;
                ResultLabel.Text = "С = " + Res.ToString() + "моль/л.";
            }
            catch
            {
                MessageBox.Show("Ошибка вычислений!");
                return;
            }

            Form1 MainForm = (Form1)Owner;

            MainForm.Volume = volume;
            MainForm.Mm = Mm;
            int Cx = (int)Math.Ceiling(Math.Log10(Res)) - 1;
            double Ca = Res / Math.Pow(10, Cx); 
            MainForm.Cx = Cx;
            MainForm.Ca = Ca;
            Clipboard.SetText(Res.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Mol_Base Mol_Base_Form = new Mol_Base();
            Mol_Base_Form.Owner = this;
            Form1 MainForm = (Form1)Owner;
            string NewMm = Mol_Base_Form.GetMm(MainForm.MM_Folder);
            if (NewMm != "@Close@") { MmEdit.Text = NewMm; };
        }

        private void Vol_TextChanged(object sender, EventArgs e)
        {
            if (!TextBoxEdit) { Form1.NumbersOnly(sender); };
        }
    }
}
