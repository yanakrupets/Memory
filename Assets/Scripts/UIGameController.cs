using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;

public class UIGameController : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Text winnerTextField;
    [SerializeField] ParticleSystem salut;
    [SerializeField] private SceneController sceneController;
    private readonly Color initialColor = Color.white;
    private readonly Color activatedColor = new Color(142 / 255f, 198 / 255f, 0);
    [SerializeField] private Transform gameCanvas;
    [SerializeField] private Transform menuCanvas;
    [SerializeField] private Transform cards;
    [SerializeField] private Transform continueButton;
    [SerializeField] private AudioSource buttonAS;
    [SerializeField] private AudioSource wonAS;

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

    private void ShowWinner(List<Player> players)
    {
        wonAS.Play();
        menuCanvas.gameObject.SetActive(true);
        cards.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        salut.Play(true);
        StringBuilder sb = new StringBuilder();
        foreach (var player in players)
        {
            sb.Append(player.Name + ", ");
        }
        sb.Remove(sb.Length - 2, 2);
        winnerTextField.text = sb + " won !!!";
        winnerTextField.fontStyle = FontStyle.Bold;
        winnerTextField.gameObject.SetActive(true);
    }

    public void OpenMenuCanvas()
    {
        buttonAS.Play();
        menuCanvas.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
        cards.gameObject.SetActive(false);
    }

    public void CloseMenuCanvas()
    {
        buttonAS.Play();
        continueButton.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
        cards.gameObject.SetActive(true);
    }

    public void PlaySound(AudioSource audioSource) =>
        audioSource.Play();

    public void OpenMenu()
    {
        buttonAS.Play();
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        buttonAS.Play();
        SceneManager.LoadScene("Game");
    }

    private Text GetInputFieldTextComponent(string playerName) 
        => gameCanvas.Find(playerName).GetComponent<Text>();
}
