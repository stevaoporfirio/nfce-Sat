using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace NFCE_Status
{

    public class ClientSocket
    {

        private byte[] buffer = new byte[8192];
        public Socket socket = null;
        public string Ip;
        public string Porta;
        private bool connected = false;
        public ClientSocket(string _ip, string _port)
        {
            Ip = _ip;
            Porta = _port;
            connectedClient();

        }
        private void connectedClient()
        {
            try
            {

                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(Ip), Convert.ToInt32(Porta));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), socket);

                connected = true;
               // socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
                
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
                DisconnectClient();
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
                DisconnectClient();
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                socket = client;
                client.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {

            try
            {
                String content = String.Empty;

                int bytesRead = socket.EndReceive(ar);

                if (bytesRead > 0)
                {
                    string strReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    MessageBox.Show(strReceived);
                }

                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                DisconnectClient();
            }
        }

        public void Send(String data)
        {
            try
            {
                if (connected == true)
                {
                    byte[] byteData = Encoding.ASCII.GetBytes(data);

                    socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), null);
                }
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e);
                DisconnectClient();
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                socket.EndSend(ar);
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
                DisconnectClient();
            }
        }
        public void DisconnectClient()
        {
            try
            {
                connected = false;
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket = null;
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
            }
            finally 
            {
                Thread.Sleep(1000);
                connectedClient();
            }
        }
    }
}
    

