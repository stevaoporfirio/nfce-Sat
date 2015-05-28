using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Xml;


namespace EnviaSeFaz
{
    interface InterfaceEnvio
    {
        void configura(Utils.ConfigureXml _config, X509Certificate2 _cert);
        void verificaStatusSefaz();
        void enviaSefaz(XmlDocument _xmlNfce);
        bool ConsultaOK();
        bool ConsultaContingencia(string _recibo);
        bool CancelamentoCupom(XmlDocument _xmlNfce);
        bool InutilizacaoCupom(XmlDocument _xmlNfce);

        string GetRecibo();
        string GetStatus();
        XmlDocument GetXmlOK();
        XmlDocument GetXmlCancelamento();
        string GetnProt();
    }
}
