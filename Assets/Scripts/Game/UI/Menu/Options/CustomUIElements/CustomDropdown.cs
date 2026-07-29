using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BirdExpert
{
    public abstract class CustomDropdown : OptionsField
    {
        [SerializeField] protected TMP_Dropdown dropdown;
        [SerializeField] protected bool skipFirst = false;
        protected int realValue
        {
            get => dropdown.value + (skipFirst? 1:0);
            set => dropdown.value = value - (skipFirst? 1:0);
        }
        protected int numberOfOptions;
        public override bool isInteractable { 
            get => dropdown.interactable; 
            set 
            { 
                dropdown.interactable = value;
                labelText.gameObject.SetActive(value);
            } 
        }
        private TextMeshProUGUI labelText;
        protected abstract Type enumType { get; }
        public override void Initialize()
        {
            labelText = dropdown.transform.Find("Label").GetComponent<TextMeshProUGUI>();
            SetOptions(enumType);
            base.Initialize();
            Actualize();
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            Language.OnLanguageChanged += UpdateLanguage;
        }
        protected abstract void SetValueInSettings(int index);
        protected virtual void OnDropdownValueChanged(int index)
        {
            if (dropdown.options.Count == 0) return;
            OnValueChanged();
            if (isValid) SetValueInSettings(realValue);
            OnSettingChanged.Invoke();
        }
        protected virtual void SetOptions(Type enumType)
        {
            dropdown.ClearOptions();
            List<string> options = Enum.GetNames(enumType).Select(el => Language.GetLang(el)).ToList();
            if (skipFirst) options.RemoveAt(0);
            numberOfOptions = options.Count;
            dropdown.AddOptions(options);
        }
        private void UpdateLanguage()
        {
            SetOptions(enumType);
            Actualize();
        }
        public void OnDestroy()
        {
            Language.OnLanguageChanged -= UpdateLanguage;
        }
    }
    public abstract class CustomDropdownForBirdsOptions : OptionsField
    {
        [SerializeField] protected TMP_Dropdown dropdown;
        protected int numberOfOptions;
        public override void Initialize()
        {
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
        protected abstract void SetValueInSettings(int index);
        protected virtual void OnDropdownValueChanged(int index)
        {
            SetValueInSettings(index);
        }
    }
}
