using ShutApp.Messages;
using ShutApp.Network;

namespace Server.Messages.Login
{
    public class LoginResponseMessage : Message
    {
        public bool Successful { get; set; } = false;
        public string SessionId { get; set; } = string.Empty;

        public override byte MessageId => LoginResponse;

        public override void Write(NetworkWriter wrt)
        {
            wrt.Write(Successful);
            wrt.WriteUTF(SessionId);
        }
    }
}