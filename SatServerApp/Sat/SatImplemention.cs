using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SatServerApp
{
    public class SatImplemention: NotificationChanged
    {
        private ConfigureSat config;
        private ParseSatReceive parseRec;

        public SatImplemention(ConfigureSat _config) 
        {
            config = _config;
            ConsultaSat();
        }

        public string NotificationChangedClient(string receive)
        {
            return ParseToXmlVenda(receive);
        }
        public string ParseToXmlVenda(string msg) 
        {
         
            if (msg == "COD")
                return parseRec.QRCode + "|C|";

            else if (msg == "QRC")
                return parseRec.QRCode + "|Q|";

            else
            {
                List<String> ls = new List<String>();
                msg = "";
                using (StreamReader sr = new StreamReader("e:\\dadpsSAT.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        if (sr.ReadLine() != "")
                        {
                            ls.Add(sr.ReadLine());
                            msg += sr.ReadLine() + "\n";
                        }
                    }
                }
                
                
                ParseSatSend parse = new ParseSatSend(msg, config);
                string s = parse.xmlString;
                int a = utils.getRandom(6);
                string b = "porfirio";
                string ret = dllSat.EnviarDadosVenda(a,b , s);
                parseRec = new ParseSatReceive(ret);
                return parseRec.textPrint+"|X|";
            }

        }

        public void ConsultaSat()
        {
            try
            {
                int a = utils.getRandom(6);
                string b = dllSat.ConsultarSAT(a);
                MessageBox.Show(b);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        
        }
    }
}
