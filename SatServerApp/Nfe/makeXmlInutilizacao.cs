using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Data;
using System.IO;
using System.Security.Cryptography.Xml;

namespace invoiceServerApp
{
    class makeXmlInutilizacao
    {
                private Utils.ConfigureXml config;
        private X509Certificate2 cert = null;
        private InutilizacaoNFCE dtNfce;
        public XmlDocument xmlDoc = new XmlDocument();
        public string nomeXml;
        public string id_banco { get; set; }
        public makeXmlInutilizacao(InutilizacaoNFCE _dtNfce, Utils.ConfigureXml _config, X509Certificate2 _cert)
        {
            config = _config;
            dtNfce = _dtNfce;
            cert = _cert;
            processCancel();
            string a = (String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles , nomeXml));
            xmlDoc.Save(String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles, nomeXml));
            assinaturaXML();
        }
        public void processCancel()
        {
            try
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("pt-BR");


                
                StringWriter XmlString = new StringWriter();

                XmlTextWriter xmlw = new XmlTextWriter(XmlString);

                DataTable dt = ManagerDB.Instance().selectDadosNfce(dtNfce.ID);
                foreach (DataRow row in dt.Rows)
                {
                    id_banco = row["id"].ToString();
                }
                
                DateTime _date = DateTime.Now;
                string ano = _date.ToString("yy");



                string id = "ID" + config.configEmitente.endereco.Cod_estado + ano + config.configEmitente.Cnpj + "65" + config.configNFCe.Serie.PadLeft(3, '0') + dtNfce.numeroInicial.PadLeft(9, '0') + dtNfce.numeroFinal.PadLeft(9, '0');

                nomeXml = DateTime.Now.ToString("MMddyyyyHHmm") + id;


                xmlw.WriteStartElement("inutNFe", "http://www.portalfiscal.inf.br/nfe");
                xmlw.WriteAttributeString("versao", "3.10");
                xmlw.WriteStartElement("infInut", "http://www.portalfiscal.inf.br/nfe");
                xmlw.WriteAttributeString("Id", id);
                xmlw.WriteElementString("tpAmb", config.configNFCe.TpAmb);
                xmlw.WriteElementString("xServ", "INUTILIZAR");
                xmlw.WriteElementString("cUF", config.configEmitente.endereco.Cod_estado);
                xmlw.WriteElementString("ano", ano);
                xmlw.WriteElementString("CNPJ", config.configEmitente.Cnpj);
                xmlw.WriteElementString("mod", "65");
                xmlw.WriteElementString("serie", config.configNFCe.Serie);
                xmlw.WriteElementString("nNFIni", dtNfce.numeroInicial);
                xmlw.WriteElementString("nNFFin", dtNfce.numeroFinal);
                xmlw.WriteElementString("xJust", "Ajuste fiscal nfce");
                xmlw.WriteEndElement();
                xmlw.WriteEndElement();
                xmlw.Close();

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

                        XmlAttributeCollection uri = xmlDoc.GetElementsByTagName("infInut").Item(0).Attributes;

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

                    foreach (var _nfe in xmlDoc.GetElementsByTagName("inutNFe").Cast<XmlElement>())
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
