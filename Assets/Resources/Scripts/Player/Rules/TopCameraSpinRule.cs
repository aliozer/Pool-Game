using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame.Rules
{
    public class TopCameraSpinRule : BaseDirectionRule
    {
        public Camera Camera { get; }
        public float Radius { get; }

        private Vector3 FirstPosition;

        public TopCameraSpinRule(Camera camera, Transform transform, float radius) : base(transform)
        {
            FirstPosition = transform.position;
            Camera = camera;
            Radius = radius;
        }


        public override bool Execute()
        {
            // zaman kalmadı :)

            return false;
        }
    }
}
