using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Concentrations
{
    public class Config
    {
        public string DataBaseFile = "base.xml";

        public void SaveToXML(String FileName)
        {
            using (Stream writer = new FileStream(FileName, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
            }
        }

        public static Config LoadFromXML(String FileName)
        {
            // загружаем данные из файла FileName
            using (Stream stream = new FileStream(FileName, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Config));

                // в тут же созданную копию класса Serializer под именем ser
                Config ser = (Config)serializer.Deserialize(stream);
                return ser;
            }
        }
    }
}
