using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameController : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] private SceneController sceneController;
    private readonly Color initialColor = Color.white;
    private readonly Color activatedColor = new Color(142 / 255f, 198 / 255f, 0);

    private void Awake()
    {
        sceneController.OnScoreChanged += UpdateScore;
        sceneController.OnPlayerActivated += ActivatePlayer;
        sceneController.OnPlayerDeactivated += DeactivatePlayer;
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

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    private Text GetInputFieldTextComponent(string playerName) 
        => canvas.transform.Find(playerName).GetComponent<Text>();
}
