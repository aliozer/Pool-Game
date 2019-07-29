
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolGame
{
    public class GameSettings: INotifyPropertyChanged
    {
        [JsonProperty("volume")]
        private float volume = 1.0f;

        public float Volume {
            get { return volume; }
            set { volume = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Volume))); }
        }


        [JsonProperty("muteMusic")]
        private bool muteMusic = false;

        public bool MuteMusic {
            get { return muteMusic; }
            set { muteMusic = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MuteMusic))); }
        }

        [JsonProperty("lastGame")]
        private LastGameData lastGame = new LastGameData();

        public LastGameData LastGame {
            get { return lastGame; }
            set { lastGame = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastGame))); }
        }

        public static string GetKey()
        {
            return "settings";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
