using AO.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame.Rules
{
    public class ThreeBallPointRule : BasePointRule
    {
        public ThreeBallPointRule(ICollection<GameObject> list) : base(list)
        {
        }


        public override int Execute()
        {

            int ballCount = 0;

            foreach (var item in List)
            {
                if (item.tag.Equals(Tags.Ball.GetStringValue()))
                {
                    ballCount++;
                }

            }

 
            return ballCount == 2 ? 1 : 0;
        }
    }
}
