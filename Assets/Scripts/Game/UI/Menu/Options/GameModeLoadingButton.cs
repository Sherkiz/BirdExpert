using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert
{
    public class GameModeLoadingButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button deleteButton;
        private GameModeLoadingPanel panel;
        public GameModeLoadingPanel Panel { set =>  panel = value; }
        private GameMode gameMode;
        public void Initialize(GameMode gameMode)
        {
            this.gameMode = gameMode;
            loadButton.onClick.AddListener(LoadGameMode);
            deleteButton.onClick.AddListener(DeleteGameMode);
            nameText.SetText(gameMode.name);
        }
        private void LoadGameMode()
        {
            if (panel != null) panel.LoadGameMode(gameMode);
        }
        private void DeleteGameMode()
        {
            if (panel != null) panel.OnDeleteGameModeClicked(gameMode);
        }
    }
}
