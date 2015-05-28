using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace invoiceServerApp
{
    public class ConfigureContigencia
    {
        
        public string Cod_estado { get; set; }
        public string TpAmb { get; set; }
        public string VersaoLayout{ get; set; }
        public string Uf { get; set; }
        public string NomeCertificadoDigital { get; set;}
        public string pathFiles { get; set; }

        public ConfigureContigencia()
        {
            setData();
        }
        public void setData()
        {
            foreach (System.Reflection.PropertyInfo a in this.GetType().GetProperties())
            {
                if (a.PropertyType == typeof(bool))
                    a.SetValue(this, false, null);
                else if (a.PropertyType == typeof(string))
                    a.SetValue(this, a.Name, null);
            }
        }
    }
}
