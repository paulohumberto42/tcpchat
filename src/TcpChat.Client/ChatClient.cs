using System;
using System.Net;
using System.Text;
using TcpChat.Messages;
using TcpChat.Networking.Client;
using TcpChat.Networking.Shared;

namespace TcpChat.Client
{
    public class ChatClient
    {
        private readonly INetworkClient networkClient;

        public ChatClient(INetworkClient networkClient)
        {
            this.networkClient = networkClient;
            this.networkClient.ServerMessageReceived += this.OnServerMessageReceived;
        }

        public string Username { get; private set; }

        public event EventHandler<Message> MessageReceived;

        public bool IsConnected => this.networkClient.IsConnected;

        public ConnectionResult Connect(IPAddress address, int port, string username)
        {
            var result = this.networkClient.Connect(address, port, Encoding.UTF8.GetBytes(username));

            if (result.Success)
            {
                this.Username = username;
            }

            return result;
        }

        public void Disconnect()
        {
            this.networkClient.Disconnect();
        }

        public void SendMessage(Message message)
        {
            this.networkClient.SendMessage(message.Serialize());
        }

        private void OnServerMessageReceived(object sender, ServerMessageReceivedEventArgs e)
        {
            var message = Message.Deserialize(e.Message);
            this.MessageReceived?.Invoke(this, message);
        }
    }
}
