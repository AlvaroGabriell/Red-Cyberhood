using System;
using System.Collections.Generic;
using UnityEngine;

public class MusicLibrary : MonoBehaviour
{
    [SerializeField] private MusicGroup[] musicGroups;
    private Dictionary<string, List<AudioClip>> musicDictionary;

    void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        musicDictionary = new Dictionary<string, List<AudioClip>>();
        foreach (MusicGroup musicGroup in musicGroups)
        {
            musicDictionary[musicGroup.name] = musicGroup.audioClips;
        }
    }

    public AudioClip GetRandomClip(string name)
    {
        if (musicDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = musicDictionary[name];
            if (audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }
        return null;
    }

    //public AudioClip GetAudioClip(string name)
    //{
    //    if (musicDictionary.ContainsKey(name))
    //    {
    //        
    //    }
    //}
}

[System.Serializable] public struct MusicGroup
{
    public string name;
    public List<AudioClip> audioClips;
}