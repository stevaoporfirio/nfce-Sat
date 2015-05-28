using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invoiceServerApp
{
    class ManagerConfig
    {
        private ConfigureContigencia overview;
        public void SaveXML(ConfigureContigencia config)
        {
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(ConfigureContigencia));
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"AppConfigContingencia.xml");
            writer.Serialize(file, config);
            file.Close();
        }
        public void WriteXML()
        {
            try
            {
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(ConfigureContigencia));
                System.IO.StreamWriter file = new System.IO.StreamWriter(@"AppConfigContingencia.xml");
                writer.Serialize(file, overview);
                file.Close();

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        public ConfigureContigencia GetConfig()
        {
            overview = new ConfigureContigencia();
            try
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(ConfigureContigencia));
                System.IO.StreamReader file = new System.IO.StreamReader(@"AppConfigContingencia.xml");
                overview = (ConfigureContigencia)reader.Deserialize(file);
                file.Close();
            }
            catch (Exception e)
            {
                WriteXML();
                
            }
            return overview;
        }
    }
}
