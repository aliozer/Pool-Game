using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AO
{
    public class ClockTimer : BaseMonoBehaviour
    {
        public event System.Action<float, float, float> Tick;

        private float timer = 0.0f;
        private bool isStarted = false;
        private bool isContinued = false;

        public int Hours { get; private set; }
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (isStarted && isContinued)
            {
                timer += Time.deltaTime;

                var span = TimeSpan.FromSeconds(timer);

                Hours = span.Hours;
                Minutes = span.Minutes;
                Seconds = span.Seconds;

                Tick?.Invoke(span.Hours, span.Minutes, span.Seconds);
            }


        }

        public void Pause()
        {
            isStarted = false;
        }

        public void Play()
        {
            if (isContinued)
            {
                isStarted = true;
            }
        }

        public void StartTime()
        {
            timer = 0.0f;
            isContinued = true;
            Play();
        }

        public void StopTime()
        {
            isContinued = false;
            Pause();
        }

    }
}
