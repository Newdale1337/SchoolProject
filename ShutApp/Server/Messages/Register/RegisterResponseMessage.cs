using ShutApp.Messages;
using ShutApp.Network;

namespace Server.Messages
{
    public class RegisterResponseMessage : Message
    {
        public bool Successful { get; set; }
        public string SessionId { get; set; } = string.Empty;

        public override byte MessageId => RegisterResponse;

        public override void Write(NetworkWriter wrt)
        {
            wrt.Write(Successful);
            wrt.WriteUTF(SessionId);
        }
    }
}