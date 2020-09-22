using System;
using TcpChat.Messages;
using TcpChat.Networking.Server;
using TcpChat.Server.MessageProcessors.Base;
using TcpChat.Server.MessageProcessors.Exceptions;
using TcpChat.Server.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace TcpChat.Server.Services
{
    public class MessageProcessorService : IMessageProcessorService
    {
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        public MessageProcessorService(ILogger<MessageProcessorService> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public void ProcessMessage(string username, Message message)
        {
            var messageType = message.GetType();

            this.logger.LogDebug("Processing message of type {messageType}", messageType.Name);

            var processor = this.GetMessageProcessor(messageType);

            if (processor == null)
            {
                throw new ProcessorNotFoundException(messageType);
            }

            processor.HandleMessage(username, message);
        }

        public void ReceiveMessage(string username, byte[] data)
        {
            try
            {
                var message = Message.Deserialize(data);
                this.ProcessMessage(username, message);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, "Error processing message - username: {username}", username);
            }
        }

        private IMessageProcessor GetMessageProcessor(Type messageType)
        {
            var processorType = typeof(IMessageProcessor<>).MakeGenericType(messageType);
            return (IMessageProcessor)this.serviceProvider.GetService(processorType);
        }
    }
}
