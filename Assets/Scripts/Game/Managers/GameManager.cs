using UnityEngine;
using UnityEngine.SceneManagement;

namespace BirdExpert
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GameMode gameMode;
        public GameModesLoader gameModesLoader { get; private set; }
        private SettingsLoader settingsLoader;
        private Settings settings;
        private BirdsLoader birdsLoader;
        [SerializeField] private LoadingSceneCanvas loadingSceneCanvas;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            Load();
        }

        private void Load()
        {
            if (loadingSceneCanvas == null) loadingSceneCanvas = FindFirstObjectByType<LoadingSceneCanvas>();
            settingsLoader = new SettingsLoader();
            settings = settingsLoader.LoadSettings();
            Debug.Log(settings.language);
            Language.LoadAllLanguages();
            Language.SetLanguage(settings.language);
            loadingSceneCanvas.SetLoadingStage("loading-gamemodes", 0.1f);
            gameModesLoader = new();
            gameModesLoader.Ready.AddListener(LoadBirds);
        }

        private void LoadBirds()
        {
            birdsLoader = gameObject.AddComponent<BirdsLoader>();
            birdsLoader.LoadingSceneCanvas = loadingSceneCanvas;
            birdsLoader.StartLoading();
            birdsLoader.Ready.AddListener(OnDataReady);
        }
        private void OnDataReady()
        {
            loadingSceneCanvas.SetLoadingStage("loading-completed", 1f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
