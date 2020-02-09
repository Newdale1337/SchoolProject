using ShutApp.Network;

namespace ShutApp.Core.Commands
{
    public abstract class Command
    {
        public abstract string UsageType { get; }
        public abstract string UsageDisplay { get; }
        public abstract bool IsAdmin { get; }
        public abstract bool Execute(string args, UserClient client);
    }
}