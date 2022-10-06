using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preload : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene("Menu");
    }
}
