using System;
using System.Drawing;

namespace ShutApp.Core
{
    public class AdminRank : Rank
    {
        public override string Prefix => "ADMIN";
        public override int RankId => 1;
        public override bool Admin => true;
        public override ConsoleColor Color => ConsoleColor.Red;
    }
}