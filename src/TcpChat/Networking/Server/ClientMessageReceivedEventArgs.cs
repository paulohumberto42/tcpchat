namespace TcpChat.Networking.Server
{
    public class ClientMessageReceivedEventArgs
    {
        public ClientMessageReceivedEventArgs(string sessionId, byte[] message)
        {
            this.SessionId = sessionId;
            this.Message = message;
        }

        public string SessionId { get; }

        public byte[] Message { get; }
    }
}
