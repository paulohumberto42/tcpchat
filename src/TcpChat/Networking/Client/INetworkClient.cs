using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TcpChat.Networking.Shared;

namespace TcpChat.Networking.Client
{
    public interface INetworkClient
    {
        bool IsConnected { get; }

        event EventHandler<ServerMessageReceivedEventArgs> ServerMessageReceived;

        ConnectionResult Connect(IPAddress address, int port, byte[] loginData);

        void Disconnect();

        void SendMessage(byte[] message);
    }
}
