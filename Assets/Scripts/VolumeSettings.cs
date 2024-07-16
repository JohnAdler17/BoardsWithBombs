using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start() {
      if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("SFXVolume")) {
        LoadVolume();
      }
      else {
        SetMusicVolume();
        SetSfxVolume();
      }
    }

    public void SetMusicVolume() {
      float volume = musicSlider.value;
      myMixer.SetFloat("music", Mathf.Log10(volume)*20);
      PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSfxVolume() {
      float volume = sfxSlider.value;
      myMixer.SetFloat("SFX", Mathf.Log10(volume)*20);
      PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume() {
      musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
      sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

      SetMusicVolume();
      SetSfxVolume();
    }
}
