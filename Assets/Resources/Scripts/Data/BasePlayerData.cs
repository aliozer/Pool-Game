using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolGame
{
    public abstract class BasePlayerData : INotifyPropertyChanged
    {

        private int point;

        public int Point {
            get { return point; }
            set { point = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Point))); }
        }


        private string name = "";

        public string Name {
            get { return name; }
            set { name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name))); }
        }

        private string userName = "";

        public string UserName {
            get { return userName; }
            set { userName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName))); }
        }

        public BasePlayerData(string userName)
        {
            UserName = userName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
  
}
