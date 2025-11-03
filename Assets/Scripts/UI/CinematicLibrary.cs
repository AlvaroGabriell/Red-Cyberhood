using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicLibrary : MonoBehaviour
{
    [SerializeField] private Cinematic[] cinematics;
    [SerializeField] private Dictionary<string, Cinematic> cinematicDictionary;

    void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        cinematicDictionary = new Dictionary<string, Cinematic>();
        foreach (Cinematic cinematic in cinematics)
        {
            cinematicDictionary[cinematic.cinematicName] = cinematic;
        }
    }

    public Cinematic GetCinematic(string cinematicName)
    {
        if (cinematicDictionary.ContainsKey(cinematicName))
        {
            return cinematicDictionary[cinematicName];
        }
        return null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

[System.Serializable]
public class Cinematic
{
    public string cinematicName;
    [SerializeField] public CinematicFrame[] cinematicFrames;
    
    [System.Serializable] public struct CinematicFrame
    {
        public Sprite sprite;
        public float timeInScreen;
        public bool fadeInFromPreviousImage;
        [Tooltip("Int percentage value. Ex: 0.3 = 30%")] public float fadeIntesity;
    }
}
