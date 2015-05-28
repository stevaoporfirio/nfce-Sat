using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.IO;
using System.Data;
using System.Security.Cryptography.Xml;
using System.Globalization;

namespace invoiceServerApp
{
    class makeXmlCancel
    {
        private Utils.ConfigureXml config;
        private X509Certificate2 cert = null;
        private CancelNFCE dtNfce;
        public XmlDocument xmlDoc = new XmlDocument();
        public string nomeXml;
        public string id_banco { get; set; }
        public makeXmlCancel(CancelNFCE _dtNfce, Utils.ConfigureXml _config, X509Certificate2 _cert)
        {
            config = _config;
            dtNfce = _dtNfce;
            cert = _cert;
            processCancel();
            string a = (String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles+ "\\canceladas", nomeXml));
            xmlDoc.Save(String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles+ "\\canceladas", nomeXml));
            assinaturaXML();
        }
        public void processCancel()
        {
            try
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("pt-BR");


                DataTable dt = ManagerDB.Instance.selectDadosNfce(dtNfce.ID);

                StringWriter XmlString = new StringWriter();

                XmlTextWriter xmlw = new XmlTextWriter(XmlString);

                if (dt.Rows.Count >= 1)
                {
                    foreach (DataRow row in dt.Rows)
                    {

                        nomeXml = DateTime.Now.ToString("MMddyyyyHHmm") + row["nprot"].ToString();
                        id_banco = row["id"].ToString();

                        xmlw.WriteStartElement("envEvento", "http://www.portalfiscal.inf.br/nfe");
                        xmlw.WriteAttributeString("versao", "1.00");
                        xmlw.WriteElementString("idLote", "01");
                        xmlw.WriteStartElement("evento");
                        xmlw.WriteAttributeString("versao", "1.00");
                        xmlw.WriteStartElement("infEvento");
                        xmlw.WriteAttributeString("Id", String.Format("{0}{1}{2}{3}", "ID", "110111", row["chave"].ToString(), "01"));
                        xmlw.WriteElementString("cOrgao", config.configEmitente.endereco.Cod_estado);
                        xmlw.WriteElementString("tpAmb", config.configNFCe.TpAmb.Substring(0, 1));
                        xmlw.WriteElementString("CNPJ", config.configEmitente.Cnpj);
                        xmlw.WriteElementString("chNFe", row["chave"].ToString());
                        xmlw.WriteElementString("dhEvento", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz", ci));
                        xmlw.WriteElementString("tpEvento", "110111");
                        xmlw.WriteElementString("nSeqEvento", "1");
                        xmlw.WriteElementString("verEvento", "1.00");
                        xmlw.WriteStartElement("detEvento");
                        xmlw.WriteAttributeString("versao", "1.00");
                        xmlw.WriteElementString("descEvento", "Cancelamento");
                        xmlw.WriteElementString("nProt", row["nprot"].ToString());
                        xmlw.WriteElementString("xJust", "Teste de Cancelamento como Evento");
                        xmlw.WriteEndElement();
                        xmlw.WriteEndElement();
                        xmlw.WriteEndElement();
                        xmlw.Close();
                    }
                }
                xmlDoc.LoadXml(XmlString.ToString());
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
            }
        }
        private void assinaturaXML()
        {
            try
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
            catch (Exception ex)
            {
                Utils.Logger.getInstance.error(ex);
                throw ex;
            }

        }
            
    }
}
