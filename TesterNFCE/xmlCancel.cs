using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.IO;


namespace TesterNFCE
{
    class xmlCancel
    {
        private X509Certificate2 cert = null;
        private Utils.ConfigureXml config;
        public XmlDocument xmlDoc = new XmlDocument();
        public xmlCancel(X509Certificate2 _cert, Utils.ConfigureXml _config)
        {
            cert = _cert;
            config = _config;
            processCancel();
        }
        public void processCancel()
        {
            try
            {
                //XmlDocument xmlDoc = new XmlDocument();
                StringWriter XmlString = new StringWriter();
                XmlTextWriter xmlw = new XmlTextWriter(XmlString);

                
                
                xmlw.WriteStartElement("envEvento", "http://www.portalfiscal.inf.br/nfe");
                xmlw.WriteAttributeString("versao", "1.00");
                xmlw.WriteElementString("idLote", "1");
                xmlw.WriteStartElement("evento", "http://www.portalfiscal.inf.br/nfe");
                xmlw.WriteAttributeString("versao", "1.00");
                xmlw.WriteStartElement("infEvento");
                // xmlw.WriteAttributeString("Id", String.Format("{0}{1}{2}{3}", "ID", "110111", row["chave"].ToString(), "01"));
                xmlw.WriteAttributeString("Id", String.Format("{0}{1}{2}{3}", "ID", "110111", "33150115051092000297650030000000381000000388", "01"));
                //xmlw.WriteAttributeString("Id", String.Format("{0}{1}", "ID", "33150115051092000297650030000000381000000388"));
                xmlw.WriteElementString("cOrgao", config.configEmitente.endereco.Cod_estado);
                xmlw.WriteElementString("tpAmb", config.configNFCe.TpAmb.Substring(0, 1));
                xmlw.WriteElementString("CNPJ", config.configEmitente.Cnpj);
                xmlw.WriteElementString("chNFe", "33150115051092000297650030000000381000000388");
                xmlw.WriteElementString("dhEvento", DateTime.Now.ToString("yyyy-MM-ddThh:mm:sszzz"));
                xmlw.WriteElementString("tpEvento", "110111");
                xmlw.WriteElementString("nSeqEvento", "1");
                xmlw.WriteElementString("verEvento", "1.00");
                xmlw.WriteStartElement("detEvento");
                xmlw.WriteAttributeString("versao", "1.00");
                xmlw.WriteElementString("descEvento", "Carta de Correcao");
                xmlw.WriteElementString("nProt", "333065000038131");
                xmlw.WriteElementString("xJust", "Cancelamento de nota");
                // xmlw.WriteElementString("xCorrecao", "Cancelamento de nota");
                //xmlw.WriteElementString("xCondUso", cond);
                xmlw.WriteEndElement();
                xmlw.WriteEndElement();
                xmlw.WriteEndElement();
                xmlw.WriteEndElement();
                xmlw.Close();
              
              xmlDoc.LoadXml(XmlString.ToString());
              assinaturaXMLCancel();
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
            }
        }
        public void assinaturaXMLCancel()
        {
            
            try
            {
                Reference reference = new Reference();
                SignedXml docXML = new SignedXml(xmlDoc);

                try
                {
                    docXML.SigningKey = cert.PrivateKey;

                    XmlAttributeCollection uri = xmlDoc.GetElementsByTagName("infEvento").Item(0).Attributes;

                    foreach (XmlAttribute atributo in uri)
                    {
                        if (atributo.Name == "Id")
                            reference.Uri = "#" + atributo.InnerText;
                    }
                }
                catch (Exception ex)
                {
                    Utils.Logger.getInstance.error(ex);
                    throw new Exception("Erro em Assinatura p1" + ex.Message);
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

                foreach (var _nfe in xmlDoc.GetElementsByTagName("evento").Cast<XmlElement>())
                {
                    _nfe.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
                }
            }
            catch (Exception ex)
            {
                Utils.Logger.getInstance.error(ex);
                throw new Exception("Erro preparando Assinatura Digital\n" + ex.Message);
            }

            //xmlDoc.PreserveWhitespace = true;


        }
    }
}
