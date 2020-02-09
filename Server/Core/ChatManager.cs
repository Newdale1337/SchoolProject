using Server.Messages;
using ShutApp.Core.Rank;
using System;
using System.Text;

namespace ShutApp.Core
{
    public class ChatManager
    {
        private StringBuilder sb = new StringBuilder();
        private RankManager _rankManager;
        public ChatManager(RankManager rankManager)
        {
            _rankManager = rankManager;
        }

        private void SwitchColor(ChatMessage msg)
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
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(msg.Prefix);
                    Console.ResetColor();
                    break;
            }
        }

        //Todo
        public string ProcessMessage(ChatMessage msg)
        {
            Console.Write("["); SwitchColor(msg); Console.Write("]");
            Console.Write(msg.Name);
            Console.Write(": " + msg.Data + "\n");

            return sb.ToString();
        }
    }
}