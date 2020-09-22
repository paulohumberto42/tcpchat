using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace TcpChat.Messages.ServerToClient
{
    [ProtoContract]
    public class PublicMessageResponse : Message
    {
        public PublicMessageResponse()
        {
        }

        public PublicMessageResponse(string sender, string text)
        {
            this.Sender = sender;
            this.Text = text;
        }

        [ProtoMember(1)]
        public string Text { get; set; }

        [ProtoMember(2)]
        public string Sender { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PublicMessageResponse response &&
                   Text == response.Text &&
                   Sender == response.Sender;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Text, Sender);
        }

        public override string ToString()
        {
            return $"{Sender}: {Text}";
        }
    }
}
