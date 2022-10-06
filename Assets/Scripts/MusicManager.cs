using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudioSource;

    [SerializeField] private AudioClip backgroundMusic;

    private void Start()
    {
        musicAudioSource.volume = 1;
    }

    public void PlayBackgroundMusic()
    {
        musicAudioSource.clip = backgroundMusic;
        musicAudioSource.Play();
    }

    public void StopPlaying()
    {
        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
        }
    }

    public void AdjustMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        musicAudioSource.volume = value / 10;
    }
}
