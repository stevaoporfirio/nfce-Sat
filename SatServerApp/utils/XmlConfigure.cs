using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace invoiceServerApp
{
    class ReadConfigure
    {
        private ConfigureXml overview;
        public void SaveXML(ConfigureXml config)
        {
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(ConfigureXml));
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"AppConfig.xml");
            writer.Serialize(file, config);
            file.Close();
        }
        public void WriteXML()
        {
            try
            {              
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(ConfigureXml));
                System.IO.StreamWriter file = new System.IO.StreamWriter(@"AppConfig.xml");
                writer.Serialize(file, overview);
                file.Close();
                
            }
            catch (Exception e)
            {
                Logger.getInstance.error(e.ToString());
                throw new Exception(e.ToString());
            }
        }
        public ConfigureXml GetConfig()
        {
            overview = new ConfigureXml();
            try
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(ConfigureXml));
                System.IO.StreamReader file = new System.IO.StreamReader(@"AppConfig.xml");
                overview = (ConfigureXml)reader.Deserialize(file);
                file.Close();
            }
            catch (Exception e)
            {
                WriteXML();
                Logger.getInstance.error(e.ToString());
                throw new Exception(e.ToString());
            }

            return overview;

        }
    }
}
