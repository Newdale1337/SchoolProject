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
            }
            else
                Log.WriteLineIfDebug("Error Accepting");

            if (!Unloading)
                Socket.BeginAccept(OnConnectionAccepted, null);
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