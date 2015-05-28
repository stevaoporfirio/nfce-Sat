using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invoiceServerApp
{
    class validador
    {
        private DadosNfce dados = new DadosNfce();
        public validador(DadosNfce _dados)
        {
            dados = _dados;
        }
        public bool validacao()
        {
            foreach (var item in dados.itensList)
            {
                if (String.IsNullOrEmpty(item.NCM))
                    return false;
            }
            return true;
        
        }
    }
}
