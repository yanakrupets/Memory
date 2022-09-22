using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private GameObject cardBack;
    [SerializeField] private SceneController controller;
    [SerializeField] private Transform parent;

    private int _id;
    public int Id
    {
        get { return _id; }
    }

    public void Initialize(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
        transform.SetParent(parent);
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
