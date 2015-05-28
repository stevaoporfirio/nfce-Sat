using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

using System.Reflection;

using System.IO;
using System.Xml;
using System.Messaging;
using invoiceServerApp;

namespace TesterNFCE
{
    public partial class Form1 : Form
    {
        private X509Certificate2 cert = null;
        private Utils.ConfigureXml config;
        private EnviaSeFaz.ManagerSeFaz enviaSeFaz;
        private string Id_db = "";
        public Form1()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                config = new Utils.ReadConfigure().GetConfig();
                lblConfig.Text = lblConfig.Text + " OK";

                GetCertificado();
                lblCertificado.Text = lblCertificado.Text + " OK";


                MessageQueue mq = new MessageQueue(@".\Private$\nfce_contingencia", false);

               


                lblFila.Text += mq.GetAllMessages().Count();
                    
                
                enviaSeFaz = new EnviaSeFaz.ManagerSeFaz(config, cert);
                
                enviaSeFaz.verificaStatusSefaz();
                lblStatusSefaz.Text = lblStatusSefaz.Text + " OK";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                throw new Exception(" Nenhum certificado encontrado");

            if (cert == null)
                throw new Exception("Certificado não encontrado:" + config.configNFCe.NomeCertificadoDigital);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            MessageQueue mq = new MessageQueue(@".\Private$\nfce_contingencia", false);
            MessageQueueTransaction transaction = new MessageQueueTransaction();
            string nomeXml = "";

            try
            {
                bool findFile = true;
                transaction.Begin();

                System.Messaging.Message msg = mq.Receive(transaction);

                msg.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });

                nomeXml = msg.Body.ToString();
                lblArquivo.Text = nomeXml;
                XmlDocument xml = new XmlDocument();
                
                try
                {
                    string dir = String.Format("{0}", config.configMaquina.pathFiles + "\\contingencia\\" + nomeXml + ".xml");
                    xml.Load(dir);
                    findFile = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Arquivo nao encontrado: "+nomeXml+".xml");
                }
                if(findFile)
                {
                    try
                    {
                        openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Filter = "XML Files (*.xml)|*.xml";
                        openFileDialog1.ShowDialog();
                        string fileXml = openFileDialog1.FileName;
                        


                        if (fileXml.Contains(nomeXml))
                        {
                            MessageBox.Show("Arquivo diferente do que deve ser buscado: "+nomeXml);
                            return;
                        }
                        xml.Load(openFileDialog1.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Arquivo nao encontrado: " + nomeXml + ".xml");
                        return;
                    }
                }

                enviaSeFaz = new EnviaSeFaz.ManagerSeFaz(config, cert);
                try
                {
                    enviaSeFaz.verificaStatusSefaz();
                }
                catch (Exception exc)
                {
                    Utils.Logger.getInstance.error(exc.ToString());
                    transaction.Abort();
                    return;
                }

                Id_db = ManagerDB.Instance.SelectMaxNFCEidDB(config.configNFCe.Serie, nomeXml.Substring(3));

                enviaSeFaz.enviaSefaz(xml);

                ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.CupomEnviadoContigencia, "Enviado Sefaz Contingencia", "");
                ManagerDB.Instance.UpdateReciboNFCe(Id_db, enviaSeFaz.GetRecibo());

                if (ConsultaEnviado())
                {

                    ChangedFile(nomeXml, false);
                    ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.AprovadoContingencia, "Aprovado Uso da NFCe (Cont)", "");
                }
                else
                {
                    ChangedFile(nomeXml, true);
                    ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.CupomRejeitadoContigencia, "Rejeitado em contingencia ", enviaSeFaz.GetStatus());
                }
                transaction.Commit();        

            }
            catch (ApplicationException appEx)
            {
                transaction.Commit();

                Utils.Logger.getInstance.error(appEx.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MessageBox.Show("arquivo processado com sucesso: " + nomeXml);
        }
        private bool ConsultaEnviado()
        {
            try
            {
                string recibo = ManagerDB.Instance.SelectNumeroRecibo(Id_db);
                Utils.Logger.getInstance.error("consulta recibo: " + recibo);
                if (recibo == "")
                    return false;

                if (enviaSeFaz.ConsultaContingencia(recibo))
                {
                    ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.CupomConsultadoContigencia, "Consulta Sefaz em contingencia ", "");
                    return true;
                }
                else
                {
                    ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.CupomConsultadoContigencia, "Ainda não enviado SEFAZ ", enviaSeFaz.GetStatus());
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

        private void button2_Click(object sender, EventArgs e)
        {

            config = new Utils.ReadConfigure().GetConfig();
            
            GetCertificado();
            
            enviaSeFaz = new EnviaSeFaz.ManagerSeFaz(config, cert);
            
            enviaSeFaz.verificaStatusSefaz();
            
            
            xmlCancel canc = new xmlCancel(cert, config);

            enviaSeFaz.CancelamentoNfce(canc.xmlDoc);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string fileXml, nomeXml;
            XmlDocument xml = new XmlDocument();
            config = new Utils.ReadConfigure().GetConfig();

            GetCertificado();
            enviaSeFaz = new EnviaSeFaz.ManagerSeFaz(config, cert);

            try
            {
                openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
                openFileDialog1.InitialDirectory = @"C:\";
                openFileDialog1.Filter = "XML Files (*.xml)|*.xml";
                openFileDialog1.ShowDialog();
                fileXml = openFileDialog1.FileName;
                xml.Load(openFileDialog1.FileName);
                nomeXml = Path.GetFileName(fileXml);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            try
            {
                enviaSeFaz.verificaStatusSefaz();
            }
            catch (Exception exc)
            {
                Utils.Logger.getInstance.error(exc.ToString());
                return;
            }
            try
            {

                Id_db = ManagerDB.Instance.SelectMaxNFCEidDB(config.configNFCe.Serie, nomeXml.Substring(3));

                enviaSeFaz.enviaSefaz(xml);

                ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.CupomEnviadoContigencia, "Enviado Sefaz Contingencia", "");
                ManagerDB.Instance.UpdateReciboNFCe(Id_db, enviaSeFaz.GetRecibo());

                if (ConsultaEnviado())
                {

                    ChangedFile(nomeXml, false);
                    ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.AprovadoContingencia, "Aprovado Uso da NFCe (Cont)", "");
                }
                else
                {
                    ChangedFile(nomeXml, true);
                    ManagerDB.Instance.InsertNfceStatus(Id_db, (int)StatusCupom.CupomRejeitadoContigencia, "Rejeitado em contingencia ", enviaSeFaz.GetStatus());
                }
               

            }
            catch (ApplicationException appEx)
            {
                Utils.Logger.getInstance.error(appEx.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MessageBox.Show("arquivo processado com sucesso: " + nomeXml);
        }

        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        config = new Utils.ReadConfigure().GetConfig();
        //        lblConfig.Text = lblConfig.Text + " OK";

        //        GetCertificado();
        //        lblCertificado.Text = lblCertificado.Text + " OK";


        //        MessageQueue mq = new MessageQueue(@".\Private$\nfce_contingencia", false);




        //        lblFila.Text += mq.GetAllMessages().Count();


        //        enviaSeFaz = new EnviaSeFaz.ManagerSeFaz(config, cert);

        //        enviaSeFaz.verificaStatusSefaz();
        //        lblStatusSefaz.Text = lblStatusSefaz.Text + " OK";

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //        //Application.Exit();
        //    }

        //}

        private void button3_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Deseja excluir mensagem da fila", "Exclusao", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
             
            }
        }
    }
}
