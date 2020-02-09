using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Server.Core;
using Server.Messages;
using Server.Storage.Models;
using ShutApp.Network;

namespace ShutApp.Storage
{
    public class Engine
    {
        public List<AccountModel> Accounts { get; }
        public string Path { get; }
        public string AccountsPath { get; }

        private CoreManager _manager;
        public Engine(string path, CoreManager manager)
        {
            _manager = manager;
            Path = path;
            AccountsPath = System.IO.Path.Combine(Path, "accounts");
            Accounts = new List<AccountModel>();
            Load();
        }

        private void Load()
        {
            if (!Directory.Exists(AccountsPath))
                Directory.CreateDirectory(AccountsPath);
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            foreach (var acc in new DirectoryInfo(AccountsPath).GetFiles("*.json", SearchOption.AllDirectories))
            {
                AccountModel accModel = JsonConvert.DeserializeObject<AccountModel>(File.ReadAllText(acc.FullName));
                accModel.Rank = _manager.RankManager.GetRankById(accModel.RankId);
                Accounts.Add(accModel);
            }
        }

        public AccountModel Login(string name, string password, UserClient client)
        {
            if (_manager.Users.Any(x => string.Equals(x.Value.AccountData?.Name, name, StringComparison.InvariantCultureIgnoreCase))) { client.Send(new ErrorMessage { ErrorData = ErrorMessage.ACCOUNT_IN_USE }); return null; }

            AccountModel acc = Accounts.FirstOrDefault(x =>
                string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(x.Password, password, StringComparison.CurrentCulture));
            if (acc == null) { client.Send(new ErrorMessage { ErrorData = ErrorMessage.INVALID_CREDENTIALS }); return null; }

            return acc;
        }

        public AccountModel Register(string name, string password, UserClient client)
        {
            if (Accounts.Any(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase)))
            {
                client.Send(new ErrorMessage { ErrorData = ErrorMessage.ACCOUNT_EXISTS });
                return null;
            }

            AccountModel accModel = new AccountModel { Name = name, Rank = new NoneRank(), Password = password, };
            string accData = JsonConvert.SerializeObject(accModel, Formatting.Indented);
            CreateAccountFiles(name, accData);
            Accounts.Add(accModel);
            client.AccountData = accModel;

            return accModel;
        }

        public void CreateAccountFiles(string name, string data)
        {
            DirectoryInfo dI = Directory.CreateDirectory($"{AccountsPath}/Account.{name}");
            File.WriteAllText($"{dI.FullName}/AccountData.json", data);
        }
    }
}