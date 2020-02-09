using System;
using System.IO;
using Newtonsoft.Json;
using ShutApp.Core;
using ShutApp.Network;

namespace Server.Storage.Models
{
    public class AccountModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public int RankId { get; set; }
        [NonSerialized] public Rank Rank;

        public void Save(UserClient client)
        {
            string accData = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.WriteAllText($"{client.StorageEngine.AccountsPath}/Account.{Name}/AccountData.json", accData);
        }
    }
}