using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Concentrations
{
    public partial class Mass : Form
    {
        public Mass()
        {
            InitializeComponent();
        }

        public void ShowModal()
        {
            ShowDialog();
        }

        private void Mass_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Form1.NumbersOnly(sender);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            double volume;
            double Mm;
            double Ca;
            double Cx;
            char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];

            //Ввод объёма
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
            if (volume<=0)
            {
                MessageBox.Show("Неправильно введён объём колбы.\nОбъём должен быть больше 0!");
                return;
            };

            //Ввод молярной массы
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

            //Ввод концентрации
            try
            {
                Ca = Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(CaEdit.Text, "[^0-9.,]", ""),
                        "[.,]", separator.ToString()));
                Cx = 0 - Convert.ToSingle(
                    Regex.Replace(
                        Regex.Replace(CxEdit.Text, "[^0-9.,]", ""),
                        "[.,]", separator.ToString()));
            }
            catch (FormatException)
            {
                MessageBox.Show("Неправильно введена желаемая концентрация.");
                return;
            };
            if (Ca <= 0)
            {
                MessageBox.Show("Неправильно введена желаемая концентрация.\nЖелаемая концентрация должна быть больше 0!");
                return;
            };

            double Res;
            try
            {
                Res = volume * Mm * Ca * Math.Pow(10, Cx) / 1000;
                ResultLabel.Text = "g = " + Res.ToString("N5") + " г.";
            }
            catch
            {
                MessageBox.Show("Ошибка вычислений");
                return;
            };

            try
            {
                Clipboard.SetText(Res.ToString());

                Form1 MainForm = (Form1)this.Owner;

                MainForm.Volume = volume;
                MainForm.Mm = Mm;
                MainForm.g = Res;
            }
            catch
            {
                MessageBox.Show("Ошибка взаимодействия форм");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Mol_Base Mol_Base_Form = new Mol_Base();
            string NewMm = Mol_Base_Form.GetMm();
            if (NewMm != "@Close@")
            {
                MmEdit.Text = NewMm;
            };
        }
    }
}
