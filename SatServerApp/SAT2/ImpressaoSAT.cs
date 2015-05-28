using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Globalization;
using System.Security.Cryptography;


namespace invoiceServerApp
{
    public class ImpressaoSAT
    {
        private CultureInfo ci = new CultureInfo("en-US");
        private Utils.ConfigureXml conf;

        private ItensNotaSAT itemNota;
        private PgtNotaSAT pgtNota;


        private DadosEmitSAT dadosEmit;
        private DadosCliSAT dadosCli;

        private DadosSATImpressao dadosSAT;

        private List<ItensNotaSAT> itens = new List<ItensNotaSAT>();
        private List<PgtNotaSAT> pgts = new List<PgtNotaSAT>();

        private DadosSATCancelamento dadosCancelamento;

        //private string dadosQR;

        public string[] DadosImpressao1;
        public string[] DadosImpressao2;
        public string[] DadosImpressao3;

        public string DadosQrCodeCancelamento;

        private TipoImpressao tipoImpressao;

        public ImpressaoSAT(XmlDocument _xml, XmlDocument _xmlCancelado , Utils.ConfigureXml _conf, TipoImpressao _tipo)
        {
            try
            {

                conf = _conf;

                tipoImpressao = _tipo;

                //dadosQR = _dadosQR;

                dadosSAT = new DadosSATImpressao();
                dadosEmit = new DadosEmitSAT();
                dadosCli = new DadosCliSAT();

                GetDadosEmit(_xml.GetElementsByTagName("emit")[0]);

                GetDadosSAT(_xml.GetElementsByTagName("infCFe")[0]);

                GetDadosDest(_xml.GetElementsByTagName("dest")[0]);

                GetDadosSAT(_xml.GetElementsByTagName("ide")[0]);

                GetDadosTtl(_xml.GetElementsByTagName("ICMSTot")[0]);

                //GetDadosAss(_xml.GetElementsByTagName("DigestValue")[0]);

                GetDadosInfCpl(_xml.GetElementsByTagName("infCpl")[0]);

                GetDadosAcresc(_xml.GetElementsByTagName("ICMSTot")[0]);

                foreach (XmlNode node in _xml.GetElementsByTagName("det"))
                {
                    itemNota = new ItensNotaSAT();

                    GetDadosItens(node); //falta pegar o valor do icms

                    itens.Add(itemNota);
                }

                foreach (XmlNode node in _xml.GetElementsByTagName("MP"))
                {
                    pgtNota = new PgtNotaSAT();

                    GetDadosPgt(node);

                    pgts.Add(pgtNota);
                }

                switch (tipoImpressao)
                {
                    case TipoImpressao.VendaCompleto:
                        DefineDadosImpressaoCompleto();
                        break;
                    case TipoImpressao.VendaResumido:
                        //
                        break;
                    case TipoImpressao.Cancelamento:
                        //
                        dadosCancelamento = new DadosSATCancelamento();

                        GetDadosCanc(_xmlCancelado.GetElementsByTagName("infCFe")[0]);
                        GetDadosCanc(_xmlCancelado.GetElementsByTagName("ide")[0]);
                        GetDadosCanc(_xmlCancelado.GetElementsByTagName("dest")[0]);

                        GetDadosCanc(_xmlCancelado.GetElementsByTagName("total")[0]);

                        DefineDadosImpressaoCancelamento();

                        break;


                }

            }catch(Exception ex)
            {
                throw new Exception("Erro preparando Impressão SAT " + ex.Message);
            }

        }

        private void DefineDadosImpressaoCompleto()
        {
            try
            {

                StringBuilder sb = new StringBuilder();

                List<String> Linhas = new List<string>();

                //Cabeçalho

                //sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));
                sb.AppendLine(String.Format("<c><b><ce>{0}</ce></c></b>", dadosEmit.nomeFantasia));
                sb.AppendLine(String.Format("<c><b><ce>{0}</ce></c></b>", dadosEmit.razao));
                sb.AppendLine(String.Format("<c><b><ce>Endereço:{0}, No:{1}</ce></b></c>", dadosEmit.logradouro, dadosEmit.numero));
                sb.AppendLine(String.Format("<c><b>CNPJ:{0}     I.E.:{1}     I.M.: </b></c>", dadosEmit.CNPJ, dadosEmit.IE));

                sb.AppendLine(String.Format("<c><b><ce>----------------------------------------------------</ce></b></c>"));

                sb.AppendLine(String.Format("<n><b><ce>Extrato No. {0}</ce></b></n>", dadosSAT.numeroCFe));
                sb.AppendLine(String.Format("<n><b><ce>CUPOM FISCAL ELETRONICO - SAT</ce></b></n> <l></l>"));

                sb.AppendLine(String.Format("<c><b><ce>----------------------------------------------------</ce></b></c>"));
                //-------



                //CORPO SAT

                if (!String.IsNullOrEmpty(dadosCli.cpf_cnpj_destinatario))
                {
                    sb.AppendLine(String.Format("<c>CPF/CNPJ Consumidor:    {0}</c>", dadosCli.cpf_cnpj_destinatario));
                }
                else
                {
                    sb.AppendLine(String.Format("<c>CPF/CNPJ Consumidor:    {0}</c>", "Nao Identificado"));
                }

                sb.AppendLine(String.Format("<c><b><ce>----------------------------------------------------</ce></c></b>"));

                sb.AppendLine(String.Format("<c><b><ce>{0,-6}|{1,-20}|{2,-3}|{3,-6}|{4,-6}|{5,-6}</ce></c></b>"
                    , "Cod", "Desc", "Qtd", "UN", "Vl.Uni", "Vl.Ttl"
                    ));

                sb.AppendLine(String.Format("<c><b><ce>----------------------------------------------------</ce></c></b>"));

                decimal vt = 0;
                decimal desc = 0;
                int contaItens = 0;
                foreach (ItensNotaSAT i in itens)
                {
                    contaItens++;

                    vt += Convert.ToDecimal(i.valorTotal, ci);
                    desc += Convert.ToDecimal(i.valorDesconto, ci);

                    string tmpDesc;
                    if (i.descricao.Length > 20)
                        tmpDesc = i.descricao.Substring(0, 20);
                    else
                        tmpDesc = i.descricao;

                    sb.AppendLine(String.Format("<c><ce>{0,-6} {1,-20} {2,-3} {3,-6} {4,-6} {5,-6}</ce></c>"
                        , i.codigo
                        , tmpDesc
                        , i.quantidade
                        , i.unidade
                        , i.valorUnidade
                        , i.valorTotal
                        ));

                    if (contaItens == 20)
                    {
                        Linhas.Add(sb.ToString());
                        sb = new StringBuilder();
                        contaItens = 0;
                    }
                }

                Linhas.Add(sb.ToString());
                sb = new StringBuilder();

                sb.AppendLine(String.Format("<l></l>"));

                sb.AppendLine(String.Format("<c> {0,-20}{1,34}</c>"
                    , "Sub Total:"
                    , vt.ToString("F2")
                    ));

                sb.AppendLine(String.Format("<c> {0,-20}{1,34}</c>"
                    , "Desconto:"
                    , desc.ToString("F2")
                    ));

                sb.AppendLine(String.Format("<c> {0,-20}{1,34}</c>"
                    , "Acrescimo:"
                    , dadosSAT.acresc
                    ));

                decimal tmpAcresc = Convert.ToDecimal(dadosSAT.acresc, ci);

                sb.AppendLine(String.Format("<c> {0,-20}{1,34}</n><l></l>"
                    , "Valor Total:"
                    , (vt - desc + tmpAcresc).ToString("F2")
                    ));

                //sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));
                //sb.AppendLine(String.Format("<c><b><ce>Forma de Pagamento</ce></b></c>"));

                foreach (PgtNotaSAT p in pgts)
                {
                    switch (p.codPgt)
                    {
                        case "01":
                            p.descPgt = "Dinheiro";
                            break;
                        case "99":
                            p.descPgt = "Outros";
                            break;
                        default:
                            p.descPgt = "Outros";
                            break;

                    }

                    sb.AppendLine(String.Format("<c><b> {0,-20}{1,34}</c></b>"
                        , p.descPgt + " R$"
                        , p.valorPgt
                        ));
                }

                sb.AppendLine(String.Format("<c><b> {0,-20}{1,34}</c></b>"
                        , "Troco R$"
                        , dadosSAT.troco
                        ));

                sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));

                sb.AppendLine(String.Format("<c><b>Informacoes Adicionais</c></b>"));

                if (dadosSAT.infCpl.Length > 0)
                {
                    String[] tmp = dadosSAT.infCpl.Split('\\');
                    foreach (string s in tmp)
                        sb.AppendLine(String.Format("<c>{0}</c>", s));
                }
                sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));

                //sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));

                //Rodapé ---------------------------------------------------
                sb.AppendLine(String.Format("<c><b><ce>SAT No. {0}</ce></c></b>", dadosSAT.numSerieSAT));
                sb.AppendLine(String.Format("<c><ce>{0} - {1}</ce></c>", dadosSAT.dataEmis, dadosSAT.horaEmis));

                string tmpChave = dadosSAT.chaveConsulta.Substring(3);

                string chave = String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}"
                    , tmpChave.Substring(0, 4)
                    , tmpChave.Substring(4, 4)
                    , tmpChave.Substring(8, 4)
                    , tmpChave.Substring(12, 4)
                    , tmpChave.Substring(16, 4)
                    , tmpChave.Substring(20, 4)
                    , tmpChave.Substring(24, 4)
                    , tmpChave.Substring(28, 4)
                    , tmpChave.Substring(32, 4)
                    , tmpChave.Substring(36, 4)
                    , tmpChave.Substring(40, 4)

                    );

                sb.AppendLine(String.Format("<c><ce>{0}</ce></c><l></l>", chave));

                sb.AppendLine(String.Format("<code128>{0}</code128><code128>{1}</code128>", tmpChave.Substring(0, 22),tmpChave.Substring(22, 22)));

                //sb.AppendLine(String.Format("<l></l><qrcode>{0}</qrcode>", dadosQR));

                //DadosQrCodeCancelamento = dadosQR;

                //FIM Rodape ----------------------------------------------

                Linhas.Add(sb.ToString());

                DadosImpressao1 = Linhas.ToArray();                
            }
            catch (Exception ex)
            {
                throw new Exception("Erro preprando dados Impressao SAT: " + ex.Message);
            }
        }

        

        private void DefineDadosImpressaoCancelamento()
        {
            try
            {               

                StringBuilder sb = new StringBuilder();

                List<String> Linhas = new List<string>();

                //Cabeçalho

                //sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));
                sb.AppendLine(String.Format("<c><b><ce>{0}</ce></c></b>", dadosEmit.nomeFantasia));
                sb.AppendLine(String.Format("<c><b><ce>{0}</ce></c></b>", dadosEmit.razao));
                sb.AppendLine(String.Format("<c><b><ce>Endereço:{0}, No:{1}</ce></b></c>", dadosEmit.logradouro, dadosEmit.numero));
                sb.AppendLine(String.Format("<c><b>CNPJ:{0}     I.E.:{1}     I.M.: </b></c>", dadosEmit.CNPJ, dadosEmit.IE));

                sb.AppendLine(String.Format("<c><b><ce>----------------------------------------------------</ce></b></c>"));

                sb.AppendLine(String.Format("<n><b><ce>Extrato No. {0}</ce></b></n>", dadosSAT.numeroCFe));
                sb.AppendLine(String.Format("<n><b><ce>CUPOM FISCAL ELETRONICO - SAT\nCancelamento</ce></b></n> <l></l>"));

                sb.AppendLine(String.Format("<c><b><ce>DADOS DO CUPOM FISCAL CANCELADO</ce></b></c>"));
                sb.AppendLine(String.Format("<c><b><ce>No. {0}</ce></b></c>", dadosCancelamento.numero));
                sb.AppendLine(String.Format("<c><b><ce>----------------------------------------------------</ce></b></c>"));
                //-------
                
                //CORPO SAT

                if (!String.IsNullOrEmpty(dadosCli.cpf_cnpj_destinatario))
                {
                    sb.AppendLine(String.Format("<c>CPF-CNPJ Consumidor:    {0}</c>", dadosCli.cpf_cnpj_destinatario));
                }
                else
                {
                    sb.AppendLine(String.Format("<c>CPF-CNPJ Consumidor:    {0}</c>", "Nao Identificado"));
                }

                sb.AppendLine(String.Format("<c><b>TOTAL:</b>{0}</c>", dadosCancelamento.valor));

                //outro XML                
                sb.AppendLine(String.Format("<c><b><ce>SAT No. {0}</ce></c></b>", dadosCancelamento.serie));
                sb.AppendLine(String.Format("<c><ce>{0} - {1}</ce></c>", dadosCancelamento.data, dadosCancelamento.hora));

                string tmpChave = dadosCancelamento.chave.Substring(3);

                string chave = String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}"
                    , tmpChave.Substring(0, 4)
                    , tmpChave.Substring(4, 4)
                    , tmpChave.Substring(8, 4)
                    , tmpChave.Substring(12, 4)
                    , tmpChave.Substring(16, 4)
                    , tmpChave.Substring(20, 4)
                    , tmpChave.Substring(24, 4)
                    , tmpChave.Substring(28, 4)
                    , tmpChave.Substring(32, 4)
                    , tmpChave.Substring(36, 4)
                    , tmpChave.Substring(40, 4)

                    );

                sb.AppendLine(String.Format("<c><ce>{0}</ce></c><l></l>", chave));

                sb.AppendLine(String.Format("<code128>{0}</code128><code128>{1}</code128>", tmpChave.Substring(0, 22), tmpChave.Substring(22, 22)));
                
                string dadosQRCancelamento = String.Format("{0}|{1}{2}|{3}|{4}|{5}"
                    , dadosCancelamento.chave
                    ,dadosCancelamento.data
                    ,dadosCancelamento.hora
                    ,dadosCancelamento.valor
                    ,dadosCancelamento.cpfCnpj
                    ,dadosCancelamento.qrCode
                    );

                //sb.AppendLine(String.Format("<l></l><b><lmodulo>4</lmodulo><correcao>3</correcao><qrcode>{0}</qrcode><b>", dadosQRCancelamento));



                Linhas.Add(sb.ToString());

                DadosImpressao1 = Linhas.ToArray();

                DadosQrCodeCancelamento = dadosQRCancelamento;

                sb = new StringBuilder();
                Linhas = new List<string>();

                //outro XML
                
                //Rodapé ---------------------------------------------------
                sb.AppendLine(String.Format("<c><b><ce>DADOS DO CUPOM FISCAL DE CANCELAMENTO</ce></b></c>"));
                sb.AppendLine(String.Format("<c><b><ce>SAT No. {0}</ce></c></b>", dadosSAT.numSerieSAT));
                sb.AppendLine(String.Format("<c><ce>{0} - {1}</ce></c>", dadosSAT.dataEmis, dadosSAT.horaEmis));

                tmpChave = dadosSAT.chaveConsulta.Substring(3);

                chave = String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}"
                    , tmpChave.Substring(0, 4)
                    , tmpChave.Substring(4, 4)
                    , tmpChave.Substring(8, 4)
                    , tmpChave.Substring(12, 4)
                    , tmpChave.Substring(16, 4)
                    , tmpChave.Substring(20, 4)
                    , tmpChave.Substring(24, 4)
                    , tmpChave.Substring(28, 4)
                    , tmpChave.Substring(32, 4)
                    , tmpChave.Substring(36, 4)
                    , tmpChave.Substring(40, 4)

                    );

                sb.AppendLine(String.Format("<c><ce>{0}</ce></c><l></l>", chave));

                sb.AppendLine(String.Format("<code128>{0}</code128><code128>{1}</code128>", tmpChave.Substring(0, 22), tmpChave.Substring(22, 22)));

                //sb.AppendLine(String.Format("<l></l><qrcode>{0}</qrcode>", dadosQR));

                //FIM Rodape ----------------------------------------------

                Linhas.Add(sb.ToString());

                DadosImpressao2 = Linhas.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro preprando dados Impressao SAT: " + ex.Message);
            }
        }

        public void GetDadosEmit(XmlNode node)
        {
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.HasChildNodes && xn.FirstChild.Name != "#text" )
                {
                    GetDadosEmit(xn);
                }
                else
                {
                    switch (xn.Name)
                    {
                        case "CNPJ":
                            dadosEmit.CNPJ = xn.InnerText;
                            break;
                        case "xFant":
                            dadosEmit.nomeFantasia = xn.InnerText;
                            break;
                        case "xLgr":
                            dadosEmit.logradouro= xn.InnerText;
                            break;
                        case "nro":
                            dadosEmit.numero= xn.InnerText;
                            break;
                        case "xBairro":
                            dadosEmit.bairro = xn.InnerText;
                            break;
                        case "xMun":
                            dadosEmit.cidade= xn.InnerText;
                            break;
                        case "IE":
                            dadosEmit.IE= xn.InnerText;
                            break;
                        
                    }
                }
            }            
        }

        public void GetDadosDest(XmlNode node)
        {
            if (node != null)
            {

                foreach (XmlNode xn in node.ChildNodes)
                {
                    if (xn.HasChildNodes && xn.FirstChild.Name != "#text")
                    {
                        GetDadosDest(xn);
                    }
                    else
                    {
                        switch (xn.Name)
                        {
                            case "CPF":
                                dadosCli.cpf_cnpj_destinatario = xn.InnerText;
                                break;
                            case "CNPJ":
                                dadosCli.cpf_cnpj_destinatario = xn.InnerText;
                                break;
                        }
                    }
                }
            }
        }

        public void GetDadosTtl(XmlNode node)
        {
            if (node != null)
            {
                foreach (XmlNode xn in node.ChildNodes)
                {
                    if (xn.HasChildNodes && xn.FirstChild.Name != "#text")
                    {
                        GetDadosSAT(xn);
                    }
                    else
                    {
                        switch (xn.Name)
                        {
                            case "vNF":
                                dadosSAT.valorTtl = Convert.ToDecimal(xn.InnerText, ci);
                                break;
                            case "vICMS":
                                dadosSAT.valorICMS = Convert.ToDecimal(xn.InnerText, ci);
                                break;
                        }
                    }
                }
            }
        }

        public void GetDadosCanc(XmlNode node)
        {
            if (node != null)
            {

                if (node.Name.Equals("infCFe"))
                    dadosCancelamento.chave = node.Attributes[0].InnerText;

                foreach (XmlNode xn in node.ChildNodes)
                {
                    if (xn.Name.Equals("infCFe"))
                        dadosCancelamento.chave = xn.Attributes[0].InnerText;

                    if (xn.HasChildNodes && xn.FirstChild.Name != "#text")
                    {
                        GetDadosSAT(xn);
                    }
                    else
                    {
                        switch (xn.Name)
                        {
                            case "nserieSAT":
                                dadosCancelamento.serie = xn.InnerText;
                                break;
                            case "nCFe":
                                dadosCancelamento.numero = xn.InnerText;
                                break;
                            case "dEmi":
                                dadosCancelamento.data = xn.InnerText;
                                break;
                            case "hEmi":
                                dadosCancelamento.hora = xn.InnerText;
                                break;
                            case "assinaturaQRCODE":
                                dadosCancelamento.qrCode = xn.InnerText;
                                break;
                            case "CPF":
                                dadosCancelamento.cpfCnpj = xn.InnerText;
                                break;
                            case "CNPJ":
                                dadosCancelamento.cpfCnpj = xn.InnerText;
                                break;
                            case "vCFe":
                                dadosCancelamento.valor = xn.InnerText;
                                break;

                        }
                    }
                }
            }
        }

        public void GetDadosSAT(XmlNode node)
        {
            if (node != null)
            {
                if (node.Name.Equals("infCFe"))
                    dadosSAT.chaveConsulta = node.Attributes[0].InnerText;

                foreach (XmlNode xn in node.ChildNodes)
                {
                    if (xn.Name.Equals("infCFe"))
                        dadosSAT.chaveConsulta = xn.Attributes[0].InnerText;

                    if (xn.HasChildNodes && xn.FirstChild.Name != "#text")
                    {
                        GetDadosSAT(xn);
                    }
                    else
                    {
                        switch (xn.Name)
                        {
                            case "nserieSAT":
                                dadosSAT.numSerieSAT = xn.InnerText;
                                break;
                            case "nCFe":
                                dadosSAT.numeroCFe = xn.InnerText;
                                break;
                            case "dEmi":
                                dadosSAT.dataEmis = xn.InnerText;
                                break;
                            case "hEmi":
                                dadosSAT.horaEmis = xn.InnerText;
                                break;
                            
                        }
                    }
                }
            }
        }

        //public void GetDadosAss(XmlNode node)
        //{
        //    foreach (XmlNode xn in node.ChildNodes)
        //    {
        //        dadosNFce.digestValue = xn.InnerText;
        //    }
        //}

        public void GetDadosAcresc(XmlNode node)
        {            
            if (node != null)
            {
                foreach (XmlNode xn in node.ChildNodes)
                {
                    if(xn.Name.Equals("vOutro"))
                        dadosSAT.acresc= xn.InnerText;
                }
            }
        }

        public void GetDadosInfCpl(XmlNode node)
        {
            if (node != null)
            {
                //foreach (XmlNode xn in node.ChildNodes)
                {
                    dadosSAT.infCpl = node.InnerText;
                }
            }
        }


        public void GetDadosItens(XmlNode node)
        {
            if (node != null)
            {
                foreach (XmlNode xn in node.ChildNodes)
                {
                    if (xn.HasChildNodes && xn.FirstChild.Name != "#text")
                    {
                        GetDadosItens(xn);
                    }
                    else
                    {
                        switch (xn.Name)
                        {
                            case "cProd":
                                itemNota.codigo = xn.InnerText;
                                break;
                            case "xProd":
                                itemNota.descricao = xn.InnerText;
                                break;
                            case "qCom":
                                itemNota.quantidade = xn.InnerText;
                                break;
                            case "uCom":
                                itemNota.unidade = xn.InnerText;
                                break;
                            case "vUnCom":
                                itemNota.valorUnidade = xn.InnerText;
                                break;
                            case "vDesc":
                                itemNota.valorDesconto = xn.InnerText;
                                break;
                        }

                    }
                }
                itemNota.valorTotal = (Convert.ToDecimal(itemNota.valorUnidade, ci) * Convert.ToDecimal(itemNota.quantidade, ci)).ToString("F2", ci);
            }
        }

        public void GetDadosPgt(XmlNode node)
        {
            if (node != null)
            {
                foreach (XmlNode xn in node.ChildNodes)
                {
                    if (xn.HasChildNodes && xn.FirstChild.Name != "#text")
                    {
                        GetDadosPgt(xn);
                    }
                    else
                    {
                        switch (xn.Name)
                        {
                            case "cMP":
                                pgtNota.codPgt = xn.InnerText;
                                break;
                            case "vMP":
                                pgtNota.valorPgt = xn.InnerText;
                                break;

                        }
                    }
                }

                switch (pgtNota.codPgt)
                {
                    case "1":
                        pgtNota.descPgt = "Dinheiro";
                        break;
                    case "99":
                        pgtNota.descPgt = "Outros";
                        break;
                }
            }
        }
    }
    

    public class DadosCliSAT
    {
        public string cpf_cnpj_destinatario;        
    }

    public class DadosSATCancelamento
    {
        public string chave;
        public string serie;
        public string numero;
        public string data;
        public string hora;
        public string cpfCnpj;
        public string qrCode;
        public string valor;
    }

    public class DadosSATImpressao
    {
        public string numSerieSAT;
        public string dataEmis;
        public string horaEmis;
        public string chaveConsulta;
        public string numeroCFe;

        //public string emissNFCe;
        //public string chaveNFCe;
        //public string reciboNFCe;

        //public string digestValue;

        public decimal valorTtl;
        public decimal valorICMS;

        public string infCpl = "";
        public string acresc = "";

        public string troco = "";

        public string via = "";
    }

    public class DadosEmitSAT
    {
        public string nomeFantasia;
        public string razao;
        public string CNPJ;
        public string bairro;
        public string cidade;
        public string logradouro;
        public string numero;
        public string IE;
        public string IM;
    }
    public class ItensNotaSAT
    {
        public string codigo;
        public string descricao;
        public string quantidade;
        public string unidade;
        public string valorUnidade;
        public string valorTotal;

        public string valorDesconto;

        public string aliQICMS;
        public string valorICMS;
    }

    public class PgtNotaSAT
    {
        public string codPgt;
        public string descPgt;
        public string valorPgt;
    }
}
