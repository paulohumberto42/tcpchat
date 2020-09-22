using TcpChat.Messages;
using TcpChat.Messages.ServerToClient;

namespace TcpChat.Server.ServiceContracts
{
    public interface IMessageSenderService
    {
        bool SendMessageToUser(string recipientUsername, Message message);

        void SendMessageToEveryoneElse(string senderUsername, Message message);

        void NotifyUser(string username, string text, NotificationLevel level);

        void NotifyEveryone(string text, NotificationLevel level);
    }
}
