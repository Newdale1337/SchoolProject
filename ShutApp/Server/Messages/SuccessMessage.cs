using ShutApp.Messages;
using ShutApp.Network;

namespace Server.Messages
{
    public class SuccessMessage : Message
    {
        public string SuccesData { get; set; } = string.Empty;
        public override byte MessageId => Success;

        public override void Write(NetworkWriter wrt)
        {
            wrt.WriteUTF(SuccesData);
        }
    }

}