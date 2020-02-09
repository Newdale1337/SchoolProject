using System;
using System.Drawing;

namespace ShutApp.Core.Rank
{
    public class AdminRank : IRank
    {
        public string Prefix { get; set; } = "ADMIN";
        public int RankId { get; set; } = 1;
        public ConsoleColor Color { get; set; } = ConsoleColor.Red;
    }
}