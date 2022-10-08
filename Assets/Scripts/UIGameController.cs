using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using System.Linq;
using System;

public class UIGameController : MonoBehaviour
{
    [SerializeField] private Text winnerTextField;
    [SerializeField] private ParticleSystem salut;
    [SerializeField] private SceneController sceneController;

    [SerializeField] private Transform gameCanvas;
    [SerializeField] private Transform menuCanvas;
    [SerializeField] private Transform cards;
    [SerializeField] private Transform continueButton;

    private readonly Color initialColor = Color.white;
    private readonly Color activatedColor = new Color(142 / 255f, 198 / 255f, 0);
    private SoundManager soundAudioSource;

    private void Awake()
    {
        sceneController.OnScoreChanged += UpdateScore;
        sceneController.OnPlayerActivated += ActivatePlayer;
        sceneController.OnPlayerDeactivated += DeactivatePlayer;
        sceneController.OnPlayerWon += ShowWinner;
    }

    public void Start()
    {
        soundAudioSource = GameObject.Find("Audio Sources").transform.Find("Sound Source").GetComponent<SoundManager>();
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

    private void ShowWinner(List<Player> players)
    {
        soundAudioSource.PlayWonSound();

        menuCanvas.gameObject.SetActive(true);
        cards.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        salut.Play(true);
        winnerTextField.text = String.Join(", ", players.Select(p => p.Name)) + " won !!!";
        winnerTextField.fontStyle = FontStyle.Bold;
        winnerTextField.gameObject.SetActive(true);
    }

    public void OpenMenuCanvas()
    {
        soundAudioSource.PlayButtonSound();

        menuCanvas.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
        cards.gameObject.SetActive(false);
    }

    public void CloseMenuCanvas()
    {
        soundAudioSource.PlayButtonSound();

        continueButton.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        cards.gameObject.SetActive(true);
    }

    public void OpenMenu()
    {
        soundAudioSource.PlayButtonSound();
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        soundAudioSource.PlayButtonSound();
        SceneManager.LoadScene("Game");
    }

    private Text GetInputFieldTextComponent(string playerName) 
        => gameCanvas.Find(playerName).GetComponent<Text>();
}
