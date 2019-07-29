using AO.RecordSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{
    public class PlayerReplayData : BaseReplayData
    {
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public Player Player { get; }
        public bool IsActive { get; }

        public BaseReplayData CueReplayData { get; }

        public PlayerReplayData(Player player)
        {
            Player = player;

            IsActive = player.gameObject.activeSelf;
            CueReplayData = new CueReplayData(Player.Cue);
            Position = Player.transform.position;
            Rotation = Player.transform.rotation;
        }


        public override void Execute()
        {
            if (IsActive)
            {
                Player.Active();
            }
            else
            {
                Player.Passive();
            }

            Player.transform.position = Position;
            Player.transform.rotation = Rotation;

            CueReplayData.Execute();

        }
    }
}
