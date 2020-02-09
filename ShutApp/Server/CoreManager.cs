using System;
using System.Collections.Generic;
using System.Linq;
using Server.Messages;
using Server.Storage.Models;
using ShutApp.Core;
using ShutApp.Messages;
using ShutApp.Network;
using ShutApp.Storage;

namespace ShutApp
{
    public class CoreManager
    {
        public ChatManager ChatManager { get; set; }
        public RankManager RankManager { get; set; }
        public Random Random { get; set; }
        public Dictionary<int, UserClient> Users { get; set; }
        public MessagePool MessagePool { get; set; }
        public Engine StorageEngine { get; set; }

        private object _lockObj = new object();
        private const int MAX_CLIENTS = 100;
        private int _nextId = 0;

        public int NextId()
        {
            lock (_lockObj)
            {
                _nextId += 1;
                return _nextId;
            }
        }

        public AccountModel GetAccountByName(string name)
        {
            return StorageEngine.Accounts.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }

        public string GenerateSessionID()
        {
            return Guid.NewGuid().ToString();
        }

        public void Broadcast(int excludedId, Message msg)  
        {
            foreach (var client in Users.Values)
            {
                if (client.Id == excludedId || client.AccountData is null) continue;
                client.Send(msg);
            }
        }

        public void AddClient(UserClient client)
        {
            lock (_lockObj)
            {
                Users.Add(client.Id, client);
            }
        }

        public void RemoveClient(int id)
        {
            lock (_lockObj)
            {
                Users.Remove(id);
            }
        }

        public void RandomGreetingMessage(string name)
        {
            string[] strs = new[]
            {
                "Welcome {USER} to the server",
                "{USER} warped to the server with a big boom!",
                "{USER} found his way to the server"
            };

            ChatMessage msg = new ChatMessage();
            msg.Data = strs[Random.Next(0, strs.Length)].Replace("{USER}", name);
            msg.Prefix = "SERVER";
            msg.Name = "";
            Broadcast(-1, msg);
        }

        public CoreManager()
        {
            Users = new Dictionary<int, UserClient>(/*MAX_CLIENTS*/);
            MessagePool = new MessagePool();
            RankManager = new RankManager();
            StorageEngine = new Engine("database", this);
            ChatManager = new ChatManager(RankManager);
            Random = new Random(Environment.TickCount);
        }

        public void Unload()
        {
            Users = null;
            _lockObj = null;

            Log.WriteLineIfDebug("CoreManager unloaded.");
        }
    }
}