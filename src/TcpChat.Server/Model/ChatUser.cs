namespace TcpChat.Server.Model
{
    public class ChatUser
    {
        public ChatUser(string username)
        {
            this.Username = username;
        }

        public string Username { get; set; }
    }
}
