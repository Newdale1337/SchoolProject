using System.Collections.Generic;
using ShutApp.Core;
using ShutApp.Core.Rank;
using ShutApp.Messages;
using ShutApp.Network;
using ShutApp.Storage;

namespace ShutApp
{
    public class CoreManager
    {
        public ChatManager ChatManager;
        public RankManager RankManager;
        private object _lockObj = new object();
        private const int MAX_CLIENTS = 100;
        public Dictionary<int, UserClient> Users;
        public MessagePool MessagePool;
        public Engine StorageEngine;
        private int _nextId = 0;
        public int NextId()
        {
            lock (_lockObj)
            {
                _nextId += 1;
                return _nextId;
            }
        }

        public void Broadcast(int excludedId, Message msg)  
        {
            foreach (var client in Users.Values)
            {
                if (client.Id == excludedId) continue;
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

        public CoreManager()
        {
            Users = new Dictionary<int, UserClient>(/*MAX_CLIENTS*/);
            MessagePool = new MessagePool();
            StorageEngine = new Engine("database");
            RankManager = new RankManager();
            ChatManager = new ChatManager(RankManager);
        }

        public void Unload()
        {
            Users = null;
            _lockObj = null;

            Log.WriteLineIfDebug("CoreManager unloaded.");
        }
    }
}