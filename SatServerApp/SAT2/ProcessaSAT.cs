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
        

        private DadosSAT dtSAT = new DadosSAT();
        private Utils.ConfigureXml config;
        private MakeXMLSAT xmlData;
        private CallbackStatusInterface InterfaceStatus;
        private string dadosQR;
        private XmlTextWriter xmlWriter;        
        private StringWriter XmlString = new StringWriter();
        private ImpressaoSAT dadosSatImp;
        private gerenciadoSAT gerenSAT;
        

        public ProcessaSAT(CallbackStatusInterface callStatus, Utils.ConfigureXml _config)
        {
            try
            {

                InterfaceStatus = callStatus;
                config = _config;
                CreateDir();
                gerenSAT = new gerenciadoSAT(config);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ProcessaCancel(CancelNFCE cancel) //"F"|"J"
        {
            try
            {
                xmlData = new MakeXMLSAT(cancel, config);
                XmlDocument xmlSATImpressaoCancelamento = new XmlDocument();
                string fileCancel = String.Format("{0}{1}\\CFe{2}.xml", config.configMaquina.pathFiles, "\\enviados", cancel.chaveCancelamento);
                if (File.Exists(fileCancel))
                {   
                    xmlSATImpressaoCancelamento.Load(fileCancel);
                }
                else
                {
                    throw new Exception("Arquivo XML CFe Original não encontrado em: " + fileCancel);
                }

                string retorno;

                try
                {
                    retorno = gerenSAT.CancelarCFe(gerenSAT.generatorKey(), config.configSAT.ChaveAtivacao, "CFe" + cancel.chaveCancelamento, xmlData.xmlCancel.OuterXml); //TODO CONF
                    //retorno = SatDLL.CancelarCFe(SatDLL.generatorKey(), config.configSAT.ChaveAtivacao, "CFe" + cancel.chaveCancelamento, xmlData.xmlCancel.OuterXml); //TODO CONF
                }
                catch (Exception exceptionSATDll)
                {
                    throw new Exception("Erro enviando comando de Cancelamento de CFe " + exceptionSATDll.Message);
                }

                string[] tmpSplit = retorno.Split('|');

                if (tmpSplit[1].Equals("07000"))
                {

                    byte[] tmpByte = Convert.FromBase64String(tmpSplit[6]);

                    String notaRet = tmpByte.ToString();

                    notaRet = System.Text.Encoding.UTF8.GetString(tmpByte);

                    string xmlName = String.Format("{0}{1}\\{2}.xml", config.configMaquina.pathFiles, "\\enviados", tmpSplit[8]);

                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(xmlName))
                    {
                        sw.Write(notaRet);

                        sw.Flush();
                    }

                    XmlDocument xmlSATImpressao = new XmlDocument();
                    xmlSATImpressao.LoadXml(notaRet);

                    dadosQR = String.Format("{0}|{1}|{2}|{3}|{4}", tmpSplit[8].Substring(3), tmpSplit[7], tmpSplit[9], tmpSplit[10], tmpSplit[11]);
                    Impressao(xmlSATImpressao, xmlSATImpressaoCancelamento, TipoImpressao.Cancelamento, cancel.printer);

                   // dadosSatImp = new ImpressaoSAT(xmlSATImpressao, xmlSATImpressaoCancelamento, config, TipoImpressao.Cancelamento);

                   // ImprimirEpsonNF.ImprimirNF(cancel.printer, dadosSatImp.DadosImpressao1, dadosSatImp.DadosImpressao2, dadosQR, dadosSatImp.DadosQrCodeCancelamento, "", true, true);
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
                    try
                    {
                        string ret = gerenSAT.DesbloquearSATBase(gerenSAT.generatorKey(), config.configSAT.ChaveAtivacao);
                        Utils.Logger.getInstance.error("desbloqueio sat: " + ret);
                    }
                    catch (Exception exceptionSATDll)
                    {
                        throw new Exception("#Erro enviando comando de Desbloqueio " + exceptionSATDll.Message + "#");
                    }

                   // retorno = SatDLL.EnviarDadosVendaBase(SatDLL.generatorKey(), config.configSAT.ChaveAtivacao, xmlData.xmlDoc.OuterXml); //TODO CONF
                    retorno = gerenSAT.EnviarDadosVendaBase(gerenSAT.generatorKey(), config.configSAT.ChaveAtivacao, xmlData.xmlDoc.OuterXml); //TODO CONF
                }
                catch (Exception exceptionSATDll)
                {
                    throw new Exception("Erro enviando comando de Venda " + exceptionSATDll.Message);
                }

                string[] tmpSplit = retorno.Split('|');
                if (tmpSplit.Length <= 1)
                    throw new Exception("#" + retorno + "#");
                
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

                    XmlDocument xmlSATImpressao = new XmlDocument();
                    xmlSATImpressao.Load(xmlName);

                    dadosQR = String.Format("{0}|{1}|{2}|{3}|{4}", tmpSplit[8].Substring(3), tmpSplit[7], tmpSplit[9], tmpSplit[10], tmpSplit[11]);
                    Impressao(xmlSATImpressao, null, TipoImpressao.VendaCompleto, dtSAT.PortaImpressora);
                }
                else
                {
                    throw new Exception(String.Format("# Erro processando Venda SAT {0} #", retorno));
                   // throw new Exception(String.Format("Erro processando SAT {0}{1}{2}{3}{4}", tmpSplit[1], tmpSplit[2], tmpSplit[3], tmpSplit[4], tmpSplit[5]));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro em Processa SAT " + ex.Message);
            }
        }

        private void Impressao(XmlDocument _xmlImprensao, XmlDocument _xmlImprensaoCancel,  TipoImpressao tipo, string printer)
        {
            try
            {
                //
                dadosSatImp = new ImpressaoSAT(_xmlImprensao, _xmlImprensaoCancel, config, tipo);                           

                ImprimirEpsonNF.ImprimirNF(printer, dadosSatImp.DadosImpressao1, dadosSatImp.DadosImpressao2, dadosQR, dadosSatImp.DadosQrCodeCancelamento, dtSAT.TefNfce.StringTEF, true, true);

            }catch(Exception ex)
            {
                throw new Exception("Erro Imprimindo SAT Cfe " + ex.Message);
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
        public string ReImpressaoDanfe(dtImprensao _dtImprensao)
        {
            XmlDocument xmlImpressao = new XmlDocument();
            string fileCancel = String.Format("{0}{1}\\CFe{2}.xml", config.configMaquina.pathFiles, "\\enviados", _dtImprensao.chaveImpressao);
            if (File.Exists(fileCancel))
            {
                xmlImpressao.Load(fileCancel);
            }
            else
            {
                throw new Exception("# Arquivo XML CFe Original não encontrado em: " + fileCancel+"#");
            }

            dadosQR = "";

            Impressao(xmlImpressao, null, TipoImpressao.VendaCompleto, _dtImprensao.portaImpressora);
            
            return "";

        }

        public string GetNota()
        {
            return "";
        }
    }
}
