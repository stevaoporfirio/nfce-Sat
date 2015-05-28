using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace invoiceServerApp
{
    class ClientTransfer
    {
        private string path = "E:\\teste\\test.txt";
        private Utils.ConfigureXml config;

        public ClientTransfer(Utils.ConfigureXml pConfig)
        {
            config = pConfig;
            
        }
        public void sendFile(String Pathfile)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                byte[] buffer = new byte[fs.Length];
                int len = (int)fs.Length;
                fs.Read(buffer, 0, len);
                fs.Close();


                BinaryFormatter br = new BinaryFormatter();
                TcpClient myclient = new TcpClient(config.configMaquina.IP, Convert.ToInt32(config.configMaquina.Porta));
                NetworkStream myns = myclient.GetStream();
                br.Serialize(myns, path);

                BinaryWriter mysw = new BinaryWriter(myns);
                mysw.Write(buffer);
                mysw.Close();

                myns.Close();
                myclient.Close();
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
            }
        }
    }
}
