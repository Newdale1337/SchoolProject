using System;
using System.Drawing;

namespace ShutApp.Core.Rank
{
    public class NoneRank : IRank
    {
        public string Prefix { get; set; } = "NONE";
        public int RankId { get; set; } = 0;
        public ConsoleColor Color { get; set; } = ConsoleColor.Gray;
    }
}