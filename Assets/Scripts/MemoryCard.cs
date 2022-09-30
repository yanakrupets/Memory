using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MemoryCard : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject cardBack;
    [SerializeField] private SceneController controller;
    [SerializeField] private Transform parent;
    [SerializeField] private AudioSource cardSound;

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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (cardBack.activeSelf && controller.canReveal)
        {
            Reveal();
            controller.CardRevealed(this);
        }
    }

    public void Reveal()
    {
        cardSound.Play();
        cardBack.SetActive(false);
    }

    public void Unreveal()
    {
        cardBack.SetActive(true);
    }
}
