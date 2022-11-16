using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private Dictionary<string, AudioClip> music = new();
    private Dictionary<string, AudioClip> sounds = new();
    
    // [SerializeField] private AudioClip overworldMusic, battleMusic;

    [SerializeField] private Dictionary<int, int> deez;

    private AudioSource track1, track2, soundSource;
    private bool firstTrackPlaying;
    public static AudioManager audioManager { get; private set; }

    void Awake()
    {
        if (audioManager != null)
        {
            Destroy(this);
        }
        else
        {
            audioManager = this;
        }
    }
    void Start()
    {
        AddFiles();
        // soundSource = gameObject.AddComponent<AudioSource>();
        track1 = gameObject.AddComponent<AudioSource>();
        track2 = gameObject.AddComponent<AudioSource>();
        
        track1.loop = true;
        track1.clip = music.FirstOrDefault(a => a.Key == "OverworldMusicAdditive_Demo3.ogg").Value;
        track1.Play();
        
        track2.loop = true;
        track2.clip = music.FirstOrDefault(a => a.Key == "CombatMusicAdditive_Demo3.ogg").Value;
        track2.volume = 0;
        track2.Play();

        firstTrackPlaying = true;
    }

    private void LoadAudio()
    {
        
    }

    private void AddFiles()
    {
        DirectoryInfo musicFiles = new("Assets/Resources/Audio/AudioFiles/OST");
        DirectoryInfo soundFiles = new("Assets/Resources/Audio/AudioFiles/Sound");
        FileInfo[] mFiles = musicFiles.GetFiles();
        FileInfo[] sFiles = soundFiles.GetFiles();
        foreach (var file in mFiles)
        {
            //AudioClip clip = (AudioClip)Resources.Load(".@\\"+file.Name);
            //music.Add(file.Name, clip);
            AddFile(file.Name, "OST");
        }
        
        foreach (var file in sFiles)
        {
            //AudioClip clip = (AudioClip)Resources.Load(".@\\"+file.Name);
            //music.Add(file.Name, clip);
            AddFile(file.Name, "Sound");
        }
    }

    private void AddFile(string fileName, string type)
    {
        switch (type)
        {
            case "OST":
                music.Add(fileName, Resources.Load<AudioClip>("Audio/AudioFiles/OST/" + fileName));
                break;
            case "Sound":
                sounds.Add(fileName, Resources.Load<AudioClip>("Audio/AudioFiles/Sound/" + fileName));
                break;
        }
    }

    public void AddSound(string key)
    {
        AddFile(key, "Sound");
    }

    public void AddMusic(string key)
    {
        AddFile(key, "OST");
    }

    public void SwapBgTrack()
    {
        StopAllCoroutines();
        if (firstTrackPlaying)
        {
            StartCoroutine(FadeTrack(track1, track2));
        }
        else
        {
            StartCoroutine((FadeTrack(track2, track1)));
        }
        firstTrackPlaying = !firstTrackPlaying;
    }
    
    private IEnumerator FadeTrack(AudioSource trackFrom, AudioSource trackTo)
    {
        float timeToFade = 1.25f;
        float timeElapsed = 0;
        while (timeElapsed < timeToFade)
        {
            trackTo.volume = Mathf.Lerp(0, 1, timeElapsed/timeToFade);
            trackFrom.volume = Mathf.Lerp(1, 0, timeElapsed/timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            SwapBgTrack();
        }

        if (Input.GetKeyDown("e"))
        {
            soundSource.PlayOneShot(sounds.First().Value, 0.5f);
        }
    }
}
