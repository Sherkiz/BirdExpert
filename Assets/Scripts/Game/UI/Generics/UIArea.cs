using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert
{
    public class UIArea : MonoBehaviour
    {
        public virtual void Initialize(bool active)
        {
            ToggleArea(active);
        }
        public virtual bool IsOpen { get => gameObject.activeSelf; }
        public virtual void OpenArea() => gameObject.SetActive(true);
        public virtual void CloseArea() => gameObject.SetActive(false);
        public virtual void ToggleArea(bool active) => gameObject.SetActive(active);
        public virtual void ToggleArea()
        {
            if (IsOpen) CloseArea();
            else OpenArea();
        }
    }
    [System.Serializable]
    public class UIOpenablePanel
    {
        public Button button;
        public UIArea area;
        private RectTransform areaRect;
        public RectTransform AreaRect 
        { 
            get
            {
                if (area == null) return null;
                if (areaRect == null) areaRect = area.GetComponent<RectTransform>();
                return areaRect;
            }
            private set
            {
                areaRect = value;
            }
        }

        public bool IsOpen { get => area.IsOpen; }
        public void OpenArea()
        {
            area.OpenArea();
            button.interactable = false;
            button.image.color = Color.white;
        }
        public void CloseArea()
        {
            area.CloseArea();
            button.interactable = true;
            button.image.color = Color.gray;
        }
    }

    public class UIAreaMainGame : UIArea
    {
        protected CanvasManager canvasManager;

        public virtual void Initialize(bool active,  CanvasManager canvasManager)
        {
            this.canvasManager = canvasManager;
            Initialize(active);
        }
    }

    public class UIAreaMenu : UIArea
    {
        protected MenuManager menuManager;
        public virtual void Initialize(bool active, MenuManager menuManager)
        {
            this.menuManager = menuManager;
            Initialize(active);
        }
    }
}