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
    

    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseBall : RecordingObject
    {
        public event CollisionEventHandler Hit;

        public AudioClip BallSound;
        public AudioClip CueSound;
        

        public float Radius {
            get {
                return GetComponent<SphereCollider>().radius;
            }
        }

        public Rigidbody Rigidbody {
            get {
                return GetComponent<Rigidbody>();
            }
        }
        

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Ball.GetStringValue()))
            {
                var magnitude = Mathf.Min(100.0f, collision.relativeVelocity.magnitude);
                SoundManager.instance.PlaySingle(BallSound, magnitude / 70.0f);
            }
            else if(collision.gameObject.tag.Equals(Tags.Cue.GetStringValue()))
            {
                var magnitude = Mathf.Min(100.0f, Rigidbody.velocity.magnitude);
                SoundManager.instance.PlaySingle(CueSound, Rigidbody.velocity.magnitude);
            }

            Hit?.Invoke(gameObject, collision);
        }

        public override BaseReplayData GetReplayData()
        {
            return new BallReplayData(this);
        }

        public override void OnReplayStarted()
        {
            GetComponent<SphereCollider>().enabled = false;
        }

        public override void OnReplayStopped()
        {
            GetComponent<SphereCollider>().enabled = true;
        }


    }
}
