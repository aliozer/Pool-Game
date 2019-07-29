using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame.Rules
{
    public abstract class BaseDirectionRule : IRule<bool>
    {
        public Transform Transform { get; set; }

        public BaseDirectionRule(Transform transform)
        {
            Transform = transform;
        }

        public BaseDirectionRule()
        {

        }


        public abstract bool Execute();
    }
}
