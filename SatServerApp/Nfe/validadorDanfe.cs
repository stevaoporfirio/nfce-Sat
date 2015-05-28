using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invoiceServerApp
{
    class validadorDanfe
    {
        public validadorDanfe(DadosNfce _dtNfce)
        {
            // validar se tem q haver 1 item e 1 pagamento 
            foreach (var item in _dtNfce.itensList)
            {
                if (item.NCM == String.Empty)
                {
                    throw new ApplicationException("Nota sem NCM."); 
                }
            }
            if (_dtNfce.pgtsList.Count < 1)
            {
                throw new ApplicationException("Nota Pagamento."); 
            }
            if (_dtNfce.itensList.Count < 1)
            {
                throw new ApplicationException("Nota Sem itens."); 
            }
            if(!String.IsNullOrEmpty(_dtNfce.CPF_CNPJ_dest))
                if(String.IsNullOrEmpty(_dtNfce.RazaoSocial_dest))
                    throw new ApplicationException("Nota sem nome.");
        
        }

    }
}
