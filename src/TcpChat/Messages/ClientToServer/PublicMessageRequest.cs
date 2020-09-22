using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace TcpChat.Messages.ClientToServer
{
    [ProtoContract]
    public class PublicMessageRequest : Message
    {
        public PublicMessageRequest()
        {
        }

        public PublicMessageRequest(string text)
        {
            this.Text = text;
        }

        [ProtoMember(1)]
        public string Text { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PublicMessageRequest request &&
                   Text == request.Text;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Text);
        }
    }
}
