using System.Threading.Tasks;
using TcpChat.Messages;
using TcpChat.Messages.ServerToClient;
using TcpChat.Networking.Server;
using TcpChat.Server.Model;
using TcpChat.Server.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace TcpChat.Server.Services
{
    public class MessageSenderService : IMessageSenderService
    {
        private readonly ILogger logger;
        private readonly IUserService userService;
        private readonly INetworkServer networkServer;

        public MessageSenderService(ILogger<MessageSenderService> logger, IUserService userService, INetworkServer networkServer)
        {
            this.logger = logger;
            this.userService = userService;
            this.networkServer = networkServer;
        }

        public bool SendMessageToUser(string recipientUsername, Message message)
        {
            this.logger.LogDebug("Sending message to {username}", recipientUsername);

            if (this.userService.TryGetUserByName(recipientUsername, out ChatUser user))
            {
                this.networkServer.SendMessage(user.Username, message.Serialize());
                return true;
            }

            return false;
        }

        public void SendMessageToEveryoneElse(string senderUsername, Message message)
        {
            this.logger.LogDebug("Sending message from {username} to everyone", senderUsername);

            var serializedMessage = message.Serialize();

            Parallel.ForEach(this.userService.GetConnectedUsers(), (user) =>
            {
                if (user.Username != senderUsername)
                {
                    this.networkServer.SendMessage(user.Username, serializedMessage);
                }
            });
        }

        public void NotifyUser(string username, string text, NotificationLevel level)
        {
            var message = new NotificationMessage(text, level);
            var serializedMessage = message.Serialize();
            this.networkServer.SendMessage(username, serializedMessage);
        }

        public void NotifyEveryone(string text, NotificationLevel level)
        {
            var message = new NotificationMessage(text, level);
            var serializedMessage = message.Serialize();

            Parallel.ForEach(this.userService.GetConnectedUsers(), (user) =>
            {
                this.networkServer.SendMessage(user.Username, serializedMessage);
            });
        }
    }
}
