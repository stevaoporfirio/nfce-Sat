using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace invoiceServerApp
{
    public class invoiceImplemention: NotificationChanged 
    {
        private Utils.ConfigureXml config;
        Dictionary<string, ParseInterface> dictionary = new Dictionary<string, ParseInterface>();
        private Object thisLock = new Object();
        private bool blockSat = false;

        public invoiceImplemention(Utils.ConfigureXml _config) 
        {
            config = _config;
        }

        public string NotificationChangedClient(string _id, string _receive)        
        {
            lock (thisLock)
            {
                if (!dictionary.ContainsKey(_id))
                {
                    if ((config.configMaquina.tipoIntegracao == Utils.eTipoIntegracao.DIMEP) || (config.configMaquina.tipoIntegracao == Utils.eTipoIntegracao.SWEDA) || (config.configMaquina.tipoIntegracao == Utils.eTipoIntegracao.BEMATECH))
                    {
                        
                        dictionary.Add(_id, new ParseSatSend(_id, config));
                        Utils.Logger.getInstance.error("ParseSatSend id: " + _id);
                    }
                    else if (config.configMaquina.tipoIntegracao == Utils.eTipoIntegracao.NFCe)
                    {
                        dictionary.Add(_id, new ParseNFCE(_id, config));
                        Utils.Logger.getInstance.error("ParseNFCE id: " + _id);
                    }
                }
            }

            return ParseBuy(_id, _receive);
        }

        public string ParseBuy(string _id, string msg)
        {
            Utils.Logger.getInstance.error("id: " + _id);
            Utils.Logger.getInstance.error("msg: " + msg);
            string retorno = "";
            try
            {
                ParseInterface parse;
                lock (thisLock)
                {
                    parse = dictionary[_id];
                }

                if (msg == "COD")
                    retorno = parse.getCodigo(msg) + "|C|";
                else if (msg == "QRC")
                    retorno = parse.getQrCode(msg) + "|Q|";
                else if (_id.Equals("TEF"))
                    retorno = parse.ReImpressaoTEF(msg);
                else if (_id.Equals("CANCEL"))
                    retorno = parse.messageCancel(msg) + "|X|";
                else if (_id.Equals("REDANFE"))
                    retorno = parse.ReImpressaoDanfe(msg);
                else if (_id.Equals("INUTILIZACAO"))
                    retorno = parse.messageInutilizacao(msg);
                else
                {
                    retorno = parse.messageParse(msg) + "|X|";
                }


            }
            catch (Exception e)
            {
                //dictionary.Remove(_id);
                //Utils.Logger.getInstance.error(_id + " :" + e.ToString());
                //retorno = e.Message.ToString().Substring(0, 100) + "|END|";
                retorno = removeReturn(e.ToString());
            }
            finally
            {
                removeCupom(_id);
                
            }
            return retorno;
            
            
        }
        private string removeReturn(string _msg)
        {
            string StrPat = @"((?<=#).*(?=#))"; 
            Regex pattern = new Regex(StrPat, RegexOptions.Compiled);
            Match m = pattern.Match(_msg);
            if (m.Success)
                return m.Value + "|END|";
            else
                return "Erro processando documento eletronico. |END|";
        }
        private void removeCupom(string _id)
        {
            lock (thisLock)
            {
                if (dictionary.ContainsKey(_id))
                {
                    dictionary.Remove(_id);
                }
            }
                
        }


    }
}
