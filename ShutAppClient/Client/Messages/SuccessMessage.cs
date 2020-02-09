using System;
using ShutApp.Messages;
using ShutApp.Network;

namespace Client.Messages
{
    public class SuccessMessage : Message
    {
        public string SuccessData { get; set; } = string.Empty;
        public override byte MessageId => Success;

        public override void Read(NetworkReader rdr)
        {
            SuccessData = rdr.ReadUTF();
        }

        public override void Handle(NetworkClient client)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(SuccessData);
            Console.ResetColor();
        }
    }
}