using UnityEngine;
using UnityEngine.SceneManagement;

namespace BirdExpert
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private MainArea mainArea;
        [SerializeField] private OptionsArea optionsArea;
        [SerializeField] private SettingsArea settingsArea;
        private UIAreaMenu activeArea;

        private void Start()
        {
            Initialize();
        }
        public void Initialize()
        {
            mainArea.Initialize(active: true, this);
            optionsArea.Initialize(active: false, this);
            settingsArea.Initialize(active: false, this);
            activeArea = mainArea;
        }
        public void OpenGameModesCreation()
        {
            activeArea.CloseArea();
            optionsArea.OpenArea();
            activeArea = optionsArea;
        }
        public void CloseGameModesCreation()
        {
            optionsArea.CloseArea();
            mainArea.OpenArea();
            activeArea = mainArea;
        }
        public void PlayGame(GameMode gameMode)
        {
            GameManager.Instance.gameMode = gameMode;
            SceneManager.LoadSceneAsync("Game");
        }
        public void OpenBirdDisplayer()
        {
            SceneManager.LoadSceneAsync("Birdrairy");
        }
        public void OpenSettings()
        {
            settingsArea.OpenArea();
        }
        public void OpenCommunity()
        {
            Debug.Log("Should Open Community !");
        }
        public void AddGameMode(GameMode gameMode)
        {
            mainArea.AddGameMode(gameMode);
            optionsArea.AddGameMode(gameMode);
        }
        public void RemoveGameMode(GameMode gameMode, bool resetOptionsArea = false)
        {
            mainArea.RemoveGameMode(gameMode);
            optionsArea.RemoveGameMode(gameMode);
            if (resetOptionsArea) optionsArea.ResetArea();
        }
        public void ActualizeGameModes() => mainArea.Actualize();
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
