using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;

namespace ShutApp.Network
{
    public class NetworkWriter : BinaryWriter
    {
        public NetworkWriter(Stream stream) : base(stream, Encoding.UTF8) { }

        public override void Write(short value) => base.Write(IPAddress.HostToNetworkOrder(value));
        public override void Write(int value) => base.Write(IPAddress.HostToNetworkOrder(value));
        public override void Write(long value) => base.Write(IPAddress.HostToNetworkOrder(value));
        public override void Write(ushort value) => base.Write((ushort)IPAddress.HostToNetworkOrder((short)value));
        public override void Write(uint value) => base.Write((uint)IPAddress.HostToNetworkOrder((int)value));
        public override void Write(ulong value) => base.Write((ulong)IPAddress.HostToNetworkOrder((long)value));
        public override void Write(float value)
        {
            var b = BitConverter.GetBytes(value);
            Array.Reverse(b);
            base.Write(b);
        }
        public override void Write(double value)
        {
            var b = BitConverter.GetBytes(value);
            Array.Reverse(b);
            base.Write(b);
        }

        public void WriteNullTerminatedString(string str)
        {
            Write(Encoding.UTF8.GetBytes(str));
            Write((byte)0);
        }

        public void WriteUTF(string str)
        {
            if (str == null)
                Write((short)0);
            else
            {
                Write((short)str.Length);
                Write(Encoding.UTF8.GetBytes(str));
            }
        }

        public void Write32UTF(string str)
        {
            Write(str.Length);
            Write(Encoding.UTF8.GetBytes(str));
        }

        public void WriteLenUTF(IList list)
        {
            Write((short)list.Count);
            for (var i = 0; i < list.Count; i++)
                WriteUTF((string)list[i]);
        }
        public void WriteLen32UTF(IList list)
        {
            Write((short)list.Count);
            for (var i = 0; i < list.Count; i++)
                Write32UTF((string)list[i]);
        }
    }

    public class NetworkReader : BinaryReader
    {
        public NetworkReader(Stream stream) : base(stream, Encoding.UTF8) { }

        public override short ReadInt16() => IPAddress.NetworkToHostOrder(base.ReadInt16());
        public override int ReadInt32() => IPAddress.NetworkToHostOrder(base.ReadInt32());
        public override long ReadInt64() => IPAddress.NetworkToHostOrder(base.ReadInt64());
        public override ushort ReadUInt16() => (ushort)IPAddress.NetworkToHostOrder(base.ReadUInt16());
        public override uint ReadUInt32() => (uint)IPAddress.NetworkToHostOrder((int)base.ReadUInt32());
        public override ulong ReadUInt64() => (ulong)IPAddress.NetworkToHostOrder((long)base.ReadUInt64());
        public override float ReadSingle()
        {
            var arr = base.ReadBytes(4);
            Array.Reverse(arr);
            return BitConverter.ToSingle(arr, 0);
        }
        public override double ReadDouble()
        {
            var arr = base.ReadBytes(8);
            Array.Reverse(arr);
            return BitConverter.ToDouble(arr, 0);
        }

        public string ReadNullTerminatedString()
        {
            var ret = new StringBuilder();
            var b = ReadByte();
            while (b != 0)
            {
                ret.Append((char)b);
                b = ReadByte();
            }
            return ret.ToString();
        }

        public string ReadUTF() => Encoding.UTF8.GetString(ReadBytes16());
        public string ReadUTF32() => Encoding.UTF8.GetString(ReadBytes32());

        public byte[] ReadBytes16() => ReadBytes(ReadInt16());
        public byte[] ReadBytes32() => ReadBytes(ReadInt32());
    }
}