using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MusicLibrary))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource audioSourceDefault, audioSourceFuture, audioSourcePast, activeTimeMusic;
    private MusicLibrary musicLibrary;
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSourceDefault = GetComponent<AudioSource>();
            musicLibrary = GetComponent<MusicLibrary>();
            DontDestroyOnLoad(gameObject); // Mantém o objeto entre as cenas
        }
        else
        {
            Destroy(gameObject); // Garante que apenas uma instância exista
        }
    }

    void Start()
    {
        audioSourceDefault.volume = 0.5f;
        audioSourceFuture.volume = 0.5f;
        audioSourcePast.volume = 0.5f;
    }

    public void PlayMusic(bool resetSong, string musicName = null)
    {
        if (musicName != null)
        {
            audioSourceDefault.clip = musicLibrary.GetRandomClip(musicName);
        }
        if (audioSourceDefault.clip != null)
        {
            if (resetSong)
            {
                audioSourceDefault.Stop(); // Para a música atual se resetSong for verdadeiro
            }
            audioSourceDefault.Play();
        }
    }

    public void PlayGameplayMusic()
    {
        audioSourceDefault.Stop();
        audioSourceFuture.Stop();
        audioSourcePast.Stop();
        audioSourceFuture.clip = musicLibrary.GetRandomClip("GameplayFuture");
        audioSourcePast.clip = musicLibrary.GetRandomClip("GameplayPast");
        audioSourceFuture.mute = false;
        audioSourcePast.mute = true;
        audioSourceFuture.Play();
        audioSourcePast.Play();

        activeTimeMusic = audioSourceFuture;
    }

    public void SwitchGameplayMusic()
    {
        if (activeTimeMusic == audioSourceFuture)
        {
            activeTimeMusic = audioSourcePast;
            audioSourceFuture.mute = true;
            audioSourcePast.mute = false;
        }
        else
        {
            activeTimeMusic = audioSourceFuture;
            audioSourceFuture.mute = false;
            audioSourcePast.mute = true;
        }
    }
    
    public void StopAllAudioSources()
    {
        audioSourceDefault.Stop();
        audioSourceFuture.Stop();
        audioSourcePast.Stop();
    }
    
    public void SetVolume(float volume)
    {
        audioSourceDefault.volume = volume;
        audioSourceFuture.volume = volume;
        audioSourcePast.volume = volume;
    }

    public void AttachSlider(Slider slider)
    {
        if (slider != null)
        {
            musicSlider = slider;
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
            SetVolume(musicSlider.value);
        }
    }

    public void PauseSource(bool pause, AudioSource audioSource)
    {
        if (pause) audioSource.Pause(); else audioSource.UnPause();
    }
}
