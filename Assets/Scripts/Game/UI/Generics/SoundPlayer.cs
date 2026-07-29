using BirdExpert;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Button button;
    private void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(ToggleSound);
        button.SetCodeText("play-sound");
    }
    private void ToggleSound()
    {
        if (audioSource.isPlaying) audioSource.Stop();
        else audioSource.Play();
    }
    public void Stop()
    {
        if (audioSource.isPlaying) audioSource.Stop();
    }
    public void SetActive(bool active) => gameObject.SetActive(active);
    public void SetAudioClip(AudioClip clip) => audioSource.clip = clip;
    public void PlayOneShot(AudioClip clip) => audioSource.PlayOneShot(clip);
    public void ResetAudioClip() => audioSource.clip = null;
    public void SetCodeText(string codeText) => button.SetCodeText(codeText);
}
