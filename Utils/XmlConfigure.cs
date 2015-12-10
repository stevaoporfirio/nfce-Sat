using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
//using System.Windows.Forms;


namespace Utils
{
    public class ReadConfigure
    {
        private ConfigureXml overview;
        public void SaveXML(ConfigureXml config)
        {
            if (!String.IsNullOrEmpty(config.configNFCe.SenhaCertificadoDigital) && (config.configNFCe.SenhaCertificadoDigital.Substring(0, 1).Equals("!")))
            {
                if (config.configMaquina.tipoIntegracao.Equals("NFCE"))
                {
                    string[] hexList = new string[config.configNFCe.SenhaCertificadoDigital.Length - 1];

                    int iHex = 0;
                    foreach (char c in config.configNFCe.SenhaCertificadoDigital.Substring(1))
                    {
                        hexList[iHex] = (Convert.ToInt32(c)).ToString("X");
                        iHex++;
                    }


                    //byte[] data = Encoding.UTF8.GetBytes(config.configNFCe.SenhaCertificadoDigital.Substring(1));
                    StringBuilder sBuilder = new StringBuilder();

                    for (int i = 0; i < hexList.Length; i++)
                    {
                        sBuilder.Append(hexList[i].ToString());
                    }

                    config.configNFCe.SenhaCertificadoDigital = sBuilder.ToString();
                }
                
            }
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(ConfigureXml));
            System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\AppConfig.xml");
            writer.Serialize(file, config);
            file.Close();
        }
        //public void WriteXML()
        //{
        //    try
        //    {              
        //        System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(ConfigureXml));
        //        System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\AppConfig.xml");
        //        writer.Serialize(file, overview);
        //        file.Close();
                
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.getInstance.error(e.ToString());
        //        throw new Exception(e.ToString());
        //    }
        //}
        public ConfigureXml GetConfig()
        {
            overview = new ConfigureXml();
            try
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(ConfigureXml));
                System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\AppConfig.xml");
                overview = (ConfigureXml)reader.Deserialize(file);
                file.Close();

                if (!String.IsNullOrEmpty(overview.configNFCe.SenhaCertificadoDigital) &&(!overview.configNFCe.SenhaCertificadoDigital.Substring(0, 1).Equals("!")))
                {
                    if (overview.configMaquina.tipoIntegracao.Equals("NFCE"))
                    {
                        var bytes = new byte[overview.configNFCe.SenhaCertificadoDigital.Length / 2];

                        for (var i = 0; i < bytes.Length; i++)
                        {
                            bytes[i] = Convert.ToByte(overview.configNFCe.SenhaCertificadoDigital.Substring(i * 2, 2), 16);
                        }

                        ////byte[] data = Encoding.UTF8.GetBytes(overview.configNFCe.SenhaCertificadoDigital);
                        //StringBuilder sBuilder = new StringBuilder();

                        //string crip = overview.configNFCe.SenhaCertificadoDigital;

                        //for (int i = 0; i < crip.Length; i++)
                        //{
                        //    string s = String.Format("{0}{1}", crip[i], crip[i + 1]);

                        //    sBuilder.Append(Convert.ToChar(Convert.ToInt32(s)));

                        //    i++;
                        //}

                        overview.configNFCe.SenhaCertificadoDigital = "!" + Encoding.UTF8.GetString(bytes);
                    }
                }
                else
                {
                    SaveXML(overview);
                    GetConfig();
                }
            }
            catch (Exception e)
            {
                Logger.getInstance.error(e.ToString());
                //WriteXML();                
                throw new Exception(e.ToString());
            }
                        
            return overview;

        }
    }
}
