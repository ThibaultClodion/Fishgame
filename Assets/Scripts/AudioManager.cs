using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private List<AudioClip> musics;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioMixer masterMixer;

    [Header("Default volumes")]
    public float MasterVolume = 1.0f;
    public float MusicVolume = 1.0f;
    public float SFXVolume = 1.0f;

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
        SetMasterVolume(MasterVolume);
        SetSFXVolume(SFXVolume);
        SetMusicVolume(MusicVolume);
        StartMusicLoop();
    }

    private void StartMusicLoop()
    {
        int musicIndex = Random.Range(0, musics.Count);
        musicSource.clip = musics[musicIndex];
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if(clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    // Volume in linear 0-1, map directly to dBs inside of the mixer to have exponential linear scaling
    // https://www.dr-lex.be/info-stuff/volumecontrols.html
    private void SetMixerGroupVolume(string groupName, float volume) {
        masterMixer.SetFloat(groupName, -(1.0f-volume) * 80);
    }

    public void SetMasterVolume(float volume)
    {
        MasterVolume = volume;
        SetMixerGroupVolume("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        SetMixerGroupVolume("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
        SetMixerGroupVolume("SFXVolume", volume);
    }
}
