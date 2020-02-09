using System;

namespace ShutApp.Core
{
    public class ServerRank : Rank
    {
        public override string Prefix => "SERVER";
        public override int RankId => 2;
        public override bool Admin => true;
        public override ConsoleColor Color => ConsoleColor.Red;
    }
}