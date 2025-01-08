using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioMixer myMixer;

    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundFXSlider;
    void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSoundFXVolume();
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMusicVolume()
    {
        float volume = _musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSoundFXVolume()
    {
        float volume = _soundFXSlider.value;
        myMixer.SetFloat("SoundFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    private void LoadVolume()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        _soundFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetMusicVolume();
        SetSoundFXVolume();
    }
}
