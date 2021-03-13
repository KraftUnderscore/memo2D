using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CardsManager))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(UIManager))]
public class GameManager : MonoBehaviour
{
    private enum GameState { None_Selected, One_Selected, Two_Selected, Game_Over }

    private GameState currentState_;
    private UIManager UIManager_;
    private CardsManager cardsManager_;

    private int score_;
    [SerializeField]
    private int scoreIncrease_;
    [SerializeField]
    private int scoreDecrease_;
    [SerializeField]
    private int scoreToLose_;
    [SerializeField]
    private Transform[] camTargets_;

    [SerializeField]
    [Range(0f, 5f)]
    private float secondsToFlipBackCards_;

    private void Awake()
    {
        cardsManager_ = GetComponent<CardsManager>();
        UIManager_ = GetComponent<UIManager>();
    }

    public void SetDifficulty(int difficulty)
    {
        cardsManager_.SetDifficulty(difficulty);
    }

    public void StartGame()
    {
        UIManager_.ResetUI();
        score_ = 0;
        currentState_ = GameState.None_Selected;

        float timeToDeploy = cardsManager_.DeployCards();
        PlaceCameraTargets(cardsManager_.CamPoints_);
        StartCoroutine(ToggleStartButton(timeToDeploy));
    }

    private void PlaceCameraTargets(Vector2[] pos)
    {
        camTargets_[0].position = pos[0];
        camTargets_[1].position = pos[1];
    }

    private IEnumerator ToggleStartButton(float time)
    {
        UIManager_.ToggleStartButtonVisibility(false);
        yield return new WaitForSeconds(time);
        UIManager_.ToggleStartButtonVisibility(true);
    }

    public void RegisterHit(GameObject hitObject)
    {
        if (currentState_ == GameState.Two_Selected ||
            currentState_ == GameState.Game_Over ) return;

        string toParse = hitObject.name;
        int cardId = int.Parse(toParse);

        if (!cardsManager_.FlipCard(cardId)) return;
        
        UpdateState();

        UIManager_.UpdateScore(score_);
        if (score_ <= scoreToLose_)
        {
            currentState_ = GameState.Game_Over;
            UIManager_.DisplayGameOver();
        }
        else if (cardsManager_.IsDone())
        {
            currentState_ = GameState.Game_Over;
            UIManager_.DisplayVictory();
        }
    }
    private void UpdateState()
    {
        if (currentState_ == GameState.One_Selected)
        {
            if (cardsManager_.IsPair())
            {
                currentState_ = GameState.None_Selected;
                cardsManager_.RemovePair();
                score_ += scoreIncrease_;
            }
            else
            {
                currentState_ = GameState.Two_Selected;
                StartCoroutine(WaitToUnflipCards());
                score_ -= scoreDecrease_;
            }
        }
        else
        {
            currentState_ = GameState.One_Selected;
        }
    }

    private IEnumerator WaitToUnflipCards()
    {
        yield return new WaitForSeconds(secondsToFlipBackCards_);
        cardsManager_.UnflipCards();
        currentState_ = GameState.None_Selected;
    }
}
