using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invoiceServerApp
{
    public class gerenciadoSAT 
    {
        private Object locker = new object();

        private interfaceSAT interfacesat;

        private static readonly gerenciadoSAT instance = new gerenciadoSAT();

        public static gerenciadoSAT Instance
        {            
            get
            {
                //Utils.Logger.getInstance.error("Instance SAT");

                return instance;
            }
        }

        public void SetConfSAT(Utils.ConfigureXml config)
        {
            //Utils.Logger.getInstance.error("SetConfig SAT");

            if (config.configMaquina.tipoIntegracao.Equals("DIMEP"))
            {
                interfacesat = new SatDimep();
                Utils.Logger.getInstance.error(String.Format("SatDimep"));
            }
            else if (config.configMaquina.tipoIntegracao.Equals("SWEDA"))
            {
                interfacesat = new SatSweda();
                Utils.Logger.getInstance.error(String.Format("SatSweda"));
            }
            else if (config.configMaquina.tipoIntegracao.Equals("BEMATECH"))
            {
                interfacesat = new SatBematech();
                Utils.Logger.getInstance.error(String.Format("SatBematech"));
            }

            if (interfacesat != null)
                DesbloquearSATBase(config.configSAT.ChaveAtivacao);
        }
        
        public int generatorKey()
        {
//            Utils.Logger.getInstance.error("GerenKey SAT");

            Random random = new Random(Guid.NewGuid().GetHashCode());

            return Convert.ToInt32(random.Next(1, 999999).ToString());
        }
        public string DesbloquearSATBase( string _codigoAtivacao)
        {
            string log = interfacesat.DesbloquearSATBase(generatorKey(),_codigoAtivacao);
            Utils.LoggerSAT.getInstance.error(String.Format("DesbloquearSAT -> {0}", log));
            //Utils.Logger.getInstance.error(String.Format("DesbloquearSAT -> {0}", log));
            return log;
        }

        public string CancelarCFe(string _codigoAtivacao, string _chave, string _dadosCancelamento)
        {
            string log = interfacesat.CancelarCFe(generatorKey(), _codigoAtivacao, _chave, _dadosCancelamento);
            Utils.LoggerSAT.getInstance.error(String.Format("CancelarCFe -> {0}", log));
            ///Utils.Logger.getInstance.error(String.Format("CancelarCFe -> {0}", log));
            return log;
        }

        public string EnviarDadosVendaBase(string _codigoDeAtivacao, string _dadosVenda)
        {            
            lock (locker)
            {
                string log = interfacesat.EnviarDadosVendaBase(generatorKey(), _codigoDeAtivacao, _dadosVenda);
                Utils.LoggerSAT.getInstance.error(String.Format("EnviarDadosVendaBase -> {0}", log));
                //Utils.Logger.getInstance.error(String.Format("EnviarDadosVendaBase -> {0}", log));
                return log;            
            }
        }
    }
}
