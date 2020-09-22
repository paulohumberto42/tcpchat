using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using TcpChat.Messages.ClientToServer;
using TcpChat.Messages.ServerToClient;
using TcpChat.Server.MessageProcessors;
using TcpChat.Server.Model;
using TcpChat.Server.ServiceContracts;
using Xunit;

namespace TcpChat.Tests.Server.Processors
{
    public class PublicMessageRequestProcessorTests
    {
        [Fact]
        public void ProcessingPublicMessageRequest_WhenAnyUser_ShouldSendMessageToEveryoneElse()
        {
            // Arrange
            var senderUser = new ChatUser("Sender");
            var message = new PublicMessageRequest("Hello!");

            var users = new ChatUser[]
            {
                senderUser,
                new ChatUser("User01"),
                new ChatUser("User02"),
                new ChatUser("User03"),
            };

            var userService = Substitute.For<IUserService>();
            userService.GetConnectedUsers().Returns(users);
            userService.TryGetUserByName(Arg.Is(senderUser.Username), out Arg.Any<ChatUser>())
                .Returns(args =>
                {
                    args[1] = senderUser;
                    return true;
                });

            var messageSenderService = Substitute.For<IMessageSenderService>();

            var processor = new PublicMessageRequestProcessor(userService, messageSenderService);

            // Act
            processor.HandleMessage(senderUser.Username, message);

            // Assert
            messageSenderService.Received(1).SendMessageToEveryoneElse(senderUser.Username, Arg.Any<PublicMessageResponse>());
        }
    }
}
