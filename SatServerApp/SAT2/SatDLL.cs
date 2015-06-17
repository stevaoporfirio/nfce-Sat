using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace invoiceServerApp
{
    public static class SatDLL
    {
        public static int generatorKey()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            return Convert.ToInt32(random.Next(1, 999999).ToString());
        }


        private const string _satDll = "dllSAT.dll";


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





        public static string DesbloquearSATBase(int _numeroSessao, string _codigoAtivacao)
        {
            return Marshal.PtrToStringAnsi(DesbloquearSAT(_numeroSessao, _codigoAtivacao));
        }

        public static string CancelarCFe(int _numeroSessao, string _codigoAtivacao, string _chave, string _dadosCancelamento)
        {
            return Marshal.PtrToStringAnsi(CancelarUltimaVenda(_numeroSessao, _codigoAtivacao, _chave, _dadosCancelamento));
        }

        public static string ConsultarSATBase(int _key)
        {
            return Marshal.PtrToStringAnsi(ConsultarSAT(_key));
        }
        public static string AtivarSATBase(int _numeroSessao, int _subComando, string _codigoDeAtivacao, string _CNPJ, int _cUF)
        {
            return Marshal.PtrToStringAnsi(AtivarSAT(_numeroSessao, _subComando, _codigoDeAtivacao, _CNPJ, _cUF));
        }
        public static string TesteFimAFimBase(int _numeroSessao, string _codigoDeAtivacao, string _dadosVenda)
        {
            return Marshal.PtrToStringAnsi(TesteFimAFim(_numeroSessao, _codigoDeAtivacao, _dadosVenda));
        }
        public static string ConsultarStatusOperacionalBase(int _numeroSessao, string _codigoDeAtivacao)
        {
            return Marshal.PtrToStringAnsi(ConsultarStatusOperacional(_numeroSessao, _codigoDeAtivacao));
        }
        public static string EnviarDadosVendaBase(int _numeroSessao, string _codigoDeAtivacao, string _dadosVenda)
        {
            return Marshal.PtrToStringAnsi(EnviarDadosVenda(_numeroSessao, _codigoDeAtivacao, _dadosVenda));
        }

        public static string ConfiguraRede(int _numeroSessao, string _codigoDeAtivacao, string _dadosConf)
        {
            return Marshal.PtrToStringAnsi(ConfigurarInterfaceDeRede(_numeroSessao, _codigoDeAtivacao, _dadosConf));
        }
    }
}
