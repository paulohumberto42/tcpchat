using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TcpChat.Messages;
using TcpChat.Networking.Shared;

namespace TcpChat.Networking.Client
{
    public class NetworkClient : INetworkClient, IDisposable
    {
        private TcpClient tcpClient;
        private byte[] buffer;
        private bool disposedValue;

        public event EventHandler<ServerMessageReceivedEventArgs> ServerMessageReceived;

        public bool IsConnected => this.tcpClient != null && this.tcpClient.Connected;
         
        public ConnectionResult Connect(IPAddress ipAddress, int port, byte[] loginData)
        {
            if (this.tcpClient != null)
            {
                this.tcpClient.Close();
            }

            this.tcpClient = new TcpClient();
            this.buffer = new byte[this.tcpClient.ReceiveBufferSize];

            // Conecta no servidor
            this.tcpClient.Connect(ipAddress, port);

            ConnectionResult result = this.Handshake(loginData);

            if (result.Success)
            {
                this.WaitForMessages();
            }
            else
            {
                this.tcpClient.Close();
            }

            return result;
        }

        private ConnectionResult Handshake(byte[] loginData)
        {
            // Envia payload de login
            this.tcpClient.Client.Send(loginData);

            var result = new ConnectionResult();

            // Recebe o boolean de sucesso
            this.tcpClient.Client.Receive(this.buffer);
            result.Success = BitConverter.ToBoolean(this.buffer);

            // Recebe os dados de login
            int resultLength = this.tcpClient.Client.Receive(this.buffer);
            result.Data = new byte[resultLength];
            Array.Copy(this.buffer, result.Data, resultLength);
            return result;
        }

        public void Disconnect()
        {
            if (this.tcpClient.Connected)
            {
                this.tcpClient.Close();
            }
        }

        public void SendMessage(byte[] message)
        {
            this.tcpClient.Client.Send(message);
        }

        private void WaitForMessages()
        {
            this.tcpClient.Client.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, OnMessageReceived, null);
        }

        private void OnMessageReceived(IAsyncResult result)
        {

            int length = 0;

            try
            {
                length = this.tcpClient.Client.EndReceive(result);
            }
            catch (Exception)
            {
                if (!this.IsConnected)
                {
                    return;
                }
            }

            var message = new byte[length];
            Array.Copy(this.buffer, message, length);
            this.ServerMessageReceived?.Invoke(this, new ServerMessageReceivedEventArgs(message));

            this.WaitForMessages();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.tcpClient?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
