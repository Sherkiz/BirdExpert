using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BirdExpert
{
    public class Habitat : CustomDropdownForBirdsOptions
    {
        public override void Initialize()
        {
            base.Initialize(); 
            List<string> options = BirdsManager.allBirds.Values.Select(bird => bird.AllHabitatsNames.ToList()).SelectMany(habitat => habitat).Distinct().ToList();
            dropdown.ClearOptions();
            dropdown.AddOptions(options);
        }
        public override void Actualize()
        {
            dropdown.value = (int)settings.answerSetting;
        }
        protected override void SetValueInSettings(int index)
        {
            settings.answerSetting = (GameSettings.AnswerSettings)index;
        }
    }
}
