using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace BirdExpert
{
    public class HintLangField : CustomDropdown
    {
        protected override Type enumType => typeof(Lang);
        public override void Actualize()
        {
            if (isInteractable) realValue = (int)settings.hintLang;
        }
        public override bool CheckValid()
        {
            isInteractable = settings.traductionMode;
            if (!isInteractable) return true;
            if (realValue == (int)settings.lang) return false;
            SetValueInSettings(realValue);
            return true;
        }
        protected override void SetValueInSettings(int index)
        {
            settings.hintLang = (Lang)index;
        }
        protected override void OnDropdownValueChanged(int index)
        {
            base.OnDropdownValueChanged(index);
        }
    }
}
