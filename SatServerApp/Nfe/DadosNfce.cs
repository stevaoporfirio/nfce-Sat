using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invoiceServerApp
{
    class DadosNfce
    {
        //DadosDestinatario
        public string RazaoSocial_dest { get; set; }
        public string JuridicaFisica_dest { get; set; }
        public string CPF_CNPJ_dest { get; set; }
        public string Ie_dest { get; set; }
        public string Email_dest { get; set; }

        public string Logradouro_dest { get; set; }
        public string Numero_dest { get; set; }
        public string Complemento_dest { get; set; }
        public string Bairro_dest { get; set; }
        public string Cep_dest { get; set; }
        public string Municipio_dest { get; set; }
        public string CodMunicipio_dest { get; set; }
        public string Uf_dest { get; set; }
        public string Pais_dest { get; set; }
        public string CodPais_dest { get; set; }


        //DadosEmitente
        public string RazaoSocial { get; set; }
        public string Fantasia { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public string Ie { get; set; }
        public string Email { get; set; }
        
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Uf { get; set; }
        public string Cidade { get; set; }
        public string Cod_cidade { get; set; }
        public string Cod_estado { get; set; }
        public string Cod_pais { get; set; }
        public string Crt { get; set; }

        public List<String> InfoAdic { get; set; }

        public string isPrint { get; set; }

        public string Acresc { get; set; }

        private string statusDadosEmitente = "";
        public List<ItensNfce> itensList = new List<ItensNfce>();
        public List<PgtNfce> pgtsList = new List<PgtNfce>();
        public TefNFCE TefNfce = new TefNFCE();

        public DadosMSG DadosMSG { get; set; }

        public string PortaImpressora { get; set; }
        public string IdAccount { get; set; }

        public string Chk_Num { get; set; }
        public string WS_ID { get; set; }

    }

    class DadosMSG
    {
        public List<string> Msg = new List<string>();
    }

    class TefNFCE
    {
        public string StringTEF { get; set; }
    }
    class dtImprensao
    {
        public string printer { get; set; }
        public string chaveImpressao { get; set; }
        public string tipoCli { get; set; }
        public string StringTEF { get; set; }
        public string portaImpressora { get; set; }
    }
    class CancelNFCE
    {
        public string WsID { get; set; }
        public string NumConta { get; set; }
        public string ID { get; set; }
        public string printer { get; set; }
        public string chaveCancelamento { get; set; }
        public string tipoCli { get; set; }
        public string cpf_cnpj { get; set; }
    }
    class InutilizacaoNFCE
    {
        public string WsID { get; set; }
        public string NumConta { get; set; }
        public string ID { get; set; }
        public string numeroInicial { get; set; }
        public string numeroFinal { get; set; }
    }
    class PgtNfce
    {
        public string Cod { get; set; }
        public string Desc { get; set; }
        public string val { get; set; }
        public string cAut { get; set; }
        public string tBand { get; set; }
    }

    class ItensNfce
    {
        //Itens
        public string tipoPag { get; set; }
        public int Num_ECF { get; set; }
        public string Serial_ECF { get; set; }
        public int COO { get; set; }    
        public string C_Prod { get; set; }
        public string C_EAN { get; set; }
        public string X_Prod { get; set; }
        public string NCM { get; set; }
        public int CFOP { get; set; }
        public string U_Com { get; set; }
        public decimal Q_Com { get; set; }
        public decimal V_UnCom { get; set; }
        public decimal V_Prod { get; set; }
        public string C_EANTrib { get; set; }
        public string U_Trib { get; set; }
        public decimal Q_Trib { get; set; }
        public decimal V_UnTrib { get; set; }
        public decimal V_Desc { get; set; }
        public int IndTot { get; set; }
        public string Tipo_Aliquota { get; set; }
        public string Valor_Aliquota { get; set; }
        public string Posicao { get; set; }
        public string CST { get; set; }           
    }
}
