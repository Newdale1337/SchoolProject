using System;
using System.Text;
using ShutApp.Network;

namespace ShutApp.Messages.Incoming
{
    public class ChatMessage : Message
    {
        public string Prefix = string.Empty;
        public string Name = string.Empty;
        public string Data = string.Empty;
        public override byte MessageId => Send;

        public override void Read(NetworkReader rdr)
        {
            Prefix = rdr.ReadUTF();
            Name = rdr.ReadUTF();
            Data = rdr.ReadUTF();
        }

        public override void Write(NetworkWriter wrt)
        {
            wrt.WriteUTF(Data);
        }

        public override void Handle(NetworkClient client)
        {
            client.ChatManager.ProcessMessage(this);
        }
    }
}