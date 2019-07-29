using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{
    public static class Prefs
    {
        public static GameSettings Settings { get; private set; }
        

        static Prefs()
        {
            Settings = GetGameSettings();
        }



        public static void Save()
        {
            PlayerPrefs.SetString(GameSettings.GetKey(), Newtonsoft.Json.JsonConvert.SerializeObject(Settings));
        }

        private static GameSettings GetGameSettings()
        {
            var settingsJson = Get(GameSettings.GetKey());

            if (string.IsNullOrEmpty(settingsJson))
            {
                return new GameSettings();
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<GameSettings>(settingsJson);

        }

        private static string Get(string key)
        {
            return PlayerPrefs.GetString(key);
        }
    }
}
