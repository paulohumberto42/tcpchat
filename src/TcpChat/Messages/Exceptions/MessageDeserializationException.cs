using System;

namespace TcpChat.Messages.Exceptions
{
    public class MessageDeserializationException : Exception
    {
        public MessageDeserializationException(byte[] messageData, Exception innerException = null)
            : base("Error serializing chat message", innerException)
        {
            MessageData = messageData;
        }

        public byte[] MessageData { get; }
    }
}
