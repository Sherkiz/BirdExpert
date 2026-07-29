using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert {
    public class GameModesArea : UIAreaMenu
    {
        [SerializeField] private GameModePanel gameModePanelTemplate;
        [SerializeField] private RectTransform gameModeCreationPanel;
        [SerializeField] private RectTransform gameModesContent;
        private ScrollRect scrollRect;
        private Dictionary<string, GameModePanel> gameModesPanelList;

        public override void Initialize(bool active)
        {
            scrollRect = GetComponent<ScrollRect>();
            SetGameModes();
        }
        public override void OpenArea()
        {
            base.OpenArea();
            if (gameModesPanelList == null) return;
            Actualize();
        }
        private void SetGameModes()
        {
            gameModesPanelList = new();
            foreach (GameMode gameMode in GameModesLoader.gameModesByName.Values) if(gameMode.isFavorite) AddPanel(gameMode);
            gameModePanelTemplate.gameObject.SetActive(false);
            Canvas.ForceUpdateCanvases();
            scrollRect.horizontalScrollbar.value = 0;
        }
        public void AddPanel(GameMode gameMode)
        {
            if (gameModesPanelList.ContainsKey(gameMode.name))
            {
                Debug.LogWarning("Already Existing Game Mode Name !");
                return;
            }
            GameModePanel panel = Instantiate(gameModePanelTemplate, gameModesContent);
            panel.gameObject.SetActive(true);
            panel.MenuManager = menuManager;
            panel.Actualize(gameMode);
            gameModesPanelList[gameMode.name] = panel;
            gameModeCreationPanel.transform.SetAsLastSibling();
        }
        public void RemovePanel(GameMode gameMode)
        {
            if (!gameModesPanelList.TryGetValue(gameMode.name, out GameModePanel panel))
            {
                Debug.LogWarning("Trying to remove panel for game mode " + gameMode.name + " which was not found.");
                return;
            }
            Destroy(panel.gameObject);
            gameModesPanelList.Remove(gameMode.name);
        }
        public void Actualize()
        {
            foreach (string gameModeName in GameModesLoader.gameModesByName.Keys)
            {
                if (gameModesPanelList.TryGetValue(gameModeName, out GameModePanel panel))
                {
                    panel.Actualize(GameModesLoader.gameModesByName[gameModeName]);
                }
            }
        }
    }
}