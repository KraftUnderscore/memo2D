using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsGenerator : MonoBehaviour
{
    private class Card
    {
        public Card(int id, GameObject cardObj)
        {
            id_ = id;
            cardObj_ = cardObj;
        }

        private int id_;
        public GameObject cardObj_ { get; }
    }

    [SerializeField]
    private string cardsContainerTag_;

    [SerializeField]
    private float offset_;
    [SerializeField]
    private int maxRowLength_;

    enum Difficulty { _2x2 = 4, _2x4 = 8, _4x4 = 16 }

    private Difficulty difficulty_ = Difficulty._4x4;

    private Transform cardsContainer_;
    private List<Card> cardsStorage_;

    private List<Card> currentGame_;

    public GameObject cardPrefab_;

    private void Awake()
    {
        cardsContainer_ = GameObject.FindGameObjectWithTag(cardsContainerTag_).transform;
        InitializeCardPool();
        currentGame_ = new List<Card>();
    }

    private void InitializeCardPool()
    {
        cardsStorage_ = new List<Card>();
        for (int i = 0; i < (int) difficulty_; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab_, cardsContainer_);
            cardObj.SetActive(false);
            Card card = new Card(i, cardObj);
            cardsStorage_.Add(card);
        }
    }

    void Start()
    {
        DeployCards();
    }

    public void DeployCards()
    {
        currentGame_.AddRange(cardsStorage_);
        Shuffle();
        PlaceCards();
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
            currentGame_[i].cardObj_.transform.position = new Vector2(i % maxRowLength_ * offset_, (Mathf.FloorToInt(i / maxRowLength_) - 1) * offset_);
            currentGame_[i].cardObj_.SetActive(true);
        }
    }
}
