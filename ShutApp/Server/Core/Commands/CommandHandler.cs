using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ShutApp.Network;

namespace ShutApp.Core.Commands
{
    public class CommandHandler
    {
        private Dictionary<string, Command> Commands;

        public CommandHandler()
        {
            Commands = new Dictionary<string, Command>();
            foreach (var type in Assembly.GetAssembly(typeof(Command)).GetTypes().Where(_ => _.IsClass && !_.IsAbstract && _.IsSubclassOf(typeof(Command)) && _.GetConstructor(Type.EmptyTypes) != null))
            {
                var appData = (Command)Activator.CreateInstance(type);
                Commands.Add(appData.UsageType, appData);
            }
        }

        public void Handle(string message, UserClient client)
        {
            string cmdType = message.Split(" ")[0];
            if (!Commands.TryGetValue(cmdType, out Command command)) return;
            command.Execute(message.Remove(0, cmdType.Length), client);
        }
    }
}