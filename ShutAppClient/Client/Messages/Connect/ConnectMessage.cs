using ShutApp.Messages;
using ShutApp.Network;

namespace Client.Messages
{
    public class ConnectMessage : Message
    {
        public string SessionId { get; set; } = string.Empty;
        public override byte MessageId => Connect;

        public override void Write(NetworkWriter wrt)
        {
            wrt.WriteUTF(SessionId);
        }
    }
}