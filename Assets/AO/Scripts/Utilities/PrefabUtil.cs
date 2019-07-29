
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO.Utilities
{
    public static class PrefabUtil
    {
        public static T Create<T>(string prefabName, string path = "") where T : Object
        {
            T result = (T)Resources.Load(Path.Combine($"prefabs/{path}", prefabName), typeof(T));

            if (result == null)
                throw new System.Exception($"{prefabName} is not found.");

            return result;
        }
    }
}
