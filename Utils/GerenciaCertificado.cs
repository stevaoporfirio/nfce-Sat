using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography.X509Certificates;

namespace Utils
{
    public class GerenciaCertificado
    {
        private static readonly GerenciaCertificado instance = new GerenciaCertificado();

        private ConfigureXml config;

        private X509Certificate2 cert;

        public static GerenciaCertificado Instance()
        {
            return instance;
        }

        private GerenciaCertificado()
        {
            //
        }

        public X509Certificate2 GetCertificado()
        {
            return cert;
        }

        public void SetCertificado(ConfigureXml _config)
        {
            config = _config;

            try
            {

                string certPath = config.configNFCe.CaminhoCertificadoDigital;

                string certPass = "";

                if (config.configNFCe.SenhaCertificadoDigital.Substring(0, 1).Equals("!"))
                    certPass = config.configNFCe.SenhaCertificadoDigital.Substring(1);
                else
                {
                    StringBuilder sBuilder = new StringBuilder();

                    string crip = config.configNFCe.SenhaCertificadoDigital;

                    Utils.Logger.getInstance.error(crip);

                    for (int i = 0; i < crip.Length; i++)
                    {
                        string s = String.Format("{0}{1}", crip[i], crip[i + 1]);
                        Utils.Logger.getInstance.error(s);
                        sBuilder.Append(Convert.ToChar(Convert.ToInt32(s)));
                        i++;
                    }
                    certPass = sBuilder.ToString();
                }

                X509Certificate2Collection collection = new X509Certificate2Collection();
                collection.Import(certPath, certPass, X509KeyStorageFlags.PersistKeySet);


                //X509Store store = new X509Store("My", StoreLocation.CurrentUser);
                //store.Open(OpenFlags.ReadOnly);
                //X509Certificate2Collection collection2 = (X509Certificate2Collection)store.Certificates;
                X509Certificate2Collection listaCertificados = collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, true); //X509FindType.FindByKeyUsage

                //store.Close();
                if (listaCertificados.Count > 0)
                {
                    foreach (var c in listaCertificados)
                    {
                        string name = c.Subject;

                        //string[] names = name.Split(',');
                        //if (names[0].Substring(3).Equals(config.configNFCe.NomeCertificadoDigital))
                        {
                            cert = c;
                            break;
                        }
                    }
                }
                else
                    throw new Exception("Nenhum certificado encontrado");

                if (cert == null)
                    throw new Exception("Certificado não encontrado:" + config.configNFCe.NomeCertificadoDigital);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro em GetCertificados\n" + ex.Message);
            }
        }

    }
}
