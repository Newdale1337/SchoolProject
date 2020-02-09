using ShutApp.Messages;
using ShutApp.Network;

namespace Server.Messages
{
    public class ErrorMessage : Message
    {
        public override byte MessageId => Error;

        public string ErrorData { get; set; } = string.Empty;

        public override void Write(NetworkWriter wrt)
        {
            wrt.WriteUTF(ErrorData);
        }

        public const string INVALID_CREDENTIALS = "INVALID_CREDENTIALS";
        public const string NOT_SIGNED_IN = "NO_ACCOUNT";
        public const string ACCOUNT_EXISTS = "ACCOUNT_ALREADY_EXISTS";
        public const string ACCOUNT_IN_USE = "ACCOUNT_IN_USE";
        public const string SESSION_ID_FAULTY = "SESSION_ID_NULL";
        public const string SESSION_ID_INVALID = "SESSION_ID_INVALID";
    }
}