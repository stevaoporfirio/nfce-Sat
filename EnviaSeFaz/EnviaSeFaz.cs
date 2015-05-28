using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Messaging;
using System.Xml;

namespace EnviaSeFaz
{
    class EnviaSeFaz
    {
        private Utils.ConfigureXml config;
        private InterfaceEnvio enviaSeFaz;
        private MessageQueue mq; //FILA

        public EnviaSeFaz(Utils.ConfigureXml _config, X509Certificate2 _cert)
        {
            mq = new MessageQueue(@".\Private$\NFCe_Contingencia", false);

            if (config.configEmitente.endereco.Uf.Equals("MT"))
                enviaSeFaz = new EnviaNfceMT();
            else if (config.configEmitente.endereco.Uf.Equals("AM"))
                enviaSeFaz = new EnviaNfceAM();

            enviaSeFaz.configura(_config,_cert);
        
        }
        public void verificaStatusSefaz()
        {

            if (mq.GetAllMessages().Count() > 0)
                throw new Exception("Contingencia");

            enviaSeFaz.verificaStatusSefaz();
        }
        public void enviaSefaz(XmlDocument _xmlNfce)
        {
            enviaSeFaz.enviaSefaz(_xmlNfce);
        }
    }
}
