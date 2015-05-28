using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace invoiceServerApp
{
    class HandleTransferClient
    {
        
        private Socket clientSock;
        private string receivedPath = "e:\\";
        private Thread thread;
        public HandleTransferClient(Socket pClientSock)
        {
            clientSock = pClientSock;

            thread = new Thread(processHandle);
            thread.Start();
            
        }
        private void processHandle()
        {

            try
            {
                NetworkStream myns = new NetworkStream(clientSock);
                BinaryFormatter br = new BinaryFormatter();
                object op = br.Deserialize(myns); // Deserialize the Object from Stream


                string fileName = op.ToString();
                while (fileName.IndexOf("\\") > -1)
                {
                    fileName = fileName.Substring(fileName.IndexOf("\\") + 1);
                }


                BinaryReader bb = new BinaryReader(myns);
                byte[] buffer = bb.ReadBytes(5000000);

                FileStream fss = new FileStream(receivedPath + fileName, FileMode.OpenOrCreate, FileAccess.Write);
                fss.Write(buffer, 0, buffer.Length);
                fss.Close();
                clientSock.Close();
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
            }
        }
    }
}
