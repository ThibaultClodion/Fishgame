using UnityEngine;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        // Initialize sliders with current volume levels
        masterSlider.value = AudioManager.Instance.MasterVolume;
        musicSlider.value = AudioManager.Instance.MusicVolume;
        sfxSlider.value = AudioManager.Instance.SFXVolume;

        // Add listeners to sliders
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void SetMasterVolume(float volume)
    {
        AudioManager.Instance.SetMasterVolume(volume);
    }

    private void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    private void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }
}
