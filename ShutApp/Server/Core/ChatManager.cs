using Server.Messages;
using System;
using System.Text;
using ShutApp.Core.Commands;
using ShutApp.Network;

namespace ShutApp.Core
{
    public class ChatManager
    {
        private StringBuilder sb = new StringBuilder();
        private RankManager _rankManager;
        private CommandHandler _commandHandler;
        public ChatManager(RankManager rankManager)
        {
            _rankManager = rankManager;
            _commandHandler = new CommandHandler();
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
        public void ProcessMessage(ChatMessage msg, UserClient client)
        {
            Console.Write("["); SwitchColor(msg); Console.Write("]");
            Console.Write(msg.Name);
            Console.Write(": " + msg.Data + "\n");

            _commandHandler.Handle(msg.Data, client);
        }
    }
}