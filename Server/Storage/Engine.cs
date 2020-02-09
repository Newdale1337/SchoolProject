using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using ShutApp.Core.Rank;
using ShutApp.Storage.Models;

namespace ShutApp.Storage
{
    public class Engine
    {
        private readonly string _path;
        private string _accountsPath;
        private List<AccountModel> _accounts;

        public Engine(string path)
        {
            _path = path;
            _accountsPath = Path.Combine(_path, "accounts");
            _accounts = new List<AccountModel>();
            Load();
        }

        private void Load()
        {
            if (!Directory.Exists(_accountsPath))
                Directory.CreateDirectory(_accountsPath);
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);

            foreach (var acc in new DirectoryInfo(_accountsPath).GetFiles("*.json", SearchOption.AllDirectories))
                _accounts.Add(JsonConvert.DeserializeObject<AccountModel>(acc.FullName));
        }

        public AccountModel Login(string name, string password)
        {
            AccountModel acc = _accounts.FirstOrDefault(x =>
                string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(x.Password, password, StringComparison.CurrentCulture));
            return acc;
        }

        public AccountModel Register(string name, string password)
        {
            if (_accounts.Any(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase))) return null;

            AccountModel accModel = new AccountModel { Name = name, Rank = new NoneRank(), Password = password, };
            string accData = JsonConvert.SerializeObject(accModel, Formatting.Indented);
            CreateAccountFiles(name, accData);
            _accounts.Add(accModel);

            return accModel;
        }

        public void CreateAccountFiles(string name, string data)
        {
            DirectoryInfo dI = Directory.CreateDirectory($"{_accountsPath}/Account.{name}");
            File.WriteAllText($"{dI.FullName}/AccountData.json", data);
        }
    }
}