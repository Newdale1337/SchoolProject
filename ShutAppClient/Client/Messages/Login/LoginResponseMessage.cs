using ShutApp.Messages;
using ShutApp.Network;

namespace Client.Messages.Login
{
    public class LoginResponseMessage : Message
    {
        public bool Successful { get; set; } = false;
        public string SessionId { get; set; } = string.Empty;

        public override byte MessageId => LoginResponse;

        public override void Read(NetworkReader rdr)
        {
            Successful = rdr.ReadBoolean();
            SessionId = rdr.ReadUTF();
        }

        public override void Handle(NetworkClient client)
        {
            if (!Successful) { client.Login(); return;}
            client.SessionId = SessionId;
            client.Send(new ConnectMessage { SessionId = client.SessionId });
            client.Clear();
        }
    }
}