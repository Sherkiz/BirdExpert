using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace BirdExpert
{
    public class GameModePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Image imagesIcon;
        [SerializeField] private Image soundsIcon;
        [SerializeField] private Image languageHintIcon;
        [SerializeField] private Image habitatHintIcon;
        [SerializeField] private Image foodHintIcon;
        [SerializeField] private Image limitIconTime;
        [SerializeField] private Image limitIconNumber;
        [SerializeField] private TextMeshProUGUI limitText;
        [SerializeField] private Button playButton;
        [SerializeField] private Button favoriteButton;
        private GameMode gameMode;
        private MenuManager menuManager;
        public MenuManager MenuManager { set =>  menuManager = value; }

        public void Actualize(GameMode gameMode)
        {
            if (gameMode == null) return;
            this.gameMode = gameMode;
            titleText.SetText(gameMode.name);
            imagesIcon.gameObject.SetActive(gameMode.imagePresenceSetting != GameSettings.DataPresenceSettings.Never);
            if (gameMode.imagePresenceSetting == GameSettings.DataPresenceSettings.OnlyWhenNeeded)
            {
                imagesIcon.SetAlpha(100f);
            }
            soundsIcon.gameObject.SetActive(gameMode.soundPresenceSetting != GameSettings.DataPresenceSettings.Never);
            if (gameMode.soundPresenceSetting == GameSettings.DataPresenceSettings.OnlyWhenNeeded)
            {
                soundsIcon.SetAlpha(100f);
            }
            languageHintIcon.gameObject.SetActive(gameMode.hintLang != Lang.None);
            habitatHintIcon.gameObject.SetActive(gameMode.allowHabitatHint);
            foodHintIcon.gameObject.SetActive(gameMode.allowFoodHint);
            limitIconNumber.gameObject.SetActive(gameMode.objective == GameSettings.GameObjective.NumberedQuizz);
            limitIconTime.gameObject.SetActive(gameMode.objective == GameSettings.GameObjective.TimedQuizz);
            string limitTextString = "";
            if (gameMode.objectiveLimitSettings == GameSettings.GameObjectiveLimitSettings.Fixed)
            {
                switch (gameMode.objective)
                {
                    case GameSettings.GameObjective.NumberedQuizz:
                        limitTextString = gameMode.birdNumberLimit.ToString() + " " + Language.GetLang("number-limit-text");
                        break;
                    case GameSettings.GameObjective.TimedQuizz:
                        limitTextString = gameMode.timeLimit.FormatMinutesSeconds();
                        break;
                    default:
                        break;
                }
            }
            else if (gameMode.objectiveLimitSettings == GameSettings.GameObjectiveLimitSettings.Custom)
            {
                switch (gameMode.objective)
                {
                    case GameSettings.GameObjective.NumberedQuizz:
                        limitTextString = Language.GetLang("number-limit-text-custom");
                        break;
                    case GameSettings.GameObjective.TimedQuizz:
                        limitTextString =  Language.GetLang("time-limit-text_custom");
                        break;
                    default:
                        break;
                }
            }
            limitText.SetText(limitTextString);
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(() => menuManager.PlayGame(gameMode));
            favoriteButton.onClick.RemoveAllListeners();
            favoriteButton.onClick.AddListener(UnfavoriteGameMode);
        }
        private void UnfavoriteGameMode()
        {
            menuManager.RemoveGameMode(gameMode);
            gameMode.isFavorite = false;
            GameManager.Instance.gameModesLoader.SaveGameMode(gameMode);
            menuManager.AddGameMode(gameMode);
        }
    }
}
