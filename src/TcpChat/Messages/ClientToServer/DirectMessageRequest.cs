using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace TcpChat.Messages.ClientToServer
{
    [ProtoContract]
    public class DirectMessageRequest : Message
    {
        DirectMessageRequest()
        {
        }

        public DirectMessageRequest(string recipient, string text, bool isPrivate = false)
        {
            this.Recipient = recipient;
            this.Text = text;
            this.IsPrivate = isPrivate;
        }

        [ProtoMember(1)]
        public string Text { get; set; }

        [ProtoMember(2)]
        public string Recipient { get; set; }

        [ProtoMember(3)]
        public bool IsPrivate { get; set; }

        public override bool Equals(object obj)
        {
            return obj is DirectMessageRequest request &&
                   Text == request.Text &&
                   Recipient == request.Recipient &&
                   IsPrivate == request.IsPrivate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Text, Recipient, IsPrivate);
        }
    }
}
