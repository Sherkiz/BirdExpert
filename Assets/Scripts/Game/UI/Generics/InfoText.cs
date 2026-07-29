using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace BirdExpert
{
    public class InfoText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private Canvas canvas;
        private RectTransform canvasRect;
        private CanvasScaler scaler;
        private RectTransform rect;

        public void Initialize()
        {
            rect = GetComponent<RectTransform>();
            canvasRect = canvas.GetComponent<RectTransform>();
            scaler = canvas.GetComponent<CanvasScaler>();
            gameObject.SetActive(false);
        }
        public void Open() => gameObject.SetActive(true);
        public void Close() => gameObject.SetActive(false);
        public void Open(string text)
        {
            infoText.SetText(text);
            Open();
        }
        public void OpenAtMousePosition(string text)
        {
            rect.position = Mouse.current.position.value;
            Open(text);
        }
    }
}
