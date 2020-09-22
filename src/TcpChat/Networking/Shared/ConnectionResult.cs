using System;
using System.Collections.Generic;
using System.Text;

namespace TcpChat.Networking.Shared
{
    public class ConnectionResult
    {
        public ConnectionResult()
        {
        }

        public ConnectionResult(bool success, byte[] data)
        {
            this.Success = success;
            this.Data = data;
        }

        public bool Success { get; set; }

        public byte[] Data { get; set; }
    }
}
