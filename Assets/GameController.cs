using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // TAMBAH INI


public class GameController : MonoBehaviour
{
    [Header("Settings")]
    public Transform fieldGrid;
    public GameObject cardPrefab;
    public List<Sprite> cardFaces;

    [Header("Timer")]
    public float maxTime = 60f; // TAMBAH INI

    [Header("UI")]
    public Text timeText;
    public Text scoreText;

    private float gameTime = 0f;
    private int score = 0;
    private bool gameOver = false; // TAMBAH INI

    private List<Card> _flippedCards = new List<Card>();
    public bool IsProcessing { get; private set; }

    // ====== TAMBAHAN UNTUK MENANG ======
    private int matchedPairs = 0;      // hitung berapa pasangan yang sudah cocok
    private int totalPairs = 0;        // total pasangan di level ini
    // ===================================

    void Start()
    {
        IsProcessing = true;
        UpdateScoreUI();
        UpdateTimeUI();
        StartGame();
    }

    void Update()
    {
        if (gameOver) return;

        gameTime += Time.deltaTime;
        if (gameTime >= maxTime)
        {
            gameOver = true;
            SceneManager.LoadScene("Game Over");
            return;
        }
        UpdateTimeUI();
    }

    void StartGame()
    {
        List<int> cardIndices = new List<int>();

        for (int i = 0; i < cardFaces.Count; i++)
        {
            cardIndices.Add(i);
            cardIndices.Add(i);
        }

        // set total pasangan (1 pasangan per sprite)
        totalPairs = cardFaces.Count;   // TAMBAH INI

        Shuffle(cardIndices);

        foreach (int index in cardIndices)
        {
            GameObject newCardObj = Instantiate(cardPrefab, fieldGrid);

            Card newCard = newCardObj.GetComponent<Card>();
            newCard.SetupCard(index, cardFaces[index], this);
        }

        IsProcessing = false;
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void CardClicked(Card card)
    {
        if (IsProcessing) return;
        _flippedCards.Add(card);

        if (_flippedCards.Count == 2)
        {
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        IsProcessing = true;
        yield return new WaitForSeconds(1.0f);

        if (_flippedCards[0].GetId() == _flippedCards[1].GetId())
        {
            Debug.Log("Pasangan Cocok!");
            score += 100;

            // opsional: disable kartu yang sudah cocok
            // _flippedCards[0].gameObject.SetActive(false);
            // _flippedCards[1].gameObject.SetActive(false);

            matchedPairs++;  // TAMBAH INI

            // cek kalau semua pasangan sudah terbuka
            if (matchedPairs >= totalPairs)   // TAMBAH INI
            {
                gameOver = true;              // supaya Update() berhenti jalan
                SceneManager.LoadScene("Menang"); // pastikan nama scenenya persis
                yield break;
            }
        }
        else
        {
            _flippedCards[0].ResetCard();
            _flippedCards[1].ResetCard();

            Debug.Log("Salah!");
            score -= 50;
            if (score < 0) score = 0;
        }

        UpdateScoreUI();

        _flippedCards.Clear();
        IsProcessing = false;
    }

    void UpdateTimeUI()
    {
        timeText.text = "Time: " + Mathf.FloorToInt(gameTime);
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }
}
