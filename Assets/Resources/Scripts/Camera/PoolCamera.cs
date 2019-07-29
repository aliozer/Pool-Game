using AO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{
    [RequireComponent(typeof(Camera))]
    public class PoolCamera: BaseMonoBehaviour
    {
        public event Action<PoolCamera> Updated;
        public event Action<bool> ChangeEnabled;

        public Camera main {
            get {
                return GetComponent<Camera>();
            }
        }

        public override void SetEnabled(bool value)
        {
            ChangeEnabled?.Invoke(value);
            GetComponent<Camera>().enabled = value;
        }
        
        protected override void OnUpdate()
        {
            base.OnUpdate();

            Updated?.Invoke(this);
        }
        

    }
}
