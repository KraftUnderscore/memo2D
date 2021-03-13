using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class CardsManager : MonoBehaviour
{
    [SerializeField]
    private string cardsContainerTag_;
    [SerializeField]
    private float spawnDelay_;
    [SerializeField]
    private float offset_;
    private int maxRowLength_;
    private int matches_;
    private Vector2 cardSize_;

    private Vector2[] camPoints_;
    public Vector2[] CamPoints_
    {
        get
        {
            return camPoints_;
        }
    }

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

    [SerializeField]
    private Sprite[] card_faces_;
    private Transform cardsContainer_;
    private List<Card> cardsStorage_;

    private List<Card> currentGame_;

    public GameObject cardPrefab_;

    private void Awake()
    {
        cardsContainer_ = GameObject.FindGameObjectWithTag(cardsContainerTag_).transform;
        InitializeCardPool();
        currentGame_ = new List<Card>();
        cardSize_ = cardPrefab_.GetComponent<BoxCollider2D>().size;
        camPoints_ = new Vector2[2];
    }

    private void InitializeCardPool()
    {
        cardsStorage_ = new List<Card>();
        int spritesIterator_ = 0;
        for (int i = 0; i < (int) Difficulty.LAST_VALUE; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab_, cardsContainer_);
            cardObj.name = i.ToString();
            cardObj.SetActive(false);
            Card card = cardObj.GetComponent<Card>(); 
            card.InitializeCard(i, i%2 == 0 ? i + 1 : i - 1, cardObj, card_faces_[spritesIterator_]);
            cardsStorage_.Add(card);
            if (i % 2 == 1) spritesIterator_++;
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

    public bool IsDone()
    {
        return matches_ == currentGame_.Count;
    }

    public void UnflipCards()
    {
        currentGame_.ForEach(delegate (Card c) { if(c.isFlipped_) c.Unflip(); });
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
                c.Disappear();
            }
        }
        matches_ += 2;
    }

    public bool FlipCard(int cardId)
    {
        foreach (Card c in currentGame_)
        {
            if (c.id_ == cardId)
            {
                if (c.isFlipped_) break;
                c.Flip();
                return true;
            }
        }
        return false;
    }

    public float DeployCards()
    {
        ResetGame();

        for (int i = 0; i < (int) Difficulty_; i++)
            currentGame_.Add(cardsStorage_[i]);

        Shuffle();
        PlaceCards();
        // CorrectContainerPosition();
        SetUpCamTargets();

        return currentGame_.Count * spawnDelay_;
    }
    
    private void SetUpCamTargets()
    {
        camPoints_[0] = currentGame_[0].transform.position;
        camPoints_[1] = currentGame_[currentGame_.Count - 1].transform.position;
    }

    private void ResetGame()
    {
        matches_ = 0;
        cardsContainer_.position = Vector2.zero;
        foreach (Card card in cardsStorage_)
        {
            card.Restore();
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
            currentGame_[i].cardObj_.transform.position = new Vector2(i % maxRowLength_ * (cardSize_.x + offset_), (Mathf.FloorToInt(i / maxRowLength_)) * (cardSize_.y + offset_));
            StartCoroutine(ActivateWithDelay(currentGame_[i].cardObj_, spawnDelay_ * i));
        }
    }

    private IEnumerator ActivateWithDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
    }
}
