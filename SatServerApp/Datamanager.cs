using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace invoiceServerApp
{
    public class Datamanager
    {        
        private ServerSocket serverComunication;
        private Utils.ConfigureXml config;
        private invoiceImplemention invoice;
        private Thread workerThread;
        ~Datamanager()
        {
            //workerObject.RequestStop();
            if (workerThread != null)
            {
                workerThread.Abort();
                workerThread.Join();
            }
        }
        public Datamanager()
        {
            try
            {

                //Utils.Logger.getInstance.error("Arquivo Conf.: " + AppDomain.CurrentDomain.BaseDirectory + "\\AppConfig.xml");
                                
                Utils.Logger.getInstance.SetFileName("SatServerApp");
                config = new Utils.ReadConfigure().GetConfig();

                CreateDir();

                if (config.configMaquina.tipoIntegracao.Equals("NFCE"))
                {
                    Utils.GerenciaCertificado.Instance().SetCertificado(config);
                }
                else
                {
                    gerenciadoSAT.Instance.SetConfSAT(config);                    
                }

                //ManagerDB.Instance().SetDBMicros(config.configMaquina.TipoDB);

                bool dbOK = false;

                while (!dbOK)
                {
                    try
                    {
                        ManagerDB.Instance().SetDBMicros(config.configMaquina.TipoDB);
                        dbOK = true;
                    }
                    catch (Exception ex)
                    {
                        Utils.Logger.getInstance.error("Erro iniciando DB\n" + ex.Message + "\nNova tentativa em 30 segundos");
                        Thread.Sleep(30000);
                    }
                }

                invoice = new invoiceImplemention(config);
                serverComunication = new ServerSocket(config, invoice);
                serverComunication.setListenerComunication(invoice);


                workerThread = new Thread(serverComunication.StartListening);
                workerThread.Start();
                
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
                MessageBox.Show(e.ToString());
                Application.Exit();
            }

        }

        public void Run(object a)
        {
            while (true)
            {                
                Thread.Sleep(10000);
            }

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
    }
}


