using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Windows.Forms;

namespace invoiceServerApp
{
    class enviaNFCE
    {
        private XmlDocument xmlNfce;
        private XmlDocument xmlConsulta = new XmlDocument();
        private XmlDocument xmlConsultaSefaz = new XmlDocument();
        private Utils.ConfigureXml config;
        private X509Certificate2 cert;
        private string Recibo = String.Empty;
        private string retRejeiçao;
        private string retRejeiçaoMotivo;
        public bool ConsultaOK = false;
        private CallbackStatusInterface statusCupom;



        public enviaNFCE(CallbackStatusInterface _statusCupom, Utils.ConfigureXml _config, X509Certificate2 _cert)
        {
            statusCupom = _statusCupom;
            config = _config;
            cert = _cert;
        }

        public void enviaSefaz(  XmlDocument _xmlNfce)
        {
            
            xmlNfce = _xmlNfce;
            if (statusCupom.GetStatusCupom() < (int)StatusCupom.CupomEnviadoSeFaz)
                envia();
            else
                ConsultarNfce();
        }
        private void envia()
        {
            try
            {
                XmlNode resp;


                if (config.configNFCe.TpAmb.Substring(0, 1).Equals("2"))
                {

                    br.gov.mt.sefaz.homologacao.NfeAutorizacao envia = new br.gov.mt.sefaz.homologacao.NfeAutorizacao();
                    envia.ClientCertificates.Add(cert);
                    envia.nfeCabecMsgValue = new br.gov.mt.sefaz.homologacao.nfeCabecMsg();
                    envia.nfeCabecMsgValue.cUF = "51";
                    envia.nfeCabecMsgValue.versaoDados = "3.10";

                    resp = envia.nfeAutorizacaoLote(xmlNfce);
                }
                else 
                {
                    br.gov.mt.sefaz.nfce.NfeAutorizacao envia = new br.gov.mt.sefaz.nfce.NfeAutorizacao();
                    envia.ClientCertificates.Add(cert);
                    envia.nfeCabecMsgValue = new br.gov.mt.sefaz.nfce.nfeCabecMsg();
                    envia.nfeCabecMsgValue.cUF = "51";
                    envia.nfeCabecMsgValue.versaoDados = "3.00";

                    resp = envia.nfeAutorizacaoLote(xmlNfce);
                }

                foreach (XmlNode n in resp.ChildNodes)
                {
                    switch (n.Name)
                    {
                        case "cStat":
                            retRejeiçao = n.InnerText;
                            break;
                        case "xMotivo":
                            retRejeiçaoMotivo = n.InnerText;
                            break;
                        default:
                            foreach (XmlNode n2 in n)
                            {
                                if (n2.Name.Equals("nRec"))
                                    Recibo = n2.InnerText;
                            }
                            break;

                    }
                }

                if (Recibo != String.Empty)
                {
                    statusCupom.NotificationChanged((int)StatusCupom.CupomEnviadoSeFaz);
                    ConsultarNfce();
                }
                else
                {
                    Utils.Logger.getInstance.error("Erro: " + retRejeiçao + " " + retRejeiçaoMotivo);
                    //Utils.Logger.getInstance.error("Error mensagem: " + retRejeiçaoMotivo);
                
                }
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }
        }


        private void ConsultarNfce()
        {
            try
            {
                retRejeiçao = "105";

                while (retRejeiçao.Equals("105"))
                {

                    XmlNode n1 = null;

                    StringWriter xmlS = new StringWriter();
                    XmlTextWriter w = new XmlTextWriter(xmlS);
                    w.WriteStartDocument();
                    w.WriteStartElement("consReciNFe", "http://www.portalfiscal.inf.br/nfe");
                    w.WriteAttributeString("versao", "3.10");
                    w.WriteElementString("tpAmb", config.configNFCe.TpAmb.Substring(0, 1));
                    w.WriteElementString("nRec", Recibo);
                    w.WriteEndElement();
                    w.Close();

                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(xmlS.ToString());


                    if (config.configNFCe.TpAmb.Substring(0, 1).Equals("2"))
                    {

                        br.gov.mt.sefaz.homologacao1.NfeRetAutorizacao ret = new br.gov.mt.sefaz.homologacao1.NfeRetAutorizacao();

                        ret.ClientCertificates.Add(cert);
                        ret.nfeCabecMsgValue = new br.gov.mt.sefaz.homologacao1.nfeCabecMsg();
                        

                        ret.nfeCabecMsgValue.cUF = "51";
                        ret.nfeCabecMsgValue.versaoDados = "3.10";


                        n1 = ret.nfeRetAutorizacaoLote(xd);
                        
                    }
                    else
                    {

                        br.gov.mt.sefaz.nfce.NfeAutorizacao ret = new br.gov.mt.sefaz.nfce.NfeAutorizacao();

                        ret.ClientCertificates.Add(cert);

                        ret.nfeCabecMsgValue = new br.gov.mt.sefaz.nfce.nfeCabecMsg();

                        ret.nfeCabecMsgValue.cUF = "51";
                        ret.nfeCabecMsgValue.versaoDados = "3.10";

                        n1 = ret.nfeAutorizacaoLote(xd);
                    }

                    
                    foreach (XmlNode n in n1.ChildNodes)
                    {
                        foreach (XmlNode nB in n.ChildNodes)
                        {
                            if (nB.HasChildNodes)
                            {

                                foreach (XmlNode nC in nB.ChildNodes)
                                {

                                    switch (nC.Name)
                                    {
                                        case "cStat":
                                            retRejeiçao = nC.InnerText;
                                            break;
                                        case "xMotivo":
                                            retRejeiçaoMotivo = nC.InnerText;
                                            break;
                                        default:
                                            foreach (XmlNode n2 in n)
                                            {
                                                if (n2.Name.Equals("nRec"))
                                                    Recibo = n2.InnerText;
                                            }
                                            break;

                                    }
                                }
                            }
                        }
                    }

                    System.Threading.Thread.Sleep(1000);
                }
                if (!retRejeiçao.Equals("100"))
                    throw new Exception(retRejeiçaoMotivo);
                else
                {
                    ConsultaOK = true;
                    statusCupom.NotificationChanged((int)StatusCupom.CupomSeFazRetornoOk);
                }
            }
            catch (Exception ex)
            {
                Utils.Logger.getInstance.error(ex);
                throw new Exception(ex.ToString());
            }
        }


        public void verificaStatusSefaz()
        {
            try
            {
                StringWriter xmlS = new StringWriter();
                XmlTextWriter w = new XmlTextWriter(xmlS);

                w.WriteStartDocument();
                w.WriteStartElement("consStatServ", "http://www.portalfiscal.inf.br/nfe");
                w.WriteAttributeString("versao", "3.10");
                w.WriteElementString("tpAmb", "2");
                w.WriteElementString("cUF", "51");
                w.WriteElementString("xServ", "STATUS");
                w.WriteEndElement();
                w.Close();

                XmlDocument xd = new XmlDocument();
                xd.LoadXml(xmlS.ToString());


                XmlNode resp;

                if (config.configNFCe.TpAmb.Substring(0, 1).Equals("2"))
                {
                    br.gov.mt.sefaz.homologacao3.NfeStatusServico2 envia = new br.gov.mt.sefaz.homologacao3.NfeStatusServico2();
                    envia.ClientCertificates.Add(cert);

                    envia.nfeCabecMsgValue = new br.gov.mt.sefaz.homologacao3.nfeCabecMsg();
                    envia.nfeCabecMsgValue.cUF = "51";
                    envia.nfeCabecMsgValue.versaoDados = "3.10";

                    resp = envia.nfeStatusServicoNF2(xd);

                }
                else
                {
                    br.gov.mt.sefaz.nfce3.NfeStatusServico2 envia = new br.gov.mt.sefaz.nfce3.NfeStatusServico2();
                    envia.ClientCertificates.Add(cert);

                    envia.nfeCabecMsgValue = new br.gov.mt.sefaz.nfce3.nfeCabecMsg();
                    envia.nfeCabecMsgValue.cUF = "51";
                    envia.nfeCabecMsgValue.versaoDados = "3.10";

                    resp = envia.nfeStatusServicoNF2(xd);
                }
                foreach (XmlElement a in resp)
                {
                    if (a.Name.Equals("xMotivo"))
                        if (!a.InnerText.Equals("Servico em Operacao"))
                            throw new Exception("Problemas de comunicaçao com SEFAZ: " + a.InnerText);
                        else
                            break;
                }
            }
            catch (Exception ex)
            {
                Utils.Logger.getInstance.error(ex);
                throw new Exception("Problemas de comunicaçao com SEFAZ: " + ex.ToString());
            }
        
        }
    }
}
