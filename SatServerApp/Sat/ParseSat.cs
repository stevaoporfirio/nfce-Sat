using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Windows.Forms;

namespace invoiceServerApp
{
    class ParseSat
    {
        private CFe vendasRet;
        private string[] msgSat = null;
        public string QrCode;
        string assQRCode;

        ParseSat(string msg)
        {

            msgSat = msg.Split('|');
            if (msgSat[1].Equals("06000") == true)
            {
                if (msgSat.Length > 6)
                {
                    try
                    {
                        string string64 = Encoding.UTF8.GetString(Convert.FromBase64String(msgSat[6]));
                        XDocument doc = XDocument.Parse(string64);
                        System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(CFe));
                        StringReader StrRead = new StringReader(string64);

                        vendasRet = (CFe)reader.Deserialize(StrRead);
                    }
                    catch (Exception e)
                    {
                        Utils.Logger.getInstance.error(e.ToString());
                    }
                }
            }
        }
        public string qrCodeMount()
        {
            assQRCode = msgSat[11];
            string chaveConsulta = vendasRet.infCFe.Id;
            string timestamp = vendasRet.infCFe.ide.dEmi + vendasRet.infCFe.ide.hEmi;
            string valor = vendasRet.infCFe.total.vCFe;
            string destinatario = vendasRet.infCFe.ide.CNPJ;

            StringBuilder codigo = new StringBuilder("");
            codigo.Append(chaveConsulta).Append("|").Append(timestamp).Append("|").Append(valor).Append("|").Append(destinatario).Append("|").Append(assQRCode);
            QrCode = codigo.ToString();

            return QrCode;

        }


        public string messageParse(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
