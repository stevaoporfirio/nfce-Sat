using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invoiceServerApp
{
    class utils
    {
        public static int getRandom(int tamanho)
        {
            int max = 1;
            for (int i = 0; i < tamanho; i++)
            {
                max *= 10;
            }
            int resultado = new Random().Next(max);
            if (resultado >= max / 10)
            {
                return resultado;
            }
            return getRandom(tamanho);
        }
    }
}
