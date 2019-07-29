using AO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolGame
{
    public enum GameMode
    {
        [StringValue("ThreeCushionsPoolGame")]
        ThreeCushions,
        [StringValue("ThreeBallPoolGame")]
        ThreeBall,
        [StringValue("FourBallPoolGame")]
        FourBall
    }
}
