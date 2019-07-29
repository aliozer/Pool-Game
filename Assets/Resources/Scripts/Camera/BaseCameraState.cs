using AO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{
 
    public abstract class BaseCameraState
    {
        public event Action<PoolCamera> CameraPositionChanged;
        
        public PoolCamera Camera { get; }
        
        public BaseCameraState(PoolCamera camera)
        {
            Camera = camera;
        }

        protected virtual void OnCameraPositionChanged()
        {
            CameraPositionChanged?.Invoke(Camera);
        }

        public abstract void Handle();
        
    }
}
