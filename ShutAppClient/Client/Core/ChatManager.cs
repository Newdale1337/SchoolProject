using System;
using System.Text;
using ShutApp.Messages.Incoming;

namespace Client.Core
{
    public class ChatManager
    {
        private StringBuilder sb = new StringBuilder();
        public void SwitchColor(ChatMessage msg)
        {
            switch (msg.Prefix)
            {
                case "ADMIN":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(msg.Prefix);
                    Console.ResetColor();
                    break;
                case "SERVER":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(msg.Prefix);
                    Console.ResetColor();
                    break;
                case "NONE":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(msg.Prefix);
                    Console.ResetColor();
                    break;
            }
        }

        //Todo
        public void ProcessMessage(ChatMessage msg)
        {
            Console.Write("["); SwitchColor(msg); Console.Write("]");
            Console.Write(msg.Name);
            Console.Write(": " + msg.Data + "\n");
        }
    }
}