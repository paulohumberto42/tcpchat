using System;
using System.Collections.Generic;
using System.Text;

namespace TcpChat.Server.MessageProcessors.Exceptions
{
    public class ProcessorNotFoundException : Exception
    {
        public ProcessorNotFoundException(Type messageType, Exception innerException = null)
            : base($"Processor not found for message of type {messageType.FullName}", innerException)
        {
            this.MessageType = messageType;
        }

        public Type MessageType { get; }
    }
}
