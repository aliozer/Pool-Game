using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

namespace AO.Utilities
{
    public static class UIUtil
    {
        public static bool IsMouseOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

    }
}
