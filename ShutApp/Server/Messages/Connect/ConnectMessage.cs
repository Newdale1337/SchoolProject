using System;
using Server.Messages.Connect;
using ShutApp;
using ShutApp.Messages;
using ShutApp.Network;

namespace Server.Messages
{
    public class ConnectMessage : Message
    {
        public string SessionId { get; set; } = string.Empty;
        public override byte MessageId => Connect;

        public override void Handle(UserClient client)
        {
            if (client.SessionId != SessionId)
            {
                client.Send(new ErrorMessage { ErrorData = ErrorMessage.SESSION_ID_FAULTY });
                client.Send(new ConnectReponseMessage{Successful = false});
                return;
            }
            client.HasSession = true;
            if (!string.IsNullOrWhiteSpace(client.AccountData.Name))
                client.Manager.RandomGreetingMessage(client.AccountData.Name);
            client.Send(new ConnectReponseMessage { Successful = true });
            client.Send(new ChatMessage { Data = $"Welcome to William's chat server! -  Current Connected users [{client.Manager.Users.Count}]", Prefix = "SERVER" });
            Console.Title = $"Taking care of {client.Manager.Users.Count} - Users...";
        }

        public override void Read(NetworkReader rdr)
        {
            SessionId = rdr.ReadUTF();
        }
    }
}