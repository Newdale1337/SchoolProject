using System;
using ShutApp.Messages;
using ShutApp.Network;

namespace Client.Messages
{
    public class ErrorMessage : Message
    {
        public override byte MessageId => Error;

        public string ErrorData { get; set; } = string.Empty;

        public override void Read(NetworkReader rdr)
        {
            ErrorData = rdr.ReadUTF();
        }

        public override void Handle(NetworkClient client)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ErrorData);
            Console.ResetColor();
        }
    }
}