using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert { 
    public class SoundsComparer : UIArea
    {
        [SerializeField] private SoundPlayer goodBirdSoundPlayer;
        [SerializeField] private SoundPlayer badBirdSoundPlayer;
        [SerializeField] private TextMeshProUGUI goodBirdText;
        [SerializeField] private TextMeshProUGUI badBirdText;
        [SerializeField] private Button closeButton;

        public override void Initialize(bool active)
        {
            base.Initialize(active);
            closeButton.onClick.AddListener(CloseArea);
            closeButton.SetCodeText("back-to-result");
        }
        public void OpenArea(BirdSound givenSound, BirdInfo goodBird, BirdInfo badBird)
        {
            goodBirdSoundPlayer.SetAudioClip(givenSound.sound);
            goodBirdText.SetText(goodBird.GetName(GameManager.Instance.gameMode.lang) + GetSoundText(givenSound.type));
            BirdSound badBirdSound = badBird.GetRandomSound(typePriority: givenSound.type, findAnyway: true);
            badBirdSoundPlayer.SetAudioClip(badBirdSound.sound);
            badBirdText.SetText(badBird.GetName(GameManager.Instance.gameMode.lang) + GetSoundText(badBirdSound.type));
            OpenArea();
        }
        public void OpenArea(QuizzAnswer answer) => OpenArea(answer.birdSound, answer.expectedBird, answer.givenAnswer);
        private string GetSoundText(SoundType soundType)
        {
            return soundType switch 
            {
                SoundType.Alarm => " (" + Language.GetLang("alarm") + ")",
                SoundType.Song => " (" + Language.GetLang("song") + ")",
                _ => ""
            };
        }
    }
}
