
using AO.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{
    public class PoolGameFactory : Object
    {
        public static BasePoolGame Create(GameMode mode)
        {
            return Instantiate(AO.Utilities.PrefabUtil.Create<BasePoolGame>(mode.GetStringValue(), "PoolGame"));
        }

    }
}
