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
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Canvas inputCanvas;
    [SerializeField] private InputField firstInput;

    private const float OFFSET_Y = 42f;
    private const string COUNT_OF_PLAYERS = "Count of Players: ";

    private List<InputField> _inputFields = new List<InputField>();

    void Start()
    {
        countOfPlayers.text = COUNT_OF_PLAYERS + slider.value;
        slider.onValueChanged.AddListener(delegate { HandleSliderValueChange(); });
    }

    public void HandleSliderValueChange()
    {
        countOfPlayers.text = COUNT_OF_PLAYERS + slider.value;
        GameSettings.PlayersCount = (int)slider.value;
    }
    
    public void HandleDropdownValueChange()
    {
        GameSettings.CardPairs = Convert.ToInt32(dropdown.options[dropdown.value].text);
    }

    public void SetPlayersNames()
    {
        GameSettings.PlayersNames.AddRange(_inputFields.Select(field => field.text));
    }

    public void EnterPlayerNames()
    {
        menuCanvas.gameObject.SetActive(false);
        inputCanvas.gameObject.SetActive(true);

        for (int i = 1; i <= slider.value; i++)
        {
            SetUpInputField(i);
        }
    }

    private void SetUpInputField(int inputIndex)
    {
        InputField inputField = inputIndex == 1 ? firstInput : Instantiate(firstInput);

        inputField.transform.SetParent(inputCanvas.transform, false);
        inputField.text = "Player " + inputIndex;
        inputField.gameObject.name = "Player" + inputIndex + "Inputfield";

        float posX = firstInput.transform.position.x;
        float posY = -(OFFSET_Y * (inputIndex - 1)) + firstInput.transform.position.y;
        float posZ = firstInput.transform.position.z;
        inputField.transform.position = new Vector3(posX, posY, posZ);

        _inputFields.Add(inputField);
    }

    public void StartGame()
    {
        SetPlayersNames();
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
