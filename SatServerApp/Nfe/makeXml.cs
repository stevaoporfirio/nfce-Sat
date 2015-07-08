using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace invoiceServerApp
{
    class makeXml
    {
        private DadosNfce dtNfce;
        private Utils.ConfigureXml config;
        private string cNF;
        public string chaveAcesso;
        private string digitoVerificador;
        private string versaoApp;
        private decimal ttlProdutos = 0;
        private decimal ttlDesc = 0;
        private decimal vOutroRateioTtl = 0;
        public X509Certificate2 cert = null;
        private XmlTextWriter xmlWriter;
        public XmlDocument xmlDoc = new XmlDocument();
        public XmlDocument xmlDocAss = new XmlDocument();
        private StringWriter XmlString = new StringWriter();
        
        private Assembly _assembly = Assembly.GetExecutingAssembly();
        private CultureInfo ci = new CultureInfo("en-US");

        private string strTef = "";

        public string nomeXml = "";

        public makeXml(DadosNfce _dtNfce, Utils.ConfigureXml _config, X509Certificate2 _cert)
        {
            config = _config;
            dtNfce = _dtNfce;
            cert = _cert;

            //strTef = _strTef;

            processXml();
        }
        
        public void processXml()
        {
            try
            {
                versaoApp = _assembly.GetName().Version.Major.ToString() + "." +
                                   _assembly.GetName().Version.Minor.ToString() + "." +
                                   _assembly.GetName().Version.Build.ToString() + "." +
                                   _assembly.GetName().Version.Revision.ToString();


                gerarSequenceNumberNfce();

                //chaveAcesso = GerarChaveDeAcesso(Convert.ToInt32(config.configNFCe.sequenceNumberNfce), DateTime.Now);

                parseNfce();

                xmlDoc.LoadXml(XmlString.ToString());

                //nomeXml = String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles, "NFe" + chaveAcesso);
                nomeXml = "NFe" + chaveAcesso;                
                
                assinaturaXML();

                xmlDocAss.Save(String.Format("{0}\\{1}.xml", config.configMaquina.pathFiles, nomeXml));
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }
        }

        private void parseNfce()
        {
            try
            {
                xmlWriter = new XmlTextWriter(XmlString);


                //cabeçalho
                xmlWriter.WriteStartElement("enviNFe", "http://www.portalfiscal.inf.br/nfe");
                xmlWriter.WriteAttributeString("versao", config.configNFCe.VersaoLayout);

                xmlWriter.WriteElementString("idLote", "1");
                xmlWriter.WriteElementString("indSinc", "0");
                xmlWriter.WriteStartElement("NFe", "http://www.portalfiscal.inf.br/nfe");
                xmlWriter.WriteStartElement("infNFe");
                xmlWriter.WriteAttributeString("Id", "NFe" + chaveAcesso);
                xmlWriter.WriteAttributeString("versao", config.configNFCe.VersaoLayout);

                //IDE
                xmlWriter.WriteStartElement("ide");
                xmlWriter.WriteElementString("cUF", config.configEmitente.endereco.Cod_estado);   //config.Cod_estado);
                xmlWriter.WriteElementString("cNF", cNF);
                xmlWriter.WriteElementString("natOp", config.configNFCe.NatOp);
                xmlWriter.WriteElementString("indPag", config.configNFCe.IndPag.Substring(0, 1));
                xmlWriter.WriteElementString("mod", "65");
                xmlWriter.WriteElementString("serie", config.configNFCe.Serie); //todo configuracao
                xmlWriter.WriteElementString("nNF", config.configNFCe.sequenceNumberNfce);
                xmlWriter.WriteElementString("dhEmi", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                xmlWriter.WriteElementString("tpNF", "1");
                xmlWriter.WriteElementString("idDest", "1");
                xmlWriter.WriteElementString("cMunFG", config.configEmitente.endereco.Cod_cidade);
                xmlWriter.WriteElementString("tpImp", config.configNFCe.TpImp.Substring(0, 1));

                if (config.configNFCe.Contingencia)
                    xmlWriter.WriteElementString("tpEmis", "9");
                else
                    xmlWriter.WriteElementString("tpEmis", config.configNFCe.TpEmis.Substring(0, 1));


                xmlWriter.WriteElementString("cDV", digitoVerificador);
                xmlWriter.WriteElementString("tpAmb", config.configNFCe.TpAmb.Substring(0, 1));
                xmlWriter.WriteElementString("finNFe", "1");
                xmlWriter.WriteElementString("indFinal", "1");
                xmlWriter.WriteElementString("indPres", "1");
                xmlWriter.WriteElementString("procEmi", "0");
                xmlWriter.WriteElementString("verProc", versaoApp);

                if (config.configNFCe.Contingencia)
                {
                    xmlWriter.WriteElementString("dhCont", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                    xmlWriter.WriteElementString("xJust", "NFC-e emitida em modo de contingencia off-line");
                }
                xmlWriter.WriteEndElement();

                //EMIT
                xmlWriter.WriteStartElement("emit");
                xmlWriter.WriteElementString("CNPJ", config.configEmitente.Cnpj);
                xmlWriter.WriteElementString("xNome", config.configEmitente.RazaoSocial);
                xmlWriter.WriteElementString("xFant", config.configEmitente.Fantasia);
                xmlWriter.WriteStartElement("enderEmit");
                xmlWriter.WriteElementString("xLgr", config.configEmitente.endereco.Logradouro);
                xmlWriter.WriteElementString("nro", config.configEmitente.endereco.Numero);
                xmlWriter.WriteElementString("xBairro", config.configEmitente.endereco.Bairro);
                xmlWriter.WriteElementString("cMun", config.configEmitente.endereco.Cod_cidade);
                xmlWriter.WriteElementString("xMun", config.configEmitente.endereco.Cidade);
                xmlWriter.WriteElementString("UF", config.configEmitente.endereco.Uf.ToUpper());
                xmlWriter.WriteElementString("CEP", config.configEmitente.endereco.Cep);
                xmlWriter.WriteElementString("cPais", "1058");
                xmlWriter.WriteElementString("xPais", "BRASIL");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteElementString("IE", config.configEmitente.Ie);
                xmlWriter.WriteElementString("CRT", config.configNFCe.Crt);
                xmlWriter.WriteEndElement();

                //Dest
                if (!String.IsNullOrEmpty(dtNfce.CPF_CNPJ_dest))
                {
                    xmlWriter.WriteStartElement("dest");

                    if (dtNfce.JuridicaFisica_dest.Equals("F"))
                        xmlWriter.WriteElementString("CPF", dtNfce.CPF_CNPJ_dest);
                    else
                        xmlWriter.WriteElementString("CNPJ", dtNfce.CPF_CNPJ_dest);

                    if (config.configNFCe.TpAmb.Substring(0, 1).Equals("2"))
                        xmlWriter.WriteElementString("xNome", "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL");
                    else
                        xmlWriter.WriteElementString("xNome", dtNfce.RazaoSocial_dest);

                    if (!String.IsNullOrEmpty(dtNfce.Cep_dest))
                    {
                        xmlWriter.WriteStartElement("enderDest");
                        xmlWriter.WriteElementString("xLgr", dtNfce.Logradouro_dest);
                        xmlWriter.WriteElementString("nro", dtNfce.Numero_dest);
                        xmlWriter.WriteElementString("xBairro", dtNfce.Bairro_dest);
                        xmlWriter.WriteElementString("cMun", dtNfce.CodMunicipio_dest);
                        xmlWriter.WriteElementString("xMun", dtNfce.Municipio_dest);
                        xmlWriter.WriteElementString("UF", dtNfce.Uf_dest.ToUpper());
                        xmlWriter.WriteElementString("CEP", dtNfce.Cep_dest);
                        xmlWriter.WriteElementString("cPais", "1058");
                        xmlWriter.WriteElementString("xPais", "BRASIL");
                        xmlWriter.WriteEndElement();
                    }

                    if (config.configNFCe.VersaoLayout.Equals("3.10"))
                    {
                        if (dtNfce.JuridicaFisica_dest.Equals("J"))
                        {
                            if (dtNfce.Ie_dest.Length > 5)
                            {
                                xmlWriter.WriteElementString("indIEDest", "1");
                                xmlWriter.WriteElementString("IE", dtNfce.Ie_dest);
                            }
                            else
                            {
                                xmlWriter.WriteElementString("indIEDest", "9");
                                //xmlWriter.WriteElementString("IE", "");
                            }
                        }
                        else
                        {
                            xmlWriter.WriteElementString("indIEDest", "9");
                            //xmlWriter.WriteElementString("IE", "");
                        }
                    }
                    else
                    {
                        if (dtNfce.JuridicaFisica_dest.Equals("J"))
                        {
                            xmlWriter.WriteElementString("IE", dtNfce.Ie_dest);
                        }
                        else
                        {
                            xmlWriter.WriteElementString("IE", "");
                        }
                    }

                    if (dtNfce.Email_dest != "")
                        xmlWriter.WriteElementString("email", dtNfce.Email_dest);
                    else
                        xmlWriter.WriteElementString("email", config.configEmitente.Email);

                    xmlWriter.WriteEndElement();
                }

                //Itens e total
                AddItens();

                //transporte
                xmlWriter.WriteStartElement("transp");
                xmlWriter.WriteElementString("modFrete", "9");
                xmlWriter.WriteEndElement();


                //pagamento
                decimal totalPago = 0;
                decimal totalPagoDinheiro = 0;
                foreach (PgtNfce pg in dtNfce.pgtsList)
                {
                        totalPago += Convert.ToDecimal(pg.val, ci);
                        if(pg.Cod == "01")
                            totalPagoDinheiro += Convert.ToDecimal(pg.val, ci);
                }

                decimal totalgeral = (ttlProdutos - ttlDesc + vOutroRateioTtl);

                
                foreach (PgtNfce pg in dtNfce.pgtsList)
                {
                    
                    if (pg.Cod != "01" && !String.IsNullOrEmpty(pg.cAut))
                    {
                        xmlWriter.WriteStartElement("pag");
                        xmlWriter.WriteElementString("tPag", pg.Cod);
                        xmlWriter.WriteElementString("vPag", pg.val);
                        xmlWriter.WriteStartElement("card");
                            xmlWriter.WriteElementString("CNPJ", config.configMaquina.CNPJCredenciadoraCartao);
                            xmlWriter.WriteElementString("tBand", pg.tBand);
                        xmlWriter.WriteElementString("cAut", pg.cAut);
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();
                    }
                    else if (pg.Cod != "01" && String.IsNullOrEmpty(pg.cAut))
                    {
                        xmlWriter.WriteStartElement("pag");
                        xmlWriter.WriteElementString("tPag", pg.Cod);
                        xmlWriter.WriteElementString("vPag", pg.val);
                        xmlWriter.WriteEndElement();
                    }
                    
                }

                if (totalPagoDinheiro > 0)
                {
                    xmlWriter.WriteStartElement("pag");
                    xmlWriter.WriteElementString("tPag", "01");
                    if (totalPago > totalgeral)
                    {
                        decimal pagDinheiro = totalPagoDinheiro - (totalPago - totalgeral);
                        xmlWriter.WriteElementString("vPag", pagDinheiro.ToString("F2", ci));
                    }
                    else
                    {
                        xmlWriter.WriteElementString("vPag", totalPagoDinheiro.ToString("F2",ci));
                    }

                    xmlWriter.WriteEndElement();
                  //  xmlWriter.WriteElementString("vPag", Convert.ToString(totalPagoDinheiro));
                }
                   
                

                





               


                //{
                //    //nao pode existir troco, enquanto quando a forma de pagamento for maior q o valor da conta
                //    //deve tirar do pagamento em dinheiro (cod 01), caso haja troco
                //    xmlWriter.WriteStartElement("pag");
                //    xmlWriter.WriteElementString("tPag", dtNfce.pgtsList[0].Cod);
                //    xmlWriter.WriteElementString("vPag", (ttlProdutos - ttlDesc + vOutroRateioTtl).ToString("F2", ci));
                //    xmlWriter.WriteEndElement();
                //}

                //Outras Info
                {
                    if (dtNfce.InfoAdic != null)
                    {
                        xmlWriter.WriteStartElement("infAdic");

                        //concatenando
                        String tmp = "";
                        foreach (string s in dtNfce.InfoAdic)
                        {
                            tmp += (s.TrimEnd().TrimStart() + " ");
                        }

                        xmlWriter.WriteElementString("infCpl", tmp.TrimStart().TrimEnd());
                        xmlWriter.WriteEndElement();
                    }
                }

                xmlWriter.WriteEndElement();
                xmlWriter.Close();
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }
            
        }

        private void AddItens()
        {
            try
            {
                //Utils.Logger.getInstance.error("MakeXML.AddItens");

                bool defineAcres = false;

                string cst = "";
                string[] cst_icms = new string[] { "00", "40", "41", "60" };
                decimal ttlBaseCalculo = 0;
                decimal ttlICMS = 0;
                int countItem = 0;

                //calculando vOutro (Acrescimo)
                decimal Acresc = Convert.ToDecimal(dtNfce.Acresc, ci);

                decimal vOutroRateioItem = 0;
                

                if (Acresc > 0)
                {
                    defineAcres = true;
                                       
                    vOutroRateioItem = Acresc / dtNfce.itensList.Count;

                }                

               // Utils.Logger.getInstance.error("MakeXML.AddItens -> Lista Itens: " + dtNfce.itensList.Count );               

                vOutroRateioTtl = 0;

                foreach (var item in dtNfce.itensList)
                {
                    //verifianco CST para definir CFOP
                    decimal aliq = 0;

                    if (Decimal.TryParse(item.Tipo_Aliquota, out aliq))
                        cst = cst_icms[0].ToString();
                    else if (item.Tipo_Aliquota.Contains("I"))
                    {
                        cst = cst_icms[1].ToString();
                        item.Valor_Aliquota = "0";
                    }
                    else if (item.Tipo_Aliquota.Contains("N"))
                    {
                        cst = cst_icms[1].ToString();
                        item.Valor_Aliquota = "0";
                    }
                    else if (item.Tipo_Aliquota.Contains("F"))
                    {
                        cst = cst_icms[3].ToString();
                        item.Valor_Aliquota = "0";
                    }

                    item.CST = cst;

                    countItem++;
                    item.Posicao = countItem.ToString();
                    xmlWriter.WriteStartElement("det");
                        xmlWriter.WriteAttributeString("nItem", countItem.ToString());

                        xmlWriter.WriteStartElement("prod");
                            xmlWriter.WriteElementString("cProd", item.C_Prod);
                            xmlWriter.WriteElementString("cEAN", "");
                            xmlWriter.WriteElementString("xProd", item.X_Prod.TrimEnd());
                            xmlWriter.WriteElementString("NCM", item.NCM);
                            
                            if(item.CST.Equals("00"))
                                xmlWriter.WriteElementString("CFOP", config.configNFCe.CFOP00);                            
                            else 
                                xmlWriter.WriteElementString("CFOP", config.configNFCe.CFOP60);

                            xmlWriter.WriteElementString("uCom", String.Format(ci, "{0:F2}", item.U_Com));
                            xmlWriter.WriteElementString("qCom", String.Format(ci, "{0:F2}", item.Q_Com));
                            xmlWriter.WriteElementString("vUnCom", String.Format(ci, "{0:F2}", item.V_UnCom));
                            xmlWriter.WriteElementString("vProd", String.Format(ci, "{0:F2}", item.V_Prod));
                            xmlWriter.WriteElementString("cEANTrib", "");
                            xmlWriter.WriteElementString("uTrib", "UN");
                            xmlWriter.WriteElementString("qTrib", String.Format(ci, "{0:F2}", item.Q_Com));
                            xmlWriter.WriteElementString("vUnTrib", String.Format(ci, "{0:F2}", item.V_UnCom));
                            
                            if(item.V_Desc > 0)
                                xmlWriter.WriteElementString("vDesc", String.Format(ci, "{0:F2}", item.V_Desc));

                            ttlDesc += item.V_Desc;

                            Utils.Logger.getInstance.error(String.Format("defineAcres: {0}", defineAcres));

                            if(defineAcres)
                            {
                                //vOutroTtl += Decimal.Round(vOutroTemp,2);

                                string strVOutroRateioTtl = vOutroRateioItem.ToString("F2");

                                vOutroRateioItem += Convert.ToDecimal(strVOutroRateioTtl);

                                vOutroRateioTtl += vOutroRateioItem;

                                //Utils.Logger.getInstance.error(String.Format("vOutroRateioItem: {0}", vOutroRateioItem));
                                //Utils.Logger.getInstance.error(String.Format("vOutroRateioTtl: {0} | Acresc: {1}", vOutroRateioTtl, Acresc));

                                if (vOutroRateioTtl > Acresc)
                                {
                                    vOutroRateioItem = vOutroRateioItem - (vOutroRateioTtl - Acresc);
                                    vOutroRateioTtl = vOutroRateioTtl - (vOutroRateioTtl - Acresc);

                                    //Utils.Logger.getInstance.error(String.Format("vOutroRateioItem: {0} | vOutroRateioTtl: {1}", vOutroRateioItem, vOutroRateioTtl));
                                }   

                                if ((Acresc > vOutroRateioTtl) && (dtNfce.itensList.Count == (countItem-1))) //acresc > ttlRateio e está no último item
                                {
                                    vOutroRateioItem = vOutroRateioItem + (Acresc - vOutroRateioTtl);
                                    vOutroRateioTtl = vOutroRateioTtl + (Acresc - vOutroRateioTtl);

                                    //Utils.Logger.getInstance.error(String.Format("vOutroRateioItem: {0} | vOutroRateioTtl: {1}", vOutroRateioItem, vOutroRateioTtl));
                                }

                                if (vOutroRateioItem > 0)
                                    xmlWriter.WriteElementString("vOutro", String.Format(ci, "{0:F2}", vOutroRateioItem));
                            }

                            xmlWriter.WriteElementString("indTot", "1");
                        xmlWriter.WriteEndElement();

                        

                        //Utils.Logger.getInstance.error(String.Format("MakeXML.AddItens -> CST/ALIQ: {0} / {1}", item.CST, item.Valor_Aliquota));

                    xmlWriter.WriteStartElement("imposto");
                        xmlWriter.WriteStartElement("ICMS");
                            if (cst.Equals("00"))
                            {
                                decimal vICMS = ((item.V_UnCom * item.Q_Com) - item.V_Desc) * (Decimal.Parse(item.Valor_Aliquota,ci) / 100);

                                //ttlICMS += vICMS;

                            xmlWriter.WriteStartElement("ICMS00");
                                xmlWriter.WriteElementString("orig", "0");
                                xmlWriter.WriteElementString("CST", "00");
                                xmlWriter.WriteElementString("modBC", "3");
                                xmlWriter.WriteElementString("vBC", String.Format(ci, "{0:F2}", (item.V_UnCom * item.Q_Com) - item.V_Desc));
                                
                                decimal tmpvalorAliq = Convert.ToDecimal(item.Valor_Aliquota,ci);
                                
                                string pIcms = tmpvalorAliq.ToString("F2", ci);
                                xmlWriter.WriteElementString("pICMS", String.Format(ci, "{0:F2}", pIcms));
                                string tmp = vICMS.ToString("F2", ci);

                                ttlICMS += Convert.ToDecimal(tmp, ci);
                                
                                xmlWriter.WriteElementString("vICMS", String.Format(ci, "{0:F2}", tmp));
                            xmlWriter.WriteEndElement(); //ICMS00

                            ttlBaseCalculo += (item.V_UnCom * item.Q_Com) - item.V_Desc;
                            }
                                else if (cst.Equals("40"))
                            {
                                xmlWriter.WriteStartElement("ICMS40");
                                    xmlWriter.WriteElementString("orig", "0");
                                    xmlWriter.WriteElementString("CST", "41");
                                xmlWriter.WriteEndElement(); //ICMS40
                            }
                            else if (cst.Equals("60"))
                            {
                                decimal vICMS = ((item.V_UnCom * item.Q_Com) - item.V_Desc) * (Decimal.Parse(item.Valor_Aliquota) / 100);

                                xmlWriter.WriteStartElement("ICMS60");
                                    xmlWriter.WriteElementString("orig", "0");
                                    xmlWriter.WriteElementString("CST", cst);
                                    xmlWriter.WriteElementString("vBCSTRet", String.Format(ci, "{0:F2}", (item.V_UnCom * item.Q_Com) - item.V_Desc));
                                    xmlWriter.WriteElementString("vICMSSTRet", String.Format(ci, "{0:F2}", vICMS));
                                xmlWriter.WriteEndElement();//ICMS60
                            }
                        xmlWriter.WriteEndElement();//ICMS

                        xmlWriter.WriteStartElement("PIS");
                            xmlWriter.WriteStartElement("PISAliq");
                                xmlWriter.WriteElementString("CST", "01");
                                xmlWriter.WriteElementString("vBC", String.Format(ci, "{0:F2}", 0));
                                xmlWriter.WriteElementString("pPIS", String.Format(ci, "{0:F2}", 0));
                                xmlWriter.WriteElementString("vPIS", String.Format(ci, "{0:F2}", 0));
                            xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("COFINS");
                            xmlWriter.WriteStartElement("COFINSAliq");
                                xmlWriter.WriteElementString("CST", "01");
                                xmlWriter.WriteElementString("vBC", String.Format(ci, "{0:F2}", 0));
                                xmlWriter.WriteElementString("pCOFINS", String.Format(ci, "{0:F2}", 0));
                                xmlWriter.WriteElementString("vCOFINS", String.Format(ci, "{0:F2}", 0));
                            xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();
                    
                    xmlWriter.WriteEndElement();//imposto                   
                xmlWriter.WriteEndElement();//det


                    ttlProdutos += (item.V_UnCom * item.Q_Com);

                    //total
                   
                }
                xmlWriter.WriteStartElement("total");
                    xmlWriter.WriteStartElement("ICMSTot");
                        xmlWriter.WriteElementString("vBC", String.Format(ci, "{0:F2}", ttlBaseCalculo));
                        xmlWriter.WriteElementString("vICMS", String.Format(ci, "{0:F2}", ttlICMS));
                        if (config.configNFCe.VersaoLayout.Equals("3.10"))
                            xmlWriter.WriteElementString("vICMSDeson", String.Format(ci, "{0:F2}", 0));
                        xmlWriter.WriteElementString("vBCST", String.Format(ci, "{0:F2}", 0));
                        xmlWriter.WriteElementString("vST", String.Format(ci, "{0:F2}", 0));
                        xmlWriter.WriteElementString("vProd", String.Format(ci, "{0:F2}", ttlProdutos));
                        xmlWriter.WriteElementString("vFrete", String.Format(ci, "{0:F2}", 0));
                        xmlWriter.WriteElementString("vSeg", String.Format(ci, "{0:F2}", 0));
                        
                        xmlWriter.WriteElementString("vDesc", String.Format(ci, "{0:F2}", ttlDesc));
                
                        xmlWriter.WriteElementString("vII", String.Format(ci, "{0:F2}", 0));
                        xmlWriter.WriteElementString("vIPI", String.Format(ci, "{0:F2}", 0));
                        xmlWriter.WriteElementString("vPIS", String.Format(ci, "{0:F2}", 0));
                        xmlWriter.WriteElementString("vCOFINS", String.Format(ci, "{0:F2}", 0));
                        xmlWriter.WriteElementString("vOutro", String.Format(ci, "{0:F2}", vOutroRateioTtl));
                        xmlWriter.WriteElementString("vNF", String.Format(ci, "{0:F2}", ttlProdutos - ttlDesc + vOutroRateioTtl));
                    xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            catch(Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }
        }

        private string GerarChaveDeAcesso(int nNF, DateTime _dt)
        {
            try
            {
                string tpEmis;
                if (config.configNFCe.Contingencia)
                    tpEmis = "9";
                else
                    tpEmis = config.configNFCe.TpEmis;

                string cUF = config.configEmitente.endereco.Cod_estado;
                string dt = _dt.ToString("yy-MM-dd");
                string ano = dt.Substring(0, 2);
                string mes = _dt.Month.ToString().PadLeft(2, '0');
                string cnpj = config.configEmitente.Cnpj;
                string modDocFiscal = "65";
                string serieDocFiscal = config.configNFCe.Serie.PadLeft(3, '0'); //"001";
                string numDocFiscal = nNF.ToString().PadLeft(9, '0');
                // string tpEmis = config.configNFCe.TpEmis;
                cNF = nNF.ToString().PadLeft(8, '0');

                string chaveAcessoParcial = cUF + ano + mes + cnpj + modDocFiscal + serieDocFiscal + numDocFiscal + tpEmis + cNF;

                string digitoVerificador = CalcularDigitoVerificador(chaveAcessoParcial);

                
                return chaveAcessoParcial + digitoVerificador;
            }
            catch (ApplicationException appx)
            {
                Utils.Logger.getInstance.error(appx);
                throw new Exception(appx.ToString());
            }
            catch (Exception ex)
            {
                Utils.Logger.getInstance.error(ex);
                throw new Exception(ex.ToString());
            }
        }
        private string CalcularDigitoVerificador(string chaveAcessoParcial)
        {
            try
            {
                int soma = 0;
                int resto = 0;
                int[] peso = { 4, 3, 2, 9, 8, 7, 6, 5 };
                int digitoRetorno;

                for (int i = 0; i < chaveAcessoParcial.Length; i++)
                    soma += peso[i % 8] * (int.Parse(chaveAcessoParcial.Substring(i, 1)));

                resto = soma % 11;
                if (resto == 0 || resto == 1)
                    digitoRetorno = 0;
                else
                    digitoRetorno = 11 - resto;
                
                digitoVerificador = digitoRetorno.ToString();
                
                return digitoVerificador;

            }
            catch (ApplicationException appx)
            {
                Utils.Logger.getInstance.error(appx);
                throw new Exception(appx.ToString());
            }
            catch (Exception ex)
            {
                Utils.Logger.getInstance.error(ex);
                throw new Exception(ex.ToString());
            }
        }

        private void gerarSequenceNumberNfce()
        {
            try
            {                
                
               string nfceSeq = ManagerDB.Instance.SelectMaxNFCE(Convert.ToInt32(config.configNFCe.Serie));

                config.configNFCe.sequenceNumberNfce = Convert.ToString(Convert.ToInt32(nfceSeq) + 1);

               Utils.ReadConfigure rConfig = new Utils.ReadConfigure();
                
                rConfig.SaveXML(config);

                chaveAcesso = GerarChaveDeAcesso(Convert.ToInt32(config.configNFCe.sequenceNumberNfce), DateTime.Now);


                ManagerDB.Instance.InsertNfceInitial(config.configNFCe.sequenceNumberNfce, config.configNFCe.Serie, dtNfce.Chk_Num, dtNfce.WS_ID, chaveAcesso);


            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }
        }

        private void assinaturaXML()
        {
            try
            {
                xmlDocAss = xmlDoc;
                
                
                if (cert == null)
                    throw new Exception("Nao foi encontrado o certificado: " + config.configNFCe.NomeCertificadoDigital);                                   

                Reference reference = new Reference();
                SignedXml docXML = new SignedXml(xmlDocAss);

                docXML.SigningKey = cert.PrivateKey;
                XmlAttributeCollection uri = xmlDocAss.GetElementsByTagName("infNFe").Item(0).Attributes;
                foreach (XmlAttribute atributo in uri)
                {
                    if (atributo.Name == "Id")
                        reference.Uri = "#" + atributo.InnerText;
                }
              
                XmlDsigEnvelopedSignatureTransform envelopedSigntature = new XmlDsigEnvelopedSignatureTransform();
                reference.AddTransform(envelopedSigntature);

                XmlDsigC14NTransform c14Transform = new XmlDsigC14NTransform();
                reference.AddTransform(c14Transform);
                docXML.AddReference(reference);

                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(new KeyInfoX509Data(cert));

                docXML.KeyInfo = keyInfo;
                docXML.ComputeSignature();

                XmlElement xmlDigitalSignature = docXML.GetXml();

                foreach (var _nfe in xmlDocAss.GetElementsByTagName("NFe").Cast<XmlElement>())
                    _nfe.AppendChild(xmlDocAss.ImportNode(xmlDigitalSignature, true));


                xmlDocAss.PreserveWhitespace = true;

            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }

           
        }
    }
}
