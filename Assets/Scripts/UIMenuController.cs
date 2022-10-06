using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] private Text countOfPlayers;
    [SerializeField] private Text errorMessageDuplicatedNames;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private List<InputField> inputFields;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider brightnessSlider;

    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Canvas inputCanvas;
    [SerializeField] private Canvas settingsCanvas;

    //[SerializeField] private float inputOffsetY = 42f;
    private MusicManager musicAudioSource;
    private SoundManager soundAudioSource;
    private Brightness brightness;

    private const string COUNT_OF_PLAYERS = "Count of Players: ";

    void Start()
    {
        musicAudioSource = GameObject.Find("Audio Sources").transform.Find("Music Source").GetComponent<MusicManager>();
        soundAudioSource = GameObject.Find("Audio Sources").transform.Find("Sound Source").GetComponent<SoundManager>();
        brightness = GameObject.Find("Brightness").GetComponent<Brightness>();

        musicAudioSource.PlayBackgroundMusic();

        SetDefaultSlidersValues();

        countOfPlayers.text = COUNT_OF_PLAYERS + slider.value;
        slider.onValueChanged.AddListener(delegate { HandleSliderValueChange(); });
    }

    public void HandleSliderValueChange()
    {
        soundAudioSource.PlayButtonSound();
        countOfPlayers.text = COUNT_OF_PLAYERS + slider.value;
        GameSettings.PlayersCount = (int)slider.value;
    }
    
    public void HandleDropdownValueChange()
    {
        soundAudioSource.PlayButtonSound();
        GameSettings.CardPairs = Convert.ToInt32(dropdown.options[dropdown.value].text);
    }

    public void PlayEnterSound()
    {
        soundAudioSource.PlayEnterSound();
    }

    public void PlaySliderSound()
    {
        soundAudioSource.PlayButtonSound();
    }

    public void ChangeSoundVolume(float value)
    {
        soundAudioSource.PlayButtonSound();
        soundAudioSource.AdjustSoundVolume(value);
    }

    public void ChangeMusicVolume(float value)
    {
        soundAudioSource.PlayButtonSound();
        musicAudioSource.AdjustMusicVolume(value);
    }

    public void ChangeBrightness(float value)
    {
        soundAudioSource.PlayButtonSound();
        brightness.AdjustBrightness(value);
    }

    public void SetDefaultSlidersValues()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 10);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 10);
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 10);
    }

    public bool TrySetPlayersNames()
    {
        GameSettings.PlayersNames = new List<string>(inputFields.Select(field => field.text));
        if (GameSettings.PlayersNames.Count != GameSettings.PlayersNames.Distinct().Count())
        {
            soundAudioSource.PlayErrorSound();
            errorMessageDuplicatedNames.gameObject.SetActive(true);
            return false;
        }
        return true;
    }

    public void EnterPlayerNames()
    {
        soundAudioSource.PlayButtonSound();

        menuCanvas.gameObject.SetActive(false);
        inputCanvas.gameObject.SetActive(true);

        var countIF = 1;
        foreach (var inputField in inputFields)
        {
            if (countIF <= slider.value)
            {
                inputField.gameObject.SetActive(true);
            }
            countIF++;
        }
    }

    public void OpenMenuCanvas()
    {
        soundAudioSource.PlayButtonSound();

        foreach (var inputField in inputFields)
        {
            inputField.gameObject.SetActive(false);
        }

        menuCanvas.gameObject.SetActive(true);
        inputCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(false);
    }

    public void OpenSettingCanvas()
    {
        soundAudioSource.PlayButtonSound();

        menuCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        if (TrySetPlayersNames())
        {
            soundAudioSource.PlayButtonSound();
            SceneManager.LoadScene("Game");
        }
    }

    public void QuitGame()
    {
        soundAudioSource.PlayButtonSound();
        Application.Quit();
    }
}
