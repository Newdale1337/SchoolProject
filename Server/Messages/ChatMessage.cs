using System;
using ShutApp.Messages;
using ShutApp.Network;

namespace Server.Messages
{
    public class ChatMessage : Message
    {
        public string Prefix = string.Empty;
        public string Name = string.Empty;
        public string Data = string.Empty;

        public override byte MessageId => Send;

        public override void Write(NetworkWriter wrt)
        {
            wrt.WriteUTF(Prefix);
            wrt.WriteUTF(Name);
            wrt.WriteUTF(Data);
        }

        public override void Read(NetworkReader rdr)
        {
            Name = rdr.ReadUTF();
            Data = rdr.ReadUTF();
        }

        public override void Handle(UserClient client)
        {
            Prefix = client.Rank.Prefix;
            client._manager.ChatManager.ProcessMessage(this);
            client._manager.Broadcast(client.Id, this);
        }
    }
}