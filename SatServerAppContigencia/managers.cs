using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Messaging;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.IO;
using System.Web.Services.Protocols;
using System.Net;
using System.Data;
using System.Xml;

namespace invoiceServerApp
{
    class managers
    {
        private BackgroundWorker worker = new BackgroundWorker();
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private MessageQueue mq = new MessageQueue(@".\Private$\nfce_contingencia", false);
        private EnviaSeFaz.ManagerSeFaz enviar;
        private X509Certificate2 cert;
        private Utils.ConfigureXml config;
        private string Id_db;

        
        public managers()
        {
            try
            {
                config = new Utils.ReadConfigure().GetConfig();

                GetCertificado();
                
                worker.DoWork += worker_DoWork;
                worker.RunWorkerAsync();
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
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
            {
                Utils.Logger.getInstance.error("Nenhum certificado encontrado");
                throw new Exception("Nenhum certificado encontrado");
            }

            if (cert == null)
            {
                Utils.Logger.getInstance.error("Certificado não encontrado:" + config.configNFCe.NomeCertificadoDigital);
                throw new Exception("Certificado não encontrado:" + config.configNFCe.NomeCertificadoDigital);

            }
        }
        private void ChangedFile(string name, bool isReject)
        {
            try
            {
                if (isReject)
                {
                    File.Move(String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles + "\\contingencia\\", name),
                                 String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles + "\\contingencia\\rejeitados", name));
                }
                else
                {
                    File.Move(String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles + "\\contingencia\\", name),
                              String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles + "\\contingencia\\enviados", name));
                }
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error("ChangedFile: " + e.ToString());
            
            }
        }
        private bool ConsultaEnviado()
        {
            try
            {                                
                string recibo = ManagerDB.Instance.SelectNumeroRecibo(Id_db);
                Utils.Logger.getInstance.error("consulta recibo: " + recibo);
                if (recibo == "")
                    return false;

                if (enviar.ConsultaContingencia(recibo))
                {
                    ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.CupomConsultadoContigencia, "Consulta Sefaz em contingencia ", "");
                    
                    return true;
                }
                else
                {
                    ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.CupomConsultadoContigencia, "Ainda não enviado SEFAZ ", enviar.GetStatus());
                    return false;
                }
                
            }
            catch (Exception e)
            {
                ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.CupomConsultadoContigencia, "Rejeitado Sefaz em contingencia ", e.Message);
                Utils.Logger.getInstance.error("Consulta Enviado: " + e.ToString());
                return false;
            }
        }

        
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!e.Cancel)
            {
                
                try
                {                    
                    if (mq.GetAllMessages().Count() > 0)
                    {
                        
                        MessageQueueTransaction transaction = new MessageQueueTransaction();
                        string nomeXml = "";

                        try
                        {
                            enviar = new EnviaSeFaz.ManagerSeFaz(config, cert);

                            transaction.Begin();                            

                            System.Messaging.Message msg = mq.Receive(transaction);

                            msg.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });

                            nomeXml = msg.Body.ToString();

                            Utils.Logger.getInstance.error("nota: " + nomeXml);

                            string dir = String.Format("{0}", config.configMaquina.pathFiles + "\\contingencia\\" + nomeXml + ".xml");

                            XmlDocument xml = new XmlDocument();

                            xml.Load(dir);

                            try
                            {
                                enviar.verificaStatusSefaz();                                
                            }
                            catch (Exception exc)
                            {
                                Utils.Logger.getInstance.error(exc.ToString());
                                transaction.Abort();
                                Thread.Sleep(10000);
                                continue;
                            }
                            
                            Id_db = ManagerDB.Instance.SelectMaxNFCEidDB(config.configNFCe.Serie, nomeXml.Substring(3));

                            enviar.enviaSefaz(xml);

                            ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.CupomEnviadoContigencia, "Enviado Sefaz Contingencia", "");
                            ManagerDB.Instance.UpdateReciboNFCe(Id_db, enviar.GetRecibo());

                            if (ConsultaEnviado())
                            { 
                                XmlDocument xmlfinal = enviar.GetXmlOK();
                                xmlfinal.Save(String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles + "\\contingencia\\", nomeXml));
                                ChangedFile(nomeXml, false);
                                ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.AprovadoContingencia, "Aprovado Uso da NFCe (Cont)", "");
                            }
                            else
                            {
                                ChangedFile(nomeXml, true);
                                ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.CupomRejeitadoContigencia, "Rejeitado em contingencia ", enviar.GetStatus());
                            }
                            transaction.Commit();                            
                        }
                        catch (WebException sEX)
                        {
                            transaction.Abort();
                            Utils.Logger.getInstance.error("Problema na Sefaz: "+sEX.ToString());
                        }
                        catch (ApplicationException appEx)
                        {
                            ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.CupomRejeitadoContigencia, "Rejeitado em contingencia ", enviar.GetStatus());

                            ChangedFile(nomeXml, true);

                            transaction.Commit();

                            Utils.Logger.getInstance.error(appEx.ToString());
                        }
                        catch (Exception ex)
                        {
                            transaction.Abort();

                            Utils.Logger.getInstance.error(ex.ToString());
                        }
                    }
                    else 
                    {
                        Thread.Sleep(60000);
                    }

                }
                catch (Exception ex)
                {
                    Utils.Logger.getInstance.error(ex.ToString());
                }
            }

            _resetEvent.Set(); // signal that worker is done
        }
        
    }

}
