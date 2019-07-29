using AO;
using AO.Extensions;
using AO.Media;
using AO.RecordSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{
    [RequireComponent(typeof(DirectionLineCalculator))]
    [RequireComponent(typeof(CapsuleCollider))]
    public abstract class BaseCue : RecordingObject, IDirection
    {
        public event Action<Collider> TriggerStay;

        public AudioClip Sound;
        public GameObject Front { get; protected set; }
        public GameObject Back { get; protected set; }

        public DirectionLineCalculator DirectionLineCalculator {
            get {
                return GetComponent<DirectionLineCalculator>();
            }
        }

        public Transform Parent {
            get {
                return transform.parent.parent;
            }
        }

        public Transform Spin {
            get {
                return transform.parent;
            }
        }

        private void OnDestroy()
        {
            DirectionLineCalculator.Stop();
        }

        public Vector3 Direction {
            get {
                return (Front.transform.position - Back.transform.position);
            }
        }

        public Vector3 GetDirection()
        {
            return new Vector3(Direction.x, 0.0f, Direction.z);
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            Front = transform.Find("Front").gameObject;
            Back = transform.Find("Back").gameObject;
        }

        private void OnTriggerStay(Collider other)
        {
            TriggerStay?.Invoke(other);
        }


        public override BaseReplayData GetReplayData()
        {
            return new CueReplayData(this);
        }

        public override void OnReplayStarted()
        {
            GetComponent<CapsuleCollider>().enabled = false;
        }

        public override void OnReplayStopped()
        {
            GetComponent<CapsuleCollider>().enabled = true;
        }

    }
}
