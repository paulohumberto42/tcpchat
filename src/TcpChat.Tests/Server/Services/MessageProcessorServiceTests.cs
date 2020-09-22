using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NuGet.Frameworks;
using TcpChat.Messages;
using TcpChat.Messages.ClientToServer;
using TcpChat.Server.MessageProcessors.Base;
using TcpChat.Server.MessageProcessors.Exceptions;
using TcpChat.Server.Services;
using Xunit;

namespace TcpChat.Tests.Server.Services
{
    public class MessageProcessorServiceTests
    {
        [Fact]
        public void ProcessMessage_WhenProcessorExists_ShouldProcessMessage()
        {
            // Arrange
            var username = "User";
            Message message = new PublicMessageRequest("Hello");

            var logger = Substitute.For<ILogger<MessageProcessorService>>();
            var serviceProvider = Substitute.For<IServiceProvider>();

            var mockProcessor = Substitute.For<IMessageProcessor<PublicMessageRequest>>();
            serviceProvider.GetService(Arg.Is(typeof(IMessageProcessor<PublicMessageRequest>))).Returns(mockProcessor);

            MessageProcessorService messageProcessorService = new MessageProcessorService(logger, serviceProvider);

            // Act
            messageProcessorService.ProcessMessage(username, message);

            // Assert
            mockProcessor.Received(1).HandleMessage(username, message);
        }

        [Fact]
        public void ProcessMessage_WhenProcessorExists_ShouldRaiseProcessorNotFoundException()
        {
            // Arrange
            var username = "User";
            Message message = new PublicMessageRequest("Hello");

            var logger = Substitute.For<ILogger<MessageProcessorService>>();
            var serviceProvider = Substitute.For<IServiceProvider>();

            var mockProcessor = Substitute.For<IMessageProcessor<PublicMessageRequest>>();
            serviceProvider.GetService(Arg.Any<Type>()).Returns(null);

            MessageProcessorService messageProcessorService = new MessageProcessorService(logger, serviceProvider);

            // Act / Assert
            Assert.Throws<ProcessorNotFoundException>(() => messageProcessorService.ProcessMessage(username, message));
        }
    }
}
