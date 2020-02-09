using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ShutApp.Messages
{
    public class MessagePool
    {
        private Dictionary<byte, Type> Messages;
        public MessagePool()
        {
            Messages = new Dictionary<byte, Type>();
            LoadMessages();
        }

        public Message Create(byte id)
        {
            if (Messages.TryGetValue(id, out Type packet))
                return (Message)Activator.CreateInstance(packet);
            return null;
        }

        private void LoadMessages()
        {
            foreach (var type in Assembly.GetAssembly(typeof(Message)).GetTypes()
                .Where(_ => _.IsClass && !_.IsAbstract && _.IsSubclassOf(typeof(Message))))
            {
                byte msgId = ((Message)Activator.CreateInstance(type)).MessageId;
                Messages.Add(msgId, type);
            }
        }

        public void Unload()
        {
            Messages = null;
        }
    }
}