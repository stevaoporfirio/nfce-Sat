using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace Utils
{
    public class XmlFinal
    {
        
        //private XmlDocument xmlFinal = new XmlDocument();
     
        public XmlFinal()
        {

        }
        public XmlDocument DanfeFinal(XmlDocument _resp, XmlDocument _nota)
        {

 
            XDeclaration declaration = new XDeclaration("1.0", "utf-8", "yes");
            XmlDocument xmlFinal = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlFinal.CreateXmlDeclaration("1.0", "utf-8", null);



            XmlNode nodeDanfe = _nota.GetElementsByTagName("NFe")[0];
            XmlNode retNode = _resp.GetElementsByTagName("protNFe")[0];


            XmlElement rootNode = xmlFinal.CreateElement("nfeProc");
            XmlAttribute xtv = xmlFinal.CreateAttribute("versao");
            xtv.Value= "3.10";
            XmlAttribute xt = xmlFinal.CreateAttribute("xmlns");
            xt.Value= "http://www.portalfiscal.inf.br/nfe";
            rootNode.Attributes.Append(xtv);
            rootNode.Attributes.Append(xt);
            
            xmlFinal.InsertBefore(xmlDeclaration, xmlFinal.DocumentElement);
            rootNode.InnerXml += nodeDanfe.OuterXml;
            rootNode.InnerXml += retNode.OuterXml;

            xmlFinal.AppendChild(rootNode);

            xmlFinal.PreserveWhitespace = true;

            return xmlFinal;
        }


        public XmlDocument DanfeFinalCancel(XmlDocument _resp, XmlDocument _nota)
        {


            XDeclaration declaration = new XDeclaration("1.0", "utf-8", "yes");
            XmlDocument xmlFinal = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlFinal.CreateXmlDeclaration("1.0", "utf-8", null);



            XmlNode nodeDanfe = _nota.GetElementsByTagName("evento")[0];
            XmlNode retNode = _resp.GetElementsByTagName("retEvento")[0];


            XmlElement rootNode = xmlFinal.CreateElement("procEventoNFe");
            XmlAttribute xtv = xmlFinal.CreateAttribute("versao");
            xtv.Value = "1.00";
            XmlAttribute xt = xmlFinal.CreateAttribute("xmlns");
            xt.Value = "http://www.portalfiscal.inf.br/nfe";
            rootNode.Attributes.Append(xtv);
            rootNode.Attributes.Append(xt);

            xmlFinal.InsertBefore(xmlDeclaration, xmlFinal.DocumentElement);
            rootNode.InnerXml += nodeDanfe.OuterXml;
            rootNode.InnerXml += retNode.OuterXml;

            xmlFinal.AppendChild(rootNode);

            xmlFinal.PreserveWhitespace = true;

            return xmlFinal;
        }

        public XmlDocument DanfeFinalInutilizacao(XmlDocument _resp, XmlDocument _nota)
        {


            XDeclaration declaration = new XDeclaration("2.0", "utf-8", "yes");
            XmlDocument xmlFinal = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlFinal.CreateXmlDeclaration("1.0", "utf-8", null);



            XmlNode nodeDanfe = _nota.GetElementsByTagName("inutNFe")[0];
            XmlNode retNode = _resp.GetElementsByTagName("retInutNFe")[0];


            XmlElement rootNode = xmlFinal.CreateElement("ProcInutNFe");
            XmlAttribute xtv = xmlFinal.CreateAttribute("versao");
            xtv.Value = "3.10";
            XmlAttribute xt = xmlFinal.CreateAttribute("xmlns");
            xt.Value = "http://www.portalfiscal.inf.br/nfe";
            rootNode.Attributes.Append(xtv);
            rootNode.Attributes.Append(xt);

            xmlFinal.InsertBefore(xmlDeclaration, xmlFinal.DocumentElement);
            rootNode.InnerXml += nodeDanfe.OuterXml;
            rootNode.InnerXml += retNode.OuterXml;

            xmlFinal.AppendChild(rootNode);

            xmlFinal.PreserveWhitespace = true;

            return xmlFinal;
        }


        private void ProcessNodes(XmlNode nodes, XmlNode pai, XmlDocument xmlFinal)
        {

            foreach (XmlNode node in nodes.ChildNodes)
            {
                XmlNode xn = null;

                if (node.Name.Equals("Signature"))
                {
                    xn = xmlFinal.CreateNode(XmlNodeType.Element, node.Name, null);
                    XmlAttribute xnSigAtrib = xmlFinal.CreateAttribute("xmlns");
                    xnSigAtrib.Value = node.NamespaceURI;
                    xn.Attributes.Append(xnSigAtrib);
                }
                else
                {
                    xn = xmlFinal.CreateNode(XmlNodeType.Element, node.Name, null);
                }


                foreach (XmlAttribute att in node.Attributes)
                {
                    XmlAttribute xnA = xmlFinal.CreateAttribute(att.Name);

                    xnA.Value = att.Value;

                    xn.Attributes.Append(xnA);
                }

                pai.AppendChild(xn);
                

                if ((node.HasChildNodes) && (!node.FirstChild.Name.Equals("#text")))
                {
                    ProcessNodes(node, xn, xmlFinal);
                }
                else
                {
                    if ((node.InnerXml.Trim().Length > 0) || (!String.IsNullOrEmpty(node.InnerXml)))
                        xn.InnerText = node.InnerText;
                }
            }
        }
        public XmlDocument DanfeFinalCancelamento(XmlDocument _resp, XmlDocument _nota)
        {
            StringWriter xmlS = new StringWriter();
            XmlTextWriter w = new XmlTextWriter(xmlS);
            w.WriteStartDocument();
            w.WriteStartElement("procCancNFe", "http://www.portalfiscal.inf.br/nfe");
            w.WriteAttributeString("versao", "1.00");
            w.WriteEndElement();
            w.Close();


            XmlDocument xmlFinal = new XmlDocument();

            

            xmlFinal.LoadXml(xmlS.ToString());

            XmlNode xnNFe = xmlFinal.CreateNode(XmlNodeType.Element, "cancNFe", null);
            xmlFinal.LastChild.AppendChild(xnNFe);

            XmlNode xnProt = xmlFinal.CreateNode(XmlNodeType.Element, "retCancNfe", null);
            xmlFinal.LastChild.AppendChild(xnProt);

            XmlNode nodeDanfe = _nota.GetElementsByTagName("cancNFe")[0];
            XmlNode retNode = _resp.GetElementsByTagName("retCancNfe")[0];


            ProcessNodes(nodeDanfe, xmlFinal.GetElementsByTagName("cancNFe")[0], xmlFinal);

            ProcessNodes(retNode, xmlFinal.GetElementsByTagName("retCancNfe")[0], xmlFinal);


            return xmlFinal;
        }
    }
}
