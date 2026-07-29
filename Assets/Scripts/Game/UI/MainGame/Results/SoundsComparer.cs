using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert { 
    public class SoundsComparer : UIArea
    {
        [SerializeField] SoundPlayer goodBirdSoundPlayer;
        [SerializeField] SoundPlayer badBirdSoundPlayer;
        [SerializeField] TextMeshProUGUI goodBirdText;
        [SerializeField] TextMeshProUGUI badBirdText;
        [SerializeField] Button closeButton;

        public override void Initialize(bool active)
        {
            base.Initialize(active);
            closeButton.onClick.AddListener(CloseArea);
            closeButton.SetCodeText("back-to-result");
        }
        public void OpenArea(BirdInfo goodBird, BirdInfo badBird, SoundType soundType = SoundType.AllSounds)
        {
            string soundText = string.Empty;
            if (soundType == SoundType.Alarm) soundText = " (" + Language.GetLang("alarm") + ")";
            else if (soundType == SoundType.Song) soundText = " (" + Language.GetLang("song") + ")";
            goodBirdSoundPlayer.SetAudioClip(goodBird.GetRandomSound(type: soundType).sound);
            badBirdSoundPlayer.SetAudioClip(badBird.GetRandomSound(type: soundType).sound);
            goodBirdText.SetText(goodBird.GetName(GameManager.Instance.gameMode.lang) + soundText);
            badBirdText.SetText(badBird.GetName(GameManager.Instance.gameMode.lang) + soundText);
            OpenArea();
        }
        public void OpenArea(QuizzAnswer answer) => OpenArea(answer.expectedBird, answer.givenAnswer, answer.birdSound.type);
    }
}
