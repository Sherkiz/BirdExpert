using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert
{
    public class AutoFillText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textBox;
        [SerializeField] private Color baseColor;
        [SerializeField] private Color arrowSelectionColor;
        private Image image;
        private RectTransform rect;
        public RectTransform Rect { get => rect; }
        public Action OnTextClicked { get; set; }
        private string birdName;
        public string BirdName
        {
            get
            {
                return birdName;
            }
            set
            {
                birdName = value;
                textBox.text = value;
            }
        }

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            SelectTextBox(false);
        }
        public void SelectTextBox(bool select)
        {
            image.color = select ? arrowSelectionColor : baseColor;
        }
        public void OnClicked()
        {
            OnTextClicked?.Invoke();
        }
    }
}
