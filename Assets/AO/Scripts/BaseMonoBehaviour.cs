using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO
{
    public class BaseMonoBehaviour: MonoBehaviour
    {
        public event Action<bool> Activated;

        public virtual void Active()
        {
            Activated?.Invoke(true);
            gameObject.SetActive(true);
        }

        public virtual void Passive()
        {
            Activated?.Invoke(false);
            gameObject.SetActive(false);
        }
        
        public virtual void SetEnabled(bool value)
        {
            enabled = value;
        }

        void Awake()
        {
            OnAwake();
        }

        void Start()
        {
            OnStart();
        }

        void Update()
        {
            OnUpdate();
        }
        

        void FixedUpdate()
        {
            OnFixedUpdate();
        }


        protected virtual void OnUpdate() { }
        protected virtual void OnFixedUpdate() { }
        protected virtual void OnStart() { }
        protected virtual void OnAwake() { }
        
    }
}
