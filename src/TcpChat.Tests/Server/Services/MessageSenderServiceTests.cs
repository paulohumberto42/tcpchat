using System.Collections.Generic;
using System.Linq;
using TcpChat.Messages.ServerToClient;
using TcpChat.Networking.Server;
using TcpChat.Server.Model;
using TcpChat.Server.ServiceContracts;
using TcpChat.Server.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace TcpChat.Tests.Server.Services
{
    public class MessageSenderServiceTests
    {
        [Fact]
        public void SendingMessageToUser_WhenUserExists_ShouldSendMessage()
        {
            // Arrange
            string username = "Recipient";
            var message = new DirectMessageResponse("Sender", username, "Hello!", true);

            var logger = Substitute.For<ILogger<MessageSenderService>>();

            var userService = Substitute.For<IUserService>();

            userService.TryGetUserByName(Arg.Is(username), out Arg.Any<ChatUser>())
                .Returns(args =>
                {
                    args[1] = new ChatUser(username);
                    return true;
                });

            var networkServer = Substitute.For<INetworkServer>();

            var messageSenderService = new MessageSenderService(logger, userService, networkServer);

            // Act
            bool result = messageSenderService.SendMessageToUser(username, message);

            // Arrange
            Assert.True(result);
            networkServer.Received(1).SendMessage(username, Arg.Any<byte[]>());
        }

        [Fact]
        public void SendingMessageToEveryoneElse_WhenAnyUser_ShouldSendMessagesToEveryoneElse()
        {
            // Arrange
            var message = new PublicMessageResponse("Sender", "Hello everyone!");

            string senderUsername = "Sender";

            var users = new List<ChatUser>()
            {
                new ChatUser(senderUsername),
                new ChatUser("User001"),
                new ChatUser("User002"),
                new ChatUser("User003"),
            };

            var logger = Substitute.For<ILogger<MessageSenderService>>();

            var userService = Substitute.For<IUserService>();
            userService.GetConnectedUsers().Returns(users);

            var networkServer = Substitute.For<INetworkServer>();

            var messageSenderService = new MessageSenderService(logger, userService, networkServer);

            // Act
            messageSenderService.SendMessageToEveryoneElse(senderUsername, message);

            // Assert
            networkServer.Received(users.Count - 1).SendMessage(Arg.Any<string>(), Arg.Any<byte[]>());
        }

        [Fact]
        public void NotifyUser_WhenUserExists_ShouldSendMessage()
        {
            // Arrange
            string username = "Recipient";

            var logger = Substitute.For<ILogger<MessageSenderService>>();

            var userService = Substitute.For<IUserService>();

            userService.TryGetUserByName(Arg.Is(username), out Arg.Any<ChatUser>())
                .Returns(args =>
                {
                    args[1] = new ChatUser(username);
                    return true;
                });

            var networkServer = Substitute.For<INetworkServer>();

            var messageSenderService = new MessageSenderService(logger, userService, networkServer);

            // Act
            messageSenderService.NotifyUser(username, "Hey!", NotificationLevel.Warning);

            // Arrange
            networkServer.Received(1).SendMessage(username, Arg.Any<byte[]>());
        }

        [Fact]
        public void NotifyEveryone_WhenAnyUser_ShouldSendMessagesToEveryone()
        {
            // Arrange
            var users = new List<ChatUser>()
            {
                new ChatUser("User001"),
                new ChatUser("User002"),
                new ChatUser("User003"),
            };

            var logger = Substitute.For<ILogger<MessageSenderService>>();

            var userService = Substitute.For<IUserService>();
            userService.GetConnectedUsers().Returns(users);

            var networkServer = Substitute.For<INetworkServer>();

            var messageSenderService = new MessageSenderService(logger, userService, networkServer);

            // Act
            messageSenderService.NotifyEveryone("Hey everyone!", NotificationLevel.Information);

            // Assert
            networkServer.Received(users.Count).SendMessage(Arg.Any<string>(), Arg.Any<byte[]>());
        }
    }
}
