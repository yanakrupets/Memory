using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private GameObject cardBack;
    [SerializeField] private SceneController controller;

    private int _id;
    public int Id
    {
        get { return _id; }
    }

    public void Initialize(int id, Sprite image)
    {
        _id = id;
        this.GetComponent<SpriteRenderer>().sprite = image;
    }

    public void OnMouseDown()
    {
        if (cardBack.activeSelf && controller.canReveal)
        {
            Reveal();
            controller.CardRevealed(this);
        }
    }

    public void Reveal()
    {
        cardBack.SetActive(false);
    }

    public void Unreveal()
    {
        cardBack.SetActive(true);
    }
}
