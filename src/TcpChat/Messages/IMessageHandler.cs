namespace TcpChat.Messages
{
    public interface IMessageHandler<T>
    {
        void HandleMessage(T receiver, string sessionId, Message message);
    }
}
