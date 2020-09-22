using System;

namespace TcpChat.Messages.Exceptions
{
    public class MessageSerializationException : Exception
    {
        public MessageSerializationException(Message chatMessage, Exception innerException = null)
            : base("Error serializing chat message", innerException)
        {
            ChatMessage = chatMessage;
        }

        public Message ChatMessage { get; }
    }
}
