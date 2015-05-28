using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace invoiceServerApp
{
    public static class ImprimirEpsonNF
    {


        [DllImport("InterfaceEpsonNF.dll")]
        private static extern int IniciaPorta(string numPorta);

        [DllImport("InterfaceEpsonNF.dll")]
        private static extern int FechaPorta();

        [DllImport("InterfaceEpsonNF.dll")]
        private static extern int ImprimeTexto(string txt);

        [DllImport("InterfaceEpsonNF.dll")]
        private static extern int ConfiguraCodigoBarras(int altura, int largura, int posicaoCaracter, int fonte, int margem);

        [DllImport("InterfaceEpsonNF.dll")]
        private static extern int ImprimeTextoTag(string txt);

        [DllImport("InterfaceEpsonNF.dll")]
        private static extern int AcionaGuilhotina(int tipoCorte);

        [DllImport("InterfaceEpsonNF.dll")]
        private static extern int ImprimeCodigoQRCODE(int restauracao, int modulo, int tipo, int versao, int modo, string cod);

        [DllImport("InterfaceEpsonNF.dll")]
        private static extern int Le_Status();

        public static bool ImprimeQRCODE(string cod)
        {
            //IniciaPorta("USB");

            ImprimeCodigoQRCODE(1, 4, 0, 10, 1, cod);

            AcionaGuilhotina(1);

            //FechaPorta();

            return true;
        }

        public static int Status()
        {
            IniciaPorta("USB");

            int resp = Le_Status();

            FechaPorta();

            return resp;
        }
        public static void ImprimirTef(string _tef, string portaImpressora)
        {
            IniciaPorta(portaImpressora);

            ImprimeTextoTag(_tef);

            AcionaGuilhotina(0);

            FechaPorta();
        }

        public static bool ImprimirNF(string portaImpressora, string[] cupom, string[] cupom2, string qr, string qrC, string tef, bool cortar, bool gaveta)
        {
            int resp;

            try
            {                
                //resp = IniciaPorta(portaImpressora);

                int count = cupom.Length / 100;

                resp = IniciaPorta(portaImpressora);

                resp = ConfiguraCodigoBarras(100, 0, 0, 0, 0);

                foreach (String s in cupom)
                {
                    resp = ImprimeTextoTag(s);
                }

                if(!(String.IsNullOrEmpty(qrC)))
                {
                    resp = ImprimeCodigoQRCODE(1, 4, 0, 10, 1, qrC);
                }

                if (cupom2 != null)
                {
                    foreach (String s in cupom)
                    {
                        resp = ImprimeTextoTag(s);
                    }
                }

                resp = ImprimeCodigoQRCODE(1, 4, 0, 10, 1, qr);

                if (tef.Length > 1)
                {
                    resp = AcionaGuilhotina(0);

                    string[] tmpTef = tef.Split('|');

                    resp = ImprimeTextoTag(String.Format("<c><b>{0}</c></b>", tmpTef[0]));

                    resp = AcionaGuilhotina(0);

                    resp = ImprimeTextoTag(String.Format("<c><b>{0}</c></b>", tmpTef[1]));
                }
                if (cortar)
                    resp = AcionaGuilhotina(0);

                if (gaveta)
                    resp = ImprimeTextoTag("<g></g>");



                return true;
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception("Erro Imprimindo CF\n" + e.ToString());
            }
            finally
            {
                resp = FechaPorta();
            }
        }
    }
}
