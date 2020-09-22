using TcpChat.Messages.ClientToServer;
using TcpChat.Messages.ServerToClient;
using TcpChat.Server.ServiceContracts;
using TcpChat.Server.MessageProcessors.Base;
using TcpChat.Server.Model;

namespace TcpChat.Server.MessageProcessors
{
    public class PublicMessageRequestProcessor : MessageProcessor<PublicMessageRequest>
    {
        private readonly IUserService userService;
        private readonly IMessageSenderService messageSenderService;

        public PublicMessageRequestProcessor(IUserService userService, IMessageSenderService messageSenderService)
        {
            this.userService = userService;
            this.messageSenderService = messageSenderService;
        }

        public override void HandleMessage(string username, PublicMessageRequest message)
        {
            if (this.userService.TryGetUserByName(username, out ChatUser sender))
            {
                var messageResponse = new PublicMessageResponse(sender.Username, message.Text);
                this.messageSenderService.SendMessageToEveryoneElse(sender.Username, messageResponse);
            }
        }
    }
}
