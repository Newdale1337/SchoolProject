using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ShutApp.Messages;

namespace ShutApp.Core
{
    public class RankManager
    {
        public Dictionary<int, Type> _ranks;

        public RankManager()
        {
            _ranks = new Dictionary<int, Type>();
            Load();
        }

        public Rank GetRankById(int id)
        {
            if (_ranks.TryGetValue(id, out Type type))
                return (Rank) Activator.CreateInstance(type);
            return null;
        }

        private void Load()
        {
            foreach (var type in Assembly.GetAssembly(typeof(Rank)).GetTypes()
                .Where(_ => _.IsClass && !_.IsAbstract && _.IsSubclassOf(typeof(Rank))))
            {
                int rankId = ((Rank)Activator.CreateInstance(type)).RankId;
                _ranks.Add(rankId, type);
            }
        }
    }
}