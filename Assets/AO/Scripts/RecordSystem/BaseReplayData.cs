using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO.RecordSystem
{
    public abstract class BaseReplayData
    {
        public float Time { get; set; }

        public abstract void Execute();
        
    }
}
