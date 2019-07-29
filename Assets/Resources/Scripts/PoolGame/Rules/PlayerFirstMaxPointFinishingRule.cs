using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolGame.Rules
{
    public class PlayerFirstMaxPointFinishingRule : BaseFinishingRule
    {
        public PlayerFirstMaxPointFinishingRule(Player[] players, int totalPoint) : base(players, totalPoint)
        {
        }

        public override bool Execute()
        {
            if (!IsCompleted)
            {
                foreach (var player in Players)
                {
                    if (player.LastPoint == TotalPoint)
                    {
                        IsCompleted = true;
                        return IsCompleted;
                    }

                }
            }

            return IsCompleted;
        }
    }
}
