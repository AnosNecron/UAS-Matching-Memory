using UnityEngine;
using UnityEngine.UI; 

public class Card : MonoBehaviour
{
    [Header("Referensi UI")]
    public Image cardImage; 
    public Button btn;      

    [Header("Asset")]
    public Sprite backSprite;

    private Sprite _faceSprite;
    private int _cardId;
    private GameController _manager;

    private bool _isFlipped = false;

    public void SetupCard(int id, Sprite face, GameController manager)
    {
        _cardId = id;
        _faceSprite = face;
        _manager = manager;

        cardImage.sprite = backSprite;

       
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnCardClicked);
    }

    void OnCardClicked()
    {
        if (_isFlipped || _manager.IsProcessing) return;

        FlipCard();
        _manager.CardClicked(this);
    }

    public void FlipCard()
    {
        _isFlipped = !_isFlipped;
        cardImage.sprite = _isFlipped ? _faceSprite : backSprite;
    }

    public int GetId() => _cardId;

    public void ResetCard()
    {
        _isFlipped = false;
        cardImage.sprite = backSprite;
    }
}