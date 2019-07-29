using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame.Rules
{
    public class TopCameraDirectionRule : BaseDirectionRule
    {

        public Camera Camera { get; }

        public TopCameraDirectionRule(Camera camera, Transform transform) : base(transform)
        {
            Camera = camera;
        }


        public override bool Execute()
        {
            if (Input.GetMouseButton(0))
            {
                var dir = Input.mousePosition - Camera.WorldToScreenPoint(Transform.position);
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.down);
                Transform.rotation = quaternion;

                return true;
            }

            return false;
            
        }
    }
}
