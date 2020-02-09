using Server.Storage.Models;
using ShutApp.Messages;
using ShutApp.Network;

namespace Server.Messages
{
    public class RegisterMessage : Message
    {
        public string Name = string.Empty;
        public string Password = string.Empty;

        public override byte MessageId => Register;

        public override void Read(NetworkReader rdr)
        {
            Name = rdr.ReadUTF();
            Password = rdr.ReadUTF();
        }

        public override void Handle(UserClient client)
        {
            AccountModel acc = client.StorageEngine.Register(Name, Password, client);
            if (acc is null){ client.Send(new RegisterResponseMessage { SessionId = null, Successful = false }); return;}
            client.Send(new RegisterResponseMessage { SessionId = client.SessionId, Successful = true });
            client.Send(new SuccessMessage{SuccesData = "Account succesfully created."});
        }
    }
}