using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concentrations
{
    public class MolInfo
    {
        /* Переменные для отображения начальных значений окон */
        public double Volume = 0;       // Объём колбы
        public double Mm = 0;           // Молярная масса
        public double g = 0;            // Величина навески
        public double Ca = 1;           // Множитель фактической концентрации
        public int Cx = 0;              // Показатель степени 10 исходной концентрации
        public double OutCa = 1;           // Множитель фактической концентрации
        public int OutCx = 0;              // Показатель степени 10 исходной концентрации

        private string mM_Folder = "/";     // Переменная, хранящая значение корневой папки в БД

        public string MM_Folder     // Корневая папка в БД
        {
            get
            {
                return mM_Folder;
            }

            set
            {
                mM_Folder = value;
            }
        }
    }
}
