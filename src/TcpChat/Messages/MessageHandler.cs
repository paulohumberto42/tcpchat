using System;

namespace TcpChat.Messages
{
    public abstract class MessageHandler<T, TMessage> : IMessageHandler<T>
        where TMessage : Message
    {
        public abstract void HandleMessage(T receiver, string sessionId, TMessage message);

        void IMessageHandler<T>.HandleMessage(T receiver, string sessionId, Message message)
        {
            if (message is TMessage typedMessage)
            {
                this.HandleMessage(receiver, sessionId, typedMessage);
            }
            else
            {
                throw new ArgumentException(
                    $"The message handler {this.GetType().FullName} expects a message of type {typeof(TMessage).FullName}");
            }
        }
    }
}
