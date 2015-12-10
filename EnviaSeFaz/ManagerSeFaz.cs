using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Messaging;
using System.Xml;

namespace EnviaSeFaz
{
    public class ManagerSeFaz
    {
        private Utils.ConfigureXml config;
        private InterfaceEnvio enviaSeFaz;

        public ManagerSeFaz(Utils.ConfigureXml _config, X509Certificate2 _cert)
        {
            config = _config;
    
            switch (config.configEmitente.endereco.Uf)
            { 
                case "MT":
                    enviaSeFaz = new EnviaNfceMT();
                    break;
                case "AM":
                    enviaSeFaz = new EnviaNfceAM();
                    break;
                case "RS":
                    enviaSeFaz = new EnviaNfceRS();
                    break;
                case "PR":
                    enviaSeFaz = new EnviaNfcePR();
                    break;
                default:
                    enviaSeFaz = new EnviaNfceRJ();
                    break;
            }

            enviaSeFaz.configura(_config, _cert);
        
        }
        public void verificaStatusSefaz()
        {
            enviaSeFaz.verificaStatusSefaz();
        }
        public void enviaSefaz(XmlDocument _xmlNfce)
        {
            enviaSeFaz.enviaSefaz(_xmlNfce);
        }
        public bool ConsultaOK()
        {
            return enviaSeFaz.ConsultaOK();
        }

        public string GetStatus()
        {
            return enviaSeFaz.GetStatus();
        }

        public string GetRecibo()
        {
            return enviaSeFaz.GetRecibo();
        }
        public XmlDocument GetXmlCancelamento()
        {
            return enviaSeFaz.GetXmlCancelamento();
        }
        public XmlDocument GetXmlOK()
        {
            return enviaSeFaz.GetXmlOK();
        }
        public bool ConsultaContingencia(string _recibo)
        {   
            return enviaSeFaz.ConsultaContingencia(_recibo);
        }
        public bool CancelamentoNfce(XmlDocument _xmlNfce)
        {
            return enviaSeFaz.CancelamentoCupom(_xmlNfce);
        }
        public bool InutilizacaoNfce(XmlDocument _xmlNfce)
        {
            return enviaSeFaz.InutilizacaoCupom(_xmlNfce);
        }

        public string GetnProt()
        {
            return enviaSeFaz.GetnProt();
        }
    }
}
