using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invoiceServerApp
{
    class gerenciadoSAT 
    {
        private Utils.ConfigureXml config;
        private interfaceSAT interfacesat;
        public gerenciadoSAT(Utils.ConfigureXml _config)
        {
            config = _config;
            if (config.configMaquina.tipoIntegracao == Utils.eTipoIntegracao.DIMEP)
            {
                interfacesat = new SatDimep();
            }
            else if (config.configMaquina.tipoIntegracao == Utils.eTipoIntegracao.SWEDA)
            {
                interfacesat = new SatSweda();
            }
            else if (config.configMaquina.tipoIntegracao == Utils.eTipoIntegracao.BEMATECH)
            {
                interfacesat = new SatBematech();
            }
        }
        public int generatorKey()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            return Convert.ToInt32(random.Next(1, 999999).ToString());
        }
        public string DesbloquearSATBase(int _numeroSessao, string _codigoAtivacao)
        {
            string log = interfacesat.DesbloquearSATBase(_numeroSessao,_codigoAtivacao);
            Utils.LoggerSAT.getInstance.error(String.Format("DesbloquearSAT -> {0}", log));
            return log;
        }

        public string CancelarCFe(int _numeroSessao, string _codigoAtivacao, string _chave, string _dadosCancelamento)
        {
            string log = interfacesat.CancelarCFe(_numeroSessao, _codigoAtivacao, _chave, _dadosCancelamento);
            Utils.LoggerSAT.getInstance.error(String.Format("CancelarCFe -> {0}", log));
            return log;
        }

        public string EnviarDadosVendaBase(int _numeroSessao, string _codigoDeAtivacao, string _dadosVenda)
        {
            string log = interfacesat.EnviarDadosVendaBase(_numeroSessao, _codigoDeAtivacao, _dadosVenda);
            Utils.LoggerSAT.getInstance.error(String.Format("EnviarDadosVendaBase -> {0}", log));
            return log;
        }
    }
}
