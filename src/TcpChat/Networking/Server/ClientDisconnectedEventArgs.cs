namespace TcpChat.Networking.Server
{
    public class ClientDisconnectedEventArgs
    {
        public string SessionId { get; }

        public ClientDisconnectedEventArgs(string sessionId)
        {
            this.SessionId = sessionId;
        }
    }
}
