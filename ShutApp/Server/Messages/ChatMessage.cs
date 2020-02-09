using ShutApp.Messages;
using ShutApp.Network;

namespace Server.Messages
{
    public class ChatMessage : Message
    {
        public string Prefix { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;

        public override byte MessageId => Send;

        public override void Write(NetworkWriter wrt)
        {
            wrt.WriteUTF(Prefix);
            wrt.WriteUTF(Name);
            wrt.WriteUTF(Data);
        }

        public override void Read(NetworkReader rdr)
        {
            Data = rdr.ReadUTF();
        }

        public override void Handle(UserClient client)
        {
            if (client.AccountData == null)
            {
                client.Send(new ErrorMessage { ErrorData = ErrorMessage.NOT_SIGNED_IN }); return;
            }
            if (!client.HasSession)
            {
                client.Send(new ErrorMessage { ErrorData = ErrorMessage.SESSION_ID_INVALID }); return;
            }
            if (Data.Length < 1)
            {
                return;
            }

            Name = client.AccountData.Name;
            Prefix = client.AccountData.Rank.Prefix;
            client.Manager.ChatManager.ProcessMessage(this, client);
            client.Manager.Broadcast(client.Id, this);
        }
    }
}