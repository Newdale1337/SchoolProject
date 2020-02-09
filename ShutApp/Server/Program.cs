using System;
using System.Threading.Tasks;
using ShutApp.Network;

namespace ShutApp
{
    class Program
    {
        private static CoreManager _manager;
        private static ConnectionEstablisher _connectionEstablisher;

        private static async Task Main(string[] args)
        {
            _manager = new CoreManager();
            _connectionEstablisher = new ConnectionEstablisher(_manager);
            _connectionEstablisher.Start();
            await Task.Delay(-1);
        }

        public void Unload()
        {
            _connectionEstablisher.Unload();
            _manager.Unload();
        }
    }
}