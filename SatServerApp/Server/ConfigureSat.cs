using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace invoiceServerApp
{
    /// <summary>
    /// Classe contendo todas as configurações de SAT e NFCe
    /// </summary>
    public class ConfigureXml
    {
        /// <summary>
        /// 0 - NFCe | 1 = SAT
        /// </summary>
        public int NFCe_SAT { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }

        public Config_NFC configNFCe { get; set; }
        public Config_SAT configSAT { get; set; }
        

        public ConfigureXml()
        {
            configNFCe = new Config_NFC();
            configSAT = new Config_SAT();
        }

        //public void WriteConfig(ConfigureXml config)
        //{
        //    try
        //    {
        //        MemoryStream memoryStream = new MemoryStream();
        //        XmlSerializer xs = new XmlSerializer(config.GetType());
        //        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                
        //        xs.Serialize(xmlTextWriter, config);
        //        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        //        memoryStream.Position = 0;
        //        XmlDocument xd = new XmlDocument();
        //        xd.Load(memoryStream);
        //        xd.Save(@"AppConfig.xml");
        //    }
        //    catch (ApplicationException appX)
        //    {
        //        throw new ApplicationException("Erro ao salvar as Configurações\n" + appX.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public ConfigureXml ReadConfig(ConfigureXml config)
        //{
        //    try
        //    {
        //        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigureXml));
        //        using (FileStream fileStream = new FileStream(@"AppConfig.xml", FileMode.Open))
        //        {
        //            XmlReader xmlReader = new XmlTextReader(fileStream);
        //            config = (ConfigureXml)xmlSerializer.Deserialize(xmlReader);
        //        }
        //        return config;
        //    }
        //    catch (ApplicationException appX)
        //    {
        //        throw new ApplicationException("ApplicationException: Erro ao ler as configurações do Emissor NF-e.\n" + appX.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException("Exception: Erro ao ler as configurações do Emissor NF-e.\n" + ex.Message);
        //    }
        //}

    }

    /// <summary>
    /// Classe com as configurações da NFC-e
    /// </summary>
    public class Config_NFC
    {
        public Config_NFCeEmitente configNFCeEmitente { get; set; }
        public Config_NFCeGeral configNFCeGeral { get; set; }
        

        public Config_NFC()
        {
            configNFCeEmitente = new Config_NFCeEmitente();
            configNFCeGeral = new Config_NFCeGeral();
        }
    }

    public class Config_SAT
    {
        public Config_SATEmitente configSATEmitente { get; set; }
        public Config_SATGeral configSATGeral { get; set; }

        public Config_SAT()
        {
            configSATEmitente = new Config_SATEmitente();
            configSATGeral = new Config_SATGeral();
        }
    }
    public class NFCePgtList
    {
        public List<NFCePgt> ListPgt = new List<NFCePgt>();
    }

    public class NFCePgt
    {
        public string codMicros;
        public string descMicros;
        public string codNFCe;
    }

    /// <summary>
    /// Configurações do estabelecimento emitente
    /// </summary>
    public class Config_NFCeEmitente
    {
        public Config_NFCeEmitente()
        {
            setData();
            endereco = new Endereco();
        }
        
        public string RazaoSocial { get; set; }
        public string Fantasia { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public string Ie { get; set; }
        public string Email { get; set; }

        public Endereco endereco { get; set; }

        public void setData()
        {
            foreach (System.Reflection.PropertyInfo a in this.GetType().GetProperties())
            {
                if (a.PropertyType == typeof(bool))
                    a.SetValue(this, false, null);
                else if (a.PropertyType == typeof(string))
                    a.SetValue(this, a.Name, null);
            }
        }
    }

    /// <summary>
    /// Configurações Gerais para emissão da NFC-e
    /// </summary>
    public class Config_NFCeGeral
    {
        public Config_NFCeGeral()
        {
            setData();
            NFCePgtList = new NFCePgtList();
        }

        public string NatOp { get; set; }
        public string TpImp { get; set; }
        public string TpEmis { get; set; }
        public string TpAmb { get; set; }
        public string digestValue { get; set; }
        public string CSC { get; set; }
        public string tokenNfce { get; set; }
        public string VersaoQrcode { get; set; }
        public string IndPag { get; set; }
        public string Crt { get; set; }
        public string pathFiles { get; set; }
        public int NfceServer { get; set; }

        public string sequenceNumberNfce { get; set; }
        public string NomeCertificadoDigital { get; set; }

        public string NCM { get; set; }
        public string CFOPOperacaoEstadual { get; set; }
        public string CFOPOperacaoInterestadual { get; set; }

        public NFCePgtList NFCePgtList { get; set; }


        public void setData()
        {
            foreach (System.Reflection.PropertyInfo a in this.GetType().GetProperties())
            {
                if (a.PropertyType == typeof(bool))
                    a.SetValue(this, false, null);
                else if (a.PropertyType == typeof(string))
                    a.SetValue(this, a.Name, null);
            }
        }
    }

    /// <summary>
    /// Configurações gerais para emissão do SAT
    /// </summary>
    public class Config_SATGeral
    {
        public Config_SATGeral()
        {
            setData();
        }
        
        public int Server { get; set; }
        public int SatServer { get; set; }
        public int NFCEServer { get; set; }
        public string pathFiles { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public string IDE_CNPJ { get; set; }
        public string IDE_signAC { get; set; }
        public string IDE_NumeroCaixa { get; set; }
        public string key_sat { get; set; }
        //public bool isSat { get; set; }

        public void setData()
        {
            foreach (System.Reflection.PropertyInfo a in this.GetType().GetProperties())
            {
                if (a.PropertyType == typeof(bool))
                    a.SetValue(this, false, null);
                else if (a.PropertyType == typeof(string))
                    a.SetValue(this, a.Name, null);
            }
        }
    }

    public class Config_SATEmitente
    {
        public Config_SATEmitente()
        {
            setData();
            endereco = new Endereco();
        }

        public string RazaoSocial { get; set; }
        public string Fantasia { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public string Ie { get; set; }
        public string Im { get; set; }
        public string Email { get; set; }

        public Endereco endereco { get; set; }
        
        public void setData()
        {
            foreach (System.Reflection.PropertyInfo a in this.GetType().GetProperties())
            {
                if (a.PropertyType == typeof(bool))
                    a.SetValue(this, false, null);
                else if (a.PropertyType == typeof(string))
                    a.SetValue(this, a.Name, null);
            }
        }
    }

    public class Endereco
    {
        public Endereco()
        {
            setData();
        }

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

        public void setData()
        {
            foreach (System.Reflection.PropertyInfo a in this.GetType().GetProperties())
            {
                if (a.PropertyType == typeof(bool))
                    a.SetValue(this, false, null);
                else if (a.PropertyType == typeof(string))
                    a.SetValue(this, a.Name, null);
            }
        }
    }
}



