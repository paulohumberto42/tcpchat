using TcpChat.Messages.ClientToServer;
using TcpChat.Messages.ServerToClient;
using TcpChat.Server.ServiceContracts;
using TcpChat.Server.MessageProcessors.Base;
using TcpChat.Server.Model;

namespace TcpChat.Server.MessageProcessors
{
    public class DirectMessageRequestProcessor : MessageProcessor<DirectMessageRequest>
    {
        private readonly IUserService userService;
        private readonly IMessageSenderService messageSenderService;

        public DirectMessageRequestProcessor(IUserService userService, IMessageSenderService messageSenderService)
        {
            this.userService = userService;
            this.messageSenderService = messageSenderService;
        }

        public override void HandleMessage(string username, DirectMessageRequest message)
        {
            if (this.userService.TryGetUserByName(username, out ChatUser sender))
            {
                if (this.userService.TryGetUserByName(message.Recipient, out ChatUser recipient))
                {
                    var messageResponse = new DirectMessageResponse(sender.Username, recipient.Username, message.Text, message.IsPrivate);

                    if (message.IsPrivate)
                    {
                        this.messageSenderService.SendMessageToUser(recipient.Username, messageResponse);
                    }
                    else
                    {
                        this.messageSenderService.SendMessageToEveryoneElse(sender.Username, messageResponse);
                    }
                }
                else
                {
                    this.messageSenderService.NotifyUser(username, $"User {message.Recipient} is not online.", NotificationLevel.Warning);
                }
            }
        }
    }
}
