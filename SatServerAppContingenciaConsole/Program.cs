using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SatServerAppContingenciaConsole
{
    class Program
    {
        public static invoiceServerApp.managers dtm;

        static void Main(string[] args)
        {
            System.Console.WriteLine("Iniciando Motor SAT/NFCe Contingencia...");

            dtm = new invoiceServerApp.managers();
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(dtm.Run));
        }
    }
}
