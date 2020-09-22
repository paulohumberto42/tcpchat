using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TcpChat.Client.UI;
using TcpChat.Messages;
using TcpChat.Messages.ClientToServer;
using TcpChat.Messages.ServerToClient;

namespace TcpChat.Client
{
    public class CommandFactory
    {
        private Regex commandRegex;
        private Dictionary<string, Action<string>> commands;
        private ChatClient chatClient;
        private MessageDisplay messageDisplay;
        public CommandFactory(ChatClient chatClient, MessageDisplay messageDisplay)
        {
            this.chatClient = chatClient;
            this.messageDisplay = messageDisplay;

            this.commandRegex = new Regex("^(/[A-Za-z]+)");
            this.commands = new Dictionary<string, Action<string>>();
            this.commands["/to"] = this.SendDirectMessage;
            this.commands["/w"] = this.SendPrivateMessage;
            this.commands["/h"] = this.ShowCommandList;
            this.commands["/q"] = this.Quit;
        }

        public void Execute(string command)
        {
            string trimmedCommand = command.Trim();

            var commandMatch = this.commandRegex.Match(trimmedCommand);

            if (commandMatch.Success)
            {
                string commandName = commandMatch.Groups[1].Value;

                if (this.commands.TryGetValue(commandName, out Action<string> commandAction))
                {
                    commandAction(trimmedCommand);
                }
                else
                {
                    this.messageDisplay.DisplayNotification("Invalid command. Type /h to see the command list.", NotificationLevel.Error);
                }
            }
            else
            {
                this.SendPublicMessage(command);
            }
        }

        private void SendPublicMessage(string text)
        {
            this.chatClient.SendMessage(new PublicMessageRequest(text)); ;
        }

        private void SendDirectMessage(string command)
        {
            string[] parameters = command.Split(' ', 3);

            if (parameters.Length < 3)
            {
                this.messageDisplay.DisplayNotification("Wrong input, please type /to [username] [message]", NotificationLevel.Warning);
                return;
            }

            this.chatClient.SendMessage(new DirectMessageRequest(parameters[1], parameters[2], false));
        }

        private void SendPrivateMessage(string command)
        {
            string[] parameters = command.Split(' ', 3);

            if (parameters.Length < 3)
            {
                this.messageDisplay.DisplayNotification("Wrong input, please type /w [username] [message]", NotificationLevel.Warning);
                return;
            }

            this.chatClient.SendMessage(new DirectMessageRequest(parameters[1], parameters[2], true));
        }

        private void ShowCommandList(string command)
        {
            this.messageDisplay.DisplayNotification("Command list:", NotificationLevel.Information);
            this.messageDisplay.DisplayNotification("  /to [username] [message]: Send a direct message to an user.", NotificationLevel.Information);
            this.messageDisplay.DisplayNotification("  /w [username] [message]: Send a private message to an user.", NotificationLevel.Information);
            this.messageDisplay.DisplayNotification("  /h : Display this help information.", NotificationLevel.Information);
        }

        private void Quit(string command)
        {
            this.chatClient.Disconnect();
        }
    }
}
