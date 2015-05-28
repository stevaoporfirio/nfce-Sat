using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace invoiceServerApp
{
    static class Program
    {

        public static Utils.Cep2IBGE cep2IBGE;
       // public static MicrosDB MicrosDB = new MicrosDB("micros", "custom", "custom");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Utils.Logger.getInstance.error("iniciando");
                cep2IBGE = new Utils.Cep2IBGE();

                String nomeMutex = "SatServerApp";
                System.Threading.Mutex mutex = null;
                try
                {
                    mutex = System.Threading.Mutex.OpenExisting(nomeMutex);
                }
                catch (System.Threading.WaitHandleCannotBeOpenedException e)
                {
                    //Utils.Logger.getInstance.error(e);    
                }

                if (mutex == null)
                {
                    mutex = new System.Threading.Mutex(true, nomeMutex);
                }
                else
                {
                    Utils.Logger.getInstance.error("Tentativa de abertura, com aplicaçao aberta...");    
                    return;
                }

            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
