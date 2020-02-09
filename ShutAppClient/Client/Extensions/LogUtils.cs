using System;

public static class Log
{
#if DEBUG
    public static void WriteLineIfDebug(string msg) => Console.WriteLine(msg);
#endif

    public static void WriteStartUpHeader()
    {
        Console.WriteLine("############################################################");
        Console.WriteLine("########## Vill du registrerar dig eller Logga in ##########");
        Console.WriteLine("############################################################");
        Console.WriteLine("############################################################");
        Console.WriteLine("################## Skriv [R] Eller [L] #####################");
        Console.WriteLine("############################################################");
    }
}