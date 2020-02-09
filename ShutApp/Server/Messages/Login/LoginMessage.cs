using Server.Storage.Models;
using ShutApp.Messages;
using ShutApp.Network;

namespace Server.Messages.Login
{
    public class LoginMessage : Message
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public override byte MessageId => Login;

        public override void Read(NetworkReader rdr)
        {
            Name = rdr.ReadUTF();
            Password = rdr.ReadUTF();
        }

        public override void Handle(UserClient client)
        {
            AccountModel acc = client.StorageEngine.Login(Name, Password, client);
            if (acc is null) { client.Send(new LoginResponseMessage { SessionId = null, Successful = false }); return; }
            client.AccountData = acc;
            client.Send(new LoginResponseMessage { SessionId = client.SessionId, Successful = true });
            client.Send(new SuccessMessage { SuccesData = "Login successful." });
        }
    }
}