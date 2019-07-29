using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO.RecordSystem
{
    public abstract class RecordingObject: BaseMonoBehaviour
    {
        public bool IsReplaying { get; set; }

        void Update()
        {
            if (IsReplaying)
            {
                OnReplay();
            }
            else
            {
                OnUpdate();
            }
        }


        protected virtual void OnReplay() { }

        public abstract BaseReplayData GetReplayData();

        public abstract void OnReplayStarted();
        public abstract void OnReplayStopped();
    }
}
