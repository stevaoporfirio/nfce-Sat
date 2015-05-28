using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;

namespace TesterNFCE
{
    class xmlEnvio
    {
        private X509Certificate2 cert = null;
        private Utils.ConfigureXml config;
        public xmlEnvio(X509Certificate2 _cert, Utils.ConfigureXml _config)
        {
            cert = _cert;
            config = _config;
        
        }
        public XmlDocument assinaturaXmlEnviar(XmlDocument _xml)
        {
            XmlDocument xmlDocAss = _xml;

            try
            {


                if (cert == null)
                    throw new Exception("Nao foi encontrado o certificado: " + config.configNFCe.NomeCertificadoDigital);

                Reference reference = new Reference();
                SignedXml docXML = new SignedXml(xmlDocAss);

                docXML.SigningKey = cert.PrivateKey;
                XmlAttributeCollection uri = xmlDocAss.GetElementsByTagName("infNFe").Item(0).Attributes;
                foreach (XmlAttribute atributo in uri)
                {
                    if (atributo.Name == "Id")
                        reference.Uri = "#" + atributo.InnerText;
                }

                XmlDsigEnvelopedSignatureTransform envelopedSigntature = new XmlDsigEnvelopedSignatureTransform();
                reference.AddTransform(envelopedSigntature);

                XmlDsigC14NTransform c14Transform = new XmlDsigC14NTransform();
                reference.AddTransform(c14Transform);
                docXML.AddReference(reference);

                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(new KeyInfoX509Data(cert));

                docXML.KeyInfo = keyInfo;
                docXML.ComputeSignature();

                XmlElement xmlDigitalSignature = docXML.GetXml();

                foreach (var _nfe in xmlDocAss.GetElementsByTagName("NFe").Cast<XmlElement>())
                    _nfe.AppendChild(xmlDocAss.ImportNode(xmlDigitalSignature, true));


                xmlDocAss.PreserveWhitespace = true;
                return xmlDocAss;
            }

            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                return null;
                throw new Exception(e.ToString());
            }


        }
    }
}
