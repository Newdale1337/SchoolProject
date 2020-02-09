using ShutApp.Messages;
using ShutApp.Network;

namespace Server.Messages.Connect
{
    public class ConnectReponseMessage : Message
    {
        public bool Successful { get; set; }

        public override byte MessageId => ConnectResponse;

        public override void Write(NetworkWriter wrt)
        {
            wrt.Write(Successful);
        }
    }
}