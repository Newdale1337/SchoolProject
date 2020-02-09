using System;
using System.Threading;
using ShutApp.Messages;
using ShutApp.Network;

namespace Client.Messages.Connect
{
    public class ConnectResponseMessage : Message
    {
        public bool Successful { get; set; }
        public override byte MessageId => ConnectResponse;

        public override void Read(NetworkReader rdr)
        {
            Successful = rdr.ReadBoolean();
        }

        public override void Handle(NetworkClient client)
        {
            if (Successful) {new Thread(client.ReadConsole).Start(); return;}
        }
    }
}