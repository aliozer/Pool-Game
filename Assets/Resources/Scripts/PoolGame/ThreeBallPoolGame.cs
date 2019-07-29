using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoolGame.Rules;

namespace PoolGame
{
    public class ThreeBallPoolGame : BasePoolGame
    {
        public override int GetBallCount()
        {
            return 3;
        }

        public override int GetMaxPlayerCount()
        {
            return 2;
        }

        public override GameMode GetMode()
        {
            return GameMode.ThreeBall;
        }

        public override BaseBall GetPlayerBall()
        {
            return Balls[Players.Count - 1];
        }

        public override BasePointRule GetPointRule()
        {
            return new ThreeBallPointRule(HitObjectList);
        }
    }
}
