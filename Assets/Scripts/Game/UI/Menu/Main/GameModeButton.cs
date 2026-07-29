using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert
{
    public class GameModeButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button playButton;
        [SerializeField] private Button favoriteButton;
        private MenuManager menuManager;
        public MenuManager MenuManager { set => menuManager = value;  }
        private GameMode gameMode;

        public void Initialize(GameMode gameMode)
        {
            this.gameMode = gameMode;
            playButton.onClick.AddListener(PlayGameMode);
            favoriteButton.onClick.AddListener(FavoriteGameMode);
            nameText.SetText(gameMode.name);
        }

        private void PlayGameMode() => menuManager.PlayGame(gameMode);
        private void FavoriteGameMode()
        {
            menuManager.RemoveGameMode(gameMode);
            gameMode.isFavorite = true;
            GameManager.Instance.gameModesLoader.SaveGameMode(gameMode);
            menuManager.AddGameMode(gameMode);
        }
    }
}
