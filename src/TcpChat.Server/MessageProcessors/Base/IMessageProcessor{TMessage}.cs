using TcpChat.Messages;

namespace TcpChat.Server.MessageProcessors.Base
{
    public interface IMessageProcessor<TMessage> : IMessageProcessor
        where TMessage : Message
    {
        public void HandleMessage(string username, TMessage message);
    }
}
