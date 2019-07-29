using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{
    [Serializable]
    public class TransformData
    {
        [SerializeField]
        private Vector3 position;
        public Vector3 Position => position;

        [SerializeField]
        private Quaternion rotation;
        public Quaternion Rotation => rotation;

        public TransformData(Vector3 position)
        {
            this.position = position;
        }

        public TransformData(Quaternion rotation)
        {
            this.rotation = rotation;
        }

        public TransformData(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}
