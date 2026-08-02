using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace BirdExpert
{
    public class PausePanel : UIArea
    {
        public UnityEvent OnPanelClosed;

        public override void CloseArea()
        {
            base.CloseArea();
            OnPanelClosed?.Invoke();
        }
        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame) CloseArea();
        }
    }
}
