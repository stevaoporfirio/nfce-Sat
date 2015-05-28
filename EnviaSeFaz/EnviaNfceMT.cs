﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace EnviaSeFaz
{
    class EnviaNfceMT : InterfaceEnvio
    {
        private XmlDocument xmlNfce;
        private XmlDocument xmlConsulta = new XmlDocument();
        private XmlDocument xmlConsultaSefaz = new XmlDocument();
        private XmlDocument xmlFinal = new XmlDocument();
        private XmlDocument xmlCancelamento;
        private Utils.ConfigureXml config;
        private X509Certificate2 cert;
        private string Recibo = String.Empty;
        private string retRejeiçao;
        private string retRejeiçaoMotivo;
        public bool ConsultaSefaz = false;
        private string nProt;
       


        public EnviaNfceMT()
        { 
        
        }
        public void configura(Utils.ConfigureXml _config, System.Security.Cryptography.X509Certificates.X509Certificate2 _cert)
        {
            //statusCupom = _statusCupom;
            config = _config;
            cert = _cert;
        }

        public void verificaStatusSefaz()
        {
            try
            {
                //throw new Exception("Problemas de comunicaçao com SEFAZ: " );
                StringWriter xmlS = new StringWriter();
                XmlTextWriter w = new XmlTextWriter(xmlS);

                w.WriteStartDocument();
                w.WriteStartElement("consStatServ", "http://www.portalfiscal.inf.br/nfe");
                w.WriteAttributeString("versao", config.configNFCe.VersaoLayout);
                w.WriteElementString("tpAmb", config.configNFCe.TpAmb);
                w.WriteElementString("cUF", "51");
                w.WriteElementString("xServ", "STATUS");
                w.WriteEndElement();
                w.Close();

                XmlDocument xd = new XmlDocument();
                xd.LoadXml(xmlS.ToString());


                XmlNode resp;

                if (config.configNFCe.TpAmb.Substring(0, 1).Equals("2"))
                {
                    br.gov.mt.sefaz.homologacao2.NfeStatusServico2 envia = new br.gov.mt.sefaz.homologacao2.NfeStatusServico2();

                    envia.Url = "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeStatusServico2";

                    envia.ClientCertificates.Add(cert);

                    envia.nfeCabecMsgValue = new br.gov.mt.sefaz.homologacao2.nfeCabecMsg();
                    envia.nfeCabecMsgValue.cUF = "51";
                    envia.nfeCabecMsgValue.versaoDados = config.configNFCe.VersaoLayout;

                    resp = envia.nfeStatusServicoNF2(xd);

                }
                else
                {
                    br.gov.mt.sefaz.nfce2.NfeStatusServico2 envia = new br.gov.mt.sefaz.nfce2.NfeStatusServico2();

                    envia.Url = "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeStatusServico2";

                    envia.ClientCertificates.Add(cert);

                    envia.nfeCabecMsgValue = new br.gov.mt.sefaz.nfce2.nfeCabecMsg();
                    envia.nfeCabecMsgValue.cUF = "51";
                    envia.nfeCabecMsgValue.versaoDados = config.configNFCe.VersaoLayout;

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

        public void enviaSefaz(System.Xml.XmlDocument _xmlNfce)
        {
            xmlNfce = _xmlNfce;
                envia();
        }

        public bool ConsultaOK()
        {
            ConsultarNfce();
            return ConsultaSefaz;
        }

        public string GetRecibo()
        {
            return Recibo;
        }
        public string GetStatus()
        {
            if (!String.IsNullOrEmpty(retRejeiçao) && !String.IsNullOrEmpty(retRejeiçaoMotivo))
                return String.Format("{0}/{1}", retRejeiçao, retRejeiçaoMotivo);
            else
                return "";
        }

        private void envia()
        {
            try
            {
                XmlNode resp;


                if (config.configNFCe.TpAmb.Substring(0, 1).Equals("2"))
                {

                    br.gov.mt.sefaz.homologacao.NfeAutorizacao envia = new br.gov.mt.sefaz.homologacao.NfeAutorizacao();

                    envia.Url = "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeAutorizacao";

                    envia.ClientCertificates.Add(cert);
                    envia.nfeCabecMsgValue = new br.gov.mt.sefaz.homologacao.nfeCabecMsg();
                    envia.nfeCabecMsgValue.cUF = "51";
                    envia.nfeCabecMsgValue.versaoDados = config.configNFCe.VersaoLayout;

                    resp = envia.nfeAutorizacaoLote(xmlNfce);
                }
                else
                {
                    br.gov.mt.sefaz.nfce.NfeAutorizacao envia = new br.gov.mt.sefaz.nfce.NfeAutorizacao();

                    envia.Url = "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeAutorizacao";

                    envia.ClientCertificates.Add(cert);
                    envia.nfeCabecMsgValue = new br.gov.mt.sefaz.nfce.nfeCabecMsg();
                    envia.nfeCabecMsgValue.cUF = "51";
                    envia.nfeCabecMsgValue.versaoDados = config.configNFCe.VersaoLayout;

                    resp = envia.nfeAutorizacaoLote(xmlNfce);
                }
                retRejeiçao = getElementXml(resp, "cStat");
                retRejeiçaoMotivo = getElementXml(resp, "xMotivo");
                Recibo = getElementXml(resp,"nRec");
                if (Recibo == String.Empty)
                {
                    Utils.Logger.getInstance.error("Nota sem recibo na SEFAZ: " +resp.InnerText);
                    throw new ApplicationException("Nota sem recibo na SEFAZ");
                }
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw e;
            }
        }
       
        private void ConsultarNfce()
        {
            try
            {
                retRejeiçao = "105";
                XmlNode n1 = null;
                XmlNode nr1 = null;
                XmlDocument retNode = new XmlDocument();
                while (retRejeiçao.Equals("105"))
                {

                    StringWriter xmlS = new StringWriter();
                    XmlTextWriter w = new XmlTextWriter(xmlS);
                    w.WriteStartDocument();
                    w.WriteStartElement("consReciNFe", "http://www.portalfiscal.inf.br/nfe");
                    w.WriteAttributeString("versao", config.configNFCe.VersaoLayout);
                    w.WriteElementString("tpAmb", config.configNFCe.TpAmb.Substring(0, 1));
                    w.WriteElementString("nRec", Recibo);
                    w.WriteEndElement();
                    w.Close();

                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(xmlS.ToString());

                    Utils.Logger.getInstance.error(xmlS.ToString());
                    if (config.configNFCe.TpAmb.Substring(0, 1).Equals("2"))
                    {

                        br.gov.mt.sefaz.homologacao1.NfeRetAutorizacao ret = new br.gov.mt.sefaz.homologacao1.NfeRetAutorizacao();

                        //ret.Url = "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeRetAutorizacao";

                        ret.ClientCertificates.Add(cert);
                        ret.nfeCabecMsgValue = new br.gov.mt.sefaz.homologacao1.nfeCabecMsg();


                        ret.nfeCabecMsgValue.cUF = "51";
                        ret.nfeCabecMsgValue.versaoDados = config.configNFCe.VersaoLayout;


                        n1 = ret.nfeRetAutorizacaoLote(xd);

                    }
                    else
                    {

                        br.gov.mt.sefaz.nfce1.NfeRetAutorizacao ret = new br.gov.mt.sefaz.nfce1.NfeRetAutorizacao();


                        //ret.Url = "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeRetAutorizacao";

                        ret.ClientCertificates.Add(cert);

                        ret.nfeCabecMsgValue = new br.gov.mt.sefaz.nfce1.nfeCabecMsg();

                        ret.nfeCabecMsgValue.cUF = "51";
                        ret.nfeCabecMsgValue.versaoDados = config.configNFCe.VersaoLayout;

                        n1 = ret.nfeRetAutorizacaoLote(xd);
                    }

                    Utils.Logger.getInstance.error("resposta SEFAZ: " + n1.OuterXml);

                    retNode = new XmlDocument();
                    retNode.LoadXml(n1.OuterXml);

                    nr1 = retNode.GetElementsByTagName("infProt")[0];

                    if (nr1 != null)
                    {
                        nProt = getElementXml(nr1, "nProt");
                        retRejeiçao = getElementXml(nr1, "cStat");
                        retRejeiçaoMotivo = getElementXml(nr1, "xMotivo");
                    }
                    else
                    {
                        retRejeiçao = getElementXml(retNode.FirstChild, "cStat");
                        retRejeiçaoMotivo = getElementXml(retNode.FirstChild, "xMotivo");
                        throw new ApplicationException(retRejeiçaoMotivo);
                    }

                    if (retRejeiçao.Equals("105"))
                        System.Threading.Thread.Sleep(100000);

                }

                if ((!retRejeiçao.Equals("100")) && (!retRejeiçao.Equals("150")))
                    throw new ApplicationException(retRejeiçaoMotivo);
                else
                {
                    xmlFinal = new Utils.XmlFinal().DanfeFinal(retNode, xmlNfce);
                    ConsultaSefaz = true;
                }
            }
            catch (Exception ex)
            {
                Utils.Logger.getInstance.error(ex);
                //throw new Exception(ex.ToString());
                throw ex;
            }
        }
        public bool ConsultaContingencia(string _recibo)
        {
            Recibo = _recibo;
            ConsultarNfce();
            return ConsultaSefaz;           
        }
        public bool CancelamentoCupom(XmlDocument _xmlNfce)
        {

            XmlDocument retorno = new XmlDocument();
            XmlNode resp = null;
            try
            {

                if (config.configNFCe.TpAmb.Substring(0, 1).Equals("2"))
                {

                    br.gov.mt.sefaz.homologacao3.RecepcaoEvento cancelamento = new br.gov.mt.sefaz.homologacao3.RecepcaoEvento();
                    cancelamento.ClientCertificates.Add(cert);
                    cancelamento.nfeCabecMsgValue = new br.gov.mt.sefaz.homologacao3.nfeCabecMsg();
                    cancelamento.nfeCabecMsgValue.cUF = config.configEmitente.endereco.Cod_estado;
                    cancelamento.nfeCabecMsgValue.versaoDados = "1.00";

                    resp = cancelamento.nfeRecepcaoEvento(_xmlNfce);
                }
                else
                {
                    br.gov.mt.sefaz.nfce3.RecepcaoEvento cancelamento = new br.gov.mt.sefaz.nfce3.RecepcaoEvento();
                    cancelamento.ClientCertificates.Add(cert);
                    cancelamento.nfeCabecMsgValue = new br.gov.mt.sefaz.nfce3.nfeCabecMsg();
                    cancelamento.nfeCabecMsgValue.cUF = config.configEmitente.endereco.Cod_estado;
                    cancelamento.nfeCabecMsgValue.versaoDados = "1.00";

                    resp = cancelamento.nfeRecepcaoEvento(_xmlNfce);
                }

                Utils.Logger.getInstance.error("resposta SEFAZ: " + resp.OuterXml);
                retorno = new XmlDocument();
                retorno.LoadXml(resp.OuterXml);

                XmlNode nr1 = retorno.GetElementsByTagName("infEvento")[0];

                string stat = getElementXml(nr1, "cStat");
                string motivo = getElementXml(nr1, "xMotivo");

                if (stat != "135")
                    throw new ApplicationException(motivo);
                
            }
            catch (Exception ex)
            {
                Utils.Logger.getInstance.error(ex);
                throw ex;
                
            }
            return true;
        }
      
        public string getElementXml(XmlNode _xmlNode, string _key)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(_xmlNode.OuterXml);
            XmlNodeList retNL = xml.GetElementsByTagName(_key);
            foreach (XmlNode nd in retNL)
            {
                return nd.InnerText;
            }

            return String.Empty;
        }
        private void ProcessNodes(XmlNodeList nodes, XmlNode pai)
        {
            foreach (XmlNode node in nodes)
            {
                XmlNode xn = xmlNfce.CreateNode(XmlNodeType.Element, node.Name, null);


                pai.AppendChild(xn);

                if ((node.HasChildNodes) && (!node.FirstChild.Name.Equals("#text")))
                {
                    ProcessNodes(node.ChildNodes, xn);
                }
                else
                {
                    xn.InnerText = node.InnerText;
                }
            }
        }
        public XmlDocument GetXmlCancelamento()
        {
            return xmlCancelamento;
        }
        public XmlDocument GetXmlOK()
        {
            return xmlNfce;
        }
        public string GetnProt()
        {
            return nProt;
        }
        public bool InutilizacaoCupom(XmlDocument _xmlNfce)
        {
            XmlDocument retorno = new XmlDocument();
            XmlNode resp = null;


            if (config.configNFCe.TpAmb.Substring(0, 1).Equals("2"))
            {

                br.gov.mt.sefaz.homologacao4.NfeInutilizacao2 inutilizacao = new br.gov.mt.sefaz.homologacao4.NfeInutilizacao2();
                inutilizacao.ClientCertificates.Add(cert);
                inutilizacao.nfeCabecMsgValue = new br.gov.mt.sefaz.homologacao4.nfeCabecMsg();
                inutilizacao.nfeCabecMsgValue.cUF = config.configEmitente.endereco.Cod_estado;
                inutilizacao.nfeCabecMsgValue.versaoDados = "3.10";

                resp = inutilizacao.nfeInutilizacaoNF2(_xmlNfce);
            }
            else
            {
                br.gov.mt.sefaz.nfce4.NfeInutilizacao2 inutilizacao = new br.gov.mt.sefaz.nfce4.NfeInutilizacao2();
                inutilizacao.ClientCertificates.Add(cert);
                inutilizacao.nfeCabecMsgValue = new br.gov.mt.sefaz.nfce4.nfeCabecMsg();
                inutilizacao.nfeCabecMsgValue.cUF = config.configEmitente.endereco.Cod_estado;
                inutilizacao.nfeCabecMsgValue.versaoDados = "3.10";

                resp = inutilizacao.nfeInutilizacaoNF2(_xmlNfce);
            }

            Utils.Logger.getInstance.error("resposta SEFAZ: " + resp.OuterXml);
            retorno = new XmlDocument();
            retorno.LoadXml(resp.OuterXml);

            XmlNode nr1 = retorno.GetElementsByTagName("retInutNFe")[0];

            string stat = getElementXml(nr1, "cStat");
            string motivo = getElementXml(nr1, "xMotivo");

            if (stat != "102")
                throw new ApplicationException(motivo);
            else
                return true;
            
        }
    }
}
