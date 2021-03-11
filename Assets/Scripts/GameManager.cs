using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardsManager))]
[RequireComponent(typeof(InputManager))]
public class GameManager : MonoBehaviour
{
    private enum GameState { None_Selected, One_Selected, Two_Selected, }

    private GameState currentState_ = GameState.None_Selected;

    private CardsManager cardsManager_;

    [SerializeField]
    [Range(0f, 5f)]
    private float secondsToFlipBackCards_;

    private void Awake()
    {
        cardsManager_ = GetComponent<CardsManager>();
    }

    public void SetDifficulty(int difficulty)
    {
        cardsManager_.SetDifficulty(difficulty);
    }

    public void StartGame()
    {
        cardsManager_.DeployCards();
    }

    public void RegisterHit(GameObject hitObject)
    {
        Debug.Log("Current state:" + (int)currentState_);
        if (currentState_ == GameState.Two_Selected) return;

        string toParse = hitObject.name;
        int result = int.Parse(toParse);

        bool hasFlipped = cardsManager_.FlipCard(result);

        if (!hasFlipped) return;

        if (currentState_ == GameState.One_Selected)
        {
            if (cardsManager_.IsPair())
            {
                currentState_ = GameState.None_Selected;
                cardsManager_.RemovePair();
            }
            else
            {
                currentState_ = GameState.Two_Selected;
                StartCoroutine(WaitToUnflipCards());
            }
        }
        else
        {
            currentState_ = GameState.One_Selected;
        }
        Debug.Log("End state:" + (int)currentState_);
    }

    private IEnumerator WaitToUnflipCards()
    {
        Debug.Log("Start timer");
        yield return new WaitForSeconds(secondsToFlipBackCards_);
        cardsManager_.UnflipCards();
        currentState_ = GameState.None_Selected;
        Debug.Log("End timer");
    }
}
