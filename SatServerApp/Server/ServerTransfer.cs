using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace invoiceServerApp
{
    class ServerTransfer
    {
        private IPEndPoint ipEnd;
        private Socket socket;
        private Utils.ConfigureXml config;

        public ServerTransfer(Utils.ConfigureXml pConfig)
        {
            try
            {
                config = pConfig;
                Thread thread = new Thread(StartSever);
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
            }

        }
         
        private void StartSever()
        {
            try
            {

                ipEnd = new IPEndPoint(IPAddress.Parse(config.configMaquina.IP), Convert.ToInt32(config.configMaquina.Porta));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                socket.Bind(ipEnd);
                socket.Listen(Convert.ToInt32(config.configMaquina.Porta));

                while (true)
                {
                    try
                    {
                        Socket clientSock = socket.Accept();
                        HandleTransferClient client = new HandleTransferClient(clientSock);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
            }
        
        }
        public void Disconnected()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(true);
        }

    }
    }

