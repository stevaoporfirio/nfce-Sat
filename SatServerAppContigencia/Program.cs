using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace invoiceServerApp
{
    static class Program
    {
        public static MicrosDB MicrosDB = new MicrosDB("micros", "custom", "custom");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            String nomeMutex = "SatServerAppContingencia";
            System.Threading.Mutex mutex = null;
            try
            {
                mutex = System.Threading.Mutex.OpenExisting(nomeMutex);
            }
            catch (System.Threading.WaitHandleCannotBeOpenedException)
            {
            }

            if (mutex == null)
            {
                mutex = new System.Threading.Mutex(true, nomeMutex);
            }
            else
            {
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
