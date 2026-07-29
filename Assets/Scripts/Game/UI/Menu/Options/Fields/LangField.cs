using System;

namespace BirdExpert
{
    public class LangField : CustomDropdown
    {
        protected override Type enumType => typeof(Lang);
        public override void Actualize()
        {
            realValue = (int)settings.lang;
        }
        protected override void SetValueInSettings(int index)
        {
            settings.lang = (Lang)index;
        }
    }
}
