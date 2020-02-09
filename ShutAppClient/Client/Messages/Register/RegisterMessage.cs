using System;
using ShutApp.Network;

namespace ShutApp.Messages
{
    public class RegisterMessage : Message
    {
        public string Name = string.Empty;
        public string Password = string.Empty;

        public override byte MessageId => Register;

        public override void Write(NetworkWriter wrt)
        {
            wrt.WriteUTF(Name);
            wrt.WriteUTF(Password);
        }
    }
}