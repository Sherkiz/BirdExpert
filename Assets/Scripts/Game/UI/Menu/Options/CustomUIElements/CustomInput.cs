using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace BirdExpert {
    public abstract class CustomInput : OptionsField
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] protected TMP_InputField inputField;
        [SerializeField] private Image titleImage;
        [SerializeField] private TextMeshProUGUI placeholderText;
        [SerializeField] private string placeholderTextCode;
        public override bool isInteractable { get => inputField.interactable; set => Toggle(value); }
        public override void Initialize()
        {
            base.Initialize();
            inputField.onValueChanged.AddListener(OnInputValueChanged);
            inputField.onValidateInput += delegate (string input, int index, char character) { return ValidateInput(input, character); };
        }
        protected virtual void Toggle(bool active)
        {
            inputField.interactable = active;
            if (titleImage != null) titleImage.SetAlpha(active ? 255f : 100f);
            titleImage.color = active ? titleImage.color : new Color(titleImage.color.r, titleImage.color.g, titleImage.color.b, 100f);
            if (titleText != null) titleText.SetAlpha(active ? 255f : 50f);
            if (!active) 
            { 
                inputField.text = "";
                isValid = true;
            }
        }
        protected abstract void SetValueInSettings(string input);
        protected virtual void OnInputValueChanged(string input)
        {
            OnValueChanged();
            if (isValid && isInteractable) SetValueInSettings(input);
            OnSettingChanged.Invoke();
        }

        protected abstract char ValidateInput(string input, char addedCharacter);
    }
}
