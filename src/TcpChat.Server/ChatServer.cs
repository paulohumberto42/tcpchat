using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using TcpChat.Networking.Server;
using TcpChat.Networking.Shared;
using TcpChat.Server.Model;
using TcpChat.Server.ServiceContracts;
using Microsoft.Extensions.Logging;
using TcpChat.Messages.ServerToClient;

namespace TcpChat.Server
{
    public class ChatServer : IConnectionValidator, IDisposable
    {
        private readonly ILogger logger;
        private readonly IUserService userService;
        private readonly IMessageSenderService messageSenderService;
        private readonly IMessageProcessorService messageProcessorService;
        private readonly INetworkServer networkServer;

        public ChatServer(
            ILogger<ChatServer> logger,
            IUserService userService,
            IMessageSenderService messageSenderService,
            IMessageProcessorService messageProcessorService,
            INetworkServer networkServer)
        {
            this.logger = logger;
            this.userService = userService;
            this.messageSenderService = messageSenderService;
            this.messageProcessorService = messageProcessorService;
            this.networkServer = networkServer;

            this.networkServer.ClientConnected += this.OnClientConnected;
            this.networkServer.ClientDisconnected += this.OnClientDisconnected;
            this.networkServer.ClientMessageReceived += this.OnClientMessageReceived;
            
        }

        public void Start(IPAddress address, int port)
        {
            this.logger.LogInformation("Chat server started - {address}:{port}", address, port);
            this.networkServer.Start(address, port, this);
        }

        public void Stop()
        {
            this.logger.LogInformation("Chat server stopped");

            this.networkServer.Stop();
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            if (this.userService.TryAddUser(e.SessionId, out _))
            {
                this.messageSenderService.NotifyEveryone($"{e.SessionId} has joined.", NotificationLevel.Information);
            }
        }

        private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            if (this.userService.TryRemoveUser(e.SessionId))
            {
                this.messageSenderService.NotifyEveryone($"{e.SessionId} has left.", NotificationLevel.Information);
            }
        }

        ConnectionResult IConnectionValidator.ValidateConnection(byte[] message, out string sessionId)
        {
            sessionId = null;

            try
            {
                string username = Encoding.UTF8.GetString(message);

                if (!this.userService.ValidateUsername(username, out string errorMessage))
                {
                    return new ConnectionResult(false, Encoding.UTF8.GetBytes(errorMessage));
                }
                else
                {
                    sessionId = username;
                    return new ConnectionResult(true, message);
                }
            }
            catch (Exception)
            {
                return new ConnectionResult(false, Encoding.UTF8.GetBytes("Unexpected error"));
            }
        }

        private void OnClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
        {
            this.messageProcessorService.ReceiveMessage(e.SessionId, e.Message);
        }

        public void Dispose()
        {
            this.networkServer?.Dispose();
        }
    }
}
