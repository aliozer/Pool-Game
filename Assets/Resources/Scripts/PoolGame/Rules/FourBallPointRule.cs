using AO.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame.Rules
{
    public class FourBallPointRule : BasePointRule
    {
        public FourBallPointRule(ICollection<GameObject> list) : base(list)
        {
        }

        public override int Execute()
        {

            int ballCount = 0;

            HashSet<Type> ballTypes = new HashSet<Type>();

            foreach (var item in List)
            {
                if (item.tag.Equals(Tags.Ball.GetStringValue()))
                {
                    ballCount++;

                    var ball = item.GetComponent<BaseBall>();
                    ballTypes.Add(ball.GetType());
                }
            }

            Debug.Log(ballCount);

            if (ballCount == 3)
            {
                return 25;
            }
            else if (ballCount == 2)
            {
                if (ballTypes.Count == 1)
                {
                    // iki kırmızı
                    return 3;
                }
            }

            // bir kırmızı bir sarı
            return ballCount == 2 ? 1 : 0;
        }
    }
}
