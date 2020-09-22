using TcpChat.Messages;
using TcpChat.Messages.ClientToServer;
using TcpChat.Messages.Exceptions;
using TcpChat.Messages.ServerToClient;
using Xunit;

namespace TcpChat.Tests.Shared
{
    public class MessageTests
    {
        [Fact]
        public void DirectMessageRequest_WhenSerializedAndDeserialized_ShouldBeEqual()
        {
            // Arrange
            Message message = new DirectMessageRequest("user", "lorem ipsum", true);

            // Act / Assert
            this.SerializeDeserializeAndCompare(message);
        }

        [Fact]
        public void PublicMessageRequest_WhenSerializedAndDeserialized_ShouldBeEqual()
        {
            // Arrange
            Message message = new PublicMessageRequest("Lorem ipsum");

            // Act / Assert
            this.SerializeDeserializeAndCompare(message);
        }

        [Fact]
        public void DirectMessageResponse_WhenSerializedAndDeserialized_ShouldBeEqual()
        {
            // Arrange
            Message message = new DirectMessageResponse("user", "user2", "Lorem ipsum", true);

            // Act / Assert
            this.SerializeDeserializeAndCompare(message);
        }

        [Fact]
        public void PublicMessageResponse_WhenSerializedAndDeserialized_ShouldBeEqual()
        {
            // Arrange
            Message message = new PublicMessageResponse("user", "Lorem ipsum");

            // Act / Assert
            this.SerializeDeserializeAndCompare(message);
        }

        [Fact]
        public void NotificationMessage_WhenSerializedAndDeserialized_ShouldBeEqual()
        {
            // Arrange
            Message message = new NotificationMessage("Lorem ipsum", NotificationLevel.Warning);

            // Act / Assert
            this.SerializeDeserializeAndCompare(message);
        }

        [Fact]
        public void WrongMessageData_WhenDeserialized_ShouldRaisedDeserializationException()
        {
            // Arrange
            byte[] data = new byte[] { 1, 2, 3, 4, 5 };

            // Act / Assert
            Assert.Throws<MessageDeserializationException>(() => Message.Deserialize(data));
        }

        [Fact]
        public void WrongMessageType_WhenSerialized_ShouldRaisedSerializationException()
        {
            // Arrange
            Message message = new WrongMessage();

            // Act / Assert
            Assert.Throws<MessageSerializationException>(() => message.Serialize());
        }

        private void SerializeDeserializeAndCompare(Message message)
        {
            // Act
            var serializedMessage = message.Serialize();
            var deserializedMessage = Message.Deserialize(serializedMessage);

            // Assert
            Assert.Equal(message, deserializedMessage);
        }

        private class WrongMessage : Message
        {
            public WrongMessage()
            {
            }
        }

    }
}
