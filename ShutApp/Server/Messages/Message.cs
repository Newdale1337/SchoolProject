using ShutApp.Network;

namespace ShutApp.Messages
{
    public abstract class Message
    {
        public abstract byte MessageId { get; }
        public virtual void Write(NetworkWriter wrt) { }
        public virtual void Read(NetworkReader rdr) { }
        public virtual void Handle(UserClient client) { }

        public const byte Login = 0;
        public const byte Register = 1;
        public const byte RegisterResponse = 2;
        public const byte Connect = 3;
        public const byte Send = 4;
        public const byte Error = 5;
        public const byte Success = 6;
        public const byte LoginResponse = 7;
        public const byte ConnectResponse = 8;
    }
}