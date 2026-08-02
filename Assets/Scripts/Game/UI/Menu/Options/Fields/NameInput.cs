using System.Linq;
using UnityEngine;

namespace BirdExpert
{
    public class NameInput : CustomInput
    {
        public override void Actualize()
        {
            inputField.text = settings.name;
        }

        protected override void SetValueInSettings(string input)
        {
            settings.name = input;
        }
        public override bool CheckValid()
        {
            if (inputField.text == "") return false;
            return true;
        }
        protected override char ValidateInput(string input, char addedCharacter)
        {
            if (addedCharacter == ' ') return ' ';
            if ((addedCharacter < 'a' || addedCharacter > 'z') && (addedCharacter < 'A' || addedCharacter > 'Z') && (addedCharacter < '0' || addedCharacter > '9'))
            {
                return '\0';
            }
            else return addedCharacter;
        }
    }
}
