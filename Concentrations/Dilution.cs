﻿/****************************************************************/
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
    public partial class Dilution : Form
    {
        public Dilution()
        {
            InitializeComponent();
        }

        public bool TextBoxEdit = false;

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void Vol_TextChanged(object sender, EventArgs e)
        {
            if (!TextBoxEdit) { Form1.NumbersOnly(sender); };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double volume;
            double InCa, Ca;
            int InCx, Cx;
 
            // Объём
            try
            {
                volume = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(Vol.Text, "[^0-9.,]", ""),
                        "[.,]", Form1.separator.ToString()));
            }
            catch(FormatException)
            {
                MessageBox.Show("Неправильно введён объём колбы.");
                return;
            }
            if (volume <= 0)
            {
                MessageBox.Show("Неправильно введён объём колбы.\nОбъём должен быть больше 0!");
                return;
            };

            //InCa
            try
            {
                InCa = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(InCaEdit.Text, "[^0-9.,]", ""),
                        "[.,]", Form1.separator.ToString()));
                InCx = 0 - Convert.ToInt32(Regex.Replace(InCxEdit.Text, "[^0-9]", ""));
            }
            catch (FormatException)
            {
                MessageBox.Show("Неправильно введена исходная концетрация.");
                return;
            }
            if (InCa <= 0)
            {
                MessageBox.Show("Неправильно введена исходная концетрация.\nМножитель исходной концентрации должен быть больше 0!");
                return;
            };

            //Ca
            try
            {
                Ca = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(CaEdit.Text, "[^0-9.,]", ""),
                        "[.,]", Form1.separator.ToString()));
                Cx = 0 - Convert.ToInt32(Regex.Replace(CxEdit.Text, "[^0-9]", ""));
            }
            catch (FormatException)
            {
                MessageBox.Show("Неправильно введена исходная концетрация.");
                return;
            }
            if (InCa <= 0)
            {
                MessageBox.Show("Неправильно введена исходная концетрация.\nМножитель исходной концентрации должен быть больше 0!");
                return;
            };

            double Res;
            //Вычисление
            try
            {
                Res = volume * Ca * Math.Pow(10, Cx) / InCa / Math.Pow(10, InCx);
            }
            catch
            {
                MessageBox.Show("Ошибка вычислений");
                return;
            };

            ResultLabel.Text = "Аликвота " + Res.ToString() + " мл.";

            Form1 MainForm = (Form1)Owner;
            MainForm.Volume = volume;
            MainForm.Ca = InCa;
            MainForm.Cx = InCx;
        }
    }
}
