using System;
using System.Collections.Generic;
using System.Text;

namespace TcpChat.Networking.Client
{
    public class ServerMessageReceivedEventArgs
    {
        public ServerMessageReceivedEventArgs(byte[] message)
        {
            this.Message = message;
        }

        public byte[] Message { get; }
    }
}
