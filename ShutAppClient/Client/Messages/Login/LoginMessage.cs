using ShutApp.Messages;
using ShutApp.Network;

namespace Client.Messages
{
    public class LoginMessage : Message
    {
        public string Name = string.Empty;
        public string Password = string.Empty;

        public override byte MessageId => Login;

        public override void Write(NetworkWriter wrt)
        {
            wrt.WriteUTF(Name);
            wrt.WriteUTF(Password);
        }
    }
}