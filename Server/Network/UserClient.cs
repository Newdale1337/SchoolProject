using System;
using System.IO;
using System.Net.Sockets;
using ShutApp.Core.Rank;
using ShutApp.Messages;
using ShutApp.Storage;

namespace ShutApp.Network
{
    public class UserClient
    {
        public int Id { get; }
        public IRank Rank;
        public string Name { get; }
        public Engine StorageEngine { get; set; }

        public CoreManager _manager;
        private Socket _socket;
        public Socket Socket => _socket;
        private BufferObject _bufferObject;
        private MessagePool _messagePool;

        public UserClient(CoreManager manager, Socket socket)
        {
            _socket = socket;
            _manager = manager;
            _messagePool = _manager.MessagePool;
            _bufferObject = new BufferObject(_socket);
            Id = _manager.NextId();
            StorageEngine = _manager.StorageEngine;
            AddToClientList(Id);
            Rank = new AdminRank();
            try
            {
                Recieve();
            }
            catch (Exception)
            {
                Disconnect("Error.");
            }
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

                if (bytesAvailable < 1) Disconnect("bytesAvailable < 1");

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
            catch (SocketException s) { }
            catch (Exception e)
            {
                Disconnect();
            }
        }

        public void Send(Message message)
        {
            try
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
            catch (Exception e)
            {
                Disconnect(e.Message);
            }
        }

        private void Disconnect(string reason = null)
        {
            if (reason != null)
                Log.WriteLineIfDebug(reason);
            _socket.BeginDisconnect(false, Disconnected, _bufferObject);
        }

        private void Disconnected(IAsyncResult ar)
        {
            BufferObject bufferObj = (BufferObject)ar.AsyncState;
            Socket socket = bufferObj.Socket;
            socket.EndDisconnect(ar);

            Log.WriteLineIfDebug($"UserClient {Id} Disconnected.");
            _manager.RemoveClient(Id);
            Unload();
        }

        public void AddToClientList(int id)
        {
            _manager.AddClient(this);
        }

        public void Unload()
        {
            _manager = null;
            _socket = null;
            _messagePool = null;

            Log.WriteLineIfDebug("UserClient unloaded.");
        }
    }
}