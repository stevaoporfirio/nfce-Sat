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
    public class DadosNota    {

        private CultureInfo ci = new CultureInfo("en-US");
        private Utils.ConfigureXml conf;

        private ItensNota itemNota;
        private PgtNota pgtNota;


        private DadosEmit dadosEmit;
        private DadosCli dadosCli;

        private DadosNFCe dadosNFce;

        private List<ItensNota> itens = new List<ItensNota>();
        private List<PgtNota> pgts = new List<PgtNota>();

        public string[] DadosImpressao;

        public string QRCode;

        //public string TEF;

        //QR
            private string chavedeacesso;
            private string versaoQrCode;
            private string amb;
            private string cpf_cnpj;
            private string data;
            private string valorTotal;
            private string valorICMS;
            private string digestValue;
            private string CSC;
            private string token;

            private string strTEF;

            //private string qrcode;
        //
        
        public DadosNota(XmlDocument _xml, Utils.ConfigureXml _conf, string recibo, string via)
        {
            //xml = _xml;

            conf = _conf;

            dadosNFce = new DadosNFCe();
            dadosEmit = new DadosEmit();
            dadosCli = new DadosCli();

            dadosNFce.reciboNFCe = recibo;
            dadosNFce.via = via;

            GetDadosEmit(_xml.GetElementsByTagName("emit")[0]);

            GetDadosNFCe(_xml.GetElementsByTagName("NFe")[0]);

            GetDadosDest(_xml.GetElementsByTagName("dest")[0]);

            GetDadosTtl(_xml.GetElementsByTagName("ICMSTot")[0]);

            GetDadosAss(_xml.GetElementsByTagName("DigestValue")[0]);

            GetDadosInfCpl(_xml.GetElementsByTagName("infAdic")[0]);
            
            GetDadosAcresc(_xml.GetElementsByTagName("ICMSTot")[0]);

            foreach (XmlNode node in _xml.GetElementsByTagName("det"))
            {
                itemNota = new ItensNota();

                GetDadosItens(node); //falta pegar o valor do icms

                itens.Add(itemNota);          
            }

            foreach(XmlNode node in _xml.GetElementsByTagName("pag"))
            {
                pgtNota = new PgtNota();

                GetDadosPgt(node);

                pgts.Add(pgtNota);
            }

            DefineDadosImpressao();

            DefineQRCode();

        }

        private void DefineQRCode()
        {
            chavedeacesso = dadosNFce.chaveNFCe.Substring(3);
            versaoQrCode = conf.configNFCe.VersaoQrcode;
            amb = conf.configNFCe.TpAmb;
            cpf_cnpj = dadosCli.cpf_cnpj_destinatario;
            data = dadosNFce.emissNFCe;
            valorTotal = dadosNFce.valorTtl.ToString("F2",ci);
            valorICMS = dadosNFce.valorICMS.ToString("F2",ci);
            digestValue = dadosNFce.digestValue;
            CSC = conf.configNFCe.CSC;
            token = conf.configNFCe.tokenNfce;
            
            MontaQrCode();
        }

        public void MontaQrCode()
        {
            string nota = "";
            data = stringToHex(data);
            digestValue = stringToHex(digestValue);

            nota = "chNFe=" + chavedeacesso;
            nota += "&nVersao=" + versaoQrCode;
            nota += "&tpAmb=" + amb;
            nota += "&cDest=" + cpf_cnpj;
            nota += "&dhEmi=" + data;
            nota += "&vNF=" + valorTotal;
            nota += "&vICMS=" + valorICMS;
            nota += "&digVal=" + digestValue;
            nota += "&cIdToken=" + CSC;

            
            string url = "";

            if (conf.configNFCe.TpAmb.Equals("1"))
                url = conf.configNFCe.UrlProducao + nota;
            else
                url = conf.configNFCe.UrlHomologacao + nota;

          
            
            
                
            //string url = "http://www.sefaz.mt.gov.br/nfce/consultanfce?" + nota;

            
                



            nota += token;
            QRCode = gerarHash(nota);
            url += "&cHashQRCode=" + QRCode;
            QRCode = url;

        }

        private string stringToHex(string dado)
        {
            char[] values = dado.ToCharArray();
            string hexOutput = "";
            foreach (char letter in values)
            {
                int value = Convert.ToInt32(letter);
                hexOutput += String.Format("{0:x}", value);
            }
            return hexOutput;
        }

        private string gerarHash(string str)
        {
            SHA1 hasher = SHA1.Create();
            StringBuilder gerarString = new StringBuilder();
            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] array = encoding.GetBytes(str);
            array = hasher.ComputeHash(array);
            foreach (byte item in array)
            {
                gerarString.Append(item.ToString("x2"));
            }

            return gerarString.ToString();
        }

        private void DefineDadosImpressao()
        {
            StringBuilder sb = new StringBuilder();

            List<String> Linhas = new List<string>();


            sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));
            sb.AppendLine(String.Format("<c><b><ce>{0}</ce></c></b>", dadosEmit.nomeFantasia));
            sb.AppendLine(String.Format("<c><b>CNPJ:{0}</c></b>", dadosEmit.CNPJ));
            sb.AppendLine(String.Format("<c><b>I.E.:{0}</c></b>", dadosEmit.IE));
            sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));

            sb.AppendLine(String.Format("<c><ce>{0}, {1} - {2} - {3}</ce></ce>"
                , dadosEmit.logradouro
                , dadosEmit.numero
                , dadosEmit.bairro
                , dadosEmit.cidade
                ));

            sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));
            sb.AppendLine(String.Format("<c><ce>Danfe NFC-e - Documento Auxiliar da</ce><c>"));
            sb.AppendLine(String.Format("<ce>Nota Fiscal Eletronica para Consumidor Final</ce>"));
            sb.AppendLine(String.Format("<ce>Nao permite aproveitamento de</ce>"));
            sb.AppendLine(String.Format("<ce>credito de ICMS</ce>"));

            if(conf.configNFCe.Contingencia)
                sb.AppendLine(String.Format("<ce>Emitida em Contingencia</ce>"));

            sb.AppendLine(String.Format("<ce>{0}",dadosNFce.via));

            sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));
            sb.AppendLine(String.Format("<c>NFCe: {0}        Serie: {1}</c>"
                , dadosNFce.numNFCe
                , dadosNFce.serieNFCe
                ));
            sb.AppendLine(String.Format("<c>Emissao:{0}</c>"
                , dadosNFce.emissNFCe
                ));

            string tmpURL = "";

            if(conf.configNFCe.TpAmb.Equals("1"))
                tmpURL = conf.configNFCe.UrlProducao;
            else
                tmpURL = conf.configNFCe.UrlHomologacao;


            sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));
            sb.AppendLine(String.Format("<c><ce>Consulte Pela Chave de Acesso em:</ce></c>"));
            sb.AppendLine(String.Format("<c><ce>{0}</ce></b>"
                , tmpURL
                ));

            string tmpChave = dadosNFce.chaveNFCe.Substring(3);

            string Chave = String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10}"
                ,tmpChave.Substring(0,4)
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

            sb.AppendLine(String.Format("<c><b><ce>Chave de Acesso</ce></c></b>"));
            sb.AppendLine(String.Format("<c><b><ce>{0}</ce></c></b>"
                , dadosNFce.chaveNFCe.Substring(3)
                ));

            sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));

            if (!String.IsNullOrEmpty(dadosCli.cpf_cnpj_destinatario))
            {
                sb.AppendLine(String.Format("<c><b><ce>Consumidor</ce></c></b>"));
                sb.AppendLine(String.Format("<c><b>CPF/CNPJ:{0}</c></b>"
                    , dadosCli.cpf_cnpj_destinatario
                    ));
            }
            else
            {
                sb.AppendLine(String.Format("<c><b><ce>Consumidor Nao Identificado</ce></c></b>"));
            }

            Linhas.Add(sb.ToString());

            sb = new StringBuilder();
            
            sb.AppendLine(String.Format("<c><b><ce>{0,-6} {1,-20} {2,-3} {3,-6} {4,-6} {5,-6}</ce></c></b>"
                , "Cod", "Desc", "Qtd", "UN", "Vl.Uni", "Vl.Ttl"
                ));

            decimal vt = 0;
            decimal desc = 0;
            int contaItens = 0;
            foreach (ItensNota i in itens)
            {
                contaItens++;

                vt += Convert.ToDecimal(i.valorTotal, ci);
                desc += Convert.ToDecimal(i.valorDesconto, ci);

                string tmpDesc;
                if (i.descricao.Length > 20)
                    tmpDesc = i.descricao.Substring(0, 20);
                else
                    tmpDesc = i.descricao;

                sb.AppendLine(String.Format("<c><b><ce>{0,-6} {1,-20} {2,-3} {3,-6} {4,-6} {5,-6}</ce></c></b>"
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

            sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));

            sb.AppendLine(String.Format("<c><b>{0,-20}{1,24}</c></b>"
                , "Sub Total:"
                , vt.ToString("F2")
                ));

            //sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));

            sb.AppendLine(String.Format("<c><b>{0,-20}{1,24}</c></b>"
                , "Desconto:"
                , desc.ToString("F2")
                ));

            sb.AppendLine(String.Format("<c><b>{0,-20}{1,24}</c></b>"
                , "Acrescimo:"
                , dadosNFce.acresc
                ));

            //sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));

            //
            decimal tmpAcresc = Convert.ToDecimal(dadosNFce.acresc, ci);

            sb.AppendLine(String.Format("<c><b>{0,-20}{1,24}</c></b>"
                , "Valor Total:"
                , (vt - desc + tmpAcresc).ToString("F2")
                ));

            sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));
            sb.AppendLine(String.Format("<c><b><ce>Forma de Pagamento</ce></b></c>"));

            foreach (PgtNota p in pgts)
            {
                switch (p.codPgt)
                {
                    case "01":
                        p.descPgt = "Dinheiro";
                        break;
                    case "03":
                        p.descPgt = "Cartao Credito";
                        break;
                    case "04":
                        p.descPgt = "Cartao Debito";
                        break;

                    case "10":
                        p.descPgt = "Vale Alim";
                        break;
                    case "11":
                        p.descPgt = "Vale Refeic";
                        break;
                    case "99":
                        p.descPgt = "Outros";
                        break;
                }
                //switch (p.codPgt)
                //{
                //    case "01":
                //        p.descPgt = "Dinheiro";
                //        break;
                //    case "99":
                //        p.descPgt = "Outros";
                //        break;
                //    default :
                //        p.descPgt = "Outros";
                //        break;

                //}

                sb.AppendLine(String.Format("<c><b>{0,-20}{1,24}</c></b>"
                    , p.descPgt
                    , p.valorPgt
                    ));
            }

            sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));

            sb.AppendLine(String.Format("<c><b>Informacoes Adicionais</c></b>"));

            if (dadosNFce.infCpl.Length > 0)
            {
                String[] tmp = dadosNFce.infCpl.Split('\\');
                foreach(string s in tmp)
                    sb.AppendLine(String.Format("<c><b>{0}</c></b>", s));
            }
            sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));

            sb.AppendLine(String.Format("<c><b><ce>{0}</ce></c></b>", dadosNFce.reciboNFCe));

            //sb.AppendLine(String.Format("<c><b><ce>------------------------------------------------</ce></c></b>"));

            sb.AppendLine(String.Format("<c><b><ce>Consulta via Leitor de QR Code"));

            Linhas.Add(sb.ToString());

            DadosImpressao = Linhas.ToArray();
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
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.HasChildNodes && xn.FirstChild.Name != "#text")
                {
                    GetDadosNFCe(xn);
                }
                else
                {
                    switch (xn.Name)
                    {
                        case "vNF":
                            dadosNFce.valorTtl = Convert.ToDecimal(xn.InnerText,ci);
                            break;                        
                        case "vICMS":
                            dadosNFce.valorICMS = Convert.ToDecimal(xn.InnerText,ci);
                            break;                        
                    }
                }
            }   
        }

        public void GetDadosNFCe(XmlNode node)
        {
            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Name.Equals("infNFe"))
                    dadosNFce.chaveNFCe = xn.Attributes[0].InnerText;

                if (xn.HasChildNodes && xn.FirstChild.Name != "#text")
                {
                    GetDadosNFCe(xn);
                }
                else
                {
                    switch (xn.Name)
                    {
                        case "Id":
                            dadosNFce.chaveNFCe = xn.Attributes[0].InnerText;
                            break;
                        case "nNF":
                            dadosNFce.numNFCe = xn.InnerText;
                            break;
                        case "serie":
                            dadosNFce.serieNFCe = xn.InnerText;
                            break;
                        case "dhEmi":
                            dadosNFce.emissNFCe = xn.InnerText;
                            break;
                    }
                }
            }
        }

        public void GetDadosAss(XmlNode node)
        {
            foreach (XmlNode xn in node.ChildNodes)
            {
                dadosNFce.digestValue = xn.InnerText;
            }
        }

        public void GetDadosAcresc(XmlNode node)
        {
            if (node != null)
            {
                foreach (XmlNode xn in node.ChildNodes)
                {
                    if(xn.Name.Equals("vOutro"))
                        dadosNFce.acresc= xn.InnerText;
                }
            }
        }

        public void GetDadosInfCpl(XmlNode node)
        {
            if (node != null)
            {
                foreach (XmlNode xn in node.ChildNodes)
                {
                    dadosNFce.infCpl = xn.InnerText;
                }
            }
        }


        public void GetDadosItens(XmlNode node)
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
            itemNota.valorTotal = (Convert.ToDecimal(itemNota.valorUnidade,ci) * Convert.ToDecimal(itemNota.quantidade,ci)).ToString("F2",ci);              
        }

        public void GetDadosPgt(XmlNode node)
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
                        case "tPag":
                            pgtNota.codPgt = xn.InnerText;
                            break;
                        case "vPag":
                            pgtNota.valorPgt = xn.InnerText;
                            break;                       

                    }                    
                }
            }
            
            switch (pgtNota.codPgt)
            {
                case "01":
                    pgtNota.descPgt = "Dinheiro";
                    break;
                case "03":
                    pgtNota.descPgt = "Cartao Credito";
                    break;
                case "04":
                    pgtNota.descPgt = "Cartao Debito";
                    break;
                
                case "10":
                    pgtNota.descPgt = "Vale Alim";
                    break;
                case "11":
                    pgtNota.descPgt = "Vale Refeic";
                    break;                                
                case "99":
                    pgtNota.descPgt = "Outros";
                    break;
            }
        }
    }

    public class DadosCli
    {
        public string cpf_cnpj_destinatario;
        //public string endereco;
        //public string municipio;
    }

    public class DadosNFCe
    {
        public string numNFCe;
        public string serieNFCe;
        public string emissNFCe;
        public string chaveNFCe;
        public string reciboNFCe;

        public string digestValue;

        public decimal valorTtl;
        public decimal valorICMS;

        public string infCpl = "";
        public string acresc = "";

        public string via = "";
    }
    
    public class DadosEmit
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
    public class ItensNota
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

    public class PgtNota
    {
        public string codPgt;
        public string descPgt;
        public string valorPgt;
    }
}

