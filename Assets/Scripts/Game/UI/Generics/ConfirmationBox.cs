using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BirdExpert
{
    public class ConfirmationBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private string textCode;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;
        public bool autoCloseWhenConfirmed = true;
        private bool isMouseOver;

        public void Open()
        {
            gameObject.SetActive(true);
            if (autoCloseWhenConfirmed) confirmButton.onClick.AddListener(Close);
        }
        public void Close()
        {
            gameObject.SetActive(false);
            confirmButton.onClick.RemoveAllListeners();
        }
        public void Open(string code)
        {
            text.SetText(Language.GetLang(code));
            Open();
        }
        public void AddConfirmationAction(UnityAction call)
        {
            confirmButton.onClick.AddListener(call);
        }
        public void AddCancelAction(UnityAction call)
        {
            cancelButton.onClick.AddListener(call);
        }
        private void Awake()
        {
            cancelButton.onClick.AddListener(Close);
            Close();
        }
        private void Update()
        {
            if (Mouse.current.leftButton.isPressed && !isMouseOver) Close();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isMouseOver = false;
        }
    }
}
