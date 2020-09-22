using TcpChat.Messages;

namespace TcpChat.Server.MessageProcessors.Base
{
    public abstract class MessageProcessor<TMessage> : IMessageProcessor<TMessage>
        where TMessage : Message
    {
        public abstract void HandleMessage(string username, TMessage message);

        public void HandleMessage(string username, Message message)
        {
            this.HandleMessage(username, (TMessage)message);
        }
    }
}
