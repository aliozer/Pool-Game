using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolGame
{
    public class LastGameData: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("time")]
        private string time = "0:00";

        public string Time {
            get { return time; }
            set { time = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Time))); }
        }

        [JsonProperty("score")]
        private int score = 0;

        public int Score {
            get { return score; }
            set { score = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Score))); }
        }


        [JsonProperty("numberOfStrokes")]
        private int numberOfStrokes = 0;

        public int NumberOfStrokes {
            get { return numberOfStrokes; }
            set { numberOfStrokes = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumberOfStrokes))); }
        }


        public static string GetKey()
        {
            return "lastGame";
        }
    }
}
