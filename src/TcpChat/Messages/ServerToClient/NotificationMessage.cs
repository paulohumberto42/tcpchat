using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace TcpChat.Messages.ServerToClient
{
    [ProtoContract]
    public class NotificationMessage : Message
    {
        public NotificationMessage()
        {
        }

        public NotificationMessage(string text, NotificationLevel level)
        {
            this.Text = text;
            this.Level = level;
        }

        [ProtoMember(1)]
        public string Text { get; set; }

        [ProtoMember(2)]
        public NotificationLevel Level { get; set; }

        public override bool Equals(object obj)
        {
            return obj is NotificationMessage message &&
                   this.Text == message.Text &&
                   this.Level == message.Level;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Text, this.Level);
        }
    }
}
