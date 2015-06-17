using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace invoiceServerApp
{
    class Datamanager
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
                
                config = new Utils.ReadConfigure().GetConfig();
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
    }
}


