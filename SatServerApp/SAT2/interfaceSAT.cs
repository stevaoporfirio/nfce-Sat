using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace invoiceServerApp
{
    public static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
    }

    interface interfaceSAT
    {
        string DesbloquearSATBase(int _numeroSessao, string _codigoAtivacao);
        string CancelarCFe(int _numeroSessao, string _codigoAtivacao, string _chave, string _dadosCancelamento);
        string EnviarDadosVendaBase(int _numeroSessao, string _codigoDeAtivacao, string _dadosVenda);
    }
}
