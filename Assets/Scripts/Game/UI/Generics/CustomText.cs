using TMPro;
using UnityEngine;

namespace BirdExpert
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CustomText : MonoBehaviour
    {
        [SerializeField] private string codeText;
        private TextMeshProUGUI tmpText;
        private Languages currentLanguage = Languages.None;
        private void Awake()
        {
            tmpText = GetComponent<TextMeshProUGUI>();
        }
        public void OnEnable()
        {
            UpdateText();
            Language.OnLanguageChanged += SetText;
        }
        private void OnDisable()
        {
            Language.OnLanguageChanged -= SetText;
        }
        private void SetText()
        {
            tmpText.SetText(Language.GetLang(codeText));
            currentLanguage = Language.CurrentLanguage;
        }
        private void UpdateText()
        {
            if (currentLanguage != Language.CurrentLanguage) SetText();
        }
    }
}
