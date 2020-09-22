using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text;
using TcpChat.Networking.Shared;
using Console = Colorful.Console;

namespace TcpChat.Client
{
    public class ChatLogin
    {
        private readonly ChatClient chatClient;
        private readonly IPAddress ipAddress;
        private readonly int port;

        public ChatLogin(ChatClient chatClient, IPAddress ipAddress, int port)
        {
            this.chatClient = chatClient;
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public string Username { get; private set; }

        public string Login()
        {
            string username;
            ConnectionResult result = null;

            do
            {
                Console.WriteLine("Welcome to the chat! Please type your username:", Color.Cyan);

                username = GetInput();

                try
                {
                    result = this.chatClient.Connect(this.ipAddress, this.port, username);

                    if (!result.Success)
                    {
                        Console.WriteLine(Encoding.UTF8.GetString(result.Data), Color.Red);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("An expected error ocurred. Please check your connection and try again.", Color.Red);
                }
            } while (result == null || !result.Success);

            return username;
        }

        private string GetInput()
        {
            return Console.ReadLine();
        }
    }
}
