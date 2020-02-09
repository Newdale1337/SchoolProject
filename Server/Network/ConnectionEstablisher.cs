using System;
using System.Net;
using System.Net.Sockets;
using Server.Messages;

namespace ShutApp.Network
{
    public class ConnectionEstablisher
    {
        public Socket Socket { get; set; }
        public bool Unloading { get; set; }
        private CoreManager CoreManager { get; set; }
        private bool Disabled { get; set; }

        private Random _random = new Random(Environment.TickCount);

        public ConnectionEstablisher(CoreManager coreManager)
        {
            CoreManager = coreManager;

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(new IPEndPoint(IPAddress.Any, 2050));
            Socket.Listen(0x100);
            Log.WriteLineIfDebug("Socket opened!");
        }

        public void Start() => Socket.BeginAccept(OnConnectionAccepted, null);

        public void DisableConnection()
        {
            Log.WriteLineIfDebug("ConnectionEstablisher Disabled");
            Disabled = true;
        }

        public void OnConnectionAccepted(IAsyncResult asyncResult)
        {
            if (Unloading || Disabled)
                return;

            var newConnectionSocket = Socket.EndAccept(asyncResult);
            if (!Unloading && newConnectionSocket != null)
            {
                new UserClient(CoreManager, newConnectionSocket);
                RandomGreetingMessage("admin");
            }
            else
                Log.WriteLineIfDebug("Error Accepting");

            if (!Unloading)
                Socket.BeginAccept(OnConnectionAccepted, null);
        }

        private void RandomGreetingMessage(string name)
        {
            string[] strs = new[]
            {
                "Welcome {USER} to the server",
                "{USER} warped to the server with a big boom!",
                "{USER} found his way to the server"
            };

            ChatMessage msg = new ChatMessage();
            msg.Data = strs[_random.Next(0, strs.Length)].Replace("{USER}", name);
            msg.Prefix = "SERVER";
            msg.Name = "";
            CoreManager.Broadcast(-1 , msg);
        }

        public void Unload()
        {
            Unloading = true;
            Socket = null;
            CoreManager = null;

            Log.WriteLineIfDebug("ConnectionEstablisher unloaded.");
        }
    }
}