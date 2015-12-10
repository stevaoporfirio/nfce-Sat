using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using invoiceServerApp;

namespace SatServerAppConsole
{
    class Program
    {
        public static Datamanager dtm;

        static void Main(string[] args)
        {
            System.Console.WriteLine("Iniciando Motor SAT/NFCe...");

            dtm = new Datamanager();
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(dtm.Run));
        }
    }
}
