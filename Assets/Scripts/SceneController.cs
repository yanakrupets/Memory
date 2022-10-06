using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SceneController : MonoBehaviour
{
    private const int ROWS = 3;
    private const float OFFSET_Y = 1.8f;

    private int totalScore = 0;
    private int _columns = 6;
    private Sprite[] _images;
    private System.Random _random = new System.Random();
    private List<Player> _players = new List<Player>();

    private Player _currentPlayer;

    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;

    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Text firstPlayerTextField;
    [SerializeField] private Player firstPlayer;
    [SerializeField] private Sprite[] imagesClubs;
    [SerializeField] private Sprite[] imagesDiamonds;
    [SerializeField] private Sprite[] imagesHearts;
    [SerializeField] private Sprite[] imagesSpades;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform gameCanvas;
    private MusicManager musicAudioSource;

    public delegate void Notify(Player player);
    public event Notify OnScoreChanged;
    public event Notify OnPlayerActivated;
    public event Notify OnPlayerDeactivated;

    public delegate void NotifyAboutWinners(List<Player> players);
    public event NotifyAboutWinners OnPlayerWon;

    public bool canReveal
    {
        get { return _secondRevealed == null; }
    }

    void Start()
    {
        musicAudioSource = GameObject.Find("Audio Sources").transform.Find("Music Source").GetComponent<MusicManager>();
        musicAudioSource.StopPlaying();

        for (int i = 0; i < GameSettings.PlayersCount; i++)
        {
            var player = CreatePlayer(i);
            CreatePlayerTextField(player, i);
            _players.Add(player);
        }

        _currentPlayer = _players[0];
        OnPlayerActivated(_currentPlayer);

        _columns = (GameSettings.CardPairs * 2) / 3;

        InitializeCards();
    }

    private void InitializeCards()
    {
        int[] cardIds = CreateCardIds();

        _images = GetImagesList(GameSettings.CardPairs / 9);

        cardIds = ShuffleArray(cardIds);

        Vector3 startPosition = originalCard.transform.position;
        var offsetX = CalculateOffsetX();
        for (int i = 0; i < _columns; i++)
        {
            for (int j = 0; j < ROWS; j++)
            {
                MemoryCard card = Instantiate(originalCard);

                int id = cardIds[i * ROWS + j];
                card.Initialize(id, _images[id]);

                float posX = (offsetX * i) + startPosition.x;
                float posY = -(OFFSET_Y * j) + startPosition.y;
                card.transform.position = new Vector3(posX, posY, startPosition.z);

                card.gameObject.SetActive(true);
            }
        }
    }

    private float CalculateOffsetX()
    {
        // formulas for calculating the location of cards
        var cameraHeight = _camera.orthographicSize * 2;
        var cameraWidth = cameraHeight * _camera.aspect;

        var positionX = Math.Abs(originalCard.gameObject.transform.position.x);
        var cardScale = originalCard.gameObject.transform.localScale.x;

        return ((cameraWidth - ((cameraWidth / 2 - positionX) - cardScale / 2) * 2) -
            (_columns * cardScale)) / (_columns - 1) + cardScale;
    }

    private Player CreatePlayer(int index)
    {
        var player = Instantiate(firstPlayer);
        player.transform.name = GameSettings.PlayersNames[index];
        player.Name = GameSettings.PlayersNames[index];
        return player;
    }

    private void CreatePlayerTextField(Player player, int playerNumber)
    {
        var offsetXTextfield = firstPlayerTextField.rectTransform.sizeDelta.x;
        var playerTextField = Instantiate(firstPlayerTextField);

        playerTextField.transform.SetParent(gameCanvas);
        playerTextField.transform.name = player.Name;
        playerTextField.text = player.Name + ": " + player.Score;

        float posX = offsetXTextfield * playerNumber + firstPlayerTextField.transform.position.x;
        float posY = firstPlayerTextField.transform.position.y;
        float posZ = firstPlayerTextField.transform.position.z;
        playerTextField.transform.position = new Vector3(posX, posY, posZ);

        playerTextField.gameObject.SetActive(true);
    }

    private int[] CreateCardIds()
    {
        int[] cardIds = new int[_columns * ROWS];
        for (int i = 0; i < cardIds.Length; i++)
        {
            cardIds[i] = i / 2;
        }

        return cardIds;
    }

    public Sprite[] GetImagesList(int count)
    {
        var imagesDictionary = CreateImagesDictionary();
        var suit = new List<string> { "B", "CH", "K", "P" };
        Sprite[] imagesSprite = new Sprite[0];
        for (int i = 0; i < count; i++)
        {
            var randomSuit = suit[_random.Next(0, suit.Count)];
            var sprites = imagesDictionary[randomSuit];
            imagesDictionary.Remove(randomSuit);
            suit.Remove(randomSuit);
            imagesSprite = imagesSprite.Concat(sprites).ToArray();
        }

        return imagesSprite;
    }

    public Dictionary<string, Sprite[]> CreateImagesDictionary()
    {
        return new Dictionary<string, Sprite[]>{
            { "B", imagesClubs},
            { "CH", imagesDiamonds},
            { "K", imagesHearts},
            { "P", imagesSpades}
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
        if (_firstRevealed.Id == _secondRevealed.Id)
        {
            _currentPlayer.UpdateScore();
            OnScoreChanged(_currentPlayer);
            totalScore++;
            if (totalScore == GameSettings.CardPairs)
            {
                OnPlayerWon(_players.Select(player => player).Where(_ => _.Score == _players.Max(_ => _.Score)).ToList());
            }
        }
        else
        {
            OnPlayerDeactivated(_currentPlayer);

            var nextPlayerIndex = _players.IndexOf(_currentPlayer) + 1;
            _currentPlayer = _players.ElementAtOrDefault(nextPlayerIndex) ?? _players[0];

            OnPlayerActivated(_currentPlayer);

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
