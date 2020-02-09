using Server.Storage.Models;
using ShutApp.Network;

namespace ShutApp.Core.Commands
{
    public class RankCommand : Command
    {
        public override string UsageType => "/rank";
        public override string UsageDisplay => "/rank <Client> <Id>";
        public override bool IsAdmin => true;
        public override bool Execute(string args, UserClient client)
        {
            RankManager manager = client.Manager.RankManager;
            string[] argArray = args.Split(" ");
            if (argArray.Length < 2) return false;

            string name = argArray[1];
            AccountModel acc = client.Manager.GetAccountByName(name);
            if (!int.TryParse(argArray[2], out int res))
                return false;

            acc.Rank = manager.GetRankById(res);
            acc.Save(client);
            return true;
        }
    }
}