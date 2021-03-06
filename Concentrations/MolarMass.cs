﻿/****************************************************************/
/*                                                              */
/*         Программа для расчёта навесок и концентраций         */
/*                          Версия 1.0                          */
/*         Класс, содержащий описание молекулы для базы         */
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
    class MolarMass     // Класс для описания молекулы
    {
        private string name;    // Хранилище имени
        private string mm;      // Хранилище молярной массы

        public string Name      // Свойство имени соединения
        {
            get
            {
                return name;    // Получаем напрямую из переменной
            }

            set
            {
                name = value;   // Присваиваем переменной напрямую
            }
        }

        public string Mm        // Свойство молярной массы соединения
        {
            get
            {
                return mm;      // Получаем напрямую
            }

            set
            {
                try             
                {
                     mm = Regex.Replace(
                            Regex.Replace(value, "[^0-9.,]", ""),
                            "[.,]", Form1.separator.ToString()); // оставляем только числа и десятичный разделитель
                }
                catch (FormatException)     // В  случае ошибки
                {
                    MessageBox.Show("Модуль MolarMass.cs. Ошибка записи молярной массы.");  // Сообщим
                    mm = "0";               // Присвоим значение 0
                    return;
                };
            }
        }
    }
}
