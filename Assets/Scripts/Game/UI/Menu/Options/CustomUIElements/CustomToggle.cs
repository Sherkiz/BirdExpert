using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert
{
    public abstract class CustomToggle : OptionsField
    {
        [SerializeField] protected Toggle toggle;
        [SerializeField] protected GameObject tickObject;
        public override void Initialize()
        {
            base.Initialize(); 
            toggle.onValueChanged.AddListener(ToggleUsed);
        }
        public override void Actualize()
        {
            tickObject.SetActive(toggle.isOn);
        }
        protected virtual void ToggleUsed(bool value)
        {
            OnValueChanged();
            OnSettingChanged.Invoke();
        }
    }
}
