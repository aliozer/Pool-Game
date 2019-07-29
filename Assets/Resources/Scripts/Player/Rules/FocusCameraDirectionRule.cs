using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame.Rules
{
    public class FocusCameraDirectionRule : BaseDirectionRule
    {
        public FocusCameraDirectionRule(Transform transform) : base(transform)
        {
        }

        public override bool Execute()
        {
            if (Input.GetMouseButton(0))
            {
                Transform.rotation =  Quaternion.Euler(0.0f, Transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X"), 0.0f);
                return true;
            }

            return false;
        }
    }
}
