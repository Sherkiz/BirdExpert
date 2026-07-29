using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert
{
    public class QuizzHint : UIArea
    {
        [Header("Components")]
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private UIArea hintArea;
        [SerializeField] private TextMeshProUGUI hintText;

        [Space(20), Header("Infos")]
        [SerializeField] private string titleTextCode;
        public override void Initialize(bool active)
        {
            base.Initialize(active);
            if (!active) return;
            text.SetCodeText(titleTextCode);
            button.onClick.AddListener(hintArea.OpenArea);
            hintArea.CloseArea();
        }
        public void SetHint(string[] hintTextArray)
        {
            string hintText = string.Empty;
            foreach (string hint in hintTextArray)
            {
                hintText += hint + "\n";
            }
            this.hintText.SetText(hintText);
        }
        public void CloseHint()
        {
            hintText.SetText(string.Empty);
            hintArea.CloseArea();
        }
    }
}
