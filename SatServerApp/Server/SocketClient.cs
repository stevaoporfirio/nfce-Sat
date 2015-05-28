using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Windows.Forms;

namespace invoiceServerApp
{
    class SocketClient
    {
        private Socket socket;
        private byte[] buffer = new byte[1024];
        private NotificationDisconnect listenerDisconnect;
        private NotificationChanged listenerChanged;
        private string id = "";
        private string BufferCupom = "";
        
        public SocketClient(NotificationChanged pListener, NotificationDisconnect pNotification, Socket socket)
        {
            listenerDisconnect = pNotification;
            listenerChanged = pListener;
            this.socket = socket;
            StartReceive();
        }
        
        private void StartReceive()
        {
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), null);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            try
            {
                if (!socket.Connected)
                    Utils.Logger.getInstance.error("cliente desconect");
                String content = String.Empty;

                int bytesRead = socket.EndReceive(ar);

                if (bytesRead > 0)
                {
                    string strReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    BufferCupom += strReceived;

                    if (strReceived.Contains("|END|"))
                    {
                        BufferCupom = BufferCupom.Replace("|END|", "");
                        if (id == "")
                        {
                            id = BufferCupom.Split('|')[0];
                            BufferCupom = BufferCupom.Remove(0, id.Length + 1);
                        }
                        ReceiveChanged(BufferCupom);
                        BufferCupom = "";
                    }
                }

                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReadCallback), null);
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
                disconnectClient();
            }

        }

        private void Send(String data)
        {
            try
            {
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), null);
                    
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
                disconnectClient();
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
                disconnectClient();
            }
        }

        private void DisconnectCallback(IAsyncResult ar)
        {
            Utils.Logger.getInstance.error("evento de desconexão");
        }

        private void disconnectClient()
        {
            try
            {
                listenerDisconnect.notificationDisconnect(this);
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(true);
            }
            catch (Exception e)
            {
                Utils.Logger.getInstance.error(e.ToString());
            }
        }
        void ReceiveChanged(string msg)
        {
            string ret = listenerChanged.NotificationChangedClient(id, msg);
            Send(ret);
        }
    }
}
