using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Windows.Forms;

namespace invoiceServerApp
{
    class ParseSatReceive
    {
        private CFe retSat;
        private string[] msgSat;
        public string QRCode { get; private set; }
        public string textPrint { get; private set; }

        public ParseSatReceive(string msg)
        {
            msgSat = msg.Split('|');
            if (msgSat[1].Equals("06000") == true)
            {
                if (msgSat.Length > 6)
                {
                    string string64 = Encoding.UTF8.GetString(Convert.FromBase64String(msgSat[6]));

                       string c = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + string64;



                       
                       retSat = new CFe();
                       try
                       {
                           XDocument doc = XDocument.Parse(string64);
                           
 
                            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(CFe));
                            StringReader StrRead = new StringReader(c);

                            retSat = (CFe)reader.Deserialize(StrRead);
                       }
                       catch (Exception e)
                       {
                           Utils.Logger.getInstance.error(e.ToString());
                       }
                    //

                     

                       string assQRCode = msgSat[11];

                       string nomeFantasia = retSat.infCFe.emit.xFant;


                       string razao = retSat.infCFe.emit.xNome;

                       string CNPJ = retSat.infCFe.emit.CNPJ;

                       string IE = retSat.infCFe.emit.IE;

                       string IM = retSat.infCFe.emit.IM;

                       string logradouro = retSat.infCFe.emit.enderEmit.xLgr;

                       string nro = retSat.infCFe.emit.enderEmit.nro;

                       string bairro = retSat.infCFe.emit.enderEmit.xBairro;

                       string cpl = retSat.infCFe.emit.enderEmit.xCpl;

                       string mun = retSat.infCFe.emit.enderEmit.xMun;

                       string nCFe = retSat.infCFe.ide.nCFe;

                       string destinatario = retSat.infCFe.emit.CNPJ;


                       string vImposto = retSat.infCFe.total.ICMSTot.vICMS;
                           

                       string endereco = "";
                       
                       if (logradouro != null)
                       {
                           endereco = endereco + logradouro;
                       }
                       if (nro != null)
                       {
                           endereco = endereco + " " + nro;
                       }
                       if (cpl != null)
                       {
                           endereco = endereco + " " + cpl;
                       }
                       if (bairro != null)
                       {
                           endereco = endereco + " " + bairro;
                       }
                       if (mun != null)
                       {
                           endereco = endereco + " " + mun;
                       }
                       string cabecalho = "";
                       if (nomeFantasia != null)
                       {
                           cabecalho = cabecalho + quebrarLinha(nomeFantasia, 54);
                       }
                       if (razao != null)
                       {
                           cabecalho = cabecalho + "\r\n" + quebrarLinha(razao, 54);
                       }
                       if (endereco.Length > 0)
                       {
                           cabecalho = cabecalho + "\r\n" + quebrarLinha(endereco, 54);
                       }
                       cabecalho = cabecalho + "\r\n";
                       if (CNPJ != null)
                       {
                           cabecalho = cabecalho + "CNPJ " + CNPJ;
                       }
                       if (IE != null)
                       {
                           cabecalho = cabecalho + " IE " + IE;

                       }

                       if (IM != null)
                       {
                           cabecalho = cabecalho + " IM " + IM;
                       }
                       cabecalho = cabecalho + "\r\n------------------------------------------------------";
                       cabecalho = cabecalho + "\r\nExtrato No. " + nCFe;
                       cabecalho = cabecalho + "\r\n" + centralizar("CUPOM FISCAL ELETRONICO", 54);
                       cabecalho = cabecalho + "\r\n------------------------------------------------------";
                       if (destinatario != null)
                       {
                           cabecalho = cabecalho + "\r\n" + centralizar(new StringBuilder("CPF/CNPJ do Consumidor: ").Append(destinatario).ToString(), 54);
                           cabecalho = cabecalho + "\r\n------------------------------------------------------";
                       }
                       cabecalho = cabecalho + "\r\n# |COD|DESC|QTD|UN|VL UNIT R$|ST|ALIQ|VL ITEM R$";
                       string corpo = "";

                       





                       int i = 0;



                       foreach (envCFeCFeInfCFeDet det in retSat.infCFe.det)
                       {
                           i++;
                           string atual = "";
                           string numero = i.ToString();
                           string codigo = det.prod.cProd;
                           string qtd = det.prod.qCom;
                           string descricao = det.prod.qCom;
                           string un = det.prod.uCom;
                           string vUn = det.prod.vUnCom;
                           string ST = i.ToString();
                           string ALIQ = i.ToString();
                           string valor = det.prod.vItem;

                           atual = atual + "\r\n" + numero + " " + codigo + " " + descricao + " " + qtd + " " + un + " " + vUn + " " + ST + " " + ALIQ + " " + valor;

                           corpo = corpo + quebrarLinha(atual, 54);
                       }

                    
                       

                       //}




                       string total = retSat.infCFe.total.vCFe;
                       string troco = retSat.infCFe.pgto.vTroco;

                       string nomeAdquirinte = retSat.infCFe.emit.xNome;

                       corpo = corpo + "\r\n             T O T A L    R $   " + total;
                       string din = troco == null ? total:Convert.ToString(Convert.ToDouble(total) + Convert.ToDouble(troco));
                    
                       corpo = corpo + "\r\n             Dinheiro     R $   " + din;
                       corpo = corpo + "\r\n             Troco        R $   " + (troco == null ? "0.00" : troco);
                       corpo = corpo + "\r\n------------------------------------------------------";
                       if (nomeAdquirinte != null)
                       {
                           corpo = corpo + "\r\n" + centralizar(nomeAdquirinte, 54);
                       }
                       if (vImposto != null)
                       {
                           string passString = new StringBuilder("Valor aproximado dos tributos R$: ").Append(vImposto).ToString();
                           corpo = corpo + "\r\n" + centralizar(passString, 54);
                       }
                       corpo = corpo + "\r\n" + centralizar(" Volte Sempre ", 54);
                       corpo = corpo + "\r\n------------------------------------------------------";
                       string rodape = "";

                       string data = retSat.infCFe.ide.dEmi;
                       string hora = retSat.infCFe.ide.hEmi;

                       string idCFe = retSat.infCFe.Id.Substring(3) + " ";

                       string novoidCFe = idCFe;
                       
                       data = data.Substring(6, 2) + "/" + data.Substring(4, 2) + "/" + data.Substring(0, 4);
                       hora = hora.Substring(0, 2) + ":" + hora.Substring(2, 2) + ":" + hora.Substring(4);
                       
                       rodape = rodape + "\r\n" + centralizar(new StringBuilder("Número de Serie do Equipamento: ").Append(retSat.infCFe.ide.nserieSAT).ToString(), 54);
                       rodape = rodape + "\r\n              " + data + " - " + hora;
                       rodape = rodape + "\r\n" + novoidCFe;
                       string textoImpressao = cabecalho + corpo + rodape;
                       
                        textPrint = textoImpressao;
                       
                    
                        string imprimir = completarQRCode(assQRCode);

                        QRCode = imprimir;



                       String[] linhas = quebrarString(textoImpressao, "\r\n");
                       String[][] doc2 = new String[(linhas.Length + 64) / 65][];
                       int j = 0;

                       for (int k = 0; k < doc2.Length; k++)
                       {
                           if (linhas.Length - j > 65)
                           {
                               doc2[k] = new String[65];
                           }
                           else
                           {
                               doc2[k] = new String[linhas.Length - j];
                           }
                           for (int i2 = 0; (i2 < 65) && (j < linhas.Length); j++)
                           {
                               doc2[k][i2] = linhas[j]; i2++;
                           }
                       }


                      // new PrintCFe(idCFe, completarQRCode(impressao, assQRCode)).execute(textoImpressao);





                       
                       //MessageBox.Show(doc2.ToString());

                }

            }
           

        }
        private string quebrarLinha(string texto, int largura)
        {
            bool primeiraLinha = true;
            string linha = "";
            for (; ; )
            {
                if (texto.Length > 54)
                {
                    if (!primeiraLinha)
                    {
                        linha = linha + "\r\n";
                    }
                    
                    linha = linha + texto.Substring(0, 54);
                    texto = texto.Substring(54);
                }
                else
                {
                    if (!primeiraLinha)
                    {
                        linha = linha + "\r\n";
                    }
                    linha = linha + texto;
                    break;
                }
                primeiraLinha = false;
            }
            return linha;
        }

        private string centralizar(string texto, int largura)
        {

            


            if (texto.Length >= largura - 1)
            {
                return texto;
            }
            int n = (largura - 1) / 2 - texto.Length / 2;
            if (n >= 1)
            {
                string s = "";
                for (int i = 0; i < n; i++)
                {
                    s = s + " ";
                }
                texto = s + texto;
            }
            return texto;
        }
        private string inserirChar(int espaco, string valor, string caracter)
        {
            int start = 0;
            string novoValor = "";
            while (start + espaco < valor.Length)
            {
                novoValor = novoValor + valor.Substring(start, start + espaco) + caracter;
                start += espaco;
            }
            novoValor = novoValor + valor.Substring(start, start + espaco);
            return novoValor;
        }
        private string completarQRCode(string assQRCode)
        {
            string chaveConsulta = retSat.infCFe.Id;
            string timestamp = retSat.infCFe.ide.dEmi + retSat.infCFe.ide.hEmi;
            string valor = retSat.infCFe.total.vCFe;
            string destinatario = retSat.infCFe.ide.CNPJ;



            StringBuilder qrCode = new StringBuilder("");
            qrCode.Append(chaveConsulta).Append("|").Append(timestamp).Append("|").Append(valor).Append("|").Append(destinatario).Append("|").Append(assQRCode);

            return qrCode.ToString();
            
        }
        public static String[] quebrarString(String conteudo, String delimitador)
        {
            string cont = conteudo;
            List<string> linhas = new List<string>();

            cont.Split('\n');

            //while (cont.IndexOf(delimitador) != 1)
            //{
            //    linhas.Add(cont.Substring(0, cont.IndexOf(delimitador)));
            //    cont = cont.Substring(cont.IndexOf(delimitador) + delimitador.Length, cont.Length);
            //}
            //linhas.Add(cont);

            string[] linhasFinal = cont.Split('\n');

            //string[] linhasFinal = new string[linhas.Count];
            //for (int i = 0; i <= linhas.Count - 1; i++)
            //{
            //    linhasFinal[i] = linhas.ElementAt<string>(i);
            //}
            return linhasFinal;
        }
     

    }

    }

