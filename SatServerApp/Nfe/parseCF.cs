using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace invoiceServerApp
{
    class parseCF
    {
        private XmlDocument xmlNfce = new XmlDocument();
        private DadosImpressao cupom = new DadosImpressao();
        private qrCodeClass qrcode = new qrCodeClass();
        private Utils.ConfigureXml config;

        public parseCF(XmlDocument _xmlNfce, Utils.ConfigureXml _config)
        {
            xmlNfce = _xmlNfce;
            config = _config;
        }
        public void MontaCupom(string _chaveAcesso)
        {
            cupom.nomeFantasia = config.configEmitente.Fantasia;
            cupom.razao = config.configEmitente.RazaoSocial;
            cupom.CNPJ = config.configEmitente.Cnpj;
            cupom.bairro = config.configEmitente.endereco.Bairro;
            cupom.cidade = config.configEmitente.endereco.Cidade;
            cupom.logradouro = config.configEmitente.endereco.Logradouro;
            cupom.numero = config.configEmitente.endereco.Numero;
            cupom.IE = config.configEmitente.Ie;
            //cupom.IM = config.configNFCe.configNFCeEmitente.Emit_IM;
            cupom.municipio = config.configEmitente.endereco.Cidade;
            qrcode.chavedeacesso = _chaveAcesso;
            getCpf_cnpj();
            getItens();
            getDigestValue();
            getDadosQrCode();
            qrcode.MontaQrCode();
            cupom.parse();
        }
        public string getNota()
        {
            return cupom.notaFiscal;
        }
        public string getQrcode()
        {
            return qrcode.qrcode;
        }
        private void getDadosQrCode()
        {
            try
            {
                qrcode.amb = config.configNFCe.TpAmb;
                qrcode.digestValue = config.configNFCe.digestValue;
                qrcode.CSC = config.configNFCe.CSC;
                qrcode.token = config.configNFCe.tokenNfce;
                qrcode.cpf_cnpj = cupom.cpf_cnpj_destinatario;
                qrcode.versaoQrCode = config.configNFCe.VersaoQrcode;

                XmlNodeList elemListTot = xmlNfce.GetElementsByTagName("ICMSTot");

                for (int i = 0; i < elemListTot.Count; i++)
                {
                    foreach (XmlNode n in elemListTot[i])
                    {
                        switch (n.Name)
                        {
                            case "vICMS":
                                qrcode.valorICMS = n.InnerText;
                                cupom.valorTotalImposto = Convert.ToDouble(n.InnerText);
                                break;
                            case "vNF":
                                qrcode.valorTotal = n.InnerText;
                                cupom.valorTotal = Convert.ToDouble(n.InnerText);
                                break;

                        }
                    }
                }

                XmlNodeList elemList = xmlNfce.GetElementsByTagName("ide");
                for (int i = 0; i < elemList.Count; i++)
                {
                    foreach (XmlNode n in elemList[i])
                    {
                        if (n.Name == "dhEmi")
                            qrcode.data = n.InnerText;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }

        }
        
        private void getItens()
        {
            try
            {
                XmlNodeList elemList = xmlNfce.GetElementsByTagName("prod");
                for (int i = 0; i < elemList.Count; i++)
                {
                    itensNota it = new itensNota();
                    foreach (XmlNode n in elemList[i])
                    {
                        switch (n.Name)
                        {
                            case "cProd":
                                it.codigo = n.InnerText;
                                break;
                            case "xProd":
                                it.descricao = n.InnerText;
                                break;
                            case "qCom":
                                it.quantidade = n.InnerText;
                                break;
                            case "uCom":
                                it.unidade = n.InnerText;
                                break;
                            case "vProd":
                                it.valorItem = n.InnerText;
                                break;
                            case "vUnTrib":
                                it.valorTributo = n.InnerText;
                                break;
                            case "vUnCom":
                                it.valorUnidade = n.InnerText;
                                break;

                        }
                    }
                    cupom.itens.Add(it);
                }
                getCpf_cnpj();
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }
        }
        private void getDigestValue()
        {
            XmlNodeList elemList = xmlNfce.GetElementsByTagName("Reference");
            for (int i = 0; i < elemList.Count; i++)
            {
                foreach (XmlNode n in elemList[i])
                {
                    if(n.Name == "DigestValue")
                            qrcode.digestValue = n.Value;                    

                    
                }
            }
        }

        private void getCpf_cnpj()
        {
            try
            {
                XmlNodeList elemList = xmlNfce.GetElementsByTagName("dest");
                for (int i = 0; i < elemList.Count; i++)
                {
                    foreach (XmlNode n in elemList[i])
                    {
                        switch (n.Name)
                        {
                            case "CPF":
                                cupom.cpf_cnpj_destinatario = n.InnerText;
                                break;
                            case "CNPJ":
                                cupom.cpf_cnpj_destinatario = n.InnerText;
                                break;

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }
        }

    }
}
