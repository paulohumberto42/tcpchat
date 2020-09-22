using System;
using System.Collections.Generic;
using System.Text;

namespace TcpChat.Messages
{
    public class MessageHandlerProvider<T>
    {
        private Dictionary<Type, IMessageHandler<T>> messageHandlers;

        public MessageHandlerProvider()
        {
            this.messageHandlers = new Dictionary<Type, IMessageHandler<T>>();
        }

        public MessageHandlerProvider<T> RegisterMessageHandler<TMessage>(MessageHandler<T, TMessage> messageHandler)
            where TMessage : Message
        {
            this.messageHandlers[typeof(TMessage)] = messageHandler;
            return this;
        }

        public bool HandleMessage(T receiver, string sessionId, Message message)
        {
            bool canHandleMessage = this.messageHandlers.TryGetValue(message.GetType(), out IMessageHandler<T> messageHandler);

            if (canHandleMessage)
            {
                messageHandler.HandleMessage(receiver, sessionId, message);
            }

            return canHandleMessage;
        }
    }
}
