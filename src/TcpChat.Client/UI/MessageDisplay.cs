using System.Drawing;
using Colorful;
using TcpChat.Messages;
using TcpChat.Messages.ServerToClient;
using Console = Colorful.Console;

namespace TcpChat.Client.UI
{
    public class MessageDisplay
    {
        private readonly string username;

        public MessageDisplay(string username)
        {
            this.username = username;
        }

        public void DisplayMessage(Message message)
        {
            if (message is PublicMessageResponse publicMessage)
            {
                this.DisplayPublicMessage(publicMessage);
            }
            else if (message is DirectMessageResponse directMessage)
            {
                this.DisplayDirectMessage(directMessage);
            }
            else if (message is NotificationMessage notificationMessage)
            {
                this.DisplayNotificationMessage(notificationMessage);
            }
        }

        public void DisplayNotification(string text, NotificationLevel level)
        {

            Color color;

            switch (level)
            {
                case NotificationLevel.Warning:
                    color = Color.Yellow;
                    break;
                case NotificationLevel.Error:
                    color = Color.Red;
                    break;
                case NotificationLevel.Information:
                default:
                    color = Color.Cyan;
                    break;
            }

            Console.WriteLine(text, color);
        }

        private void DisplayPublicMessage(PublicMessageResponse publicMessage)
        {
            Console.WriteLineFormatted(
                "{0}: {1}",
                Color.Gray,
                new Formatter(publicMessage.Sender, Color.Cyan),
                new Formatter(publicMessage.Text, Color.White));
        }

        private void DisplayDirectMessage(DirectMessageResponse directMessage)
        {
            if (directMessage.IsPrivate)
            {
                this.DisplayPrivateMessage(directMessage);
            }
            else if (directMessage.Recipient == this.username)
            {
                this.DisplayMessageToUser(directMessage);
            }
            else
            {
                this.DisplayMessageToOther(directMessage);
            }
        }

        private void DisplayPrivateMessage(DirectMessageResponse directMessage)
        {
            Console.WriteLineFormatted(
                "{0} whispers to {1}: {2}",
                Color.Gray,
                new Formatter(directMessage.Sender, Color.LimeGreen),
                new Formatter("you", Color.Red),
                new Formatter(directMessage.Text, Color.Green));
        }

        private void DisplayMessageToUser(DirectMessageResponse directMessage)
        {
            Console.WriteLineFormatted(
                "{0} to {1}: {2}",
                Color.Gray,
                new Formatter(directMessage.Sender, Color.LimeGreen),
                new Formatter("you", Color.Red),
                new Formatter(directMessage.Text, Color.Lime));
        }

        private void DisplayMessageToOther(DirectMessageResponse directMessage)
        {
            Console.WriteLineFormatted(
                "{0} to {1}: {2}",
                Color.Gray,
                new Formatter(directMessage.Sender, Color.LimeGreen),
                new Formatter(directMessage.Recipient, Color.LimeGreen),
                new Formatter(directMessage.Text, Color.White));
        }

        private void DisplayNotificationMessage(NotificationMessage notificationMessage)
        {
            this.DisplayNotification(notificationMessage.Text, notificationMessage.Level);
        }
    }
}
