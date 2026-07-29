using UnityEngine;

namespace BirdExpert
{
    public abstract class SettingsOption : MonoBehaviour
    {
        protected Settings currentSettings;
        public virtual void Initialize(SettingsArea settingsArea)
        {
            currentSettings = settingsArea.currentSettings;
        }
    }
}
