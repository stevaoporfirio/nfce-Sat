using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Messaging;

namespace invoiceServerApp
{

    public enum StatusDiretorio {Enviada, Rejeitada, Contingencia }    

    class ProcessaNfce
    {
        private DadosNfce dtNFCE;
        private Utils.ConfigureXml config;
        private makeXml xmlData;
        private DadosNota cupom;
        private CallbackStatusInterface InterfaceStatus;
        private EnviaSeFaz.ManagerSeFaz enviaSeFaz;
        public X509Certificate2 cert = null;

        private StatusDiretorio sd;
        private MessageQueue mq; //FILA

        private string id_db;


        public ProcessaNfce(CallbackStatusInterface callStatus, Utils.ConfigureXml _config)
        {

            InterfaceStatus = callStatus;
            config = _config;
            CreateDir();
            GetCertificado();

            enviaSeFaz = new EnviaSeFaz.ManagerSeFaz(config, cert);
        }

        public string ProcessaCancel(CancelNFCE _cncNfce)
        {
            makeXmlCancel xmlData = null;
            try
            {
                xmlData = new makeXmlCancel(_cncNfce, config, cert);

                id_db = xmlData.id_banco;

                ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.xmlCancelamentoGerado, "XML de cancelamento gerado", "");

                XmlDocument arqNfce = xmlData.xmlDoc;

                enviaSeFaz.verificaStatusSefaz();

                if (enviaSeFaz.CancelamentoNfce(arqNfce))
                    ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.xmlCancelamentoEnviado, "NFCE Cancelamento", "");



            }
            catch (ApplicationException ex)
            {
                ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.xmlCancelamentoRejeicao, "NFCE Cancelamento rejeitado", "");
                Utils.Logger.getInstance.error(ex);
                return "NFCE Cancelamento rejeitado";
            }
            catch (Exception e)
            {
                ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.xmlCancelamentoRejeicao, "NFCE Cancelamento rejeitado", "");
                Utils.Logger.getInstance.error(e);
                return "NFCE Cancelamento rejeitado";
            }
            finally
            {
                XmlDocument xmlCancel = enviaSeFaz.GetXmlCancelamento();
                xmlCancel.Save(String.Format("{0}\\canceladas\\{1}.xml", config.configMaquina.pathFiles, xmlData.nomeXml));
            }

            return "NFCE Cancelamento Com sucesso";

        }
        public string ProcessaInutilizacao(InutilizacaoNFCE _cncNfce)
        {
            makeXmlInutilizacao xmlData = null;
            try
            {
                xmlData = new makeXmlInutilizacao(_cncNfce, config, cert);

                id_db = xmlData.id_banco;

                ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.xmlInutilizacaoGerado, "XML de Inutilizacao gerado", "");

                XmlDocument arqNfce = xmlData.xmlDoc;

                enviaSeFaz.verificaStatusSefaz();

                if (enviaSeFaz.InutilizacaoNfce(arqNfce))
                    ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.xmlInutilizacaoEnviado, "NFCE Inutilizado gerado", "");


            }
            catch (ApplicationException ex)
            {
                ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.xmlCancelamentoRejeicao, "NFCE Inutilizacao rejeitado", "");
                Utils.Logger.getInstance.error(ex);
                return "NFCE Inutilizado rejeitado";
            }
            catch (Exception e)
            {
                ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.xmlCancelamentoRejeicao, "NFCE Inutilizacao rejeitado", "");
                Utils.Logger.getInstance.error(e);
                return "NFCE Inutilizacao rejeitado";
            }
            finally
            {
                XmlDocument xmlCancel = enviaSeFaz.GetXmlCancelamento();
                xmlCancel.Save(String.Format("{0}\\inutilizadas\\{1}.xml", config.configMaquina.pathFiles, xmlData.nomeXml));
            }
            return "NFCE Inutilizacao Com sucesso";
        }
        
        public void ProcessaCupom(DadosNfce _dtNFCE)
        {
            dtNFCE = _dtNFCE;
            mq = new MessageQueue(@".\Private$\NFCe_Contingencia", false);
            try
            {
                if (mq.GetAllMessages().Count() > 0)
                {
                    Utils.Logger.getInstance.error(dtNFCE.IdAccount + ": Contingencia: fila count(" + mq.GetAllMessages().Count() + ")" + dtNFCE.IdAccount);
                    throw new Exception("Contingencia");
                }

                enviaSeFaz.verificaStatusSefaz();

                config.configNFCe.Contingencia = false;

            }
            catch (Exception ex)
            {
                Utils.Logger.getInstance.error("Contingencia: " + dtNFCE.IdAccount);
                Utils.Logger.getInstance.error(ex.ToString());
                config.configNFCe.Contingencia = true;
            }
            
            xmlData = new makeXml(dtNFCE, config, cert);
            id_db = ManagerDB.Instance.SelectMaxNFCEidDB(config.configNFCe.Serie, xmlData.chaveAcesso);
            
            string resposta = config.configNFCe.Contingencia ? "XML gerado em Contingencia" : "XML gerado";

            ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.CupomGeradoXml, resposta, "");
            
            Processa();
        
        }

        public void Processa()
        {
            
            XmlDocument arqNfce = xmlData.xmlDocAss;
            bool consultaSefaz = false;
            
            try
            {
                if ((InterfaceStatus.GetStatusCupom() < (int)StatusCupom.CupomSeFazRetornoOk) && (!config.configNFCe.Contingencia))
                {
                    enviaSeFaz.enviaSefaz(arqNfce);
                    ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.CupomEnviadoSeFaz, "Enviado Sefaz", "");
                    ManagerDB.Instance.UpdateReciboNFCe(id_db, enviaSeFaz.GetRecibo());
                }

                if (!config.configNFCe.Contingencia)
                {
                    consultaSefaz = enviaSeFaz.ConsultaOK();
                    ManagerDB.Instance.UpdatenProtNFCe(id_db, enviaSeFaz.GetnProt());
                    ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.CupomSeFazRetornoOk, "Consulta Sefaz Sucesso", "");
                    sd = StatusDiretorio.Enviada;
                    arqNfce = enviaSeFaz.GetXmlOK();
                    arqNfce.Save(String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles, xmlData.nomeXml));
                }
                
                Impressao();

                if (!config.configNFCe.Contingencia) //Gravar depois da Impressao!
                    ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.Aprovado, "Aprovado Uso da NFCe", "");
                
            }
            catch (Exception e)
            {
                sd = StatusDiretorio.Rejeitada;
                Utils.Logger.getInstance.error(e);

                string rejeicao = (e.Message.Length > 500) ? e.Message.Substring(500) : e.Message;
                ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.CupomRejeitado, "NFCE Rejeitado", rejeicao);

                throw new Exception(dtNFCE.IdAccount + ": Rejeiçao de xml:" + e.ToString());
            }
            finally
            {
                if (config.configNFCe.Contingencia)
                    sd = StatusDiretorio.Contingencia;
                
                
                 ChangedFile(xmlData.nomeXml, sd);                
            }
        }
        private void GetCertificado()
        {
            X509Store store = new X509Store("My", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2Collection listaCertificados = collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, true);

            store.Close();
            if (listaCertificados.Count > 0)
            {
                foreach (var c in listaCertificados)
                {
                    string name = c.Subject;

                    string[] names = name.Split(',');
                    if (names[0].Substring(3).Equals(config.configNFCe.NomeCertificadoDigital))
                    {
                        cert = c;
                        break;
                    }
                }
            }
            else
                throw new Exception(dtNFCE.IdAccount + ": Nenhum certificado encontrado");

            if(cert == null)
                throw new Exception(dtNFCE.IdAccount + ": Certificado não encontrado:" + config.configNFCe.NomeCertificadoDigital);
        }

        private void CreateDir()
        {
            try
            {
                string dir = String.Format("{0}", config.configMaquina.pathFiles + "\\enviados");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string dir2 = String.Format("{0}", config.configMaquina.pathFiles + "\\rejeitados");
                if (!Directory.Exists(dir2))
                    Directory.CreateDirectory(dir2);

                string dir3 = String.Format("{0}", config.configMaquina.pathFiles + "\\contingencia");
                if (!Directory.Exists(dir3))
                    Directory.CreateDirectory(dir3);

                string dirCancel = String.Format("{0}", config.configMaquina.pathFiles + "\\canceladas");
                if (!Directory.Exists(dirCancel))
                    Directory.CreateDirectory(dirCancel);

                string dir4 = String.Format("{0}", config.configMaquina.pathFiles + "\\contingencia\\enviados");
                if (!Directory.Exists(dir4))
                    Directory.CreateDirectory(dir4);

                string dir5 = String.Format("{0}", config.configMaquina.pathFiles + "\\contingencia\\rejeitados");
                if (!Directory.Exists(dir5))
                    Directory.CreateDirectory(dir5);

                string dir6 = String.Format("{0}", config.configMaquina.pathFiles + "\\inutilizadas");
                if (!Directory.Exists(dir6))
                    Directory.CreateDirectory(dir6);

                
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
            }

        }

        private void ChangedFile(string name, StatusDiretorio sd)
        {
            try
            {
                switch ((int)sd)
                {
                    case 0:
                        File.Move(String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles, name),
                                 String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles + "\\enviados", name));
                        break;
                    case 1:
                        File.Move(String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles, xmlData.nomeXml),
                                  String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles + "\\rejeitados", xmlData.nomeXml));
                        break;
                    case 2:
                        File.Move(String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles, xmlData.nomeXml),
                              String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles + "\\contingencia", xmlData.nomeXml));

                        sendMSMQ(xmlData.nomeXml);

                        break;
                }
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
            }
        
        }
        
        private void Impressao()
        {
            try
            {
                Utils.Logger.getInstance.error(dtNFCE.IdAccount + ": impressora: " + dtNFCE.PortaImpressora);

                cupom = new DadosNota(xmlData.xmlDocAss, config, enviaSeFaz.GetRecibo(), "Via Consumidor");

                ImprimirEpsonNF.ImprimirNF(dtNFCE.PortaImpressora, cupom.DadosImpressao,null, cupom.QRCode,"", dtNFCE.TefNfce.StringTEF, true, true);

                InterfaceStatus.NotificationChanged((int)StatusCupom.CupomImpresso);

                ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.CupomImpresso, "Cupom impresso", "");

                if (config.configNFCe.Contingencia)
                {
                    ImprimirEpsonNF.ImprimirNF(dtNFCE.PortaImpressora, cupom.DadosImpressao,null, cupom.QRCode,"", "", true, true);
                    ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.CupomImpressoContingencia, "Cupom impresso em contingencia", "");
                }

            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
            }
        
        }


        private void sendMSMQ(string msg)
        {
            try
            {
                ManagerDB.Instance.InsertNfceStatus(id_db, (int)StatusCupom.CupomEnviadoFila, "Cupom Enviado para fila de contingencia", "");
                //gravando na fila
                MessageQueueTransaction transaction = new MessageQueueTransaction();
                transaction.Begin();
                System.Messaging.Message a = new System.Messaging.Message();
                a.Body = msg;
                mq.Send(a, transaction);
                transaction.Commit();
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
            }
            finally 
            {
                mq.Close();
            }
        
        }

        public string GetQrcode()
        {
            //return cupom.QRCode;
            return "";
        }
        public string GetNota()
        {
            //return cupom.DadosImpressao;
            return "";
        }
        public string ReImpressaoDanfe(Utils.ConfigureXml _config, string _id, string _Ip)
        {
            string arquivo = String.Format("{0}", config.configMaquina.pathFiles + "\\enviados\\NFe" + _id + ".xml");
            
            
            if (!File.Exists(arquivo))
            {
                arquivo = String.Format("{0}", config.configMaquina.pathFiles + "\\contingencia\\enviados\\NFe" + _id + ".xml");
                if (!File.Exists(arquivo))
                    return "Documento nao encontrado.";
            }

                
            XmlDocument xml = new XmlDocument();
            
            xml.Load(arquivo);

            cupom = new DadosNota(xml, config, "", "Via Consumidor");

            ImprimirEpsonNF.ImprimirNF(_Ip, cupom.DadosImpressao,null, cupom.QRCode,"", "", true, true);

            
            return "Impresso com sucesso";
        }

    }
}
