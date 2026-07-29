using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace BirdExpert 
{
    public class AudioVolumeSlider : SettingsOption
    {
        [SerializeField] private Slider slider;
        [SerializeField] private AudioMixer audioMixer;

        private void Awake()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
        }
        public override void Initialize(SettingsArea settingsArea)
        {
            base.Initialize(settingsArea);
            slider.value = currentSettings.audioVolume;
        }
        private void OnValueChanged(float val)
        {
            val += 0.000001f;
            float volume = Mathf.Log10(val) * 20;
            audioMixer.SetFloat("MasterVolume", volume);
            currentSettings.audioVolume = val;
        }
    }
}
