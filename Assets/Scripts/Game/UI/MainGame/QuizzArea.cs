using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert { 
    public class QuizzArea : UIAreaMainGame
    {
        [SerializeField] private Image birdDisplayer;
        [SerializeField] private SoundPlayer soundPlayer;
        [SerializeField] private InputFieldManager inputFieldManager;
        [SerializeField] private ResultArea resultArea;
        [SerializeField] private TextMeshProUGUI limitText;
        [SerializeField] private TextMeshProUGUI otherLangNameText;
        [SerializeField] private QuizzHint habitatHint;
        [SerializeField] private QuizzHint foodHint;
        private BirdInfo currentBird;
        private BirdImage currentImage;
        private BirdSound currentSound;
        private BirdsManager birdsManager { get => canvasManager.birdsManager; }
        public GameMode gameMode { get => canvasManager.gameMode; }
        private int birdCount;
        private bool allowBirdRepetition;
        private float timeCounter;
        private bool isQuizRunning;
        private List<QuizzAnswer> allAnswers;
        private List<BirdInfo> previousBirds => allAnswers.Select(ans => ans.expectedBird).ToList();

        public override void Initialize(bool active)
        {
            base.Initialize(active);
            birdsManager.InitGoodBirds();
            ResetQuiz();
            if (gameMode.answerSetting == GameSettings.AnswerSettings.Direct)
            {
                resultArea.onBackButtonPressed += GoToNextBird;
                resultArea.SetMode(inQuizz: true);
            }
            resultArea.Initialize(false);
            soundPlayer.transform.parent.gameObject.SetActive(gameMode.soundPresenceSetting != GameSettings.DataPresenceSettings.Never);
            birdDisplayer.gameObject.SetActive(gameMode.imagePresenceSetting != GameSettings.DataPresenceSettings.Never);
            habitatHint.Initialize(gameMode.allowHabitatHint);
            foodHint.Initialize(gameMode.allowFoodHint);
            birdCount = 0;
            allAnswers = new();
            otherLangNameText.gameObject.SetActive(gameMode.traductionMode);
            inputFieldManager.Initialize(this);
            isQuizRunning = true;
        }

        private void Update()
        {
            if (!isQuizRunning) { return; }
            if (canvasManager.timeForQuizz != 0 && gameMode.objective == GameSettings.GameObjective.TimedQuizz)
            {
                timeCounter += Time.deltaTime;
                if (timeCounter > canvasManager.timeForQuizz)
                {
                    StopQuizz();
                }
                UpdateTimeText();
            }
        }
        private void UpdateTimeText()
        {
            limitText.SetText(Mathf.RoundToInt(timeCounter).FormatMinutesSeconds() + "  (" + Mathf.RoundToInt(canvasManager.timeForQuizz).FormatMinutesSeconds() + ")");
        }
        private void UpdateNumberText()
        {
            limitText.SetText(Language.GetLang("bird")+" n°"+birdCount+"/"+canvasManager.numberOfBirdsInQuizz);
        }

        private void DisplayBird(BirdImage birdImage)
        {
            if (birdImage.image != null)
            {
                birdDisplayer.sprite = birdImage.image;
                currentImage = birdImage;
            }
            else { Debug.Log("No Image found !"); }
        }
        private void ResetImage()
        {
            birdDisplayer.sprite = null;
            currentImage = new();
        }
        private void SetSound(BirdSound birdSound)
        {
            if (birdSound.sound != null)
            {
                soundPlayer.SetActive(true);
                soundPlayer.SetAudioClip(birdSound.sound);
                currentSound = birdSound;
            }
            else { Debug.Log("No Sound found !"); }
        }
        private void ResetSound()
        {
            soundPlayer.ResetAudioClip();
            currentSound = new();
        }

        public void SetUpNewBird(BirdInfo bird)
        {
            if (gameMode.soundPresenceSetting == GameSettings.DataPresenceSettings.OnlyWhenNeeded) soundPlayer.SetActive(false);
            if (canvasManager.numberOfBirdsInQuizz !=0) UpdateNumberText();
            if (ShouldDisplayImage(bird)) DisplayBird(bird.GetRandomImage(gameMode.imageSetting));
            if (ShouldDisplaySound(bird)) SetSound(bird.GetRandomSound(typePriority: gameMode.soundSetting, findAnyway: true));
            if (gameMode.traductionMode) otherLangNameText.SetText(bird.GetName(gameMode.hintLang));
            if (gameMode.allowHabitatHint) habitatHint.SetHint(bird.AllHabitatsNames);
            if (gameMode.allowFoodHint) foodHint.SetHint(bird.AllTrophicNiches);
            currentBird = bird;
            Debug.Log(bird.spCode + " is now set up !");
        }
        private bool ShouldDisplayImage(BirdInfo bird)
        {
            return gameMode.imagePresenceSetting switch
            {
                GameSettings.DataPresenceSettings.Always => true,
                GameSettings.DataPresenceSettings.OnlyWhenNeeded => bird.NeedsImage,
                _ => false,
            };
        }
        private bool ShouldDisplaySound(BirdInfo bird)
        {
            return gameMode.soundPresenceSetting switch
            {
                GameSettings.DataPresenceSettings.Always => true,
                GameSettings.DataPresenceSettings.OnlyWhenNeeded => bird.NeedsSound,
                _ => false,
            };
        }
        public void StopBirdSound() => soundPlayer.Stop();

        public void SetUpRandomBird()
        {
            birdCount++;
            Debug.Log(birdCount);
            BirdInfo bird = birdsManager.GetRandomBird();
            while (bird == currentBird)
            {
                bird = birdsManager.GetRandomBird();
            }
            SetUpNewBird(bird);
        }
        public void StartQuizz()
        {
            OpenArea();
            SetUpRandomBird();
        }

        public void ProcessAnswer(string input)
        {
            QuizzAnswer quizzAnswer = GetAnswer(input);
            allAnswers.Add(quizzAnswer);
            if (!allowBirdRepetition) birdsManager.AddBirdToSkip(currentBird);
            ResetQuiz();
            if (gameMode.answerSetting == GameSettings.AnswerSettings.Direct)
            {
                resultArea.SetResults(quizzAnswer);
                if (birdCount == canvasManager.numberOfBirdsInQuizz) resultArea.StopQuizz();
            }
            else GoToNextBird();
        }
        private void ResetQuiz()
        {
            inputFieldManager.Interactable = false;
            if (gameMode.soundPresenceSetting != GameSettings.DataPresenceSettings.Never)
            {
                StopBirdSound();
                ResetSound();
            }
            if (gameMode.imagePresenceSetting != GameSettings.DataPresenceSettings.Never) ResetImage();
            if (gameMode.allowHabitatHint) habitatHint.CloseHint();
        }
        private QuizzAnswer GetAnswer(string input)
        {
            bool isCorrect = false;
            BirdInfo givenAnswer = null;
            if (input != "")
            {
                isCorrect = (input == currentBird.GetName(gameMode.lang)); ;
                if (!isCorrect) givenAnswer = birdsManager.GetBirdFromLang(gameMode.lang, input);
            }
            string hintName = gameMode.traductionMode ? currentBird.GetName(gameMode.hintLang) : string.Empty;
            return new QuizzAnswer(birdCount, currentBird, isCorrect, hintName, new BirdImage(currentImage.image, currentImage.sex), new BirdSound(currentSound.sound, currentSound.type), givenAnswer);
        }
        private void GoToNextBird()
        {
            inputFieldManager.ResetInputField();
            if (gameMode.objective == GameSettings.GameObjective.NumberedQuizz && birdCount == canvasManager.numberOfBirdsInQuizz)
            { 
                StopQuizz();
                return;
            }
            inputFieldManager.Interactable = true;
            SetUpRandomBird();
        }

        private void StopQuizz()
        {
            if (gameMode.objective == GameSettings.GameObjective.TimedQuizz) allAnswers.Add(GetAnswer(""));
            canvasManager.StopQuizz(allAnswers);
            timeCounter = 0;
            isQuizRunning = false;
            CloseArea();
        }
    }
    public struct QuizzAnswer
    {
        public int number;
        public BirdInfo expectedBird;
        public bool isCorrect;
        public string hintLanguageName;
        public BirdImage birdImage;
        public BirdSound birdSound;
        public BirdInfo givenAnswer;

        public QuizzAnswer(int number, BirdInfo expectedBird, bool isCorrect, string hintLanguageName, BirdImage birdImage, BirdSound birdSound, BirdInfo givenAnswer)
        {
            this.number = number;
            this.expectedBird = expectedBird;
            this.isCorrect = isCorrect;
            this.hintLanguageName = hintLanguageName;
            this.birdImage = birdImage;
            this.birdSound = birdSound;
            this.givenAnswer = givenAnswer;
        }
    }
}
