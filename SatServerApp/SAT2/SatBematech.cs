using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace invoiceServerApp
{
    class SatBematech : interfaceSAT
    {
        private const string _satDll = "BemaSAT.dll";

        private IntPtr pDll;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate string Desbloc(int _numeroSessao, string _codigoAtivacao);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate string Venda(int _numeroSessao, string _codigoDeAtivacao, string _dadosVenda);
        
        public string DesbloquearSATBase(int _numeroSessao, string _codigoAtivacao)
        {
            try
            {
                //return Marshal.PtrToStringAnsi(DesbloquearSAT(_numeroSessao, _codigoAtivacao));

                pDll = NativeMethods.LoadLibrary(AppDomain.CurrentDomain.BaseDirectory + "BemaSAT.dll");

                IntPtr pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "DesbloquearSAT");

                Desbloc desbloc = (Desbloc)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(Desbloc));

                string theResult = desbloc(_numeroSessao, _codigoAtivacao);

                //bool result = NativeMethods.FreeLibrary(pDll);

                return theResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CancelarCFe(int _numeroSessao, string _codigoAtivacao, string _chave, string _dadosCancelamento)
        {
            //return Marshal.PtrToStringAnsi(CancelarUltimaVenda(_numeroSessao, _codigoAtivacao, _chave, _dadosCancelamento));
            return "";
        }

        public string EnviarDadosVendaBase(int _numeroSessao, string _codigoDeAtivacao, string _dadosVenda)
        {
            //return Marshal.PtrToStringAnsi(EnviarDadosVenda(_numeroSessao, _codigoDeAtivacao, _dadosVenda));

            //pDll = NativeMethods.LoadLibrary(@"dllSAT.dll");

            IntPtr pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "EnviarDadosVenda");

            Venda venda = (Venda)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(Venda));

            string theResult = venda(_numeroSessao, _codigoDeAtivacao, _dadosVenda);

            //bool result = NativeMethods.FreeLibrary(pDll);

            return theResult;
        }
       
    }
}
