using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert 
{ 

    public class AnswerSummaryPanel : UIArea
    {
        [SerializeField] private Image birdImage;
        [SerializeField] private SoundPlayer soundPlayer;
        [SerializeField] private TextMeshProUGUI answerText;
        [SerializeField] private GameObject correctionIconRight;
        [SerializeField] private GameObject correctionIconWrong;
        [SerializeField] private TextMeshProUGUI correctionText;
        [SerializeField] private Button detailsButton;
        private QuizzAnswer answer;
        private EndArea endArea;
        private GameMode gameMode { get => GameManager.Instance.gameMode; }

        public void Initialize(QuizzAnswer answer, EndArea endArea, bool lastInTimeQuiz = false)
        {
            this.answer = answer;
            this.endArea = endArea;
            if (answer.birdImage.image != null) birdImage.sprite = answer.birdImage.image;
            else birdImage.gameObject.SetActive(false);
            if (answer.birdSound.sound != null)
            {
                soundPlayer.SetAudioClip(answer.birdSound.sound);
                soundPlayer.Stop();
            }
            else soundPlayer.SetActive(false);
            answerText.SetText(answer.expectedBird.GetName(gameMode.lang));
            if (!lastInTimeQuiz)
            {
                correctionIconRight.SetActive(answer.isCorrect);
                correctionIconWrong.SetActive(!answer.isCorrect);
                if (answer.isCorrect) correctionText.SetCodeText("good-answer");
                else correctionText.SetText(Language.GetLang("wrong-answer-beginning") + answer.givenAnswer.GetName(gameMode.lang));
                detailsButton.SetCodeText("correction-details-button");
                detailsButton.onClick.AddListener(OpenResultsDetails);
            }
            else
            {
                correctionIconRight.SetActive(false);
                correctionIconWrong.SetActive(false);
                detailsButton.gameObject.SetActive(false);
                correctionText.SetText(Language.GetLang("did-not-have-time"));
            }
        }
        private void OpenResultsDetails() => endArea.OpenResultDetails(answer);
    }
}
