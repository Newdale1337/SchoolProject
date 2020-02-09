using System;
using System.Drawing;

namespace ShutApp.Core
{
    public abstract class Rank
    {
        public abstract string Prefix { get; }
        public abstract int RankId { get; }
        public abstract bool Admin { get; }
        public abstract ConsoleColor Color { get; }
    }
}