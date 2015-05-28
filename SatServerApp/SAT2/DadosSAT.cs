using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invoiceServerApp
{
    class DadosSAT
    {
        //DadosEmitente        
        public string CNPJ { get; set; }        
        public string IE { get; set; }
        public string IM { get; set; }
        
        
        //DadosDestinatario        
        public string JuridicaFisica_dest { get; set; }
        public string CPF_CNPJ_dest { get; set; }
        public string Nome_dest { get; set; }        
        

        public List<String> InfoAdic { get; set; }        

        public string Acresc { get; set; }

        //private string statusDadosEmitente = "";
        
        public List<ItensSAT> itensList = new List<ItensSAT>();
        public List<PgtSAT> pgtsList = new List<PgtSAT>();
        public TefSAT TefNfce = new TefSAT();

        public DadosMSG DadosMSG { get; set; }

        public string PortaImpressora { get; set; }
        public string IdAccount { get; set; }

        public string Chk_Num { get; set; }
        public string WS_ID { get; set; }

    }

    class DadosMSGSAT
    {
        public List<string> Msg = new List<string>();
    }

    class TefSAT
    {
        public string StringTEF { get; set; }
    }
        
    class PgtSAT
    {
        public string Cod { get; set; }
        public string Desc { get; set; }
        public string Val { get; set; }
    }

    class ItensSAT
    {
        //Itens                
        public string C_Prod { get; set; }
        public string C_EAN { get; set; }
        public string X_Prod { get; set; }
        public string NCM { get; set; }
        public int CFOP { get; set; }
        public string U_Com { get; set; }
        public decimal Q_Com { get; set; }
        public decimal V_UnCom { get; set; }
        //public decimal V_Prod { get; set; }
        public decimal V_Desc { get; set; }
                
        public string Tipo_Aliquota { get; set; }
        public string Valor_Aliquota { get; set; }
        public string Posicao { get; set; }
        public string CST { get; set; }           
    }
    
}
