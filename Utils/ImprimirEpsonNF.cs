using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Utils
{
    public class ImprimirEpsonNF
    {       
        public ImprimirEpsonNF(string ip)
        {
                              
        }

        private string portaImpressora;
        private string[] cupom;
        private string[] cupom2;
        private string qr;
        private string qrC;
        private string tef;
        private bool cortar;
        private bool gaveta;

        //Formatos
        byte[] formatoCondensado = new byte[] { 27, 33, 1 };
        byte[] formatoNegrito = new byte[] { 27, 33, 9 };
        byte[] formatoCentralizado = new byte[] { 27, 97, 49 };

        byte[] formatoLimpar = new byte[] { 27, 33, 0 };
        byte[] limparTudo = new byte[] { 27, 64};

        byte[] formataGuilhotina = new byte[] { 29, 86, 48 };
        byte[] pulaLinha = new byte[] { 10 };


        byte[] abrirGavetaP2 = new byte[] {27,112,0,25,250};
        byte[] abrirGavetaP5 = new byte[] { 27, 112, 1, 25, 250 };

        byte[] ativarBuzzer2 = new byte[] { 16,20,1,0,8 };
        byte[] ativarBuzzer5 = new byte[] { 16, 20, 1, 1, 8 };


        public void SetDados(string _portaImpressora, string[] _cupom, string[] _cupom2, string _qr, string _qrC, string _tef, bool _cortar, bool _gaveta)
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

        public void ImprimirTef(string _tef, string portaImpressora)
        {
            //implementar reprint  
            IPAddress ipAddress = IPAddress.Parse(portaImpressora);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, Convert.ToInt32("9100"));

            string[] tmpTef = _tef.Split('|');

            List<String> linha = new List<string>();

            //linha.Add("<nl>");
            //linha.Add("<nl>");
            //linha.Add("<gl>");
            linha.Add(String.Format("<nn>{0}", tmpTef[0]));
            linha.Add("<nl>");
            linha.Add("<nl>");
            if (tmpTef.Length > 1)
            {
                linha.Add("<gl>");
                linha.Add(String.Format("<nn>{0}", tmpTef[1]));
                linha.Add("<ll>");
                linha.Add("<ll>");
            }
            Print(remoteEP, linha.ToArray());
        }

        public void ImprimirNF()
        {
            IPAddress ipAddress = IPAddress.Parse(portaImpressora);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, Convert.ToInt32("9100"));
            
            List<string> linha = new List<string>();

            try
            {
                int count = cupom.Length / 100;               

                foreach (String s in cupom)
                {
                    linha.Add(s);
                }

                if (!(String.IsNullOrEmpty(qrC)))
                {
                    linha.Add(String.Format("<qr>{0}", qrC));
                }

                if (cupom2 != null)
                {
                    foreach (String s in cupom2)
                    {
                        linha.Add(s);
                    }
                }

                if (!String.IsNullOrEmpty(qr))
                {
                    linha.Add(String.Format("<qr>{0}", qr));
                }

                if (!String.IsNullOrEmpty(tef) && tef.Length > 2)
                {
                    string[] tmpTef = tef.Split('|');

                    linha.Add("<nl>");
                    linha.Add("<nl>");
                    linha.Add("<gl>");
                    linha.Add(String.Format("<nn>{0}", tmpTef[0]));                    
                    linha.Add("<nl>");
                    linha.Add("<nl>");
                    linha.Add("<gl>");
                    linha.Add(String.Format("<nn>{0}", tmpTef[1]));
                    linha.Add("<ll>");
                    linha.Add("<ll>");

                    
                    
                }

                Print(remoteEP, linha.ToArray());
                                
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception("Erro Imprimindo CF - Preparando Dados\n" + e.ToString());
            }            
        }

        private void Print(IPEndPoint remoteEP, string[] linha)
        {
            try
            {
                using (Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    //
                    client.Connect(remoteEP); // <---------------- Abriu                    

                    //Linhas formatadas?                                  
                    foreach (string s in linha)
                    {
                        byte[] dados = Encoding.UTF8.GetBytes(s.Substring(4));
                        client.Send(formatoCondensado);                        

                        if (s.Substring(0, 4).Equals("<nb>"))
                        {
                            client.Send(formatoNegrito);
                            client.Send(dados);
                        }
                        else if (s.Substring(0, 4).Equals("<nc>"))
                        {
                            client.Send(formatoCentralizado);
                            client.Send(dados);
                        }
                        else if (s.Substring(0, 4).Equals("<bc>"))
                        {
                            client.Send(formatoNegrito);
                            client.Send(formatoCentralizado);
                            client.Send(dados);
                        }
                        else if (s.Substring(0, 4).Equals("<nl>"))
                        {
                            client.Send(pulaLinha);
                            //client.Send(dados);
                        }
                        else if (s.Substring(0, 4).Equals("<nn>"))
                        {
                            client.Send(dados);
                        }
                        else if (s.Substring(0, 4).Equals("<gl>"))
                        {
                            client.Send(formataGuilhotina);
                        }

                        client.Send(pulaLinha);
                        client.Send(formatoLimpar);

                        if (s.Substring(0, 4).Equals("<br>"))
                        {
                            //h - definindo altura
                            byte[] CodBarras1 = new byte[] { 
                                                            29, 
                                                            104, 
                                                            60 //altura: 1-255
                                                        };

                            //w - Definindo tamanho do código de barras - largura
                            byte[] CodBarras2 = new byte[] { 
                                                            29, 
                                                            119, 
                                                            1 //largura: 2-6
                                                        };

                            //H - posição dos caracteres
                            byte[] CodBarras3 = new byte[] { 
                                                            29, 
                                                            72, 
                                                            48 //posição dos caracteres: 0 - não imprime
                                                        };
                            
                            //k - imprimindo codigo de barras
                            byte[] CodBarras4 = new byte[dados.Length + 6];

                            CodBarras4[0] = 29;
                            CodBarras4[1] = 107;
                            CodBarras4[2] = 73;
                            CodBarras4[3] = 24;
                            CodBarras4[4] = 123;
                            CodBarras4[5] = 65;

                            //string barcode = s.Substring(4);

                            for (int i = 0; i < dados.Length; i++)
                            {
                                CodBarras4[i + 6] = dados[i];
                                //CodBarras4[i + 6] -= 48;
                            }

                            client.Send(limparTudo);

                            client.Send(formatoCentralizado);

                            client.Send(CodBarras1);
                            client.Send(CodBarras2);
                            client.Send(CodBarras3);
                            client.Send(CodBarras4);
                            //client.Send(pulaLinha);                            
                        }

                        if (s.Substring(0, 4).Equals("<qr>"))
                        {
                            //QR
                            string qrCodeData = s.Substring(4);
                            int data_len = qrCodeData.Length + 3;
                            byte data_pL = (byte)(data_len % 256);
                            byte data_pH = (byte)(data_len / 256);

                            //165 - select the model
                            byte[] formataQR1 = new byte[] { 
                            29, //GS
                            40, //(
                            107,//k
                            4,  //pL
                            0,  //pH
                            49, //cn
                            65, //fn
                            50, //n1 = 49 ou 50
                            0   //n2 = 0
                        };

                            //167 - select the size 
                            byte[] formataQR2 = new byte[] { 
                            29,  //GS
                            40,  //(
                            107, //k
                            3,   //pL
                            0,   //pH
                            49,  //cn
                            67,  //fn
                            3    //size = 1-16
                        };

                            //169 - select the error correction level
                            byte[] formataQR3 = new byte[] { 
                            29,  //GS
                            40,  //(
                            107, //k
                            3,   //pL
                            0,   //pH
                            49,  //cn
                            69,  //fn
                            49   //Correction Error level M: 15% | 48=7%, 49=15%,50=25%,51=30%
                        };

                            //180 - define data to print
                            byte[] formataQR4 = new byte[8 + qrCodeData.Length];

                            formataQR4[0] = 29;  //GS
                            formataQR4[1] = 40;  //(
                            formataQR4[2] = 107; //k
                            formataQR4[3] = data_pL; //pL
                            formataQR4[4] = data_pH; //pH
                            formataQR4[5] = 49; //cn
                            formataQR4[6] = 80; //fn
                            formataQR4[7] = 48; //m

                            for (int i = 0; i < qrCodeData.Length; i++)
                            {
                                formataQR4[i + 8] = (byte)qrCodeData[i]; //caracteres to print
                            }

                            //181 - print
                            byte[] formataQR5 = new byte[] { 
                            29,  //GS
                            40,  //(
                            107, //k
                            3,  //pL
                            0,  //pH
                            49, //cn
                            81, //fn
                            48  //m
                        };

                            client.Send(limparTudo);
                            client.Send(formatoCentralizado);

                            client.Send(formataQR1);
                            client.Send(formataQR2);
                            client.Send(formataQR3);
                            client.Send(formataQR4);
                            client.Send(formataQR5);
                        }
                    }

                    client.Send(pulaLinha);
                    client.Send(pulaLinha);
                    client.Send(pulaLinha);
                    client.Send(pulaLinha);
                    client.Send(pulaLinha);

                    client.Send(formataGuilhotina);

                    client.Send(abrirGavetaP2);
                    client.Send(abrirGavetaP5);
                    client.Send(ativarBuzzer2);
                    client.Send(ativarBuzzer5);

                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Utils.Logger.getInstance.error(ex);
                throw new Exception("Erro Imprimindo CF - Enviando Impressora\n" + ex.ToString());
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
                    
                    string linha = "";
                    
                    foreach (String s in cupom)
                    {                    
                        linha += s;
                    }
                                        
                    if (!(String.IsNullOrEmpty(qrC)))
                    {                    
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
                        linha += String.Format("<l></l><qrcode>{0}<lmodulo>4</lmodulo><correcao>1</correcao></qrcode>", qr);
                    }

                    if (!String.IsNullOrEmpty(tef) && tef.Length > 1)
                    {
                        string[] tmpTef = tef.Split('|');

                        linha += String.Format("<c><b>{0}</c></b>", tmpTef[0]);
                        linha += "<gui></gui>";
                        linha += String.Format("<c><b>{0}</c></b>", tmpTef[1]);                       
                    }

                    linha += "<g></g><gui></gui>";

                    bool go = false;

                    //while (!go)
                    //{
                    //    lock (locker)
                    //    {
                    //        resp = fechar();
                    //        go = (resp == 1);
                    //        Logger.getInstance.error(String.Format("Fechar {0} | {1}", resp, portaImpressora));

                    //        resp = iniciaPorta(portaImpressora);
                    //        go = (resp == 1);
                    //        Logger.getInstance.error(String.Format("iniciaPorta {0} | {1}", resp, portaImpressora));

                    //        resp = config(100, 0, 0, 0, 0);
                    //        go = (resp == 1);

                    //        resp = print(linha);
                    //        go = (resp == 1);
                    //        Logger.getInstance.error(String.Format("print {0} | {1}", resp, portaImpressora));
                    //    }
                    //}                   
                }
                catch (Exception e)
                {
                    Utils.Logger.getInstance.error(e);
                    throw new Exception("Erro Imprimindo CF\n" + e.ToString());
                }                
            }
        }
    }
}
