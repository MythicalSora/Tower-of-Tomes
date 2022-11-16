using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private Dictionary<string, AudioClip> music;
    private Dictionary<string, AudioClip> sounds;
    
    [SerializeField] private AudioClip overworldMusic, battleMusic;

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
        soundSource = gameObject.AddComponent<AudioSource>();
        track1 = gameObject.AddComponent<AudioSource>();
        track2 = gameObject.AddComponent<AudioSource>();
        
        track1.loop = true;
        track1.clip = overworldMusic;
        track1.Play();
        
        track2.loop = true;
        track2.clip = battleMusic;
        track2.volume = 0;
        track2.Play();

        firstTrackPlaying = true;
    }

    private void LoadAudio()
    {
        
    }

    private void AddFiles()
    {
        DirectoryInfo src = new("Assets/AudioFiles/OST");
        FileInfo[] files = src.GetFiles();
        foreach (var file in files)
        {
            //AudioClip clip = (AudioClip)Resources.Load(".@\\"+file.Name);
            //music.Add(file.Name, clip);
            Debug.Log(file.Name);
        }
    }

    public void AddSound(string key,AudioClip ac)
    {
        sounds.Add(key, ac);
    }

    public void addMusic(string key, AudioClip ac)
    {
        music.Add(key, ac);
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
            //soundSource.PlayOneShot(sounds[0], 0.5f);
        }
    }
}
