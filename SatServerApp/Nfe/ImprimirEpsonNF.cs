using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace invoiceServerApp
{
    public class ImprimirEpsonNFTDados
    {
        public ImprimirEpsonNFTDados(string _portaImpressora, string[] _cupom, string[] _cupom2, string _qr, string _qrC, string _tef, bool _cortar, bool _gaveta)
        {
            portaImpressora = _portaImpressora;
            cupom = _cupom;
            cupom2 = _cupom2;
            qr = _qr;
            qrC = _qrC;
            tef = _tef;
            cortar = _cortar;
            gaveta = _gaveta;
        }

        public string portaImpressora;
        public string[] cupom;
        public string[] cupom2;
        public string qr;
        public string qrC;
        public string tef;
        public bool cortar;
        public bool gaveta;
    }

    class ImprimirEpsonNF
    {
        private IntPtr pDll;        

        private static ImprimirEpsonNF[] instance = new ImprimirEpsonNF[99]; // = new ImprimirEpsonNF();

        private static List<String> portList = new List<string>();

        private object locker = new object();

        private ImprimirEpsonNF(string porta)
        {
            pDll = NativeMethods.LoadLibrary(@"InterfaceEpsonNF.dll");
            
            IntPtr pAddressOfFunctionToCallInicia = NativeMethods.GetProcAddress(pDll, "IniciaPorta");
            IniciaPorta iniciaPorta = (IniciaPorta)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCallInicia, typeof(IniciaPorta));

            iniciaPorta(porta);
                        
        }

        public static ImprimirEpsonNF Instance(string porta)
        {
            int i = 0;
            bool found = false;


            foreach (string s in portList)
            {
                if (porta.Equals(s))
                {
                    found = true;
                    break;
                }

                i++;
            }

            if (!found)
                portList.Add(porta);

            if (instance[i] == null)
                instance[i] = new ImprimirEpsonNF(porta);

            return instance[i];
        }
       
        public void ImprimirTef(string _tef, string portaImpressora)
        {
            //IntPtr pAddressOfFunctionToCall;

            //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "IniciaPorta");
            //IniciaPorta iniciaPorta = (IniciaPorta)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(IniciaPorta));

            IntPtr pAddressOfFunctionToCallConfigCodBarra = NativeMethods.GetProcAddress(pDll, "ConfiguraCodigoBarras");
            ConfiguraCodigoBarras config = (ConfiguraCodigoBarras)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCallConfigCodBarra, typeof(ConfiguraCodigoBarras));

            IntPtr pAddressOfFunctionToCallTextoTag = NativeMethods.GetProcAddress(pDll, "ImprimeTextoTag");
            ImprimeTextoTag print = (ImprimeTextoTag)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCallTextoTag, typeof(ImprimeTextoTag));

            IntPtr pAddressOfFunctionToCallQR = NativeMethods.GetProcAddress(pDll, "ImprimeCodigoQRCODE");
            ImprimeCodigoQRCODE qrCode = (ImprimeCodigoQRCODE)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCallQR, typeof(ImprimeCodigoQRCODE));

            //IntPtr pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "AcionaGuilhotina");
            //AcionaGuilhotina cut = (AcionaGuilhotina)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(AcionaGuilhotina));

            //IntPtr pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "FechaPorta");
            //fechar = (FechaPorta)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(FechaPorta));

            //IntPtr pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "IniciaPorta");
            //IniciaPorta iniciaPorta = (IniciaPorta)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(IniciaPorta));

            //iniciaPorta(portaImpressora);

            print(_tef);

            //cut(0);

            //fechar();
        }

        //
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int IniciaPorta(string numPorta);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ConfiguraCodigoBarras(int altura, int largura, int posicaoCaracter, int fonte, int margem);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ImprimeTextoTag(string txt);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ImprimeCodigoQRCODE(int restauracao, int modulo, int tipo, int versao, int modo, string cod);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int AcionaGuilhotina(int tipoCorte);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int FechaPorta();

        FechaPorta fechar;
        //        

        //private string portaImpressora;
        //private string[] cupom;
        //private string[] cupom2;
        //private string qr;
        //private string qrC;
        //private string tef;
        //private bool cortar;
        //private bool gaveta;

        //public void DefinirImpressao(string _portaImpressora, string[] _cupom, string[] _cupom2, string _qr, string _qrC, string _tef, bool _cortar, bool _gaveta)
        //{
        //    portaImpressora = _portaImpressora;
        //    cupom = _cupom;
        //    cupom2 = _cupom2;
        //    qr = _qr;
        //    qrC = _qrC;
        //    tef = _tef;
        //    cortar = _cortar;
        //    gaveta = _gaveta;
        //}


        public void ImprimirNF(ImprimirEpsonNFTDados dados)
        {
            int resp;

            lock (locker)
            {
                try
                {
                    

                    int count = dados.cupom.Length / 100;

                    //IntPtr pAddressOfFunctionToCall;

                    //pDll = NativeMethods.LoadLibrary(@"InterfaceEpsonNF.dll");

                    //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "IniciaPorta");
                    //IniciaPorta iniciaPorta = (IniciaPorta)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(IniciaPorta));

                    //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "ConfiguraCodigoBarras");
                    //ConfiguraCodigoBarras config = (ConfiguraCodigoBarras)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(ConfiguraCodigoBarras));

                    //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "ImprimeTextoTag");
                    //ImprimeTextoTag print = (ImprimeTextoTag)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(ImprimeTextoTag));

                    //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "ImprimeCodigoQRCODE");
                    //ImprimeCodigoQRCODE qrCode = (ImprimeCodigoQRCODE)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(ImprimeCodigoQRCODE));

                    //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "AcionaGuilhotina");
                    //AcionaGuilhotina cut = (AcionaGuilhotina)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(AcionaGuilhotina));

                    //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "FechaPorta");
                    //fechar = (FechaPorta)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(FechaPorta));

                    IntPtr pAddressOfFunctionToCallConfigCodBarra = NativeMethods.GetProcAddress(pDll, "ConfiguraCodigoBarras");
                    ConfiguraCodigoBarras config = (ConfiguraCodigoBarras)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCallConfigCodBarra, typeof(ConfiguraCodigoBarras));

                    IntPtr pAddressOfFunctionToCallTextoTag = NativeMethods.GetProcAddress(pDll, "ImprimeTextoTag");
                    ImprimeTextoTag print = (ImprimeTextoTag)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCallTextoTag, typeof(ImprimeTextoTag));

                    IntPtr pAddressOfFunctionToCallQR = NativeMethods.GetProcAddress(pDll, "ImprimeCodigoQRCODE");
                    ImprimeCodigoQRCODE qrCode = (ImprimeCodigoQRCODE)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCallQR, typeof(ImprimeCodigoQRCODE));

                    //resp = fechar();

                    //resp = iniciaPorta(portaImpressora);

                    resp = config(100, 0, 0, 0, 0);

                    string linha = "";

                    foreach (String s in dados.cupom)
                    {
                        //resp = print(s);
                        linha += s;
                    }

                    //resp = print(linha);                    

                    if (!(String.IsNullOrEmpty(dados.qrC)))
                    {
                        //resp = qrCode(1, 4, 0, 10, 1, qrC);
                        linha += String.Format("<l></l><qrcode>{0}<lmodulo>4</lmodulo><correcao>1</correcao></qrcode>", dados.qrC);
                    }

                    if (dados.cupom2 != null)
                    {
                        foreach (String s in dados.cupom2)
                        {
                            linha += s;
                        }
                    }

                    if (!String.IsNullOrEmpty(dados.qr))
                    {
                        //resp = qrCode(1, 4, 0, 10, 1, qr);
                        linha += String.Format("<l></l><qrcode>{0}<lmodulo>4</lmodulo><correcao>1</correcao></qrcode>", dados.qr);
                    }

                    if (!String.IsNullOrEmpty(dados.tef) && dados.tef.Length > 1)
                    {
                        //resp = cut(0);

                        string[] tmpTef = dados.tef.Split('|');

                        linha += String.Format("<c><b>{0}</c></b>", tmpTef[0]);
                        linha += "<gui></gui>";
                        linha += String.Format("<c><b>{0}</c></b>", tmpTef[1]);

                        //resp = print(String.Format("<c><b>{0}</c></b>", tmpTef[0]));

                        //resp = cut(0);

                        //resp = print(String.Format("<c><b>{0}</c></b>", tmpTef[1]));
                    }

                    linha += "<g></g><gui></gui>";

                    resp = print(linha);

                    //if (cortar)
                    //resp = cut(0);

                    //if (gaveta)
                    //resp = print("<g></g>");
                    
                }
                catch (Exception e)
                {
                    Utils.Logger.getInstance.error(e);
                    throw new Exception("Erro Imprimindo CF\n" + e.ToString());
                }
                finally
                {
                    //resp = fechar();
                }
            }
        }

        public void ImprimirNF(string portaImpressora, string[] cupom, string[] cupom2, string qr, string qrC, string tef, bool cortar, bool gaveta)
        {
            int resp;

            //lock (locker)
            {
                try
                {
                    int count = cupom.Length / 100;

                    //IntPtr pAddressOfFunctionToCall;

                    //pDll = NativeMethods.LoadLibrary(@"InterfaceEpsonNF.dll");

                    //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "IniciaPorta");
                    //IniciaPorta iniciaPorta = (IniciaPorta)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(IniciaPorta));

                    //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "ConfiguraCodigoBarras");
                    //ConfiguraCodigoBarras config = (ConfiguraCodigoBarras)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(ConfiguraCodigoBarras));

                    //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "ImprimeTextoTag");
                    //ImprimeTextoTag print = (ImprimeTextoTag)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(ImprimeTextoTag));

                    //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "ImprimeCodigoQRCODE");
                    //ImprimeCodigoQRCODE qrCode = (ImprimeCodigoQRCODE)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(ImprimeCodigoQRCODE));

                    //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "AcionaGuilhotina");
                    //AcionaGuilhotina cut = (AcionaGuilhotina)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(AcionaGuilhotina));

                    //pAddressOfFunctionToCall = NativeMethods.GetProcAddress(pDll, "FechaPorta");
                    //fechar = (FechaPorta)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(FechaPorta));

                    IntPtr pAddressOfFunctionToCallConfigCodBarra = NativeMethods.GetProcAddress(pDll, "ConfiguraCodigoBarras");
                    ConfiguraCodigoBarras config = (ConfiguraCodigoBarras)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCallConfigCodBarra, typeof(ConfiguraCodigoBarras));

                    IntPtr pAddressOfFunctionToCallTextoTag = NativeMethods.GetProcAddress(pDll, "ImprimeTextoTag");
                    ImprimeTextoTag print = (ImprimeTextoTag)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCallTextoTag, typeof(ImprimeTextoTag));

                    IntPtr pAddressOfFunctionToCallQR = NativeMethods.GetProcAddress(pDll, "ImprimeCodigoQRCODE");
                    ImprimeCodigoQRCODE qrCode = (ImprimeCodigoQRCODE)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCallQR, typeof(ImprimeCodigoQRCODE));

                    //resp = fechar();

                    //resp = iniciaPorta(portaImpressora);

                    resp = config(100, 0, 0, 0, 0);

                    string linha = "";
                    
                    foreach (String s in cupom)
                    {
                        //resp = print(s);
                        linha += s;
                    }

                    //resp = print(linha);                    

                    if (!(String.IsNullOrEmpty(qrC)))
                    {
                        //resp = qrCode(1, 4, 0, 10, 1, qrC);
                        linha += String.Format("<l></l><qrcode>{0}<lmodulo>4</lmodulo><correcao>1</correcao></qrcode>", qrC);
                    }

                    if (cupom2 != null)
                    {
                        foreach (String s in cupom2)
                        {
                            linha += s;
                        }
                    }

                    if (!String.IsNullOrEmpty(qr))
                    {
                        //resp = qrCode(1, 4, 0, 10, 1, qr);
                        linha += String.Format("<l></l><qrcode>{0}<lmodulo>4</lmodulo><correcao>1</correcao></qrcode>", qr);
                    }

                    if (!String.IsNullOrEmpty(tef) && tef.Length > 1)
                    {
                        //resp = cut(0);

                        string[] tmpTef = tef.Split('|');

                        linha += String.Format("<c><b>{0}</c></b>", tmpTef[0]);
                        linha += "<gui></gui>";
                        linha += String.Format("<c><b>{0}</c></b>", tmpTef[1]);

                        //resp = print(String.Format("<c><b>{0}</c></b>", tmpTef[0]));

                        //resp = cut(0);

                        //resp = print(String.Format("<c><b>{0}</c></b>", tmpTef[1]));
                    }

                    linha += "<g></g><gui></gui>";

                    resp = print(linha);                    

                    //if (cortar)
                        //resp = cut(0);

                    //if (gaveta)
                        //resp = print("<g></g>");

                    //return true;
                }
                catch (Exception e)
                {
                    Utils.Logger.getInstance.error(e);
                    throw new Exception("Erro Imprimindo CF\n" + e.ToString());
                }
                finally
                {
                    //resp = fechar();
                }
            }
        }
    }
}
