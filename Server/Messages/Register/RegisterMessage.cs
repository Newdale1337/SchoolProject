using ShutApp.Messages;
using ShutApp.Network;
using ShutApp.Storage.Models;

namespace Server.Messages
{
    public class RegisterMessage : Message
    {
        public string Name = string.Empty;
        public string Password = string.Empty;

        public override byte MessageId => Message.Register;

        public override void Read(NetworkReader rdr)
        {
            Name = rdr.ReadUTF();
            Password = rdr.ReadUTF();
        }

        public override void Handle(UserClient client)
        {
            AccountModel acc = client.StorageEngine.Register(Name, Password);
        }
    }
}