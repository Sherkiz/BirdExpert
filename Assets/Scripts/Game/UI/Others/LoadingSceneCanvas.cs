using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert
{
    public class LoadingSceneCanvas : MonoBehaviour
    {
        [SerializeField] Slider loadingSlider;
        [SerializeField] TextMeshProUGUI loadingText;

        public void SetLoadingStage(string text, float sliderValue)
        {
            loadingText.SetText(Language.GetLang(text));
            loadingSlider.value = sliderValue;
        }
    }
}
