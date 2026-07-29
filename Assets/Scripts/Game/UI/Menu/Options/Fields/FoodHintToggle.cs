using UnityEngine;
namespace BirdExpert
{
    public class FoodHintToggle : CustomToggle
    {
        public override void Actualize()
        {
            base.Actualize();
            toggle.isOn = settings.allowFoodHint;
        }
        protected override void ToggleUsed(bool value)
        {
            settings.allowFoodHint = value;
            base.ToggleUsed(value);
        }
    }
}
