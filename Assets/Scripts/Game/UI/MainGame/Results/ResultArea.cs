using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert {
    public class ResultArea : UIArea
    {
        [SerializeField] private Color rightColor;
        [SerializeField] private Color wrongColor;
        [SerializeField] private Button backButton;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private TextMeshProUGUI hintLangText;
        [SerializeField] private Image birdDisplayer;
        [SerializeField] private SoundPlayer soundPlayer;
        [SerializeField] private Button compareBirdsButton;
        [SerializeField] private BirdsComparer birdsComparer;
        [SerializeField] private Button compareSoundsButton;
        [SerializeField] private SoundsComparer soundsComparer;
        public delegate void OnNextBirdButtonPressed();
        public OnNextBirdButtonPressed onBackButtonPressed;
        private Image bgImage;
        private QuizzAnswer answer;

        public override void Initialize(bool active)
        {
            base.Initialize(active);
            bgImage = GetComponent<Image>();
            ResetArea();
            compareBirdsButton.SetCodeText("compare-birds-button");
            compareBirdsButton.onClick.AddListener(() => birdsComparer.OpenArea(answer));
            compareSoundsButton.SetCodeText("compare-sounds-button");
            compareSoundsButton.onClick.AddListener(() => soundsComparer.OpenArea(answer));
            birdsComparer.Initialize(false);
            soundsComparer.Initialize(false); 
            onBackButtonPressed += ResetArea;
            backButton.onClick.AddListener(onBackButtonPressed.Invoke);
        }
        public void SetMode(bool inQuizz = true)
        {
            if (!inQuizz)
            {
                onBackButtonPressed = null;
                onBackButtonPressed += ResetArea;
                backButton.SetCodeText("results-back-button");
            }
            else
            {
                backButton.SetCodeText("results-nextbird-button");
            }
        }
        public void SetResults(QuizzAnswer answer)
        {
            this.answer = answer;
            bgImage.color = answer.isCorrect ? rightColor : wrongColor;
            if (answer.birdImage.image != null)
            {
                birdDisplayer.gameObject.SetActive(true);
                birdDisplayer.sprite = answer.birdImage.image;
            }
            else birdDisplayer.gameObject.SetActive(false);
            if (answer.birdSound.sound != null)
            {
                soundPlayer.transform.parent.gameObject.SetActive(true);
                soundPlayer.SetAudioClip(answer.birdSound.sound);
            }
            else soundPlayer.transform.parent.gameObject.SetActive(false);
            if (answer.isCorrect) SetUpGoodResult();
            else SetUpBadResult(answer.givenAnswer.GetName(GameManager.Instance.gameMode.lang), answer.expectedBird.GetName(GameManager.Instance.gameMode.lang));
            hintLangText.gameObject.SetActive(GameManager.Instance.gameMode.traductionMode);
            hintLangText.SetText(answer.hintLanguageName);
            OpenArea();
        }
        private void SetUpGoodResult()
        {
            resultText.SetCodeText("good-answer");
            compareBirdsButton.gameObject.SetActive(false);
        }

        private void SetUpBadResult(string input, string answer)
        {
            resultText.SetText(Language.GetLang("wrong-answer-beginning") + input + Language.GetLang("wrong-answer-end") + answer);
            compareBirdsButton.gameObject.SetActive(true);
        }
        public void ResetArea()
        {
            CloseArea();
            resultText.text = "";
        }
        public void StopQuizz() => backButton.SetCodeText("results-end-button");
    } 
}
