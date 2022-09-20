using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameController : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    //[SerializeField] Canvas canvasMenu;
    [SerializeField] Text winnerTextField;
    [SerializeField] ParticleSystem salut;
    [SerializeField] private SceneController sceneController;
    private readonly Color initialColor = Color.white;
    private readonly Color activatedColor = new Color(142 / 255f, 198 / 255f, 0);
    [SerializeField] private Transform gameCanvas;
    [SerializeField] private Transform menuCanvas;

    private void Awake()
    {
        sceneController.OnScoreChanged += UpdateScore;
        sceneController.OnPlayerActivated += ActivatePlayer;
        sceneController.OnPlayerDeactivated += DeactivatePlayer;
        sceneController.OnPlayerWon += ShowWinner;
    }

    private void UpdateScore(Player player)
    {
        GetInputFieldTextComponent(player.Name).text = player.Name + ": " + player.Score;
    }

    private void ActivatePlayer(Player player)
    {
        Text textField = GetInputFieldTextComponent(player.Name);
        textField.fontStyle = FontStyle.Bold;
        textField.color = activatedColor;
    }

    private void DeactivatePlayer(Player player)
    {
        Text textField = GetInputFieldTextComponent(player.Name);
        textField.fontStyle = FontStyle.Normal;
        textField.color = initialColor;
    }

    private void ShowWinner(Player player)
    {
        menuCanvas.gameObject.SetActive(true);
        salut.Play(true);
        winnerTextField.text = player.Name + " won !!!";
        winnerTextField.gameObject.SetActive(true);
    }

    public void OpenMenuCanvas()
    {
        menuCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    private Text GetInputFieldTextComponent(string playerName) 
        => gameCanvas.Find(playerName).GetComponent<Text>();
}
