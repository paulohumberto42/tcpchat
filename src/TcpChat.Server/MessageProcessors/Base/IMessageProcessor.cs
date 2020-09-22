using TcpChat.Messages;

namespace TcpChat.Server.MessageProcessors.Base
{
    public interface IMessageProcessor
    {
        public void HandleMessage(string username, Message message);
    }
}
