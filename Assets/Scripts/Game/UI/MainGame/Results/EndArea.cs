using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert
{
    public enum QuizReward
    {
        None,
        Bronze,
        Silver,
        Gold
    }
    public class EndArea : UIAreaMainGame
    {
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] AnswerSummaryPanel answerSummaryPanelTemplate;
        [SerializeField] ResultArea resultArea;
        [SerializeField] Image bronzeIcon;
        [SerializeField] Image silverIcon;
        [SerializeField] Image goldIcon;
        private List<QuizzAnswer> allAnswers;
        private List<AnswerSummaryPanel> panels;
        public void StopQuizz(List<QuizzAnswer> allAnswers)
        {
            this.allAnswers = allAnswers;
            int numberOfBirds = GameManager.Instance.gameMode.objective switch
            {
                GameSettings.GameObjective.NumberedQuizz => allAnswers.Count,
                GameSettings.GameObjective.TimedQuizz => allAnswers.Count - 1,
                _ => 0,
            };
            int score = allAnswers.Where(answer => answer.isCorrect).Count();
            scoreText.text = score.ToString() + "/" + numberOfBirds.ToString();
            float rate = (float)score / numberOfBirds;
            QuizReward reward = EvaluateQuiz(rate);
            switch (reward)
            {
                case QuizReward.Bronze:
                    bronzeIcon.gameObject.SetActive(true);
                    break;
                case QuizReward.Silver:
                    silverIcon.gameObject.SetActive(true);
                    break;
                case QuizReward.Gold:
                    goldIcon.gameObject.SetActive(true);
                    break;
                default: break;
            }
            OpenArea();
            answerSummaryPanelTemplate.CloseArea();
            resultArea.SetMode(inQuizz: false);
            SetUpAllCorrections();
        }
        private QuizReward EvaluateQuiz(float rate) => GameManager.Instance.gameMode.objective switch
            {
            GameSettings.GameObjective.NumberedQuizz => EvaluateNumberedQuiz(rate),
            GameSettings.GameObjective.TimedQuizz => EvaluateTimedQuiz(rate),
            _ => QuizReward.None,
            };

        private QuizReward EvaluateNumberedQuiz(float rate)
        {
            if (rate > 0.95) return QuizReward.Gold;
            if (rate > 0.75) return QuizReward.Silver;
            if (rate > 0.5) return QuizReward.Bronze;
            return QuizReward.None;
        }
        private QuizReward EvaluateTimedQuiz(float rate)
        {
            float timeRate = allAnswers.Count / ((float) GameManager.Instance.gameMode.timeLimit / 60);
            if (rate > 0.95 && timeRate > 6f) return QuizReward.Gold;
            if (rate > 0.75 && timeRate > 3f) return QuizReward.Silver;
            if (rate > 0.5 && timeRate > 2f) return QuizReward.Bronze;
            return QuizReward.None;
        }
        private void SetUpAllCorrections()
        {
            panels ??= new();
            for(int i = 0; i < allAnswers.Count; i++)
            {
                if (GameManager.Instance.gameMode.objective == GameSettings.GameObjective.TimedQuizz && i == allAnswers.Count - 1)
                {
                    SetUpCorrection(allAnswers[i], true);
                    break;
                }
                SetUpCorrection(allAnswers[i]);
            }
        }
        private void SetUpCorrection(QuizzAnswer answer, bool lastInTimeQuiz = false)
        {
            AnswerSummaryPanel panel = Instantiate(answerSummaryPanelTemplate, answerSummaryPanelTemplate.transform.parent);
            panel.Initialize(answer, this, lastInTimeQuiz);
            panel.OpenArea();
            panels.Add(panel);
        }
        public void OpenResultDetails(QuizzAnswer answer)
        {
            resultArea.SetResults(answer);
        }
        public void ClearArea()
        {
            foreach (var panel in panels) Destroy(panel.gameObject);
            panels.Clear();
            bronzeIcon.gameObject.SetActive(false);
            silverIcon.gameObject.SetActive(false);
            goldIcon.gameObject.SetActive(false);
        }
    }
}
