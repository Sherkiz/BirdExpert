using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BirdExpert
{
    public class MainArea : UIAreaMenu
    {
        [SerializeField] private GameModesArea gameModeArea;
        [SerializeField] private BottomPanel bottomPanel;
        public override void Initialize(bool active)
        {
            base.Initialize(active);
            gameModeArea.Initialize(true, menuManager);
            bottomPanel.Initialize(true, menuManager);
        }
        public override void OpenArea()
        {
            base.OpenArea();
            gameModeArea.OpenArea();
        }
        public void AddGameMode(GameMode gameMode) 
        {
            if (gameMode.isFavorite) gameModeArea.AddPanel(gameMode);
            else bottomPanel.AddGameMode(gameMode);
        }
        public void RemoveGameMode(GameMode gameMode)
        {
            if (gameMode.isFavorite) gameModeArea.RemovePanel(gameMode);
            else bottomPanel.RemoveGameMode(gameMode);
        }
        public void Actualize() => gameModeArea.Actualize();
    }
}
