using System;
using System.Net.Sockets;

namespace TcpChat.Networking.Server
{
    internal class ClientSession
    {
        private readonly TcpClient client;
        private readonly byte[] buffer;

        public ClientSession(string sessionId, TcpClient client)
        {
            this.SessionId = sessionId;
            this.client = client;
            this.buffer = new byte[client.ReceiveBufferSize];
        }

        public string SessionId { get; }

        public void WaitForMessage(AsyncCallback callback)
        {
            this.client.Client.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, callback, this);
        }

        public void SendMessage(byte[] message)
        {
            this.client.Client.Send(message);
        }

        public byte[] ReceiveMessage(IAsyncResult result)
        {
            int length = this.client.Client.EndReceive(result);
            var message = new byte[length];
            Array.Copy(this.buffer, message, length);
            return message;
        }

        public void Disconnect()
        {
            this.client.Client.Close();
        }
    }
}
