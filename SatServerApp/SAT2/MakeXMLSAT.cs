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
    class MakeXMLSAT
    {
        private DadosSAT dtSAT;
        private CancelNFCE cancelSAT;
        private Utils.ConfigureXml config;

        private XmlTextWriter xmlWriter;
        public XmlDocument xmlDoc = new XmlDocument();
        public XmlDocument xmlDocAss = new XmlDocument();
        private StringWriter XmlString = new StringWriter();
        public XmlDocument xmlCancel = new XmlDocument();
                
        private CultureInfo ci = new CultureInfo("en-US");

        private decimal ttlProdutos = 0;
        private decimal ttlDesc = 0;
        private decimal vOutroRateioTtl = 0;


        public MakeXMLSAT(DadosSAT _dtSAT, Utils.ConfigureXml _config)
        {
            config = _config;
            dtSAT = _dtSAT;
            
            processXml();
        }
        public MakeXMLSAT(CancelNFCE _cancelSAT, Utils.ConfigureXml _config)
        {
            config = _config;



            processCancel(_cancelSAT);
        }

        public void processXml()
        {
            try
            {
                parseNfce();

                xmlDoc.LoadXml(XmlString.ToString());                
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
                xmlWriter.WriteStartElement("CFe");

                    xmlWriter.WriteStartElement("infCFe");
                    xmlWriter.WriteAttributeString("versaoDadosEnt", "0.06");
                        //IDE
                        xmlWriter.WriteStartElement("ide");
                            xmlWriter.WriteElementString("CNPJ", config.configSAT.CNPJ_SoftwareHouse);
                            xmlWriter.WriteElementString("signAC", config.configSAT.IDE_signAC);
                            xmlWriter.WriteElementString("numeroCaixa", config.configSAT.IDE_NumeroCaixa);                            
                        xmlWriter.WriteEndElement();

                        //EMIT
                        xmlWriter.WriteStartElement("emit");
                            xmlWriter.WriteElementString("CNPJ", config.configEmitente.Cnpj);
                            xmlWriter.WriteElementString("IE", config.configEmitente.Ie);
                            //xmlWriter.WriteElementString("IM", "");
                            xmlWriter.WriteElementString("indRatISSQN","N");                        
                        xmlWriter.WriteEndElement();

                        //Dest
                        xmlWriter.WriteStartElement("dest");
                        if (!String.IsNullOrEmpty(dtSAT.CPF_CNPJ_dest))
                        {
                            if (dtSAT.JuridicaFisica_dest.Equals("F"))
                                xmlWriter.WriteElementString("CPF", dtSAT.CPF_CNPJ_dest);
                            else
                                xmlWriter.WriteElementString("CNPJ", dtSAT.CPF_CNPJ_dest);                                                     
                        }

                        xmlWriter.WriteEndElement();

                        //Itens e total
                        AddItens();

                        //pagamento
                        xmlWriter.WriteStartElement("pgto");
                        foreach(PgtSAT pg in dtSAT.pgtsList)                        
                        {                            
                            xmlWriter.WriteStartElement("MP");
                                xmlWriter.WriteElementString("cMP", pg.Cod); //dtSAT.pgtsList[0].Cod);

                                xmlWriter.WriteElementString("vMP", pg.Val); //(ttlProdutos - ttlDesc + vOutroRateioTtl).ToString("F2", ci));
                            xmlWriter.WriteEndElement();                            
                        }
                        xmlWriter.WriteEndElement();

                        //Outras Info
                        {
                            if (dtSAT.InfoAdic != null)
                            {
                                xmlWriter.WriteStartElement("infAdic");

                                //concatenando
                                String tmp = "";
                                foreach (string s in dtSAT.InfoAdic)
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
                Utils.Logger.getInstance.error("MakeXML.AddItens");

                bool defineAcres = false;

                string cst = "";
                string[] cst_icms = new string[] { "00", "40", "41", "60" };
                decimal ttlBaseCalculo = 0;
                decimal ttlICMS = 0;
                int countItem = 0;

                //calculando vOutro (Acrescimo)
                decimal Acresc = Convert.ToDecimal(dtSAT.Acresc, ci);

                decimal vOutroRateioItem = 0;


                if (Acresc > 0)
                {
                    defineAcres = true;

                    vOutroRateioItem = Acresc / dtSAT.itensList.Count;

                }

                Utils.Logger.getInstance.error("MakeXML.AddItens -> Lista Itens: " + dtSAT.itensList.Count);

                vOutroRateioTtl = 0;

                foreach (var item in dtSAT.itensList)
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
                            xmlWriter.WriteElementString("xProd", item.X_Prod.TrimEnd());                            
                            xmlWriter.WriteElementString("NCM", item.NCM);

                            if (item.CST.Equals("00"))
                                xmlWriter.WriteElementString("CFOP", config.configSAT.CFOP00);
                            else if (item.CST.Equals("40"))
                                xmlWriter.WriteElementString("CFOP", config.configSAT.CFOP60);
                            else if (item.CST.Equals("60"))
                                xmlWriter.WriteElementString("CFOP", config.configSAT.CFOP60);

                                xmlWriter.WriteElementString("uCom", String.Format(ci, "{0:F2}", item.U_Com));
                                xmlWriter.WriteElementString("qCom", String.Format(ci, "{0:0.0000}", item.Q_Com));
                                xmlWriter.WriteElementString("vUnCom", String.Format(ci, "{0:F2}", item.V_UnCom));

                                xmlWriter.WriteElementString("indRegra", "A");
                                
                                if (item.V_Desc > 0)
                                    xmlWriter.WriteElementString("vDesc", String.Format(ci, "{0:F2}", item.V_Desc));

                                ttlDesc += item.V_Desc;

                                Utils.Logger.getInstance.error(String.Format("defineAcres: {0}", defineAcres));

                                if (defineAcres)
                                {
                                    //vOutroTtl += Decimal.Round(vOutroTemp,2);

                                    string strVOutroRateioTtl = vOutroRateioItem.ToString("F2");

                                    vOutroRateioItem += Convert.ToDecimal(strVOutroRateioTtl);

                                    vOutroRateioTtl += vOutroRateioItem;

                                    Utils.Logger.getInstance.error(String.Format("vOutroRateioItem: {0}", vOutroRateioItem));
                                    Utils.Logger.getInstance.error(String.Format("vOutroRateioTtl: {0} | Acresc: {1}", vOutroRateioTtl, Acresc));

                                    if (vOutroRateioTtl > Acresc)
                                    {
                                        vOutroRateioItem = vOutroRateioItem - (vOutroRateioTtl - Acresc);
                                        vOutroRateioTtl = vOutroRateioTtl - (vOutroRateioTtl - Acresc);

                                        Utils.Logger.getInstance.error(String.Format("vOutroRateioItem: {0} | vOutroRateioTtl: {1}", vOutroRateioItem, vOutroRateioTtl));
                                    }

                                    if ((Acresc > vOutroRateioTtl) && (dtSAT.itensList.Count == (countItem - 1))) //acresc > ttlRateio e está no último item
                                    {
                                        vOutroRateioItem = vOutroRateioItem + (Acresc - vOutroRateioTtl);
                                        vOutroRateioTtl = vOutroRateioTtl + (Acresc - vOutroRateioTtl);

                                        Utils.Logger.getInstance.error(String.Format("vOutroRateioItem: {0} | vOutroRateioTtl: {1}", vOutroRateioItem, vOutroRateioTtl));
                                    }

                                    if (vOutroRateioItem > 0)
                                        xmlWriter.WriteElementString("vOutro", String.Format(ci, "{0:F2}", vOutroRateioItem));
                                }
                                                    
                                xmlWriter.WriteEndElement();

                                Utils.Logger.getInstance.error(String.Format("MakeXML.AddItens -> CST/ALIQ: {0} / {1}", item.CST, item.Valor_Aliquota));

                                xmlWriter.WriteStartElement("imposto");
                                //xmlWriter.WriteElementString("vItem12741", "1.00"); //REVER
                                    xmlWriter.WriteStartElement("ICMS");
                                    if (cst.Equals("00"))
                                    {
                                        decimal vICMS = ((item.V_UnCom * item.Q_Com) - item.V_Desc) * (Decimal.Parse(item.Valor_Aliquota, ci) / 100);

                                        //ttlICMS += vICMS;

                                        xmlWriter.WriteStartElement("ICMS00");
                                        xmlWriter.WriteElementString("Orig", "0");
                                        xmlWriter.WriteElementString("CST", "00");
                                        //xmlWriter.WriteElementString("modBC", "3");
                                        //xmlWriter.WriteElementString("vBC", String.Format(ci, "{0:F2}", (item.V_UnCom * item.Q_Com) - item.V_Desc));

                                        decimal tmpvalorAliq = Convert.ToDecimal(item.Valor_Aliquota, ci);

                                        string pIcms = tmpvalorAliq.ToString("F2", ci);
                                        
                                        xmlWriter.WriteElementString("pICMS", String.Format(ci, "{0:F2}", pIcms));
                                        
                                        string tmp = vICMS.ToString("F2", ci);

                                        ttlICMS += Convert.ToDecimal(tmp, ci);

                                        //xmlWriter.WriteElementString("vICMS", String.Format(ci, "{0:F2}", tmp));
                                        xmlWriter.WriteEndElement(); //ICMS00

                                        ttlBaseCalculo += (item.V_UnCom * item.Q_Com) - item.V_Desc;
                                    }
                                    else if (cst.Equals("40"))
                                    {
                                        xmlWriter.WriteStartElement("ICMS40");
                                        xmlWriter.WriteElementString("Orig", "0");
                                        xmlWriter.WriteElementString("CST", "41");
                                        xmlWriter.WriteEndElement(); //ICMS40
                                    }
                                    else if (cst.Equals("60"))
                                    {
                                        decimal vICMS = ((item.V_UnCom * item.Q_Com) - item.V_Desc) * (Decimal.Parse(item.Valor_Aliquota) / 100);

                                        xmlWriter.WriteStartElement("ICMS40");
                                        xmlWriter.WriteElementString("Orig", "0");
                                        xmlWriter.WriteElementString("CST", "60");
                                        //xmlWriter.WriteElementString("vBCSTRet", String.Format(ci, "{0:F2}", (item.V_UnCom * item.Q_Com) - item.V_Desc));
                                        //xmlWriter.WriteElementString("vICMSSTRet", String.Format(ci, "{0:F2}", vICMS));
                                        xmlWriter.WriteEndElement();//ICMS60
                                    }
                                xmlWriter.WriteEndElement();//ICMS

                                xmlWriter.WriteStartElement("PIS");
                                    xmlWriter.WriteStartElement("PISNT");
                                        xmlWriter.WriteElementString("CST", "04");                                        
                                    xmlWriter.WriteEndElement();
                                xmlWriter.WriteEndElement();

                                xmlWriter.WriteStartElement("COFINS");
                                    xmlWriter.WriteStartElement("COFINSNT");
                                        xmlWriter.WriteElementString("CST", "04");                                
                                    xmlWriter.WriteEndElement();
                                xmlWriter.WriteEndElement();

                                xmlWriter.WriteEndElement();//imposto                   
                                xmlWriter.WriteEndElement();//det


                    ttlProdutos += (item.V_UnCom * item.Q_Com);

                    //total

                }
                xmlWriter.WriteStartElement("total");
                //xmlWriter.WriteStartElement("ICMSTot");
                //xmlWriter.WriteElementString("vBC", String.Format(ci, "{0:F2}", ttlBaseCalculo));
                //xmlWriter.WriteElementString("vICMS", String.Format(ci, "{0:F2}", ttlICMS));
                //if (config.configNFCe.VersaoLayout.Equals("3.10"))
                //    xmlWriter.WriteElementString("vICMSDeson", String.Format(ci, "{0:F2}", 0));
                //xmlWriter.WriteElementString("vBCST", String.Format(ci, "{0:F2}", 0));
                //xmlWriter.WriteElementString("vST", String.Format(ci, "{0:F2}", 0));
                //xmlWriter.WriteElementString("vProd", String.Format(ci, "{0:F2}", ttlProdutos));
                //xmlWriter.WriteElementString("vFrete", String.Format(ci, "{0:F2}", 0));
                //xmlWriter.WriteElementString("vSeg", String.Format(ci, "{0:F2}", 0));

                //xmlWriter.WriteElementString("vDesc", String.Format(ci, "{0:F2}", ttlDesc));

                //xmlWriter.WriteElementString("vII", String.Format(ci, "{0:F2}", 0));
                //xmlWriter.WriteElementString("vIPI", String.Format(ci, "{0:F2}", 0));
                //xmlWriter.WriteElementString("vPIS", String.Format(ci, "{0:F2}", 0));
                //xmlWriter.WriteElementString("vCOFINS", String.Format(ci, "{0:F2}", 0));
                //xmlWriter.WriteElementString("vOutro", String.Format(ci, "{0:F2}", vOutroRateioTtl));
                //xmlWriter.WriteElementString("vNF", String.Format(ci, "{0:F2}", ttlProdutos - ttlDesc + vOutroRateioTtl));
                //xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }
        }
        public void processCancel(CancelNFCE _cancelSAT)
        {
            try
            {
                //xmlSATImpressaoCancelamento = new XmlDocument();


                //string fileCancel = String.Format("{0}{1}\\CFe{2}.xml", config.configMaquina.pathFiles, "\\enviados", _cancelSAT.chaveCancelamento);

                //if (File.Exists(fileCancel))
                //{   
                //    this.xmlCancel.Load(fileCancel);
                //}
                //else
                //{
                //    throw new Exception("Arquivo XML CFe Original não encontrado em: " + fileCancel);
                //}

                xmlWriter = new XmlTextWriter(XmlString);

                //cabeçalho
                xmlWriter.WriteStartElement("CFeCanc");
                xmlWriter.WriteStartElement("infCFe");
                xmlWriter.WriteAttributeString("chCanc", "CFe" + _cancelSAT.chaveCancelamento);
                xmlWriter.WriteStartElement("ide");
                xmlWriter.WriteElementString("CNPJ", config.configSAT.CNPJ_SoftwareHouse);
                xmlWriter.WriteElementString("signAC", config.configSAT.IDE_signAC);
                xmlWriter.WriteElementString("numeroCaixa", config.configSAT.IDE_NumeroCaixa);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("emit");
                xmlWriter.WriteEndElement();


                xmlWriter.WriteStartElement("dest");
                if (!(String.IsNullOrEmpty(_cancelSAT.cpf_cnpj)))
                {
                    if (_cancelSAT.tipoCli.Equals("F"))
                        xmlWriter.WriteElementString("CPF", _cancelSAT.cpf_cnpj);
                    else if (_cancelSAT.tipoCli.Equals("J"))
                        xmlWriter.WriteElementString("CNPJ", _cancelSAT.cpf_cnpj);
                }

                xmlWriter.WriteEndElement();




                xmlWriter.WriteStartElement("total");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();

                xmlWriter.Close();

               //XmlDocument xmlCancel = new XmlDocument();
                xmlCancel.LoadXml(XmlString.ToString());

                string xmlName = String.Format("{0}{1}\\{2}.xml", config.configMaquina.pathFiles, "\\gerados", String.Format("Cancelamento_{0}_{1}_{2}", _cancelSAT.chaveCancelamento, DateTime.Now.ToString("ddMMyyyy"), DateTime.Now.ToString("hhmmss")));
                xmlCancel.Save(xmlName);

            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
            }
        }

    }
}
