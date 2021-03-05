using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsGenerator : MonoBehaviour
{
    private class Card
    {
        private int id_;

    }

    [SerializeField]
    private readonly string cardsContainerTag_;

    private Transform cardsContainer_;
    private List<Card> cardsStorage_;

    private void Awake()
    {
        cardsContainer_ = GameObject.FindGameObjectWithTag(cardsContainerTag_).transform;
        // if (cardsContainer == null) throw new System.Exception("Couldn't find cards container with tag: " + cardsContainerTag);
        InitializeCardPool();
    }

    private void InitializeCardPool()
    {
        cardsStorage_ = new List<Card>();
        // TODO: add cards from ScriptableObjects
    }

    void Start()
    {
        Generate();
    }

    void Update()
    {
        
    }

    public void Generate()
    {

    }
}
