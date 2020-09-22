using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace TcpChat.Messages.ServerToClient
{
    [ProtoContract]
    public class DirectMessageResponse : Message
    {
        DirectMessageResponse()
        {
        }

        public DirectMessageResponse(string sender, string recipient, string text, bool isPrivate)
        {
            this.Sender = sender;
            this.Recipient = recipient;
            this.Text = text;
            this.IsPrivate = isPrivate;
        }

        [ProtoMember(1)]
        public string Text { get; set; }

        [ProtoMember(2)]
        public string Sender { get; set; }

        [ProtoMember(3)]
        public string Recipient { get; set; }

        [ProtoMember(4)]
        public bool IsPrivate { get; set; }

        public override string ToString()
        {
            return $"{Sender} to {Recipient}: {Text}";
        }

        public override bool Equals(object obj)
        {
            return obj is DirectMessageResponse response &&
                   Text == response.Text &&
                   Sender == response.Sender &&
                   Recipient == response.Recipient &&
                   IsPrivate == response.IsPrivate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Text, Sender, Recipient, IsPrivate);
        }
    }
}
