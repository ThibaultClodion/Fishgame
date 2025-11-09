using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private List<AudioClip> musics;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [HideInInspector] public float MasterVolume = 1.0f;
    [HideInInspector] public float MusicVolume = 1.0f;
    [HideInInspector] public float SFXVolume = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        StartMusicLoop();
    }

    private void StartMusicLoop()
    {
        int musicIndex = Random.Range(0, musics.Count);
        musicSource.clip = musics[musicIndex];
        musicSource.loop = true;
        musicSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        musicSource.volume = MasterVolume * MusicVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        if(clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
        sfxSource.volume = MasterVolume * SFXVolume;
    }
}
