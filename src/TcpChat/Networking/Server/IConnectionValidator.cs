using System;
using System.Collections.Generic;
using System.Text;
using TcpChat.Networking.Shared;

namespace TcpChat.Networking.Server
{
    public interface IConnectionValidator
    {
        ConnectionResult ValidateConnection(byte[] message, out string sessionId);
    }
}
