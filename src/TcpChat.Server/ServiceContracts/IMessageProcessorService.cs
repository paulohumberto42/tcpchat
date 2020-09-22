using TcpChat.Messages;

namespace TcpChat.Server.ServiceContracts
{
    public interface IMessageProcessorService
    {
        void ReceiveMessage(string username, byte[] data);

        void ProcessMessage(string username, Message message);
    }
}
