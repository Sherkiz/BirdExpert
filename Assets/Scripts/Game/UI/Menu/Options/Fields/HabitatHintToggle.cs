using UnityEngine;
namespace BirdExpert
{
    public class HabitatHintToggle : CustomToggle
    {
        public override void Actualize()
        {
            base.Actualize();
            toggle.isOn = settings.allowHabitatHint;
        }
        protected override void ToggleUsed(bool value)
        {
            settings.allowHabitatHint = value;
            base.ToggleUsed(value);
        }
    }
}
