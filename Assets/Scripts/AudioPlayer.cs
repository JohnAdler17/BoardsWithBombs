using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    static AudioPlayer instance; //static variables persist through all instances of a class

    [SerializeField] private List<AudioSource> backgroundTracks;

    [SerializeField] private AudioSource soundEffectAudioSource;

    [SerializeField] private AudioClip sizzleSoundEffect;

    [SerializeField] private AudioClip boomSoundEffect;

    [SerializeField] private AudioClip P1moveSE;

    [SerializeField] private AudioClip P2moveSE;

    void Awake() {
      ManageSingleton();
    }

    void ManageSingleton() {

      if (instance != null)
      {
        gameObject.SetActive(false); //by disabling the audio player before destroying it, you can make sure nothing else in the scene will try to access the instance that will be destroyed
        Destroy(gameObject);
      }
      else {
        instance = this;
        DontDestroyOnLoad(gameObject);
      }

    }

    private void StopAllMusic() {
      foreach (AudioSource song in backgroundTracks) {
        song.Stop();
      }
    }

    public void ChangeMusic(int trackIndex) {
      StopAllMusic();
      backgroundTracks[trackIndex].Play();
    }

    public void PlaySizzleSoundEffect() {
      soundEffectAudioSource.PlayOneShot(sizzleSoundEffect);
    }

    public void PlayBoomSoundEffect() {
      soundEffectAudioSource.PlayOneShot(boomSoundEffect);
    }

    public void PlayP1MoveSoundEffect() {
      soundEffectAudioSource.PlayOneShot(P1moveSE);
    }

    public void PlayP2MoveSoundEffect() {
      soundEffectAudioSource.PlayOneShot(P2moveSE);
    }

}
