using AO.RecordSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{
    public class CueReplayData: BaseReplayData
    {
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public Vector3 ParentPosition { get; }
        public Quaternion ParentRotation { get; }

        public BaseCue Cue { get; }
        

        public CueReplayData(BaseCue cue)
        {
            Cue = cue;

            Position = Cue.transform.localPosition;
            Rotation = Cue.transform.localRotation;
            ParentPosition = Cue.Parent.localPosition;
            ParentRotation = Cue.Parent.localRotation;
        }

        public override void Execute()
        {
            Cue.transform.localPosition = Position;
            Cue.transform.localRotation = Rotation;
            Cue.Parent.localPosition = ParentPosition;
            Cue.Parent.localRotation = ParentRotation;
        }
    }
}
