using System;
using ShutApp.Core;

namespace Server.Core
{
    public class NoneRank : Rank
    {
        public override string Prefix => "NONE";
        public override int RankId => 0;
        public override bool Admin => false;
        public override ConsoleColor Color => ConsoleColor.Gray;
    }
}