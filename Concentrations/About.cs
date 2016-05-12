/****************************************************************/
/*                                                              */
/*         Программа для расчёта навесок и концентраций         */
/*                          Версия 1.1                          */
/*                     Модуль «О программе»                     */
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
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Concentrations
{
    public partial class About : Form  //Окно «О программе»
    {
        // константы.
        // Автор
        private const string Author = "Grigory A. Kim";
        // Организация
        private const string Organization = "Institute of Organic Synthesis UB RAS, Ekaterinburg, Russia";
        // Годы действия лицензии
        private string Year = "2015-" + DateTime.Now.Year.ToString();
        // Сама лицензия
        private const string BSD_License = "Copyright (c) <YEAR>, <OWNER>. All rights reserved.\n\n" +
                                           "Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:\n\n"+
                                           "Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.\n\n" +
                                           "Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.\n\n" +
                                           "Neither the name of the <ORGANIZATION> nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.\n\n" +
                                           "THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS \"AS IS\" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.";

        public About()
        {
            InitializeComponent();
            label1.Text = Regex.Replace(label1.Text, "<YEAR>", DateTime.Now.Year.ToString());
            VersionLabel.Text = "Версия " + Form1.Version;  //Пишем номер версии (Константа Form1)
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("mailto:kim-g@ios.uran.ru");      //Открываем почту по умолчанию, письмо мне ;-)
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();                                        // Закрываем окно
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Формируем текст лицензии, заменяя места для вставки на константы
            string TextToShow = Regex.Replace(      
                                                Regex.Replace(
                                                                Regex.Replace( 
                                                                                BSD_License, "<ORGANIZATION>", Organization
                                                                              ) , "<YEAR>", Year
                                                              ), "<OWNER>", Author + ", " + Organization
                                              );
            MessageBox.Show(TextToShow,"BSD License");      // ...и выводим его
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://kim-g.ru/concentrations");    //Переходим на страницу скачивания.
        }
    }
}
