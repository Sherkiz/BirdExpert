using System;

namespace BirdExpert
{
    public class Answers : CustomDropdown
    {
        protected override Type enumType => typeof(GameSettings.AnswerSettings);
        public override void Actualize()
        {
            realValue = (int)settings.answerSetting;
        }

        protected override void SetValueInSettings(int index)
        {
            settings.answerSetting = (GameSettings.AnswerSettings)index;
        }
    }
}
