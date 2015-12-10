using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Messaging;

using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.IO;
using System.Web.Services.Protocols;
using System.Net;
using System.Data;
using System.Xml;

namespace invoiceServerApp
{
    public class managers
    {
        //private BackgroundWorker worker = new BackgroundWorker();
        private Thread workerThread;
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
                Utils.Logger.getInstance.SetFileName("SatServerAppContingencia");
                
                config = new Utils.ReadConfigure().GetConfig();

                //Verificando SAT
                gerenciadoSAT.Instance.SetConfSAT(config);
                ManagerDB.Instance().SetDBMicros(config.configMaquina.TipoDB);
                GetCertificado();                

                Utils.Logger.getInstance.error("Inicializando Motor de Contingencia");

                workerThread = new Thread(DoWork);
                workerThread.Start();
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
            }

        }

        public void Run(object a)
        {
            while (true)
            {
                Thread.Sleep(10000);
            }

        }

        private void GetCertificado()
        {
            try
            {

                string certPath = config.configNFCe.CaminhoCertificadoDigital;

                string certPass = "";

                if (config.configNFCe.SenhaCertificadoDigital.Substring(0, 1).Equals("!"))
                    certPass = config.configNFCe.SenhaCertificadoDigital.Substring(1);
                else
                {
                    StringBuilder sBuilder = new StringBuilder();

                    string crip = config.configNFCe.SenhaCertificadoDigital;

                    Utils.Logger.getInstance.error(crip);

                    for (int i = 0; i < crip.Length; i++)
                    {
                        string s = String.Format("{0}{1}", crip[i], crip[i + 1]);
                        Utils.Logger.getInstance.error(s);
                        sBuilder.Append(Convert.ToChar(Convert.ToInt32(s)));
                        i++;
                    }
                    certPass = sBuilder.ToString();
                }

                X509Certificate2Collection collection = new X509Certificate2Collection();
                collection.Import(certPath, certPass, X509KeyStorageFlags.PersistKeySet);


                //X509Store store = new X509Store("My", StoreLocation.CurrentUser);
                //store.Open(OpenFlags.ReadOnly);
                //X509Certificate2Collection collection2 = (X509Certificate2Collection)store.Certificates;
                X509Certificate2Collection listaCertificados = collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, true);

                //store.Close();
                if (listaCertificados.Count > 0)
                {
                    foreach (var c in listaCertificados)
                    {
                        //string name = c.Subject;

                        //string[] names = name.Split(',');
                        //if (names[0].Substring(3).Equals(config.configNFCe.NomeCertificadoDigital))
                        {
                            cert = c;
                            break;
                        }
                    }
                }
                else
                    throw new Exception("Nenhum certificado encontrado");

                if (cert == null)
                    throw new Exception("Certificado não encontrado:" + config.configNFCe.NomeCertificadoDigital);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro em GetCertificados\n" + ex.Message);
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
                string recibo = ManagerDB.Instance().SelectNumeroRecibo(Id_db);
                Utils.Logger.getInstance.error("consulta recibo: " + recibo);
                if (recibo == "")
                    return false;

                if (enviar.ConsultaContingencia(recibo))
                {
                    ManagerDB.Instance().InsertNfceStatus(Id_db, (int)StatusCupom.CupomConsultadoContigencia, "Consulta Sefaz em contingencia ", "");
                    
                    return true;
                }
                else
                {
                    ManagerDB.Instance().InsertNfceStatus(Id_db, (int)StatusCupom.CupomConsultadoContigencia, "Ainda não enviado SEFAZ ", enviar.GetStatus());
                    return false;
                }
                
            }
            catch (Exception e)
            {
                ManagerDB.Instance().InsertNfceStatus(Id_db, (int)StatusCupom.CupomConsultadoContigencia, "Rejeitado Sefaz em contingencia ", e.Message);
                Utils.Logger.getInstance.error("Consulta Enviado: " + e.ToString());
                return false;
            }
        }

        
        private void DoWork(object o)
        {
            while (true)
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

                            Utils.Logger.getInstance.error("Enviando nota: " + nomeXml);

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
                            
                            Id_db = ManagerDB.Instance().SelectMaxNFCEidDB(config.configNFCe.Serie, nomeXml.Substring(3));

                            enviar.enviaSefaz(xml);

                            ManagerDB.Instance().InsertNfceStatus(Id_db, (int)StatusCupom.CupomEnviadoContigencia, "Enviado Sefaz Contingencia", "");
                            ManagerDB.Instance().UpdateReciboNFCe(Id_db, enviar.GetRecibo());

                            if (ConsultaEnviado())
                            { 
                                XmlDocument xmlfinal = enviar.GetXmlOK();
                                xmlfinal.Save(String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles + "\\contingencia\\", nomeXml));
                                ChangedFile(nomeXml, false);
                                ManagerDB.Instance().InsertNfceStatus(Id_db, (int)StatusCupom.AprovadoContingencia, "Aprovado Uso da NFCe (Cont)", "");
                            }
                            else
                            {
                                ChangedFile(nomeXml, true);
                                ManagerDB.Instance().InsertNfceStatus(Id_db, (int)StatusCupom.CupomRejeitadoContigencia, "Rejeitado em contingencia ", enviar.GetStatus());
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
                            ManagerDB.Instance().InsertNfceStatus(Id_db, (int)StatusCupom.CupomRejeitadoContigencia, "Rejeitado em contingencia ", enviar.GetStatus());

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
                        Thread.Sleep(9000);
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
