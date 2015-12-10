using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class FilaImpressao
    {
        private static readonly FilaImpressao instance = new FilaImpressao();
        
        public static FilaImpressao Instance()
        {
            return instance;
        }

        //public void Enfileira(ImprimirEpsonNFTDados _dados)
        //{
        //    fila.Enqueue(_dados);

        //    System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(Processa));                        
        //}

        //private void Processa(object s)
        //{
        //    while (fila.Count > 0)
        //    {
        //        ImprimirEpsonNFTDados imp = fila.Dequeue();

        //        ImprimirEpsonNF.Instance(imp.portaImpressora).ImprimirNF(imp);
        //    }
        //}
    }
}
