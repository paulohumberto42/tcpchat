namespace TcpChat.Networking.Server
{
    public class ClientConnectedEventArgs
    {
        public string SessionId { get; }

        public ClientConnectedEventArgs(string sessionId)
        {
            this.SessionId = sessionId;
        }
    }
}
