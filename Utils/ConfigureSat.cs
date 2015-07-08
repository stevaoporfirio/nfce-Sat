using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Utils
{
    //public enum eTipoIntegracao { NFCE, BEMATECH, DIMEP, SWEDA };
    public enum eImpressao { Servidor, MaquinaEnviouRequisicao };

    /// <summary>
    /// Classe contendo todas as configurações de SAT e NFCe
    /// </summary>
    public class ConfigureXml
    {
        /// <summary>
        /// Classe contendo todas as configurações de SAT e NFCe
        /// </summary>
        
            /// <summary>
            /// 0 - NFCe | 1 = SAT
            /// </summary>
            //public int NFCe_SAT { get; set; }

            public Config_Maquina configMaquina { get; set; }
            public Config_Emitente configEmitente { get; set; }
            public Config_NFC configNFCe { get; set; }
            public Config_SAT configSAT { get; set; }

            public ConfigureXml()
            {
                configMaquina = new Config_Maquina();
                configEmitente = new Config_Emitente();
                configNFCe = new Config_NFC();
                configSAT = new Config_SAT();
            }            
        }

      




        /// <summary>
        /// Classe com as configurações da NFC-e
        /// </summary>
        public class Config_NFC
        {
            public Config_NFC()
            {
                setData();
            }
            
            public string NatOp { get; set; }
            public string TpImp { get; set; }
            public string TpEmis { get; set; }
            public string TpAmb { get; set; }
            public string IndPag { get; set; }
            public string Crt { get; set; }
            public string tokenNfce { get; set; }
            public string VersaoQrcode { get; set; }
            public string CSC { get; set; }
            public string UrlProducao { get; set; }
            public string UrlHomologacao { get; set; }
            public string digestValue { get; set; }

            public bool Contingencia { get; set; }

            public string sequenceNumberNfce { get; set; }
            public string Serie { get; set; }
            public string Id_banco { get; set; }
            public string NomeCertificadoDigital { get; set; }

            public string NCM { get; set; }
            public string VersaoLayout{ get; set; }
            
            public string CFOP00 { get; set; }
            public string CFOP60 { get; set; }

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

        public class Config_SAT
        {
            public Config_SAT()
            {
                setData();
            }

            public string IDE_signAC { get; set; }
            public string IDE_NumeroCaixa { get; set; }
            public string ChaveAtivacao { get; set; }

            public string CFOP00 { get; set; }
            public string CFOP60 { get; set; }

            public string CNPJ_SoftwareHouse { get; set; }            

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

        ///// <summary>
        ///// Configurações do estabelecimento emitente
        ///// </summary>
        //public class Config_NFCeEmitente
        //{
        //    public Config_NFCeEmitente()
        //    {
        //        setData();
        //        endereco = new Endereco();
        //    }

        //    public string RazaoSocial { get; set; }
        //    public string Fantasia { get; set; }
        //    public string Cnpj { get; set; }
        //    public string Telefone { get; set; }
        //    public string Ie { get; set; }
        //    public string Email { get; set; }

        //    public Endereco endereco { get; set; }

        //    public void setData()
        //    {
        //        foreach (System.Reflection.PropertyInfo a in this.GetType().GetProperties())
        //        {
        //            if (a.PropertyType == typeof(bool))
        //                a.SetValue(this, false, null);
        //            else if (a.PropertyType == typeof(string))
        //                a.SetValue(this, a.Name, null);
        //        }
        //    }
        //}

        /// <summary>
        /// Configurações Gerais para emissão da NFC-e
        /// </summary>
        public class Config_NFCeGeral
        {
            public Config_NFCeGeral()
            {
                // setData();
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
            public string CNPJCredenciadoraCartao { get; set; }
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

        public class Config_Emitente
        {
            public Config_Emitente()
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



        public class NFCePgtList
        {
            public List<NFCePgt> ListPgt = new List<NFCePgt>();
        }

        public class NFCePgt
        {
            public string codMicros;
            public string descMicros;
            public string codNFCe;
            public string codBandNfce;
        }




        public class Config_Maquina
        {
            public Config_Maquina()
            {
                setData();
            }

            public bool isServidor { get; set; }
            public string tipoIntegracao { get; set; }
            public eImpressao impressao { get; set; }
            public string pathFiles { get; set; }
            public string IP { get; set; }
            public string Porta { get; set; }
            public string CNPJCredenciadoraCartao { get; set; }
            public NFCePgtList PgtList = new NFCePgtList();

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



