using System;
using System.Threading.Tasks;
using ShutApp.Network;

namespace Client
{
    class Program
    {
        private static NetworkClient Client;
        static async Task Main(string[] args)
        {
            Client = new NetworkClient();

            await Task.Delay(-1);
        }
    }
}
