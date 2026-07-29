using System;
using UnityEngine;

namespace BirdExpert
{
    public class TimeField : CustomInput
    {
        private int lowLimit = 30;
        private int upLimit = 1800;
        protected override void SetValueInSettings(string input)
        {
            settings.timeLimit = int.Parse(input);
        }
        public override void Actualize()
        {
            inputField.text = settings.timeLimit.ToString();
        }
        public override bool CheckValid()
        {
            isInteractable = !((settings.objective != GameSettings.GameObjective.TimedQuizz) || (settings.objectiveLimitSettings != GameSettings.GameObjectiveLimitSettings.Fixed));
            if (!isInteractable) return true;
            string input = inputField.text;
            if (input == "") return false;
            if (int.Parse(input) < lowLimit) return false;
            return true;
        }
        protected override char ValidateInput(string input, char addedCharacter)
        {
            if (int.TryParse(Char.ToString(addedCharacter), out int addedInt))
            {
                int ans = (input == "") ? addedInt : int.Parse(input) * 10 + addedInt;
                if (ans >= 0 && ans <= upLimit)
                {
                    return addedCharacter;
                }
                return '\0';
            }
            return '\0';
        }
    }
}
