using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert
{
    public class GameModeLoadingButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;
        private GameModeLoadingPanel panel;
        public GameModeLoadingPanel Panel { set =>  panel = value; }
        private string gameModeName;
        private GameMode gameMode { get => GameModesLoader.gameModesByName[gameModeName]; }
        public void Initialize(string gameModeName)
        {
            this.gameModeName = gameModeName;
            nameText.SetText(gameModeName);
        }
        public void LoadGameMode()
        {
            if (panel != null) panel.LoadGameMode(gameMode);
        }
        public void DeleteGameMode()
        {
            if (panel != null) panel.OnDeleteGameModeClicked(gameMode);
        }
    }
}
