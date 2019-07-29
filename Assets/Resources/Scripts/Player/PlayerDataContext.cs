using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolGame
{
    public class PlayerDataContext: BasePlayerData
    {
        public PlayerDataContext(string userName) : base(userName)
        {
            Name = "";
            CuePrefabName = "DefaultCue";
        }

        public string CuePrefabName { get; set; }
        


    }
}
