using System;

namespace BirdExpert
{
    public class NumberOfBirds : CustomInput
    {
        private int lowLimit = 3;
        private int upLimit = 100;
        protected override void SetValueInSettings(string input)
        {
            settings.birdNumberLimit = int.Parse(input);
        }
        public override void Actualize() 
        { 
            inputField.text = settings.birdNumberLimit.ToString();
        }
        public override bool CheckValid()
        {
            isInteractable = !((settings.objectiveLimitSettings != GameSettings.GameObjectiveLimitSettings.Fixed) || (settings.objective != GameSettings.GameObjective.NumberedQuizz));
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
