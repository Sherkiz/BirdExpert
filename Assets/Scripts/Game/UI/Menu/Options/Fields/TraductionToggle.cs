using UnityEngine;
namespace BirdExpert
{
    public class TraductionToggle : CustomToggle
    {
        [SerializeField] private HintLangField hintLangField;
        public override void Actualize()
        {
            base.Actualize();
            toggle.isOn = settings.traductionMode;
        }
        public override bool CheckValid()
        {
            if (settings.imagePresenceSetting != GameSettings.DataPresenceSettings.Always && settings.soundPresenceSetting != GameSettings.DataPresenceSettings.Always)
            {
                if (!settings.traductionMode) return false;
            }
            return true;
        }
        protected override void ToggleUsed(bool value)
        {
            settings.traductionMode = value;
            base.ToggleUsed(value);
        }
    }
}
