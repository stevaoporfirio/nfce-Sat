using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;

namespace invoiceServerApp
{
    /***         
     Status Da nota fiscal
     CupomParse - Parse concluido
     CupomGeradoXml - Xml de envio montado
     CupomEnviadoSeFaz - xml enviado SEFAZ
     CupomSeFazRetornoOk - retorno positivo da SEFAZ
     CupomImpresso - cupom impresso
     CupomImpressoContingencia - cupom impresso Contingencia
     ***/
    public enum StatusCupom { cupomVazio, CupomParse, CupomGeradoXml, CupomEnviadoSeFaz, CupomSeFazRetornoOk, CupomImpresso, CupomImpressoContingencia, CupomRejeitado, CupomEnviadoFila, CupomConsultadoContigencia, CupomEnviadoContigencia, CupomRejeitadoContigencia, Aprovado, AprovadoContingencia, xmlCancelamentoGerado, xmlCancelamentoRejeicao, xmlCancelamentoEnviado, xmlInutilizacaoGerado, xmlInutilizacaoRejeicao, xmlInutilizacaoEnviado };

    class ParseNFCE : ParseInterface , CallbackStatusInterface
    {
        private Utils.ConfigureXml config;
        const string identificadorHeader = "H";
        const string identificadorHeaderCliente = "H1";
        const string identificadorHeaderConta = "H2";
        const string identificadorHeaderInfoAdic = "H3";
        const string identificadorHeaderTEF = "T1";
        const string identificadorItens = "I";
        const string identificadorPagamentos = "P";
        
        private CultureInfo CI = new CultureInfo("en-US");
        private DadosNfce dtNfce;
        private ProcessaNfce processaDados;
       
        private int StatusNota = 0;

        private string id;

        public ParseNFCE(string _id, Utils.ConfigureXml _config) 
        {
            id = _id;
            config = _config;
            dtNfce = new DadosNfce();
            dtNfce.IdAccount = _id;
            processaDados = new ProcessaNfce(this, config);
        }

        private void SpliData(string msg)
        {            
            try
            {
                string[] dados = msg.Split('?');

                //if (StatusNota == 0) SEM Retry
                {
                    string[] dados2 = dados[0].Split('\n');
                    foreach (string line in dados2)
                    {
                        if (line != "")
                        {
                            if (line.Substring(0, 2).Equals(identificadorHeaderCliente))
                            {
                                DefineHeader(line);
                            }
                            else if (line.Substring(0, 1).Equals(identificadorItens))
                            {
                                ParseAddItem(line);
                            }
                            else if (line.Substring(0, 1).Equals(identificadorPagamentos))
                            {
                                AddPay(line);
                            }
                            else if (line.Substring(0, 2).Equals(identificadorHeaderConta))
                            {
                                AddConta(line);
                            }
                            else if (line.Substring(0, 2).Equals(identificadorHeaderInfoAdic))
                            {
                                AddInfoAdic(line);

                            }
                        }
                    }
                }

                NotificationChanged((int)StatusCupom.CupomParse);
                
                if(dados[1].Length > 3)
                    dtNfce.TefNfce.StringTEF = dados[1].Substring(3);

                validadorDanfe vl = new validadorDanfe(dtNfce);
             
                processaDados.ProcessaCupom(dtNfce);

            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(id +": "+ e.ToString());
                throw new Exception(e.ToString());
            }

        }

        
        private void AddConta(string line)
        {
            try
            {
                string[] dados = line.Split('|');
                dtNfce.isPrint = dados[1];

                dtNfce.WS_ID = dados[2];
                dtNfce.Chk_Num = dados[3];

                dtNfce.Acresc = dados[6];

                dtNfce.PortaImpressora = dados[7];
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }

        }

        private void AddInfoAdic(string line)
        {
            try
            {
                dtNfce.InfoAdic = new List<string>();

                string[] dados = line.Split('|');
                for (int i = 1; i < dados.Length; i++)
                {
                    dtNfce.InfoAdic.Add(dados[i]);
                }

            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }

        }

        private void AddPay(string line)
        {
            string[] dados = line.Split('|');

            PgtNfce p = new PgtNfce();

            p.Cod = "99";

            foreach (Utils.NFCePgt pgConfig in config.configMaquina.PgtList.ListPgt)
            {
                if(dados[1].Equals(pgConfig.codMicros))
                {
                    p.Cod = pgConfig.codNFCe;
                    p.Desc = pgConfig.descMicros;
                    p.tBand = pgConfig.codBandNfce;

                    break;
                }
            }

            //p.Desc = dados[2];
            p.val = dados[3];
            // pegar o nsu do bilhete e montar os grupo baseado no xml

            if (dados.Length >= 5)
                p.cAut = dados[4];
            else
                p.cAut = "";

            dtNfce.pgtsList.Add(p);           
            
        }

        private void DefineHeader(string line)
        {
            try
            {
                string[] dados = line.Split('|');

                dtNfce.JuridicaFisica_dest = dados[1];                
                dtNfce.RazaoSocial_dest = dados[2];
                dtNfce.CPF_CNPJ_dest = dados[3];
                dtNfce.Email_dest = dados[11];
                if (!String.IsNullOrEmpty(dtNfce.CPF_CNPJ_dest))
                {
                    if (String.IsNullOrEmpty(dtNfce.RazaoSocial_dest))
                        dtNfce.RazaoSocial_dest = config.configEmitente.Fantasia;
                }

                if (!String.IsNullOrEmpty(dados[8])) //CEP
                {
                    dtNfce.Logradouro_dest = dados[4];
                    dtNfce.Numero_dest = dados[5];
                    dtNfce.Complemento_dest = dados[6];
                    dtNfce.Bairro_dest = dados[7];
                    dtNfce.Cep_dest = dados[8];
                    dtNfce.Municipio_dest = dados[9];
                    dtNfce.Uf_dest = dados[10];

                    dtNfce.CodMunicipio_dest = Program.cep2IBGE.FindIBGE(dados[8]);

                    dtNfce.Pais_dest = "";
                    dtNfce.CodPais_dest = "1058"; 
                    
                }


                if(dados[1].Equals("J"))
                    dtNfce.Ie_dest = dados[12];

                
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }

        }
        private void ParseAddItem(string line)
        {
            try
            {
                Utils.Logger.getInstance.error(String.Format("ParseAddItem -> {0}",line));

                string[] dados = line.Split('|');


                //inserindo na lista de itens da nota fiscal
                ItensNfce prod = new ItensNfce();
                prod.C_Prod = dados[1];
                prod.X_Prod = dados[2];
                prod.Q_Com = Convert.ToInt32(dados[3]);
                prod.Tipo_Aliquota = dados[6];
                prod.U_Com = "UN";
                prod.U_Trib = "UN";
                prod.V_Desc = Convert.ToDecimal(dados[7], CI);
                prod.NCM = dados[8].TrimEnd().TrimStart();
                
                prod.V_UnCom = Convert.ToDecimal(dados[4], CI);

                prod.V_Prod = prod.Q_Com * prod.V_UnCom;

                if (!dados[6].Equals("FF"))
                {
                    prod.Valor_Aliquota = prod.Tipo_Aliquota;
                }
                else if (dados[6].Equals("FF"))
                {
                    prod.Valor_Aliquota = "0";
                }

                dtNfce.itensList.Add(prod);
            }
            catch (Exception e)
            {

                Utils.Logger.getInstance.error(e.ToString());
                throw new Exception(e.ToString());
            }
        }

        

        public string messageParse(string msg)
        {
            SpliData(msg);

            return processaDados.GetNota();
        }

        public string getCodigo(string msg)
        {
            return "";
        }
        public string ReImpressaoTEF(string _msg)
        {
            try
            {
                string[] dados = _msg.Split('|');
                string portaImpressora = dados[0];
                string msgTEF = dados[1];
                ImprimirEpsonNF.ImprimirTef(msgTEF, portaImpressora);
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
            }
            return "|END|";

        }
        public string getQrCode(string msg)
        {
            return processaDados.GetQrcode();
        }

        public void NotificationChanged(int status)
        {
            StatusNota = status;
        }

        public int GetStatusCupom()
        {
            return StatusNota;
        }
        public string messageCancel(string msg)
        {
            string ret = "";
            try
            {
                CancelNFCE cancel = new CancelNFCE();
                string[] dados = msg.Split('|');
                cancel.ID = dados[0];
                
                
                 ret = processaDados.ProcessaCancel(cancel);

            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
            }
            return ret;
        }
        public string messageInutilizacao(string msg)
        {
            string ret = "";
            InutilizacaoNFCE inutili = new InutilizacaoNFCE();
            string[] dados = msg.Split('|');
            inutili.ID = dados[0];
            inutili.numeroInicial = dados[1];
            if (dados.Length > 2)
                inutili.numeroFinal = dados[2];
            else
                inutili.numeroFinal = inutili.numeroInicial;

            ret = processaDados.ProcessaInutilizacao(inutili);

            return ret;
        }
        public string ReImpressaoDanfe(string msg)
        {
            string[] dados = msg.Split('|');
            string numDanfe = dados[0];
            string portaImpressora = dados[1];
            return processaDados.ReImpressaoDanfe(config, numDanfe, portaImpressora);
            
        }
        public string DesbloqueioSat(string msg)
        {
            return "";
        }
    }
}
