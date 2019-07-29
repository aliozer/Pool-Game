using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Slider;

namespace AO.UI
{
    public class PowerBar: BaseMonoBehaviour, IPointerUpHandler
    {

        public SliderEvent onPowerApply { get; set; }
        public SliderEvent onValueChanged { get; set; }
 

        public float Value {
            get { return slider.value; }
            set {
                slider.value = value;

            }
        }


        private Slider slider;

        private bool IsPower = false;

        public PowerBar()
        {
            onPowerApply = new SliderEvent();
            onValueChanged = new SliderEvent();
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            slider = GetComponent<Slider>();

            slider.onValueChanged.AddListener(delegate { OnValueChanged(); });

        }
  
        private void OnValueChanged()
        {
            if (!IsPower)
            {
                onValueChanged.Invoke(Value); 
            }
            else
            {
                IsPower = false;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onPowerApply.Invoke(Value);
            IsPower = true;
            slider.value = 0;
        }
        
    }
}
