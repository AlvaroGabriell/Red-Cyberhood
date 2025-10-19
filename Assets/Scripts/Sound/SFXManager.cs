using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SFXLibrary))]
public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    private static AudioSource audioSource;
    private static SFXLibrary sfxLibrary;
    [SerializeField] private Slider sfxSlider;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            sfxLibrary = GetComponent<SFXLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = sfxLibrary.GetRandomClip(soundName);
        if (audioClip != null) audioSource.PlayOneShot(audioClip);
    }
    // Plays the SFX in another AudioSource
    public static void Play(string soundName, AudioSource substitute)
    {
        AudioClip audioClip = sfxLibrary.GetRandomClip(soundName);
        if (audioClip != null) substitute.PlayOneShot(audioClip);
    }
    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void AttachSlider(Slider slider)
    {
        if (slider != null)
        {
            sfxSlider = slider;
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.AddListener(delegate { SetVolume(sfxSlider.value); });
            sfxSlider.value = audioSource.volume;
        }
    }

    public void PauseSource(bool pause)
    {
        if (pause) audioSource.Pause(); else audioSource.UnPause();
    }
}
