using ShutApp.Messages;
using ShutApp.Network;

namespace Server.Messages
{
    public class RegisterResponseMessage : Message
    {
        public bool Success;
        

        public override byte MessageId => RegisterResponse;

        public override void Write(NetworkWriter wrt)
        {
            //wrt.Write(Success);
            //wrt.Write(SessionId);
        }
    }
}