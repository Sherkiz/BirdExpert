using UnityEngine;
using System;
using System.Linq;

namespace BirdExpert
{
    public class QuizzLimitMode : CustomDropdown
    {
        protected override Type enumType => typeof(GameSettings.GameObjectiveLimitSettings);
        public override void Actualize()
        {
            realValue = (int)settings.objectiveLimitSettings;
        }

        protected override void SetValueInSettings(int index)
        {
            settings.objectiveLimitSettings = (GameSettings.GameObjectiveLimitSettings)index;
        }
    }
}
