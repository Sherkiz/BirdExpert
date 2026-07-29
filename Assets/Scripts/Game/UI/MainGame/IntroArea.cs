using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace BirdExpert
{
    public class IntroArea : UIAreaMainGame
    {
        [SerializeField] private TMP_InputField startInput;
        [SerializeField] private Button validateButton;
        [SerializeField] private TextMeshProUGUI titleText;
        private int numberOfBirds;
        private int timeOfQuizz;
        private float lowLimit
        {
            get
            {
                if (canvasManager.gameMode.objective == GameSettings.GameObjective.TimedQuizz) return 30;
                if (canvasManager.gameMode.objective == GameSettings.GameObjective.NumberedQuizz) return 1;
                return 0;
            }
        }
        private float upLimit
        {
            get
            {
                if (canvasManager.gameMode.objective == GameSettings.GameObjective.TimedQuizz) return 3600;
                if (canvasManager.gameMode.objective == GameSettings.GameObjective.NumberedQuizz) return 50;
                return 0;
            }
        }
        public override void Initialize(bool active)
        {
            base.Initialize(active);
            string titleCode = "choose-quiz-" + ((canvasManager.gameMode.objective == GameSettings.GameObjective.TimedQuizz) ? "time" : "number");
            titleText.SetText(Language.GetLang("choose-quiz"));
            validateButton.onClick.AddListener(StartQuizz);
            startInput.onValueChanged.AddListener(OnInputValueChange);
            startInput.onValidateInput += delegate (string input, int index, char character) { return ValidateInput(input, character); };
            validateButton.interactable = false;
        }

        private char ValidateInput(string input, char addedCharacter)
        {
            if (int.TryParse(Char.ToString(addedCharacter), out int addedInt))
            {
                int ans = (input == "") ? addedInt : int.Parse(input)*10 + addedInt;
                if (ans >= lowLimit && ans <= upLimit)
                {
                    Debug.Log(ans);
                    return addedCharacter;
                }
                Debug.Log(ans);
                Debug.Log("Too Big !");
                return '\0';
            }
            Debug.Log("Not a number !");
            return '\0';
        }
        private void OnInputValueChange(string input)
        {
            validateButton.interactable = false;
            if (input == "") return;
            int inp = int.Parse(input);
            if (inp != 0) 
            { 
                validateButton.interactable = true;
                if (canvasManager.gameMode.objective == GameSettings.GameObjective.NumberedQuizz) numberOfBirds = inp;
                if (canvasManager.gameMode.objective == GameSettings.GameObjective.TimedQuizz) timeOfQuizz = inp;
            }
        }
        private void StartQuizz()
        {
            CloseArea();
            canvasManager.numberOfBirdsInQuizz = numberOfBirds;
            canvasManager.timeForQuizz = timeOfQuizz;
            canvasManager.StartQuizz();
        }
    }
}
