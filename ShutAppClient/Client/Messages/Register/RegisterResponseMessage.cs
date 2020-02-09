using Client.Messages;
using ShutApp.Network;

namespace ShutApp.Messages
{
    public class RegisterResponseMessage : Message
    {
        public bool Successful { get; set; }
        public string SessionId { get; set; } = string.Empty;

        public override byte MessageId => RegisterResponse;

        public override void Read(NetworkReader rdr)
        {
            Successful = rdr.ReadBoolean();
            SessionId = rdr.ReadUTF();
        }

        public override void Handle(NetworkClient client)
        {
            if (!Successful){client.RegisterAccount(); return;}
            client.SessionId = SessionId;
            client.Send(new ConnectMessage{SessionId = client.SessionId});
            client.Clear();
        }
    }

}