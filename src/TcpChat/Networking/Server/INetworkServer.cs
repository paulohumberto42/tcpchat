using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TcpChat.Networking.Server
{
    public interface INetworkServer : IDisposable
    {
        event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;

        event EventHandler<ClientConnectedEventArgs> ClientConnected;

        event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;

        void Start(IPAddress address, int port, IConnectionValidator connectionValidator);

        void Stop();

        bool SendMessage(string sessionId, byte[] message);
    }
}
