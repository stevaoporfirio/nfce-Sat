using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invoiceServerApp
{
    interface interfaceSAT
    {
        string DesbloquearSATBase(int _numeroSessao, string _codigoAtivacao);
        string CancelarCFe(int _numeroSessao, string _codigoAtivacao, string _chave, string _dadosCancelamento);
        string EnviarDadosVendaBase(int _numeroSessao, string _codigoDeAtivacao, string _dadosVenda);
    }
}
