using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame.Rules
{
    public abstract class BasePointRule : IRule<int>
    {

        public ICollection<GameObject> List { get; }

        public BasePointRule(ICollection<GameObject> list)
        {
            List = list;
        }

        public abstract int Execute();
    }
}
