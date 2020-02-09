using System;

namespace ShutApp.Core.Rank
{
    public class ServerRank : IRank
    {
        public string Prefix { get; set; } = "SERVER";
        public int RankId { get; set; } = 2;
        public ConsoleColor Color { get; set; } = ConsoleColor.Red;
    }
}