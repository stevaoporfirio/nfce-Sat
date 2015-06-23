using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace invoiceServerApp
{
    class SatSweda : interfaceSAT
    {
        private const string _satDll = "SATDLL.dll";


        [DllImport(_satDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern IntPtr ConsultarSAT(Int32 numeroSessao);

        [DllImport(_satDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern IntPtr ConsultarStatusOperacional(Int32 numeroSessao, [MarshalAs(UnmanagedType.AnsiBStr)] string codigoDeAtivacao);

        [DllImport(_satDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern IntPtr AtivarSAT(Int32 numeroSessao, Int32 subComando, [MarshalAs(UnmanagedType.AnsiBStr)] string codigoDeAtivacao, [MarshalAs(UnmanagedType.AnsiBStr)] string CNPJ, Int32 cUF);

        [DllImport(_satDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern IntPtr TesteFimAFim(Int32 numeroSessao, [MarshalAs(UnmanagedType.AnsiBStr)] string codigoDeAtivacao, [MarshalAs(UnmanagedType.AnsiBStr)] string dadosVenda);

        [DllImport(_satDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern IntPtr EnviarDadosVenda(Int32 numeroSessao, [MarshalAs(UnmanagedType.AnsiBStr)] string codigoDeAtivacao, [MarshalAs(UnmanagedType.AnsiBStr)] string dadosVenda);

        [DllImport(_satDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern IntPtr CancelarUltimaVenda(Int32 numeroSessao, [MarshalAs(UnmanagedType.AnsiBStr)] string codigoDeAtivacao, [MarshalAs(UnmanagedType.AnsiBStr)] string chave, [MarshalAs(UnmanagedType.AnsiBStr)] string dadosCancelamento);

        [DllImport(_satDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern IntPtr AssociarAssinatura(Int32 numeroSessao, [MarshalAs(UnmanagedType.AnsiBStr)] string codigoDeAtivacao, [MarshalAs(UnmanagedType.AnsiBStr)] string CNPJ, [MarshalAs(UnmanagedType.AnsiBStr)] string assinaturaCNPJ);

        [DllImport(_satDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern IntPtr ConfigurarInterfaceDeRede(Int32 numeroSessao, [MarshalAs(UnmanagedType.AnsiBStr)] string codigoDeAtivacao, [MarshalAs(UnmanagedType.AnsiBStr)] string dadosConfiguracao);

        [DllImport(_satDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern IntPtr ComunicarCertificadoICPBRASIL(Int32 numeroSessao, [MarshalAs(UnmanagedType.AnsiBStr)] string codigoDeAtivacao, [MarshalAs(UnmanagedType.AnsiBStr)] string certificado);

        [DllImport(_satDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern IntPtr DesbloquearSAT(Int32 numeroSessao, [MarshalAs(UnmanagedType.AnsiBStr)] string codigoDeAtivacao);


        public string DesbloquearSATBase(int _numeroSessao, string _codigoAtivacao)
        {
            return Marshal.PtrToStringAnsi(DesbloquearSAT(_numeroSessao, _codigoAtivacao));
        }

        public string CancelarCFe(int _numeroSessao, string _codigoAtivacao, string _chave, string _dadosCancelamento)
        {
            return Marshal.PtrToStringAnsi(CancelarUltimaVenda(_numeroSessao, _codigoAtivacao, _chave, _dadosCancelamento));
        }

        public string EnviarDadosVendaBase(int _numeroSessao, string _codigoDeAtivacao, string _dadosVenda)
        {
            return Marshal.PtrToStringAnsi(EnviarDadosVenda(_numeroSessao, _codigoDeAtivacao, _dadosVenda));
        }
    }
}
