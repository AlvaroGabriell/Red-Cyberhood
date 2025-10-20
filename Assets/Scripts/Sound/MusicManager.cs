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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayMusic(bool resetSong, String musicName = null)
    {
        if (musicName != null)
        {
            audioSourceDefault.clip = musicLibrary.GetRandomClip(name);
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
        audioSourceFuture.clip = musicLibrary.GetRandomClip("GameplayFuture");
        audioSourcePast.clip = musicLibrary.GetRandomClip("GameplayPast");
        audioSourceFuture.Play();
        audioSourcePast.Play();

        activeTimeMusic = audioSourceFuture;
    }

    public void SwitchGameplayMusic()
    {
        if(activeTimeMusic == audioSourceFuture)
        {
            activeTimeMusic = audioSourcePast;
            audioSourceFuture.mute = true;
            audioSourcePast.mute = false;
        } else
        {
            activeTimeMusic = audioSourceFuture;
            audioSourceFuture.mute = false;
            audioSourcePast.mute = true;
        }
    }
    
    public void SetVolume(float volume)
    {
        audioSourceDefault.volume = volume;
    }

    public void AttachSlider(Slider slider)
    {
        if (slider != null)
        {
            musicSlider = slider;
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
            musicSlider.value = audioSourceDefault.volume;
        }
    }

    public void PauseSource(bool pause, AudioSource audioSource)
    {
        if (pause) audioSource.Pause(); else audioSource.UnPause();
    }
}
