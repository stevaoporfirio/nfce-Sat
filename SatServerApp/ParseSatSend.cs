using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace SatServerApp
{
    internal class HeaderCliSat
    {
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public string End { get; set; }
        public string Num { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Doc { get; set; }
    }

    internal class HeaderCHKSat
    {
        public string NumConta { get; set; }
        public string Mesa { get; set; }
        public string Empregado { get; set; }
    }

    internal class ItemSat
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Quantidade { get; set; }
        public string ValorUni { get; set; }
        public string ValorTotal { get; set; }
        public string AliqICMS { get; set; }
    }

    internal class PaySat
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string ValorTotal { get; set; }
    }
    class ParseSatSend
    {
        public CFe CFe { get; private set; }

        private ConfigureSat config;

        private List<envCFeCFeInfCFeDet> detList;
        private List<envCFeCFeInfCFePgtoMP> pgtList;

        private string[] dados;
        public string xmlString { get; private set; }

        const string identificadorHeader = "H";
        const string identificadorHeaderCliente = "H1";
        const string identificadorHeaderConta = "H2";
        const string identificadorItens = "I";
        const string identificadorPagamentos = "P";

        public ParseSatSend(string _dados, ConfigureSat _config)
        {
            dados = _dados.Split('\n');
            config = _config;

            //config = new ConfigureSat();

            CFe = new CFe();
            CFe.infCFe = new envCFeCFeInfCFe();            

            detList = new List<envCFeCFeInfCFeDet>();
            pgtList = new List<envCFeCFeInfCFePgtoMP>();

            LoadConfig();

            DefineDadosConfig(); //Dados Config

            SpliDados(dados); //Dados Micros

            SerializaGeral();
        }
        

        private void LoadConfig()
        {
           /* System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(ConfigureSat));
            System.IO.StreamReader file = new System.IO.StreamReader(@"satConfig.xml");
            config = (ConfigureSat)reader.Deserialize(file);*/
        }

        private void DefineDadosConfig()
        {
            CFe.infCFe.ide = new envCFeCFeInfCFeIde();
            CFe.infCFe.ide.CNPJ = config.IDE_CNPJ;
            CFe.infCFe.ide.signAC = config.IDE_signAC;
            CFe.infCFe.ide.numeroCaixa = config.IDE_NumeroCaixa;

            CFe.infCFe.emit = new envCFeCFeInfCFeEmit();
            CFe.infCFe.emit.CNPJ = config.Emit_CNPJ;
            CFe.infCFe.emit.IE = config.Emit_IE;
            CFe.infCFe.emit.IM = config.Emit_IM;

            CFe.infCFe.emit.indRatISSQN = "N";
        }

        private void SerializaGeral()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
                //settings.Encoding           = Encoding.UTF8;
                //settings.ConformanceLevel   = ConformanceLevel.Document;
                //settings.OmitXmlDeclaration = false;
                //settings.CloseOutput        = true;
                //settings.Indent             = true;
                //settings.IndentChars        = "  ";
                //settings.NewLineHandling    = NewLineHandling.Replace;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = false;
            

            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(CFe));

            using(StringWriter textWriter = new StringWriter()) 
            {
                using(XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings)) 
                {
                    writer.Serialize(xmlWriter, CFe);
                }
                xmlString = textWriter.ToString(); //This is the output as a string
            }



            //System.IO.StringWriter str = new System.IO.StringWriter();
            //writer.Serialize(str, CFe);

            //XmlTextWriter xw = new XmlTextWriter(str);

            //xmlString = xw


            
        }

        private void SpliDados(string[] dados)
        {
            foreach (string line in dados)
            {
                if (line != "")
                {
                    if (line.Substring(0, 1).Equals(identificadorHeader))
                    {
                        DefineHeader(line);
                    }
                    else if (line.Substring(0, 1).Equals(identificadorItens))
                    {
                        AddItem2(line);
                    }
                    else if (line.Substring(0, 1).Equals(identificadorPagamentos))
                    {
                        AddPay(line);
                    }
                }
            }

            CFe.infCFe.det = detList.ToArray();
           
            CFe.infCFe.pgto.MP = pgtList.ToArray();
        }

        private void DefineHeader(string line)
        {
            string[] dados = line.Split('|');

            if(dados[0].Equals(identificadorHeaderCliente))
            {
                envCFeCFeInfCFeDest dest = new envCFeCFeInfCFeDest();
                if (dados[1].Equals("F"))
                {
                    dest.ItemElementName = ItemChoiceType.CPF;
                }
                else if (dados[1].Equals("J"))
                {
                    dest.ItemElementName = ItemChoiceType.CNPJ;
                }

                dest.Item = dados[8];
                dest.xNome = dados[2];                
            }
        }

        private void AddItem(string line)
        {
            string[] dados = line.Split('|');

            envCFeCFeInfCFeDet det = new envCFeCFeInfCFeDet();
            det.prod = new envCFeCFeInfCFeDetProd();

            //det.nItem = (CFe.infCFe.det.Length + 1).ToString();
            det.prod.cProd = dados[1];
            det.prod.xProd = dados[2];
            det.prod.CFOP = "1234";         //TODO Config
            det.prod.uCom = "UN";
            det.prod.qCom = dados[3];
            det.prod.vUnCom = dados[4];
            det.prod.NCM = "00";             //TODO Parametrização
            det.prod.indRegra = "A";
            
            det.imposto = new envCFeCFeInfCFeDetImposto();
            envCFeCFeInfCFeDetImpostoICMS icms = new envCFeCFeInfCFeDetImpostoICMS();       

            if (!dados[6].Equals("FF"))
            {                         
                envCFeCFeInfCFeDetImpostoICMSICMS00 imp = new envCFeCFeInfCFeDetImpostoICMSICMS00();

                imp.CST = "00";
                imp.Orig = "0";             //TODO Config
                imp.pICMS = dados[6];
                imp.vICMS = "0";

                icms.Item = imp;
                det.imposto.Item = icms;                

                envCFeCFeInfCFeDetImpostoPISPISAliq pis = new envCFeCFeInfCFeDetImpostoPISPISAliq();
                pis.CST = "01";
                pis.vBC = dados[6];
                pis.pPIS = "0";             //TODO Config
                pis.vPIS = "0";             //Calcular

                det.imposto.PIS = new envCFeCFeInfCFeDetImpostoPIS();
                det.imposto.PIS.Item = pis;


            }
            else if(dados[6].Equals("FF"))
            {                
                envCFeCFeInfCFeDetImpostoICMSICMS40 imp = new envCFeCFeInfCFeDetImpostoICMSICMS40();
                imp.CST = "60";
                imp.Orig = "00";

                icms.Item = imp;
                det.imposto.Item = icms;                

                //envCFeCFeInfCFeDetImpostoPISPISNT pis = new envCFeCFeInfCFeDetImpostoPISPISNT();
                //pis.CST = "4";

                //det.imposto.PIS.Item = pis;

            }
               
            //TODO NN II             

            det.nItem = (detList.Count + 1).ToString();

            detList.Add(det);
        }

        private void AddPay(string line)
        {
            string[] dados = line.Split('|');
            CFe.infCFe.pgto = new envCFeCFeInfCFePgto();

            envCFeCFeInfCFePgtoMP pgt = new envCFeCFeInfCFePgtoMP();
            pgt.cMP = dados[1];
            pgt.vMP = dados[3];

            //TODO TEF            

            pgtList.Add(pgt);            
        }
        private void AddItem2(string line)
        {
            string[] dados = line.Split('|');

            envCFeCFeInfCFeDet det = new envCFeCFeInfCFeDet();
            det.prod = new envCFeCFeInfCFeDetProd();

            //det.nItem = (CFe.infCFe.det.Length + 1).ToString();
            det.prod.cProd = "001";
            det.prod.xProd = "Pao de forma";
            det.prod.CFOP = "0001";         //TODO Config
            det.prod.uCom = "kg";
            det.prod.qCom = "1.0000";
            det.prod.vUnCom = "1.000";
            det.prod.indRegra = "A";

            det.imposto = new envCFeCFeInfCFeDetImposto();
            envCFeCFeInfCFeDetImpostoICMS icms = new envCFeCFeInfCFeDetImpostoICMS();

            
            
            envCFeCFeInfCFeDetImpostoICMSICMS00 imp = new envCFeCFeInfCFeDetImpostoICMSICMS00();

            imp.CST = "00";
            imp.Orig = "0";             //TODO Config
            imp.pICMS = "1.00";

            icms.Item = imp;
            det.imposto.Item = icms;

            envCFeCFeInfCFeDetImpostoPISPISAliq pis = new envCFeCFeInfCFeDetImpostoPISPISAliq();
            pis.CST = "01";
            pis.vBC = "1.00";
            pis.pPIS = "1.000";             //TODO Config

            det.imposto.PIS = new envCFeCFeInfCFeDetImpostoPIS();
            det.imposto.PIS.Item = pis;

            envCFeCFeInfCFeDetImpostoCOFINSCOFINSAliq cofinsAliq = new envCFeCFeInfCFeDetImpostoCOFINSCOFINSAliq();
            cofinsAliq.CST = "01";
            cofinsAliq.pCOFINS = "1.0000";
            cofinsAliq.vBC = "1.00";

            //envCFeCFeInfCFeDetImpostoCOFINSST confisst = new envCFeCFeInfCFeDetImpostoCOFINSST();
            //confisst.vBC = "1.00";
            //confisst.pCOFINS = "1.0000";
            //confisst.Items

            det.imposto.COFINS = new envCFeCFeInfCFeDetImpostoCOFINS();
            det.imposto.COFINS.Item = cofinsAliq;

            //det.imposto.COFINSST = new envCFeCFeInfCFeDetImpostoCOFINSST();
            //det.imposto.COFINSST = confisst;

            det.nItem = (detList.Count + 1).ToString();

            detList.Add(det);
        }

    }
}
