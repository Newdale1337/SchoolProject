using ShutApp.Core.Rank;

namespace ShutApp.Storage.Models
{
    public class AccountModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public IRank Rank { get; set; }
    }
}