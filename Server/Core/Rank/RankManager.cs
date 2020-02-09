using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ShutApp.Messages;

namespace ShutApp.Core.Rank
{
    public class RankManager
    {
        public Dictionary<int, Type> _ranks;

        public RankManager()
        {
            _ranks = new Dictionary<int, Type>();
        }

        private void Load()
        {
            foreach (var type in Assembly.GetAssembly(typeof(IRank)).GetTypes()
                .Where(_ => _.IsClass && !_.IsAbstract && _.IsSubclassOf(typeof(IRank))))
            {
                int rankId = ((IRank)Activator.CreateInstance(type)).RankId;
                _ranks.Add(rankId, type);
            }
        }
    }
}