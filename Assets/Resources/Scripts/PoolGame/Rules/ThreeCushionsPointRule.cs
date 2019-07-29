using AO.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame.Rules
{
    public class ThreeCushionsPointRule : BasePointRule
    {
        public ThreeCushionsPointRule(ICollection<GameObject> list) : base(list)
        {
        }

        public override int Execute()
        {
   
            int ballCount = 0;
            int wallCount = 0;

            foreach (var item in List)
            {
                if (item.tag.Equals(Tags.Ball.GetStringValue()))
                {
                    ballCount++;
                }
                else if (item.tag.Equals(Tags.Wall.GetStringValue()))
                {
                    wallCount++;
                }
            }

           
            return (ballCount == 2 && wallCount >= 3) ? 1 : 0;
        }
    }
}
