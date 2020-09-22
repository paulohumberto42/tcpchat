using System;
using System.Collections.Generic;
using System.Text;
using TcpChat.Messages;
using TcpChat.Messages.ServerToClient;

namespace TcpChat.Client.UI
{
    public class ChatScreen
    {
        private readonly ChatClient client;
        private readonly CommandFactory commandFactory;
        public readonly MessageDisplay messageDisplay;

        public ChatScreen(ChatClient client, CommandFactory commandFactory, MessageDisplay messageDisplay)
        {
            this.client = client;
            this.commandFactory = commandFactory;
            this.messageDisplay = messageDisplay;
            this.client.MessageReceived += OnMessageReceived;
        }

        public void Run()
        {

            while (client.IsConnected)
            {
                string command = Console.ReadLine();
                this.commandFactory.Execute(command);
            }

            this.messageDisplay.DisplayNotification("Good bye! Come back later.", NotificationLevel.Information);
            Console.ReadKey();
        }

        private void OnMessageReceived(object sender, Message e)
        {
            this.messageDisplay.DisplayMessage(e);
        }
    }
}
