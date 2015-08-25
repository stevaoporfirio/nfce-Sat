using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace invoiceServerApp
{
    public class StateObject
    {
        public Socket skt = null;
        public const int buffersize = 8192;
        public byte[] buffer = new byte[buffersize];
        public StringBuilder sb = new StringBuilder();
    }
    class ServerSocket//: NotificationDisconnect
    {
        private Socket socketListener;
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private Utils.ConfigureXml config;
        private NotificationChanged listener;
        public static Socket handler;
        

        public ServerSocket(Utils.ConfigureXml _Config, NotificationChanged pListener)
        {
            config = _Config;
            listener = pListener;
            //this.StartListening();
        }
        public void setListenerComunication(NotificationChanged pListener)
        {
            listener = pListener;
        }
        private string getIpLocalMachine()
        {
            if (!String.IsNullOrEmpty(config.configMaquina.IP))
                return config.configMaquina.IP;

            NetworkInterface[] a = NetworkInterface.GetAllNetworkInterfaces();
        
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                var addr = ni.GetIPProperties().GatewayAddresses.FirstOrDefault();
                if (addr != null)
                {
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {

                                return ip.Address.ToString();
                            }
                        }
                    }
                }
            }

            return "127.0.0.1";

        }
        public void StartListening() 
        {
            byte[] bytes = new Byte[8192];
            try
            {
                IPEndPoint myEndPoint = new IPEndPoint(IPAddress.Parse(getIpLocalMachine()), Convert.ToInt32(config.configMaquina.Porta));
                socketListener = new Socket(myEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socketListener.Bind(myEndPoint);
                socketListener.Listen((int)SocketOptionName.MaxConnections);
                while (true)
                {
                    allDone.Reset();
                    socketListener.BeginAccept(new AsyncCallback(AcceptCallback), socketListener);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                throw new Exception(e.ToString());
            }        
        }

        public void AcceptCallback(IAsyncResult ar) 
        {
            try
            {
                allDone.Set();
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);
                StateObject state = new StateObject();
                state.skt = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.buffersize, 0, new AsyncCallback(ReadCallback), state);
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
            }
        }
        public void ReadCallback(IAsyncResult ar)
        {
            try
            {
                String content = String.Empty;
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.skt;
                int bytesRead = handler.EndReceive(ar);
                string id = "";
                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    handler.BeginReceive(state.buffer, 0, StateObject.buffersize, 0, new AsyncCallback(ReadCallback), state);
                    content = state.sb.ToString();
                    if (content.Contains("|END|"))
                    {
                        content = content.Replace("|END|", "");
                        if (id == "")
                        {
                            id = content.Split('|')[0];
                            content = content.Remove(0, id.Length + 1);
                        }
                        ReceiveChanged(id, handler, content);
                        content = "";
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
            }
        }
        private void Send(Socket handler, String data)
        {
            try
            {
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
            }
        }
        private void ReceiveChanged(string _id, Socket socketClient, string msg)
        {
            string ret = listener.NotificationChangedClient(_id, msg);

            Send(socketClient, ret);
        }
    }       
}
