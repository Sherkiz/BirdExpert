using BirdExpert;
using UnityEngine;
using UnityEngine.UI;

public class AdaptativeLayout : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup layoutGroup;
    private RectTransform rect;
    private Vector2 size;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        size = rect.rect.size;
        FitLayoutSize();
    }
    private void FitLayoutSize()
    {
        Canvas.ForceUpdateCanvases();
        float newX = layoutGroup.RealSize().x - rect.rect.width;
        float newY = layoutGroup.RealSize().y - rect.rect.height;
        rect.sizeDelta = new(newX, newY);
    }
}
