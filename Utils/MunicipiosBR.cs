using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class Cep2IBGE
    {
        private Brasil brasil;

        public Cep2IBGE()
        {
            brasil = new Brasil();

            LoadXMLMunicipios();
        }

        private void LoadXMLMunicipios()
        {
            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Brasil));
            System.IO.StreamReader file = new System.IO.StreamReader(@"MunicipioBR.xml");
            brasil = (Brasil)reader.Deserialize(file);
            file.Close();
        }

        public string FindIBGE(string cep)
        {
            int iCep = Convert.ToInt32(cep.Substring(0, 5));

            var qMunicipio = from munic in brasil.municipiosBR.AsEnumerable()
                             where (
                                    (iCep >= munic.CEP1Inicial && iCep <= munic.CEP1Final) ||
                                    (iCep >= munic.CEP2Inicial && iCep <= munic.CEP2Final) ||
                                    (iCep >= munic.CEP3Inicial && iCep <= munic.CEP3Final)
                                   )
                             select new
                             {
                                 ibge = munic.CodMunicipioIBGECompleto
                             };
            
            if (qMunicipio == null)
                throw new Exception("CEP Nao Encontrado");
            else
                return qMunicipio.FirstOrDefault().ibge;   
        }
    }

    public class Brasil
    {
        public List<MunicipiosBR> municipiosBR { get; set; }

        public Brasil()
        {
            municipiosBR = new List<MunicipiosBR>();
        }
    }

    public class MunicipiosBR
    {
        public String UFId { get; set; }
        public String Municipio { get; set; }
        public String MunicipioEx { get; set; }
        public String CodMunicipioIBGECompleto { get; set; }
        public String CodMunicipioIBGE { get; set; }
        public String CodMunicipioSIAF { get; set; }
        public String CodMunicipioSINP { get; set; }
        
        public int CEP1Inicial { get; set; }
        public int CEP1Final { get; set; }
        public int CEP2Inicial { get; set; }
        public int CEP2Final { get; set; }
        public int CEP3Inicial { get; set; }
        public int CEP3Final { get; set; }
    }
}
