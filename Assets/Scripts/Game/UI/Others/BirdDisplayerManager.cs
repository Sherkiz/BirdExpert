using UnityEngine;
using UnityEngine.SceneManagement;

namespace BirdExpert
{
    public class BirdDisplayerManager : UIArea
    {
        [SerializeField] private BirdSpeciesDisplayer birdSpeciesDisplayer;

        private void Start()
        {
            Initialize();
        }
        private void Initialize()
        {
            birdSpeciesDisplayer.Initialize();
        }
        public void GoBackToMenu()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
