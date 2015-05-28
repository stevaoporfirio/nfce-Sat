using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace invoiceServerApp
{
    public enum TipoImpressao {VendaCompleto, VendaResumido, Cancelamento };

    class ProcessaSAT
    {
        

        private DadosSAT dtSAT;
        private Utils.ConfigureXml config;
        private MakeXMLSAT xmlData;

        private XmlDocument xmlSATImpressao;
        private XmlDocument xmlSATImpressaoCancelamento;

        private CallbackStatusInterface InterfaceStatus;

        private string dadosQR;

        private XmlTextWriter xmlWriter;        
        private StringWriter XmlString = new StringWriter();

        ImpressaoSAT dadosSatImp;

        public ProcessaSAT(CallbackStatusInterface callStatus, Utils.ConfigureXml _config)
        {
            try
            {

                InterfaceStatus = callStatus;
                config = _config;
                CreateDir();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ProcessaCancel(string printer, string chaveCancelamento,string tipoCli, string cpf_cnpj) //"F"|"J"
        {
            try
            {
                //xmlSATImpressaoCancelamento = new XmlDocument();

                string fileCancel = String.Format("{0}{1}\\CFe{2}.xml", config.configMaquina.pathFiles, "enviados", chaveCancelamento);
                
                if (File.Exists(fileCancel))
                {
                    xmlSATImpressaoCancelamento = new XmlDocument();
                    xmlSATImpressaoCancelamento.Load(fileCancel);
                }
                else
                {
                    throw new Exception("Arquivo XML CFe Original não encontrado em: " + fileCancel);
                }

                xmlWriter = new XmlTextWriter(XmlString);

                //cabeçalho
                xmlWriter.WriteStartElement("CFeCanc");
                xmlWriter.WriteStartElement("infCFe");
                xmlWriter.WriteAttributeString("chCanc", "CFe" + chaveCancelamento);
                xmlWriter.WriteStartElement("ide");
                xmlWriter.WriteElementString("CNPJ", config.configSAT.CNPJ_SoftwareHouse);
                xmlWriter.WriteElementString("signAC", config.configSAT.IDE_signAC);
                xmlWriter.WriteElementString("numeroCaixa", config.configSAT.IDE_NumeroCaixa);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("emit");
                xmlWriter.WriteEndElement();

                
                xmlWriter.WriteStartElement("dest");
                if(!(String.IsNullOrEmpty(cpf_cnpj)))
                {
                    if(tipoCli.Equals("F"))
                        xmlWriter.WriteElementString("CPF", cpf_cnpj);
                    else if(tipoCli.Equals("J"))
                        xmlWriter.WriteElementString("CNPJ", cpf_cnpj);
                }

                xmlWriter.WriteEndElement();
                
                


                xmlWriter.WriteStartElement("total");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();

                xmlWriter.Close();

                XmlDocument xmlCancel = new XmlDocument();
                xmlCancel.LoadXml(XmlString.ToString());

                string xmlName = String.Format("{0}{1}\\{2}.xml", config.configMaquina.pathFiles, "gerados", String.Format("Cancelamento_{0}_{1}_{2}", chaveCancelamento, DateTime.Now.ToString("ddMMyyyy"), DateTime.Now.ToString("hhmmss")));
                xmlCancel.Save(xmlName);
                
                string retorno;

                try
                {
                    retorno = SatDLL.CancelarCFe(SatDLL.generatorKey(), config.configSAT.ChaveAtivacao, "CFe" + chaveCancelamento, xmlCancel.OuterXml); //TODO CONF
                }
                catch (Exception exceptionSATDll)
                {
                    throw new Exception("Erro enviando comando de Cancelamento de CFe " + exceptionSATDll.Message);
                }

                string[] tmpSplit = retorno.Split('|');

                if (tmpSplit[1].Equals("07000"))
                {

                    byte[] tmpByte = Convert.FromBase64String(tmpSplit[6]);

                    String nota = tmpByte.ToString();

                    nota = System.Text.Encoding.UTF8.GetString(tmpByte);

                    xmlName = String.Format("{0}{1}\\{2}.xml", config.configMaquina.pathFiles, "enviados", tmpSplit[8]);

                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(xmlName))
                    {
                        sw.Write(nota);

                        sw.Flush();
                    }

                    xmlSATImpressao = new XmlDocument();
                    xmlSATImpressao.Load(xmlName);

                    dadosQR = String.Format("{0}|{1}|{2}|{3}|{4}", tmpSplit[8].Substring(3), tmpSplit[7], tmpSplit[9], tmpSplit[10], tmpSplit[11]);
                    //Impressao(TipoImpressao.Cancelamento, printer);

                    dadosSatImp = new ImpressaoSAT(xmlSATImpressao, xmlSATImpressaoCancelamento, config, TipoImpressao.Cancelamento);

                    ImprimirEpsonNF.ImprimirNF(printer, dadosSatImp.DadosImpressao1,dadosSatImp.DadosImpressao2, dadosQR,dadosSatImp.DadosQrCodeCancelamento, "", true, true);
                }
                else
                {
                    throw new Exception(String.Format("Erro processando SAT Cancelamento {0}{1}{2}{3}{4}", tmpSplit[1], tmpSplit[2], tmpSplit[3], tmpSplit[4], tmpSplit[5]));
                }


                return "SAT: Cancelamento Com sucesso";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro processabdo Cancelamento " + ex.Message);
            }

        }

        public void ProcessaCupom(DadosSAT _dtSAT)
        {
            try
            {
                dtSAT = _dtSAT;

                xmlData = new MakeXMLSAT(dtSAT, config);

                string xmlName = String.Format("{0}{1}\\{2}.xml", config.configMaquina.pathFiles, "\\gerados", String.Format("{0}_{1}_{2}_{3}", dtSAT.WS_ID, dtSAT.Chk_Num, DateTime.Now.ToString("ddMMyyyy"), DateTime.Now.ToString("hhmmss")));

                xmlData.xmlDoc.Save(xmlName);

                Processa();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void Processa()
        {
            try
            {
                string retorno = string.Empty;

                try
                {
                    retorno = SatDLL.EnviarDadosVendaBase(SatDLL.generatorKey(), config.configSAT.ChaveAtivacao, xmlData.xmlDoc.OuterXml); //TODO CONF
                }
                catch (Exception exceptionSATDll)
                {
                    throw new Exception("Erro enviando comando de Venda " + exceptionSATDll.Message);
                }

                string[] tmpSplit = retorno.Split('|');

                if (tmpSplit[1].Equals("06000"))
                {

                    byte[] tmpByte = Convert.FromBase64String(tmpSplit[6]);

                    String nota = tmpByte.ToString();

                    nota = System.Text.Encoding.UTF8.GetString(tmpByte);

                    string xmlName = String.Format("{0}{1}\\{2}.xml", config.configMaquina.pathFiles, "\\enviados", tmpSplit[8]);

                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(xmlName))
                    {
                        sw.Write(nota);

                        sw.Flush();
                    }

                    xmlSATImpressao = new XmlDocument();
                    xmlSATImpressao.Load(xmlName);

                    dadosQR = String.Format("{0}|{1}|{2}|{3}|{4}", tmpSplit[8].Substring(3), tmpSplit[7], tmpSplit[9], tmpSplit[10], tmpSplit[11]);
                    Impressao(TipoImpressao.VendaCompleto, dtSAT.PortaImpressora);
                }
                else
                {
                    throw new Exception(String.Format("Erro processando SAT {0}{1}{2}{3}{4}", tmpSplit[1], tmpSplit[2], tmpSplit[3], tmpSplit[4], tmpSplit[5]));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro em Processa SAT " + ex.Message);
            }
        }

        private void Impressao(TipoImpressao tipo, string printer)
        {
            try
            {
                //
                dadosSatImp = new ImpressaoSAT(xmlSATImpressao, xmlSATImpressaoCancelamento, config, tipo);

                ImprimirEpsonNF.ImprimirNF(printer, dadosSatImp.DadosImpressao1,null,  dadosQR, "", dtSAT.TefNfce.StringTEF, true, true);

            }catch(Exception ex)
            {
                throw new Exception("Erro Imprimindo SAT FCe " + ex.Message);
            }
        }
        private void CreateDir()
        {
            try
            {
                string dir = String.Format("{0}", config.configMaquina.pathFiles + "\\gerados");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                string dir2 = String.Format("{0}", config.configMaquina.pathFiles + "\\enviados");
                if (!Directory.Exists(dir2))
                    Directory.CreateDirectory(dir2);


            }
             catch(Exception ex)
            {
                throw new Exception("Erro na criação de diretorio: " + ex.Message);
            }
        }
        

        public string GetNota()
        {
            //return cupom.DadosImpressao;
            return "";
        }
    }
}
