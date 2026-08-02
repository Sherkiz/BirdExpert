
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert {
    public class GameModeLoadingPanel : UIAreaMenu
    {
        [SerializeField] GameModeLoadingButton buttonTemplate;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private ConfirmationBox confirmationBox;
        private Dictionary<GameMode, GameModeLoadingButton> buttons;
        private OptionsArea optionsArea;
        public OptionsArea OptionsArea { set =>  optionsArea = value; }
        public override void Initialize(bool active)
        {
            base.Initialize(active);
            buttons = new();
            scrollRect.gameObject.SetActive(false);
            SetButtons();
        }
        public void ToggleViewport() => scrollRect.gameObject.SetActive(!scrollRect.gameObject.activeSelf);
        public override void CloseArea() => scrollRect.gameObject.SetActive(false);
        private void SetButtons()
        {
            foreach (GameMode gameMode in GameModesLoader.gameModesByName.Values) AddButton(gameMode);
            buttonTemplate.gameObject.SetActive(false);
        }
        public void AddButton(GameMode gameMode)
        {
            GameModeLoadingButton gameModeButton = Instantiate(buttonTemplate, scrollRect.content);
            gameModeButton.gameObject.SetActive(true);
            gameModeButton.Panel = this;
            gameModeButton.Initialize(gameMode.name);
            buttons[gameMode] = gameModeButton;
        }
        public void LoadGameMode(GameMode gameMode) => optionsArea.LoadGameMode(gameMode);
        public void RemoveGameMode(GameMode gameMode)
        {
            if (!buttons.TryGetValue(gameMode, out GameModeLoadingButton button))
            {
                Debug.LogWarning("Trying to remove button for game mode " + gameMode.name + " which was not found.");
                return;
            }
            Destroy(button.gameObject);
            buttons.Remove(gameMode);
        }
        public void OnDeleteGameModeClicked(GameMode gameMode) 
        {
            confirmationBox.AddConfirmationAction(() => AskGameModeDeletion(gameMode));
            confirmationBox.Open("delete-gamemoded-confirmation");
        }
        public void AskGameModeDeletion(GameMode gameMode)
        {
            menuManager.RemoveGameMode(gameMode, true);
            GameManager.Instance.gameModesLoader.TryRemoveGameMode(gameMode);
        }
    } 
}
