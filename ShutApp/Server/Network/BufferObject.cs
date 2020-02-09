using System.Net.Sockets;

namespace ShutApp.Network
{
    public class BufferObject
    {
        public const int BUFFER_SIZE = ushort.MaxValue + 1;
        public const int HEAD_BUFFER_SIZE = 1;
        private byte[] _buffer;
        private byte[] _headBuffer;
        public Socket Socket { get; set; }
        public byte[] Buffer => _buffer;
        public byte[] HeaderBuffer => _headBuffer;
        public BufferObject(Socket skt)
        {
            Socket = skt;
            Reset();
        }

        public void Reset()
        {
            _buffer = new byte[BUFFER_SIZE];
            _headBuffer = new byte[HEAD_BUFFER_SIZE];
        }

        public void Unload()
        {
            _buffer = null;
            Socket = null;

            Log.WriteLineIfDebug("BufferObject unloaded.");
        }
    }
}