/****************************************************************/
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
    class MolarMass
    {
        private string name;
        private string mm;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Mm
        {
            get
            {
                return mm;
            }

            set
            {
                try
                {
                    char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];

                    mm = Regex.Replace(
                            Regex.Replace(value, "[^0-9.,]", ""),
                            "[.,]", separator.ToString());
                }
                catch (FormatException)
                {
                    MessageBox.Show("Модуль MolarMass.cs. Ошибка записи молярной массы.");
                    mm = "0";
                    return;
                };
            }
        }
    }
}
