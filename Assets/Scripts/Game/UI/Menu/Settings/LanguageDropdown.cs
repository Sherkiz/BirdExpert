using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace BirdExpert
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LanguageDropdown : SettingsOption
    {
        [SerializeField] private TMP_Dropdown dropdown;
        public override void Initialize(SettingsArea settingsArea)
        {
            base.Initialize(settingsArea);
            SetOptions();
            dropdown.value = ((int) currentSettings.language) - 1;
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
        private void SetOptions()
        {
            dropdown.ClearOptions();
            List<string> options = Enum.GetNames(typeof(Languages)).Select(el => Language.GetLang(el)).ToList();
            options.RemoveAt(0);
            dropdown.AddOptions(options);
        }
        private void OnDropdownValueChanged(int index)
        {
            Languages lang = (Languages)(index + 1);
            Language.SetLanguage(lang);
            currentSettings.language = lang;
        }
    }
}
