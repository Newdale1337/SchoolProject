using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Client.Core;
using Client.Messages;
using ShutApp.Messages;
using ShutApp.Messages.Incoming;

namespace ShutApp.Network
{
    public class NetworkClient
    {
        public string SessionId { get; set; } = string.Empty;
        public AccountData AccountData { get; set; }
        public ChatManager ChatManager { get; set; }
        private Socket _socket;
        private BufferObject _bufferObject;
        private MessagePool _messagePool;
        public NetworkClient()
        {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _bufferObject = new BufferObject(_socket);
            _messagePool = new MessagePool();
            _socket.BeginConnect("127.0.0.1", 2050, ConnectDone, _bufferObject);
            ChatManager = new ChatManager();
        }
        private string ReadConsoleString(string msg)
        {
            Console.WriteLine("######################");
            Console.Write(msg); string res = Console.ReadLine();
            return res;
        }
        private char ReadConsoleChar(string msg)
        {
            Console.WriteLine("######################");
            Console.Write(msg); char res = Console.ReadKey().KeyChar;
            return res;
        }
        public void Login(string msg = null)
        {
            string Name = ReadConsoleString("Name: ");
            string Password = ReadConsoleString("Password: ");

            Send(new LoginMessage
            {
                Name = Name,
                Password = Password
            });
        }
        public void RegisterAccount()
        {
            AccountData = new AccountData()
            {
                Name = ReadConsoleString("Name: ")
            };

            string password = ReadConsoleString("Password: ");

            Send(new RegisterMessage { Name = AccountData.Name, Password = password });
        }
        public void Clear()
        {
            Console.Clear();
        }
        public void ReadConsole()
        {
            ChatMessage message = new ChatMessage();
            while (true)
            {
                message.Data = Console.ReadLine();
                Send(message);
            }
        }
        public void Send(Message message)
        {
            byte[] tempBuffer = new byte[BufferObject.BUFFER_SIZE];
            using (MemoryStream ms = new MemoryStream(tempBuffer))
            using (NetworkWriter wrt = new NetworkWriter(ms))
            {
                wrt.Write(message.MessageId);
                message.Write(wrt);
            }

            _socket.Send(tempBuffer);
            tempBuffer = null;
        }
        private void Menu()
        {
            char resp = ReadConsoleChar("[R]egister or [L]ogin: ");
            if (string.Equals(resp.ToString(), "r", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.Clear();
                RegisterAccount();
            }
            else if (string.Equals(resp.ToString(), "l", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.Clear();
                Login();
            }
            else
            {
                Console.Clear();
                Menu();
            }
        }
        private void ConnectDone(IAsyncResult ar)
        {
            BufferObject bufferObject = (BufferObject)ar.AsyncState;
            Socket socket = bufferObject.Socket;
            socket.EndConnect(ar);
            Menu();
            Recieve();
        }
        public void Recieve() => _socket.BeginReceive(_bufferObject.Buffer, 0, BufferObject.BUFFER_SIZE,
            SocketFlags.None, DataRecieved, _bufferObject);
        private void DataRecieved(IAsyncResult ar)
        {
            try
            {
                _bufferObject = (BufferObject)ar.AsyncState;
                Socket socket = _bufferObject.Socket;
                Message message;

                int bytesAvailable = socket.EndReceive(ar);

                if (bytesAvailable < 1) _socket.Disconnect(true);

                using (MemoryStream ms = new MemoryStream(_bufferObject.Buffer))
                using (NetworkReader rdr = new NetworkReader(ms))
                {
                    byte packetId = rdr.ReadByte();
                    message = _messagePool.Create(packetId);
                    message.Read(rdr);
                    _bufferObject.Reset();
                }

                message.Handle(this);
                Recieve();
            }
            catch (Exception)
            {
                _socket.Disconnect(true);
            }
        }
    }
}