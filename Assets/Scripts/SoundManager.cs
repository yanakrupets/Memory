using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource soundAudioSource;

    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip enterSound;
    [SerializeField] private AudioClip reverbSound;
    [SerializeField] private AudioClip wonSound;

    private void Start()
    {
        soundAudioSource.volume = PlayerPrefs.GetFloat("SoundVolume", 10) / 10;
    }

    public void PlayButtonSound()
    {
        soundAudioSource.PlayOneShot(buttonSound);
    }

    public void PlayErrorSound()
    {
        soundAudioSource.PlayOneShot(errorSound);
    }

    public void PlayEnterSound()
    {
        soundAudioSource.PlayOneShot(enterSound);
    }

    public void PlayReverbSound()
    {
        soundAudioSource.PlayOneShot(reverbSound);
    }

    public void PlayWonSound()
    {
        soundAudioSource.PlayOneShot(wonSound);
    }

    public void AdjustSoundVolume(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
        soundAudioSource.volume = value / 10;
    }
}
