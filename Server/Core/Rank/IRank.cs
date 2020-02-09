using System;
using System.Drawing;

namespace ShutApp.Core.Rank
{
    public interface IRank
    {
        public string Prefix { get; set; }
        public int RankId { get; set; }
        public ConsoleColor Color { get; set; }
    }
}