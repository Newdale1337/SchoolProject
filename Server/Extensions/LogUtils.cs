using System;

public static class Log
{
#if DEBUG
    public static void WriteLineIfDebug(string msg) => Console.WriteLine(msg);
#endif
}