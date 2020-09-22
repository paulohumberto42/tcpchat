using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using TcpChat.Messages.ClientToServer;
using TcpChat.Messages.ServerToClient;
using TcpChat.Server.MessageProcessors;
using TcpChat.Server.Model;
using TcpChat.Server.ServiceContracts;
using Xunit;

namespace TcpChat.Tests.Server.Processors
{
    public class DirectMessageRequestProcessorTests
    {
        [Fact]
        public void ProcessingDirectMessageRequest_WhenNonPrivate_ShouldSendMessageToEveryoneElse()
        {
            // Arrange
            var senderUser = new ChatUser("Sender");
            var recipientUser = new ChatUser("RecipientUser");
            var message = new DirectMessageRequest(recipientUser.Username, "Hello!", false);

            var users = new ChatUser[]
            {
                senderUser,
                recipientUser,
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

            userService.TryGetUserByName(Arg.Is(recipientUser.Username), out Arg.Any<ChatUser>())
                .Returns(args =>
                {
                    args[1] = recipientUser;
                    return true;
                });

            var messageSenderService = Substitute.For<IMessageSenderService>();

            var processor = new DirectMessageRequestProcessor(userService, messageSenderService);

            // Act
            processor.HandleMessage(senderUser.Username, message);

            // Assert
            messageSenderService.Received(1).SendMessageToEveryoneElse(senderUser.Username, Arg.Any<DirectMessageResponse>());
        }

        [Fact]
        public void ProcessingDirectMessageRequest_WhenPrivate_ShouldSendMessageToUser()
        {
            // Arrange
            var senderUser = new ChatUser("Sender");
            var recipientUser = new ChatUser("RecipientUser");
            var message = new DirectMessageRequest(recipientUser.Username, "Hello!", true);

            var users = new ChatUser[]
            {
                senderUser,
                recipientUser,
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

            userService.TryGetUserByName(Arg.Is(recipientUser.Username), out Arg.Any<ChatUser>())
                .Returns(args =>
                {
                    args[1] = recipientUser;
                    return true;
                });

            var messageSenderService = Substitute.For<IMessageSenderService>();

            var processor = new DirectMessageRequestProcessor(userService, messageSenderService);

            // Act
            processor.HandleMessage(senderUser.Username, message);

            // Assert
            Assert.Equal(
                1,
                messageSenderService
                .ReceivedCalls()
                .Count(p => p.GetMethodInfo().Name == nameof(IMessageSenderService.SendMessageToUser)));
        }
    }
}
