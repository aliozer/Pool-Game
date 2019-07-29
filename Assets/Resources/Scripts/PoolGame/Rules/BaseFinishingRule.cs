using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolGame.Rules
{
    public abstract class BaseFinishingRule : IRule<bool>
    {
        public Player[] Players { get; }
        public int TotalPoint { get; }

        public bool IsCompleted { get; protected set; }

        public BaseFinishingRule(Player[] players, int totalPoint)
        {
            Players = players;
            TotalPoint = totalPoint;
        }

        public abstract bool Execute();
    }
}
