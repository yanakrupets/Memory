using System;
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

    public const float offsetY = 42f;

    private List<string> inputFieldsNames = new List<string>();

    void Start()
    {
        menuCanvas.gameObject.SetActive(true);
        inputCanvas.gameObject.SetActive(false);

        countOfPlayers.text = "Count of Players: " + slider.value;
        slider.onValueChanged.AddListener(delegate { SliderValueChange(); });
    }

    public void SliderValueChange()
    {
        countOfPlayers.text = "Count of Players: " + slider.value;
        GameSettings.PlayersCount = (int)slider.value;
    }
    
    public void DropdownValueChange()
    {
        GameSettings.CardPairs = Convert.ToInt32(dropdown.options[dropdown.value].text);
    }

    public void SetPlayersNamesAndStartTheGame()
    {
        foreach (var name in inputFieldsNames)
        {
            GameSettings.PlayersNames.Add(GameObject.Find(name).GetComponent<InputField>().text);
        }

        StartGame();
    }

    public void EnterPlayerNames()
    {
        menuCanvas.gameObject.SetActive(false);
        inputCanvas.gameObject.SetActive(true);

        Vector3 startPosition = firstInput.transform.position;

        for (int i = 0; i < slider.value; i++)
        {
            InputField inputField;

            if (i == 0)
            {
                inputField = firstInput;
            }
            else
            {
                inputField = Instantiate(firstInput) as InputField;
            }
            //inputField.gameObject.SetActive(true);

            inputField.transform.SetParent(inputCanvas.transform, false);
            inputField.text = "Player " + (i + 1);
            inputField.gameObject.name = "Player" + (i + 1) + "Inputfield";

            inputFieldsNames.Add(inputField.gameObject.name);

            float posX = firstInput.transform.position.x;
            float posY = -(offsetY * i) + firstInput.transform.position.y;
            float posZ = firstInput.transform.position.z;
            inputField.transform.position = new Vector3(posX, posY, posZ);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
