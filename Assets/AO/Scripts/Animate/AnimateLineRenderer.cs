using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO.Animate
{
    [RequireComponent(typeof(LineRenderer))]
    public class AnimateLineRenderer: BaseMonoBehaviour
    {
        private LineRenderer lineRenderer;
        public float scrollSpeed = 4.0f;

        protected override void OnStart()
        {
            base.OnStart();

            lineRenderer = GetComponent<LineRenderer>();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            float offset = -Time.time * scrollSpeed;
            lineRenderer.materials[0].mainTextureOffset = new Vector2(offset, 0);
        }
    }
}
