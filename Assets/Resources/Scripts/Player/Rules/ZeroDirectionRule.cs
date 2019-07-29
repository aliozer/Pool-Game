using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame.Rules
{
    public class ZeroDirectionRule : BaseDirectionRule
    {
      
        public override bool Execute()
        {
            return false;
        }
    }
}
