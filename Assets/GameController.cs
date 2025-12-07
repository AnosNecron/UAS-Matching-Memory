using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Settings")]
    public Transform fieldGrid;    
    public GameObject cardPrefab;   
    public List<Sprite> cardFaces;  


    private List<Card> _flippedCards = new List<Card>(); 
    public bool IsProcessing { get; private set; } 

    void Start()
    {
        IsProcessing = true; 
        StartGame();
    }

    void StartGame()
    {
        
        List<int> cardIndices = new List<int>();

       
        for (int i = 0; i < cardFaces.Count; i++)
        {
            cardIndices.Add(i);
            cardIndices.Add(i);
        }
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
        }
        else
        {
          
            _flippedCards[0].ResetCard();
            _flippedCards[1].ResetCard();
        }

        
        _flippedCards.Clear();
        IsProcessing = false; 
    }

}
