using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace invoiceServerApp
{

    class qrCodeClass
    {
        public string chavedeacesso;
        public string versaoQrCode;
        public string amb;
        public string cpf_cnpj;
        public string data;
        public string valorTotal;
        public string valorICMS;
        public string digestValue;
        public string CSC;
        public string token;
        
        public string qrcode;

        public void MontaQrCode()
        {
            string nota = "";
            data = stringToHex(data);
            digestValue = stringToHex(digestValue);

            nota = "chNFe=" + chavedeacesso;
            nota += "&nVersao=" + versaoQrCode;
            nota += "&tpAmb=" + amb;
            nota += "&cDest=" + cpf_cnpj;
            nota += "&dhEmi=" + data;
            nota += "&vNF=" + valorTotal;
            nota += "&vICMS=" + valorICMS;
            nota += "&digVal=" + digestValue;
            nota += "&cIdToken=" + CSC;

            //string url = "http://homologacao.sefaz.mt.gov.br/nfce/consultanfce?" + nota;
            string url = "http://www.sefaz.mt.gov.br/nfce/consultanfce?" + nota;
            nota += token;
            qrcode = gerarHash(nota);
            url += "&cHashQRCode=" + qrcode;
            qrcode = url;

        }

        private string stringToHex(string dado)
        {
            char[] values = dado.ToCharArray();
            string hexOutput = "";
            foreach (char letter in values)
            {
                int value = Convert.ToInt32(letter);
                hexOutput += String.Format("{0:x}", value);
            }
            return hexOutput;
        }

        private string gerarHash(string str)
        {
            SHA1 hasher = SHA1.Create();
            StringBuilder gerarString = new StringBuilder();
            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] array = encoding.GetBytes(str);
            array = hasher.ComputeHash(array);
            foreach (byte item in array)
            {
                gerarString.Append(item.ToString("x2"));
            }

            return gerarString.ToString();
        }

    }
    class itensNota
    {
        public string codigo;
        public string descricao;
        public string quantidade;
        public string unidade;
        public string valorUnidade;
        public string valorTributo;
        public string valorItem;

    }

    class DadosImpressao
    {

        public string nomeFantasia;
        public string razao;
        public string CNPJ;
        public string bairro;
        public string cidade;
        public string cpf_cnpj_destinatario;
        public string logradouro;
        public string numero;
        public string IE;
        public string IM;
        public string endereco;
        public string municipio;
        public List<itensNota> itens = new List<itensNota>();

        private int length = 49;
        public string notaFiscal;
        public double valorTotal;
        public double valorTotalImposto;

        public void parse()
        {
            notaFiscal = center(this.nomeFantasia);
            notaFiscal += center(this.razao);
            notaFiscal += center(this.logradouro + ", " + this.numero + " - " + this.bairro + " - " + this.cidade);
            notaFiscal += "\r\n\r\n";

            string printCnpj = String.Format(@"{0:00\.000\.000\/0000\-00}", Convert.ToUInt64(this.CNPJ));

            notaFiscal += center("CNPJ: " + printCnpj + " IE: " + this.IE);
            notaFiscal += center("IM: " + this.IM);
            notaFiscal += "\r\n";
            notaFiscal += center("DANFE NFC-e - Documento Auxiliar da Nota Fiscal");
            notaFiscal += center("de Consumidor Eletrônica");
            notaFiscal += "\r\n";
            notaFiscal += center("CPF/CNPJ CONSUMIDOR: " + this.cpf_cnpj_destinatario);
            notaFiscal += "\r\n";
            notaFiscal += center("#|COD|DESC|QTD|UN| VL UN R$|(VLTR R$)*| VL ITEM R");


            int count = 0;
            string linha = "";
            foreach (itensNota item in this.itens)
            {
                count++;
                linha = Convert.ToString(count).PadLeft(3, '0');
                linha += " ";
                linha += item.codigo.PadRight(5, '0');
                linha += " ";
                linha += item.descricao.PadLeft(16, ' ');
                linha += " ";
                linha += item.quantidade.PadLeft(5, ' ');
                linha += " ";
                linha += item.unidade.PadLeft(2, ' ');
                linha += " ";
                linha += item.valorUnidade.PadLeft(5, ' ');
                linha += " ";
                linha += item.valorTributo.PadRight(2, '0') + "%";
                linha += " ";
                linha += item.valorUnidade.PadRight(4, ' ');
                notaFiscal += center(linha);

                //valorTotal += Convert.ToDouble(item.valorUnidade);
                //valorTotalImposto += Convert.ToDouble(item.valorTributo);
            }

            string total = Convert.ToString(valorTotal).PadLeft(41, ' ');
            notaFiscal += "\r\n";
            notaFiscal += "Total R$" + total;
        }
        private string center(string str)
        {
            int spaces = length - str.Length;
            int padLeft = spaces / 2;
            string retorno = str.PadLeft(padLeft + str.Length, ' ');
            return retorno + "\n";
        }

    }
}
