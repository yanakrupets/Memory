using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int Score { get; set; } = 0;
    public string Name { get; set; } = "Player";
    public bool isActive = false;
    //public Text ScoreTextField { get; set; }

    public void UpdateScore()
    {
        Score++;
        //ScoreTextField.text = Name + ": " + Score;
    }

    public void ActivatePlayer()
    {
        
    }
}
