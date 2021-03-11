using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    private class Card
    {
        public Card(int id, int pairId, GameObject cardObj)
        {
            id_ = id;
            pairId_ = pairId;
            cardObj_ = cardObj;
            isFlipped_ = false;
        }

        public int id_ { get; }
        public int pairId_ { get; }
        public bool isFlipped_;
        public GameObject cardObj_ { get; }
    }

    [SerializeField]
    private string cardsContainerTag_;

    [SerializeField]
    private float offset_;
    private int maxRowLength_;
    private Vector2 cardSize_;

    enum Difficulty { _2x2 = 4, _2x4 = 8, _4x4 = 16 , LAST_VALUE = _4x4}

    private Difficulty difficulty_;
    private Difficulty Difficulty_
    {
        get { return difficulty_; }
        set 
        { 
            difficulty_ = value;
            maxRowLength_ = value switch
            {
                Difficulty._2x2 => 2,
                _ => 4,
            };
        }
    }

    private Transform cardsContainer_;
    private List<Card> cardsStorage_;

    private List<Card> currentGame_;

    public GameObject cardPrefab_;

    private void Awake()
    {
        cardsContainer_ = GameObject.FindGameObjectWithTag(cardsContainerTag_).transform;
        InitializeCardPool();
        currentGame_ = new List<Card>();
        cardSize_ = cardPrefab_.transform.GetChild(0).GetComponent<SpriteRenderer>().size;
    }

    private void InitializeCardPool()
    {
        cardsStorage_ = new List<Card>();
        for (int i = 0; i < (int) Difficulty.LAST_VALUE; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab_, cardsContainer_);
            cardObj.name = i.ToString();
            cardObj.SetActive(false);
            Card card = new Card(i, i%2 == 0 ? i + 1 : i - 1,cardObj);
            cardsStorage_.Add(card);
        }
    }

    public void SetDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                Difficulty_ = Difficulty._2x2;
                break;
            case 1:
                Difficulty_ = Difficulty._2x4;
                break;
            case 2:
                Difficulty_ = Difficulty._4x4;
                break;
            default:
                Difficulty_ = Difficulty._2x2;
                break;
        }
    }

    public void UnflipCards()
    {
        currentGame_.ForEach(delegate (Card c) { c.isFlipped_ = false; });
    }

    public bool IsPair()
    {
        List<Card> flipped = new List<Card>(2);
        foreach(Card c in currentGame_)
        {
            if (c.isFlipped_) flipped.Add(c);
        }

        if(flipped.Count == 2 && flipped[0].pairId_ == flipped[1].id_)
            return true;

        return false;
    }

    public void RemovePair()
    {
        foreach(Card c in currentGame_)
        {
            if(c.isFlipped_)
            {
                c.cardObj_.SetActive(false);
                c.isFlipped_ = false;
            }
        }
    }

    public bool FlipCard(int cardId)
    {
        foreach (Card c in currentGame_)
        {
            if (c.id_ == cardId)
            {
                if (c.isFlipped_) break;
                Debug.Log("flipping " + cardId);
                c.isFlipped_ = true;
                return true;
            }
        }
        return false;
    }

    public void DeployCards()
    {
        ResetGame();

        for (int i = 0; i < (int) Difficulty_; i++)
            currentGame_.Add(cardsStorage_[i]);

        Shuffle();
        PlaceCards();
        CorrectContainerPosition();
    }

    private void ResetGame()
    {
        cardsContainer_.position = Vector2.zero;
        foreach (Card card in currentGame_)
        {
            card.cardObj_.SetActive(false);
            card.isFlipped_ = false;
        }
        currentGame_.Clear();
    }

    private void CorrectContainerPosition()
    {
        switch (Difficulty_)
        {
            case Difficulty._2x2:
                cardsContainer_.position = new Vector2(-cardSize_.x, -cardSize_.y);
                break;
            case Difficulty._2x4:
                cardsContainer_.position = new Vector2(-cardSize_.x - offset_, -cardSize_.y);
                break;
            case Difficulty._4x4:
                cardsContainer_.position = new Vector2(-cardSize_.x - offset_, -cardSize_.y - offset_);
                break;
        }
    }

    private void Shuffle()
    {
        int n = currentGame_.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            Card value = currentGame_[k];
            currentGame_[k] = currentGame_[n];
            currentGame_[n] = value;
        }
    }

    private void PlaceCards()
    {
        for (int i = 0; i < currentGame_.Count; i++)
        {
            currentGame_[i].cardObj_.transform.position = new Vector2(i % maxRowLength_ * offset_, (Mathf.FloorToInt(i / maxRowLength_)) * offset_);
            currentGame_[i].cardObj_.SetActive(true);
        }
    }
}
