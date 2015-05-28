using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace invoiceServerApp
{
    public class dllSat
    {
        
        [DllImport("C:\\SAT\\SAT.dll",CharSet = CharSet.Ansi‏, CallingConvention = CallingConvention.Cdecl)]
        public static extern string ConsultarSAT(int paramInt);

        [DllImport("C:\\SAT\\SAT.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern String EnviarDadosVenda(int paramInt, String paramString1, String paramString2);

        [DllImport("C:\\SAT\\SAT.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern String CancelarUltimaVenda(int paramInt, String paramString1, String paramString2, String paramString3);


        [DllImport("C:\\SAT\\SAT.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern String TesteFimAFim(int paramInt, String paramString1, String paramString2);

        [DllImport("C:\\SAT\\SAT.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern String ConsultarNumeroSessao(int paramInt1, String paramString, int paramInt2);

        [DllImport("C:\\SAT\\SAT.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern String DesligarSAT();
    }
}
