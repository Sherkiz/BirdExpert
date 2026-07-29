using UnityEngine;
using UnityEngine.UI;

namespace BirdExpert
{
    [RequireComponent(typeof(ScrollRect))]
    [RequireComponent(typeof(AudioSource))]
    public class BirdSpeciesDisplayer : UIArea
    {
        [SerializeField] private BirdSpeciesPanel panelTemplate;
        private ScrollRect scrollRect;
        private AudioSource audioSource;
        public void Initialize()
        {
            scrollRect = GetComponent<ScrollRect>();
            audioSource = GetComponent<AudioSource>();
            SetBirds();
        }

        private void SetBirds()
        {
            foreach(BirdInfo bird in BirdsManager.allBirds.Values)
            {
                BirdSpeciesPanel panel = Instantiate(panelTemplate, scrollRect.content);
                panel.BirdSpeciesDisplayer = this;
                panel.Initialize(bird);
            }
            panelTemplate.gameObject.SetActive(false);
        }

        public void PlayAudioClip(AudioClip clip)
        {
            if (audioSource.isPlaying) audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }
}
