using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert
{
    public class BirdSpeciesPanel : MonoBehaviour
    {
        [SerializeField] private Image birdImage;
        [SerializeField] private TextMeshProUGUI birdNameText;
        [SerializeField] private Button alarmSoundButton;
        [SerializeField] private Button songSoundButton;
        private BirdSpeciesDisplayer birdSpeciesDisplayer;
        public BirdSpeciesDisplayer BirdSpeciesDisplayer { set => birdSpeciesDisplayer = value; }

        public void Initialize(BirdInfo bird)
        {
            birdImage.sprite = bird.GetRandomImage(GameSettings.ImageSettings.Base, Sex.Male).image;
            birdNameText.SetText(bird.GetName(Lang.Latin));
            AudioClip alarmClip = bird.GetRandomSound(SoundType.Alarm, false).sound;
            if (alarmClip != null) alarmSoundButton.onClick.AddListener(() => birdSpeciesDisplayer.PlayAudioClip(alarmClip));
            else alarmSoundButton.gameObject.SetActive(false);
            AudioClip songClip = bird.GetRandomSound(SoundType.Song, false).sound;
            if (songClip != null) songSoundButton.onClick.AddListener(() => birdSpeciesDisplayer.PlayAudioClip(songClip));
            else songSoundButton.gameObject.SetActive(false);
        }
    }
}
