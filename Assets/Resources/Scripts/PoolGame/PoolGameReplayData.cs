using AO.RecordSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolGame
{
    public class PoolGameReplayData : BaseReplayData
    {

        public BasePoolGame Game { get; }
        public BaseReplayData PlayerReplayData { get; }
        public List<BaseReplayData> BallReplayDataList { get;  }

        public PoolGameReplayData(BasePoolGame game)
        {
            Game = game;

            BallReplayDataList = new List<BaseReplayData>();

            for (int i = 0; i < Game.Balls.Count; i++)
            {
                BallReplayDataList.Add(Game.Balls[i].GetReplayData());
            }


            PlayerReplayData = Game.Player.GetReplayData();
        }


        public override void Execute()
        {
            foreach (var item in BallReplayDataList)
            {
                item.Execute();
            }

            PlayerReplayData.Execute();
        }
        
    }
}
