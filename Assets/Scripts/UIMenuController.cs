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
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Canvas inputCanvas;
    [SerializeField] private Canvas settingsCanvas;
    [SerializeField] private InputField firstInput;
    [SerializeField] private float inputOffsetY = 42f;
    [SerializeField] private AudioSource buttonAS;
    [SerializeField] private AudioSource errorAS;

    private const string COUNT_OF_PLAYERS = "Count of Players: ";

    private List<InputField> _inputFields = new List<InputField>();

    void Start()
    {
        countOfPlayers.text = COUNT_OF_PLAYERS + slider.value;
        slider.onValueChanged.AddListener(delegate { HandleSliderValueChange(); });
    }

    public void HandleSliderValueChange()
    {
        buttonAS.Play();
        countOfPlayers.text = COUNT_OF_PLAYERS + slider.value;
        GameSettings.PlayersCount = (int)slider.value;
    }
    
    public void HandleDropdownValueChange()
    {
        buttonAS.Play();
        GameSettings.CardPairs = Convert.ToInt32(dropdown.options[dropdown.value].text);
    }

    public bool TrySetPlayersNames()
    {
        GameSettings.PlayersNames = new List<string>(_inputFields.Select(field => field.text));
        if (GameSettings.PlayersNames.Count != GameSettings.PlayersNames.Distinct().Count())
        {
            errorAS.Play();
            errorMessageDuplicatedNames.gameObject.SetActive(true);
            return false;
        }
        return true;
    }

    public void EnterPlayerNames()
    {
        buttonAS.Play();

        menuCanvas.gameObject.SetActive(false);
        inputCanvas.gameObject.SetActive(true);

        _inputFields = new List<InputField>();
        for (int i = 1; i <= slider.value; i++)
        {
            SetUpInputField(i);
        }
    }

    public void OpenMenuCanvas()
    {
        buttonAS.Play();

        menuCanvas.gameObject.SetActive(true);
        inputCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(false);
    }

    private void SetUpInputField(int inputIndex)
    {
        InputField inputField = inputIndex == 1 ? firstInput : Instantiate(firstInput);

        inputField.transform.SetParent(inputCanvas.transform, false);
        inputField.text = "Player " + inputIndex;
        inputField.gameObject.name = "Player" + inputIndex + "Inputfield";

        float posX = firstInput.transform.position.x;
        float posY = -(inputOffsetY * (inputIndex - 1)) + firstInput.transform.position.y;
        float posZ = firstInput.transform.position.z;
        inputField.transform.position = new Vector3(posX, posY, posZ);

        _inputFields.Add(inputField);
    }

    public void OpenSettingCanvas()
    {
        buttonAS.Play();

        menuCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        buttonAS.Play();

        if (TrySetPlayersNames())
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void QuitGame()
    {
        buttonAS.Play();
        Application.Quit();
    }
}
