using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BirdExpert
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private QuizzArea quizzArea;
        [SerializeField] private IntroArea introArea;
        [SerializeField] private EndArea endArea;
        public GameMode gameMode { get => GameManager.Instance.gameMode; }
        public BirdsManager birdsManager { get; private set; }
        public int numberOfBirdsInQuizz;
        public float timeForQuizz;
        private bool needsIntro { get => gameMode.objectiveLimitSettings == GameSettings.GameObjectiveLimitSettings.Custom; }

        private void Start()
        {
            InitializeCanvasManager();
        }
        public void InitializeCanvasManager()
        {
            numberOfBirdsInQuizz = gameMode.objective == GameSettings.GameObjective.NumberedQuizz ? gameMode.birdNumberLimit : 0;
            timeForQuizz = gameMode.objective == GameSettings.GameObjective.TimedQuizz ? gameMode.timeLimit : 0f;
            birdsManager = new();
            endArea.Initialize(active: false, this);
            quizzArea.Initialize(active: !needsIntro, this);
            if (needsIntro)
            {
                introArea.Initialize(active: true, this);
            }
            else
            {
                StartQuizz();
            }
        }

        public void StartQuizz()
        {
            quizzArea.StartQuizz();
        }

        public void StopQuizz(List<QuizzAnswer> allAnswers) 
        { 
            endArea.StopQuizz(allAnswers);
        }
        public void ReplayQuiz()
        {
            endArea.ClearArea();
            InitializeCanvasManager();
        }
        public void BackToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
