using AO.RecordSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{
    public class BallReplayData : BaseReplayData
    {
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public BaseBall Ball { get; }
        
        public BallReplayData(BaseBall ball)
        {
            Ball = ball;

            Position = Ball.transform.position;
            Rotation = Ball.transform.rotation;
        }

        public override void Execute()
        {
            Ball.transform.position = Position;
            Ball.transform.rotation = Rotation;
        }
    }
}
