using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SceneController : MonoBehaviour
{
    private const int gridRows = 3;
    private int gridCols = 6;
    public float offsetX = 2.2f;
    private float offsetY = 1.8f;
    private float offsetXTextfield = 1f;
    private Sprite[] images;
    private Dictionary<string, Sprite[]> imagesDictionary = new Dictionary<string, Sprite[]>();
    private readonly string[] suit = new string[] { "B", "CH", "K", "P" };
    private System.Random random = new System.Random();
    private List<GameObject> players = new List<GameObject>();

    private int _score = 0;

    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;

    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Text firstPlayerTextField;
    [SerializeField] private GameObject firstPlayer;
    [SerializeField] private Sprite[] imagesB;
    [SerializeField] private Sprite[] imagesCH;
    [SerializeField] private Sprite[] imagesK;
    [SerializeField] private Sprite[] imagesP;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform canvas;

    public bool canReveal
    {
        get { return _secondRevealed == null; }
    }

    void Start()
    {
        ////////////////////////////////////////////////////////////////////////////
        offsetXTextfield = firstPlayerTextField.rectTransform.sizeDelta.x;
        for (int i = 0; i < GameSettings.PlayersCount; i++)
        {
            Text playerTextField;
            GameObject player;
            if (i == 0)
            {
                playerTextField = firstPlayerTextField;

                player = firstPlayer;
                player.transform.name = GameSettings.PlayersNames[i];
                var playerComponent = player.GetComponent<Player>();
                playerComponent.isActive = true;
                playerComponent.Name = GameSettings.PlayersNames[i];

                players.Add(player);
            }
            else
            {
                playerTextField = Instantiate(firstPlayerTextField) as Text;

                player = Instantiate(firstPlayer) as GameObject;
                player.transform.name = GameSettings.PlayersNames[i];
                var playerComponent = player.GetComponent<Player>();
                playerComponent.isActive = true;
                playerComponent.Name = GameSettings.PlayersNames[i];

                players.Add(player);
            }

            playerTextField.transform.SetParent(canvas);
            playerTextField.transform.name = players[i].GetComponent<Player>().Name;
            playerTextField.text = players[i].GetComponent<Player>().Name + ": " + players[i].GetComponent<Player>().Score;

            float posX = (offsetXTextfield * i) + firstPlayerTextField.transform.position.x;
            float posY = firstPlayerTextField.transform.position.y;
            float posZ = firstPlayerTextField.transform.position.z;
            playerTextField.transform.position = new Vector3(posX, posY, posZ);
        }
        ////////////////////////////////////////////////////////////////////////////////
        gridCols = (GameSettings.CardPairs * 2) / 3;

        var h = _camera.orthographicSize * 2;
        var w = h * _camera.aspect;

        // formulas for calculating the location of cards
        var positionX = Math.Abs(originalCard.gameObject.transform.position.x);
        var cardScale = originalCard.gameObject.transform.localScale.x;
        offsetX = ((w - ((w / 2 - positionX) - cardScale / 2) * 2) - (gridCols * cardScale)) / (gridCols - 1) + cardScale;

        int[] numbers = new int[gridCols * gridRows];
        var currentElement = 1;
        for (int i = 2; i < numbers.Length; i++)
        {
            numbers[i] = currentElement;
            if (numbers[i] == numbers[i - 1])
            {
                currentElement++;
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////
        images = GetImagesList(GameSettings.CardPairs / 9);

        Vector3 startPos = originalCard.transform.position;

        numbers = ShuffleArray(numbers);
        var currentIndex = 0;

        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MemoryCard card;

                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }

                int id = numbers[currentIndex];
                card.SetCard(id, images[id]);
                currentIndex++;

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
    }

    public Sprite[] GetImagesList(int count)
    {
        imagesDictionary = CreateImagesDictionary();
        Sprite[] imagesSprite = new Sprite[0];
        for (int i = 0; i < count; i++)
        {
            var randomSuit = suit[random.Next(0, suit.Length)];
            var sprites = imagesDictionary[randomSuit];
            imagesDictionary.Remove(randomSuit);
            imagesSprite = imagesSprite.Concat(sprites).ToArray();
        }

        return imagesSprite;
    }

    public Dictionary<string, Sprite[]> CreateImagesDictionary()
    {
        return new Dictionary<string, Sprite[]>{
            { "B", imagesB},
            { "CH", imagesCH},
            { "K", imagesK},
            { "P", imagesP}
        };
    }

    public void CardRevealed(MemoryCard card)
    {
        if (_firstRevealed == null)
        {
            _firstRevealed = card;
        }
        else
        {
            _secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        if (_firstRevealed.id == _secondRevealed.id)
        {
            _score++;
            scoreLabel.text = "Score: " + _score;
        }
        else
        {
            yield return new WaitForSeconds(.5f);

            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
        }

        _firstRevealed = null;
        _secondRevealed = null;
    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = UnityEngine.Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }
}
