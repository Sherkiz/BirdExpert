using System.Collections.Generic;
using UnityEngine;

namespace BirdExpert
{
    public class SettingsArea : UIAreaMenu
    {
        [SerializeField] private List<SettingsOption> settingsOptionsList;
        public Settings currentSettings { get; set; }
        public override void Initialize(bool active)
        {
            base.Initialize(active);
            currentSettings = SettingsLoader.currentSettings;
            foreach (var option in settingsOptionsList) option.Initialize(this);
        }
        public void SaveSettings()
        {
            SettingsLoader.SaveSettings(currentSettings);
        }
    }
}
