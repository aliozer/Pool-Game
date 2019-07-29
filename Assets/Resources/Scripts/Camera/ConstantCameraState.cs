using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{
    public class ConstantCameraState : BaseCameraState
    {
        public ConstantCameraState(PoolCamera camera) : base(camera)
        {
        }
        

        public override void Handle()
        {
            Camera.SetEnabled(true);
            OnCameraPositionChanged();
        }
    }
}
