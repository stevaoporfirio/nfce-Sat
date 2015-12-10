using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Threading;

namespace enviaBK
{
    public partial class Form1 : Form
    {
        private Utils.ConfigureXml config;
        private EnviaSeFaz.ManagerSeFaz enviar;
        public X509Certificate2 cert = null;
        public Form1()
        {
            InitializeComponent();
        }
        private string CalcularDigitoVerificador(string chaveAcessoParcial)
        {
            try
            {
                int soma = 0;
                int resto = 0;
                int[] peso = { 4, 3, 2, 9, 8, 7, 6, 5 };
                int digitoRetorno;

                for (int i = 0; i < chaveAcessoParcial.Length; i++)
                    soma += peso[i % 8] * (int.Parse(chaveAcessoParcial.Substring(i, 1)));

                resto = soma % 11;
                if (resto == 0 || resto == 1)
                    digitoRetorno = 0;
                else
                    digitoRetorno = 11 - resto;

                string digitoVerificador = digitoRetorno.ToString();

                return digitoVerificador;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public void assinarxml(XmlDocument _xml)
        {
            Reference reference = new Reference();
            SignedXml docXML = new SignedXml(_xml);

            docXML.SigningKey = cert.PrivateKey;
            XmlAttributeCollection uri = _xml.GetElementsByTagName("infNFe").Item(0).Attributes;
            foreach (XmlAttribute atributo in uri)
            {
                if (atributo.Name == "Id")
                    reference.Uri = "#" + atributo.InnerText;
            }

            XmlDsigEnvelopedSignatureTransform envelopedSigntature = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(envelopedSigntature);

            XmlDsigC14NTransform c14Transform = new XmlDsigC14NTransform();
            reference.AddTransform(c14Transform);
            docXML.AddReference(reference);

            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(cert));

            docXML.KeyInfo = keyInfo;
            docXML.ComputeSignature();

            XmlElement xmlDigitalSignature = docXML.GetXml();

            foreach (var _nfe in _xml.GetElementsByTagName("NFe").Cast<XmlElement>())
                _nfe.AppendChild(_xml.ImportNode(xmlDigitalSignature, true));


            _xml.PreserveWhitespace = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                config = new Utils.ReadConfigure().GetConfig();
                Utils.GerenciaCertificado.Instance().SetCertificado(config);
                cert = Utils.GerenciaCertificado.Instance().GetCertificado();
                enviar = new EnviaSeFaz.ManagerSeFaz(config, cert);


                string[] filePaths = Directory.GetFiles(@"E:\\Projetos Extras\\erro estrutura xml\\xmlRj\\contingencia", "*.xml");
                foreach (string xmlString in filePaths)
                {
                    

                    XmlDocument xml = new XmlDocument();
                    XmlDocument xmlRep = new XmlDocument();



                    xmlRep.Load(xmlString);



                    string namfile = Path.GetFileNameWithoutExtension(xmlString);

                    string newName = namfile.Remove(0, 4);
                    newName = "3" + newName;



                    string digitoVerificadorOld = newName.Remove(0, newName.Length - 1);
                    newName = newName.Remove(newName.Length - 1, 1);

                    string digitoVerificador = CalcularDigitoVerificador(newName);

                    newName = "NFe" + newName + digitoVerificador;

                    //MessageBox.Show(newName);

                    string StrXml = xmlRep.InnerXml;
                    StrXml = StrXml.Replace("4314902", "3303302");
                    StrXml = StrXml.Replace("<cUF>43</cUF>", "<cUF>33</cUF>");
                    StrXml = StrXml.Replace("<cDV>" + digitoVerificadorOld + "</cDV>", "<cDV>" + digitoVerificador + "</cDV>");

                    StrXml = StrXml.Replace(namfile, newName);

                    xmlRep.InnerXml = StrXml;

                    xml = xmlRep;

                    XmlNode nodeAss = xml.DocumentElement.GetElementsByTagName("Signature")[0];
                    if (nodeAss != null)
                        nodeAss.ParentNode.RemoveChild(nodeAss);

                    XmlNode nodeAut = xml.DocumentElement.GetElementsByTagName("protNFe")[0];
                    if (nodeAut != null)
                        nodeAut.ParentNode.RemoveChild(nodeAut);


                    assinarxml(xml);
                    try
                    {
                         enviar.enviaSefaz(xml);
                   
                        if (enviar.ConsultaContingencia(enviar.GetRecibo()))
                        {
                            XmlDocument xmlfinal = enviar.GetXmlOK();
                            xmlfinal.Save("E:\\Projetos Extras\\erro estrutura xml\\xmlRj\\enviado\\" + newName + ".xml");
                        }
                        else
                        {
                            xml.Save("E:\\Projetos Extras\\erro estrutura xml\\xmlRj\\rejeitado\\" + newName + ".xml");
                        }
                    }
                    catch (Exception ex)
                    {
                        xml.Save("E:\\Projetos Extras\\erro estrutura xml\\xmlRj\\rejeitado\\" + newName + ".xml");
                    }

                    Thread.Sleep(60);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            config = new Utils.ReadConfigure().GetConfig();
            Utils.GerenciaCertificado.Instance().SetCertificado(config);
            cert = Utils.GerenciaCertificado.Instance().GetCertificado();
            enviar = new EnviaSeFaz.ManagerSeFaz(config, cert);




            string[] filePaths = Directory.GetFiles(@"E:\\Projetos Extras\\erro estrutura xml\\xml Parana\\enviar\\rejeitados\\rejeitados", "*.xml");


            foreach (string xmlString in filePaths)
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(xmlString);

                string namfile = Path.GetFileNameWithoutExtension(xmlString);

                string StrXml = xml.InnerXml;

                StrXml = StrXml.Replace("nfeProc", "enviNFe");
                StrXml = StrXml.Replace("<NFe", "<idLote>1</idLote><indSinc>0</indSinc><NFe");

                xml.InnerXml = StrXml;


                XmlNode nodeAss = xml.DocumentElement.GetElementsByTagName("Signature")[0];
                if (nodeAss != null)
                    nodeAss.ParentNode.RemoveChild(nodeAss);

                XmlNode nodeAut = xml.DocumentElement.GetElementsByTagName("protNFe")[0];
                if (nodeAut != null)
                    nodeAut.ParentNode.RemoveChild(nodeAut);

                assinarxml(xml);
                try
                {
                    enviar.enviaSefaz(xml);

                    if (enviar.ConsultaContingencia(enviar.GetRecibo()))
                    {
                        XmlDocument xmlfinal = enviar.GetXmlOK();
                        xmlfinal.Save("E:\\Projetos Extras\\erro estrutura xml\\xml Parana\\enviar\\envRej\\" + namfile + ".xml");
                    }
                    else
                    {
                        xml.Save("E:\\Projetos Extras\\erro estrutura xml\\xml Parana\\enviar\\rejeitados\\" + namfile + ".xml");
                    }
                }
                catch (Exception ex)
                {
                    xml.Save("E:\\Projetos Extras\\erro estrutura xml\\xml Parana\\enviar\\rejeitados\\" + namfile + ".xml");
                }

                Thread.Sleep(60);


                

            }
        }
    }
}
