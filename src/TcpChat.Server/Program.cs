using System;
using System.Net;
using TcpChat.Messages.ClientToServer;
using TcpChat.Networking.Server;
using TcpChat.Server.ServiceContracts;
using TcpChat.Server.MessageProcessors;
using TcpChat.Server.MessageProcessors.Base;
using TcpChat.Server.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace TcpChat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .Enrich.FromLogContext()
              .WriteTo.Console()
              .CreateLogger();

            IServiceCollection services = new ServiceCollection();
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            services.AddScoped<INetworkServer, NetworkServer>();
            services.AddScoped<ChatServer>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMessageSenderService, MessageSenderService>();
            services.AddScoped<IMessageProcessorService, MessageProcessorService>();
            
            services.AddTransient<IMessageProcessor<PublicMessageRequest>, PublicMessageRequestProcessor>();
            services.AddTransient<IMessageProcessor<DirectMessageRequest>, DirectMessageRequestProcessor>();

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var chatServer = scope.ServiceProvider.GetRequiredService<ChatServer>();
                chatServer.Start(IPAddress.Any, 9999);
                Console.ReadKey();
            }
        }
    }
}
