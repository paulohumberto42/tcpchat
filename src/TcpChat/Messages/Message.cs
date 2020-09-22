using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TcpChat.Messages.ClientToServer;
using TcpChat.Messages.Exceptions;
using TcpChat.Messages.ServerToClient;
using ProtoBuf;

namespace TcpChat.Messages
{
    [ProtoContract]
    [ProtoInclude(100, typeof(PublicMessageRequest))]
    [ProtoInclude(101, typeof(PublicMessageResponse))]
    [ProtoInclude(102, typeof(DirectMessageRequest))]
    [ProtoInclude(103, typeof(DirectMessageResponse))]
    [ProtoInclude(104, typeof(NotificationMessage))]
    public abstract class Message
    {

        public static Message Deserialize(byte[] data)
        {
            try
            {

                return Serializer.Deserialize<Message>(data.AsSpan());
            }
            catch (Exception ex)
            {
                throw new MessageDeserializationException(data, ex);
            }
        }

        public byte[] Serialize()
        {
            try
            {

                using (var stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, this);
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new MessageSerializationException(this, ex);
            }
        }
    }
}
