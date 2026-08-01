using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert
{
    public class GamemodeNamesDisplayer : UIAreaMenu
    {
        [SerializeField] private GameModeButton gameModeButtonTemplate;
        [SerializeField] private Button toggleButton;
        private ScrollRect scrollRect;
        private Dictionary<GameMode, GameModeButton> buttons;
        public override void Initialize(bool active)
        {
            base.Initialize(active);
            scrollRect = GetComponent<ScrollRect>();
            buttons = new();
            SetButtons();
            CheckInteractable();
        }

        private void SetButtons()
        {
            foreach (GameMode gameMode in GameModesLoader.gameModesByName.Values) if (!gameMode.isFavorite) AddButton(gameMode);
            gameModeButtonTemplate.gameObject.SetActive(false);
        }
        public void AddButton(GameMode gameMode)
        {
            if (buttons.ContainsKey(gameMode)) return;
            if (toggleButton.interactable == false) toggleButton.interactable = true;
            GameModeButton gameModeButton = Instantiate(gameModeButtonTemplate, scrollRect.content);
            gameModeButton.gameObject.SetActive(true);
            gameModeButton.MenuManager = menuManager;
            gameModeButton.Initialize(gameMode);
            buttons[gameMode] = gameModeButton;
        }
        public void RemoveButton(GameMode gameMode) 
        {
            if (!buttons.TryGetValue(gameMode, out GameModeButton button))
            {
                Debug.LogWarning("Trying to remove button for game mode " + gameMode.name + " which was not found.");
                return;
            }
            Destroy(button.gameObject);
            buttons.Remove(gameMode);
            CheckInteractable();
        }
        private void CheckInteractable() 
        {
            if (buttons.Count == 0)
            {
                CloseArea();
                toggleButton.interactable = false;
            }
        }
    }
}
