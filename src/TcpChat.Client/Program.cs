using System.Drawing;
using System.Net;
using System.Text;
using TcpChat.Client.UI;
using TcpChat.Messages.ClientToServer;
using TcpChat.Networking.Client;
using TcpChat.Networking.Shared;
using Console = Colorful.Console;

namespace TcpChat.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var networkClient = new NetworkClient();
            var chatClient = new ChatClient(networkClient);
            var chatLogin = new ChatLogin(chatClient, IPAddress.Parse("127.0.0.1"), 9999);

            string username = chatLogin.Login();

            var messageDisplay = new MessageDisplay(username);
            var commandFactory = new CommandFactory(chatClient, messageDisplay);
            var chatScreen = new ChatScreen(chatClient, commandFactory, messageDisplay);

            chatScreen.Run();
        }
    }
}
